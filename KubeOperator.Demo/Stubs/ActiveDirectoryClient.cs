
namespace KubeOperator.Demo
{
    public class ActiveDirectoryClient : IActiveDirectoryClient
    {
        public Task RequestServiceAccountAsync(string name, IEnumerable<string> membership, CancellationToken cancellationToken = default)
        {
            var filename = $"sa_{name}";
            if (!File.Exists(filename))
            {
                using var tmp = File.Create(filename);
            }

            return Task.CompletedTask;  
        }

        public Task<bool> ServiceAccountExistsAsync(string name, CancellationToken cancellationToken = default)
        {
            var filename = $"sa_{name}";
            if (File.Exists(filename))
            {
                return Task.FromResult(new FileInfo(filename).CreationTimeUtc < DateTime.UtcNow.AddMinutes(-2));
            }

            return Task.FromResult(false);
        }
    }
}
