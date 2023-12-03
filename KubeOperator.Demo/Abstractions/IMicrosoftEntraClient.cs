namespace KubeOperator.Demo
{
    public interface IMicrosoftEntraClient
    {
        Task<bool> AppRegistrationExistsAsync(string name, CancellationToken cancellationToken = default);
        Task CreateAppRegistrationAsync(string name, CancellationToken cancellationToken = default);
        Task<AzureAppRegistration> GetAppRegistrationAsync(string name, CancellationToken cancellationToken);
    }
}
