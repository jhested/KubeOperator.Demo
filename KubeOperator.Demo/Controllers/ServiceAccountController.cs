using k8s.Models;
using KubeOperator.Demo.Entities;
using KubeOps.Operator.Controller;
using KubeOps.Operator.Controller.Results;
using KubeOps.Operator.Rbac;

namespace KubeOperator.Demo.Controllers
{
    [EntityRbac(typeof(V1Alpha1ServiceAccount), Verbs = RbacVerb.All)]
    public class ServiceAccountController(
        ILogger<ServiceAccountController> logger,
        ServiceAccountReconciler serviceAccountManager) : IResourceController<V1Alpha1ServiceAccount>
    {
        private readonly ILogger<ServiceAccountController> _logger = logger;
        private readonly ServiceAccountReconciler _serviceAccountManager = serviceAccountManager;

        public async Task<ResourceControllerResult?> ReconcileAsync(V1Alpha1ServiceAccount entity)
        {
            _logger.LogInformation("Reconciling {type} '{entity}'", nameof(V1Alpha1ServiceAccount), entity.Name());

            await _serviceAccountManager.ReconcileAsync(entity);
            return ResourceControllerResult.RequeueEvent(TimeSpan.FromHours(1));
        }
    }
}
