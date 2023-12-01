using KubeOps.Operator;
using Microsoft.AspNetCore.Hosting;

namespace KubeOperator.Demo;

public static class Program
{
    public static Task<int> Main(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
           .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
           .Build()
           .RunOperatorAsync(args);
    }
}