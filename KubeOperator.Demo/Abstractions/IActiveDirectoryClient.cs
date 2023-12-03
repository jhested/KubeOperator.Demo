namespace KubeOperator.Demo
{
    public interface IActiveDirectoryClient
    {
        Task<bool> ServiceAccountExistsAsync(string name, CancellationToken cancellationToken = default);
        Task RequestServiceAccountAsync(string name, IEnumerable<string> membership, CancellationToken cancellationToken = default); 
    }
}
