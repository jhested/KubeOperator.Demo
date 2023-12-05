using k8s.Models;
using KubeOperator.Demo.KStatus;
using KubeOps.KubernetesClient;

namespace KubeOperator.Demo
{
    public class ServiceAccountReconciler(
        IKubernetesClient kubernetesClient,
        IActiveDirectoryClient activeDirectoryClient,
        IMicrosoftEntraClient microsoftEntraClient) : IServiceAccountReconciler
    {
        public IKubernetesClient KubernetesClient { get; } = kubernetesClient;
        public IActiveDirectoryClient ActiveDirectoryClient { get; } = activeDirectoryClient;
        public IMicrosoftEntraClient MicrosoftEntraClient { get; } = microsoftEntraClient;

        public async Task<ReconcileResult> ReconcileAsync(V1Alpha1ServiceAccount entity, CancellationToken cancellationToken = default)
        {
            if(entity.Status.ObservedGeneration != entity.Generation())
            {
                entity.Status.ObservedGeneration = entity.Generation();
                entity.Status.SetCondition(ConditionReason.Reconciling, ConditionType.Ready, ConditionStatus.Unknown);
                await KubernetesClient.UpdateStatus(entity);
            }

            var status = entity.Status.GetCondition(ConditionType.Ready)!.Value;
            if (!await ActiveDirectoryClient.ServiceAccountExistsAsync(entity.Spec.Name) && status.Reason != ConditionReason.ServiceAccountRequested)
            {
                await ActiveDirectoryClient.RequestServiceAccountAsync(
                    entity.Spec.Name,
                    entity.Spec.Memberships.Select(x => x.GroupName),
                    cancellationToken);

                entity.Status.SetCondition(ConditionReason.ServiceAccountRequested, ConditionType.Ready, ConditionStatus.False);
                await KubernetesClient.UpdateStatus(entity);

                return new ReconcileResult { RequeueAfter = TimeSpan.FromMinutes(2) };
            }

            status = entity.Status.GetCondition(ConditionType.Ready)!.Value;
            if(!await MicrosoftEntraClient.AppRegistrationExistsAsync(entity.Spec.Name, cancellationToken) && status.Reason != ConditionReason.AppRegistrationRequested)
            {
                await MicrosoftEntraClient.CreateAppRegistrationAsync(entity.Spec.Name, cancellationToken);

                entity.Status.SetCondition(ConditionReason.AppRegistrationRequested, ConditionType.Ready, ConditionStatus.False);

                await KubernetesClient.UpdateStatus(entity);

                return new ReconcileResult { RequeueAfter = TimeSpan.FromMinutes(2) };
            }

            entity.Status.AzureAppRegistration = await MicrosoftEntraClient.GetAppRegistrationAsync(entity.Spec.Name, cancellationToken);

            entity.Status.SetCondition(ConditionReason.ReconciliationSucceeded, ConditionType.Ready, ConditionStatus.True);
            await KubernetesClient.UpdateStatus(entity);

            return await Task.FromResult(new ReconcileResult { RequeueAfter = TimeSpan.FromMinutes(20) });
        }
    }
}
