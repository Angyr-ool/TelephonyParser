using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace TelephonyParser.EwsdParser.Infrastructure;

public class EwsdServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
{
    public ContainerBuilder CreateBuilder(IServiceCollection services)
    {
        services.AddLogging();
        var builder = new ContainerBuilder();
        builder.Populate(services);
        return builder;
    }

    public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
    {
        var container = containerBuilder.Build();
        return new AutofacServiceProvider(container);
    }
}
