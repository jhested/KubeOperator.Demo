using KubeOps.Operator;

namespace KubeOperator.Demo
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddKubernetesOperator();

            services.AddSingleton<IActiveDirectoryClient, ActiveDirectoryClient>();
            services.AddSingleton<IMicrosoftEntraClient, MicrosoftEntraClient>();

            services.AddTransient<ServiceAccountReconciler>();
        }

        public void Configure(IApplicationBuilder app)
        {
            // fire up the mappings for the operator
            // this is technically not needed, but if you don't call this
            // function, the healthchecks and mappings are not
            // mapped to endpoints (therefore not callable)
            app.UseKubernetesOperator();
        }
    }
}
