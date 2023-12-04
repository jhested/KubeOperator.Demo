using KubeOps.KubernetesClient;

namespace KubeOperator.Demo
{
    public interface IServiceAccountReconciler
    {
        Task<ReconcileResult> ReconcileAsync(V1Alpha1ServiceAccount entity, CancellationToken cancellationToken = default);
    }
}