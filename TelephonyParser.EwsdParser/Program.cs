using Autofac;
using Microsoft.Extensions.Hosting;
using TelephonyParser.EwsdParser.Infrastructure;

var ewsdServiceProviderFactory = new EwsdServiceProviderFactory();

var hostAppBuilder = Host.CreateApplicationBuilder(args);

hostAppBuilder.ConfigureContainer(ewsdServiceProviderFactory, (ContainerBuilder builder) =>
{
    builder.RegisterType<EwsdParserHostedService>().As<IHostedService>().SingleInstance();
    builder.RegisterType<DateTimeContext>().As<IDateTimeContext>().SingleInstance();
});

var appHost = hostAppBuilder.Build();

await appHost.RunAsync();