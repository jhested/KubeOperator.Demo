using AutoFixture.Xunit2;
using k8s.Models;
using KubeOps.KubernetesClient;
using Moq;

namespace KubeOperator.Demo.Tests.Reconcilers
{
    public class ServiceAccountReconcilerTests
    {
        [Theory, AutoMoqData]
        public async Task ReconcileAsync_Sets_Ready_Status_To_Unknown_When_Generation_Changes(
            V1Alpha1ServiceAccount entity,            
            [Frozen] IKubernetesClient kubernetesClient,
            ServiceAccountReconciler sut)
        {
            var statusCondition = default(Condition);
            entity.Status.ObservedGeneration = entity.Generation() - 1;
            var kubeMoq = Mock.Get(kubernetesClient);
            kubeMoq.Setup(x => x.UpdateStatus(It.IsAny<V1Alpha1ServiceAccount>()))
                .Callback<V1Alpha1ServiceAccount>(x =>
                {
                    var condition = x.Status.GetCondition(ConditionType.Ready)!.Value;
                    if (condition.Status == ConditionStatus.Unknown)
                    {
                        statusCondition = condition;
                    }
                })
                .Returns(Task.CompletedTask);

            await sut.ReconcileAsync(entity);
            Assert.Equal(ConditionStatus.Unknown, statusCondition!.Status);
        }

        [Theory, AutoMoqData]
        public async Task ReconcileAsync_Sets_Ready_Status_To_True_When_Ready_Reconcile_Succeeds(
            [Frozen] IActiveDirectoryClient activeDirectoryClient,
            [Frozen] IMicrosoftEntraClient entraClient,
            V1Alpha1ServiceAccount entity,
            ServiceAccountReconciler sut)
        {
            Mock.Get(activeDirectoryClient)
                .Setup(x => x.ServiceAccountExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())) 
                .ReturnsAsync(true);

            Mock.Get(entraClient)
                .Setup(x => x.AppRegistrationExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            await sut.ReconcileAsync(entity);
            var status = entity.Status.GetCondition(ConditionType.Ready);
            Assert.Equal(ConditionStatus.True, status!.Value.Status);
        }

        [Theory, AutoMoqData]
        public async Task ReconcileAsync_Requests_ServiceAccount_When_Not_Exists(
            [Frozen] IActiveDirectoryClient activeDirectoryClient,
            V1Alpha1ServiceAccount entity,
            ServiceAccountReconciler sut)
        {
            var mock = Mock.Get(activeDirectoryClient);
            mock.Setup(x => x.ServiceAccountExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);   

            await sut.ReconcileAsync(entity);

            mock.Verify(x => x.RequestServiceAccountAsync(It.IsAny<string>(), It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()));
        }

        [Theory, AutoMoqData]
        public async Task ReconcileAsync_Creates_AppReg_When_Not_Exists(
            [Frozen] IActiveDirectoryClient activeDirectoryClient,
            [Frozen] IMicrosoftEntraClient entraClient, 
            V1Alpha1ServiceAccount entity,
            ServiceAccountReconciler sut)
        {
            var adMock = Mock.Get(activeDirectoryClient);
            adMock.Setup(x => x.ServiceAccountExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var entraMock = Mock.Get(entraClient);
            entraMock.Setup(x => x.AppRegistrationExistsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            await sut.ReconcileAsync(entity);

            entraMock.Verify(x => x.CreateAppRegistrationAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()));
        }
    }
}
