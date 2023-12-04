using KubeOps.KubernetesClient;

namespace KubeOperator.Demo
{
    public interface IServiceAccountReconciler
    {
        IActiveDirectoryClient ActiveDirectoryClient { get; }
        IKubernetesClient KubernetesClient { get; }
        IMicrosoftEntraClient MicrosoftEntraClient { get; }

        Task<ReconcileResult> ReconcileAsync(V1Alpha1ServiceAccount entity, CancellationToken cancellationToken = default);
    }
}