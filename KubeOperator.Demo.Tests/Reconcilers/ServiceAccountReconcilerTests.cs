using AutoFixture.Xunit2;

namespace KubeOperator.Demo.Tests.Reconcilers
{
    public class ServiceAccountReconcilerTests
    {
        [Theory, AutoData]
        public async Task ReconcileAsync_Sets_Ready_Status_To_Unknown_When_Generation_Changes()
        {

        }

        [Theory, AutoData]
        public async Task ReconcileAsync_Sets_Ready_Status_To_True_When_Ready_Reconcile_Succeeds()
        {

        }

        [Theory, AutoData]
        public async Task ReconcileAsync_Sets_Ready_Status_To_False_When_Ready_Reconcile_Fails()
        {

        }

        [Theory, AutoData]
        public async Task ReconcileAsync_Sets_Ready_Status_To_False_When_Waiting_For_Azure_App_Reg()
        {

        }

        [Theory, AutoData]
        public async Task ReconcileAsync_Sets_Ready_Status_To_False_When_Waiting_For_ServiceAccount()
        {

        }
    }
}
