using k8s.Models;
using KubeOperator.Demo.KStatus;
using KubeOps.KubernetesClient;

namespace KubeOperator.Demo
{
    public class ServiceAccountReconciler(
        IKubernetesClient kubernetesClient,
        IActiveDirectoryClient activeDirectoryClient,
        IMicrosoftEntraClient microsoftEntraClient)
    {
        public IKubernetesClient KubernetesClient { get; } = kubernetesClient;
        public IActiveDirectoryClient ActiveDirectoryClient { get; } = activeDirectoryClient;
        public IMicrosoftEntraClient MicrosoftEntraClient { get; } = microsoftEntraClient;

        public async Task<ReconcileResult> ReconcileAsync(V1Alpha1ServiceAccount entity, CancellationToken cancellationToken = default)
        {
            // Set status

            if(entity.Status.ObservedGeneration != entity.Generation())
            {
                entity.Status.ObservedGeneration = entity.Generation();
                entity.Status.SetCondition(ConditionReason.Reconciling, ConditionType.Ready, ConditionStatus.Unknown);
                await KubernetesClient.UpdateStatus(entity);
            }

            // Check AD
            if(await ShouldRequestServiceAccount(entity, cancellationToken))
            {
                await ActiveDirectoryClient.RequestServiceAccountAsync(entity.Spec.Name, entity.Spec.Memberships.Select(x => x.GroupName), cancellationToken);
                return await NotReady(entity, ConditionReason.ServiceAccountRequested, "Waiting for Windows AD");
            }

            if(await ShouldRequestAzureAppRegistration(entity, cancellationToken))
            {
                await MicrosoftEntraClient.CreateAppRegistrationAsync(entity.Spec.Name, cancellationToken);
                return await NotReady(entity, ConditionReason.AppRegistrationRequested, "Waiting for Azure AD");
            }

            entity.Status.AzureAppRegistration = await MicrosoftEntraClient.GetAppRegistrationAsync(entity.Spec.Name, cancellationToken);
            return await Ready(entity);
        }

        private async Task<bool> ShouldRequestServiceAccount(V1Alpha1ServiceAccount entity, CancellationToken cancellationToken = default)
        {
            var status = entity.Status.GetCondition(ConditionType.Ready)!;
            if (!await ActiveDirectoryClient.ServiceAccountExistsAsync(entity.Spec.Name, cancellationToken)
                && status.Value.Reason != ConditionReason.ServiceAccountRequested)
            {
                return true;
            }

            return false;
        }

        private async Task<bool> ShouldRequestAzureAppRegistration(V1Alpha1ServiceAccount entity, CancellationToken cancellationToken = default)
        {
            var status = entity.Status.GetCondition(ConditionType.Ready)!;
            if (!await MicrosoftEntraClient.AppRegistrationExistsAsync(entity.Spec.Name, cancellationToken)
                && status.Value.Reason != ConditionReason.ServiceAccountRequested)
            {
                return true;
            }

            return false;
        }

        private async Task<ReconcileResult> NotReady(V1Alpha1ServiceAccount entity, string reason, string message)
{
            entity.Status.SetCondition(reason, ConditionType.Ready, ConditionStatus.False, message);
            await KubernetesClient.UpdateStatus(entity);
            return new ReconcileResult { RequeueAfter = TimeSpan.FromMinutes(5) };
        }

        private async Task<ReconcileResult> Ready(V1Alpha1ServiceAccount entity)
        {
            entity.Status.SetCondition(ConditionReason.Reconciling, ConditionType.Ready, ConditionStatus.True);
            await KubernetesClient.UpdateStatus(entity);
            return new ReconcileResult { RequeueAfter = TimeSpan.FromHours(1) };
        }
    }
}
