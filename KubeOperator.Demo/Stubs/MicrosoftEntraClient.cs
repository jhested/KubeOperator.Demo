
namespace KubeOperator.Demo
{
    public class MicrosoftEntraClient : IMicrosoftEntraClient
    {
        public Task<bool> AppRegistrationExistsAsync(string name, CancellationToken cancellationToken = default)
        {
            var filename = $"appreg_{name}";
            if (File.Exists(filename))
            {
                return Task.FromResult(new FileInfo(filename).CreationTimeUtc < DateTime.UtcNow.AddMinutes(-2));
            }

            return Task.FromResult(false);
        }

        public Task CreateAppRegistrationAsync(string name, CancellationToken cancellationToken = default)
        {
            var filename = $"appreg_{name}";
            if (!File.Exists(filename))
            {
                using var tmp = File.Create(filename);
            }

            return Task.CompletedTask;
        }

        public Task<AzureAppRegistration> GetAppRegistrationAsync(string name, CancellationToken cancellationToken)
        {
            return Task.FromResult(new AzureAppRegistration
            {
                ApplicationId = Guid.NewGuid().ToString(),
                ApplictionName = name
            });
        }
    }
}
