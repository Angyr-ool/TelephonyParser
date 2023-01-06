using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using TelephonyParser.EwsdParser.BusinessLogic;
using TelephonyParser.EwsdParser.BusinessLogic.FileParseLogics;
using TelephonyParser.EwsdParser.BusinessLogic.FilesProcessLogics;
using TelephonyParser.EwsdParser.Infrastructure;

var environmentVariableName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

if (string.IsNullOrEmpty(environmentVariableName))
{
    Console.WriteLine("DOTNET_ENVIRONMENT is empty");
    return;
}

var configurationBuilder = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.{environmentVariableName}.json", optional: false, reloadOnChange: false);

var configurationRoot = configurationBuilder.Build();

var ewsdServiceProviderFactory = new EwsdServiceProviderFactory();

var hostAppBuilder = Host.CreateApplicationBuilder(args);

hostAppBuilder.ConfigureContainer(ewsdServiceProviderFactory, builder =>
{
    builder.RegisterType<EwsdParserHostedService>().As<IHostedService>().SingleInstance();
    builder.RegisterType<DateTimeContext>().As<IDateTimeContext>().SingleInstance();
    builder.RegisterType<EwsdFilesProcessLogic>().As<IEwsdFilesProcessLogic>().SingleInstance();
    builder.RegisterType<EwsdFileParsingLogic>().As<IEwsdFileParsingLogic>().SingleInstance();
    builder.Register(
            _ => configurationRoot.GetSection(nameof(EwsdSettings)).Get<EwsdSettings>()!)
        .SingleInstance();
});

var appHost = hostAppBuilder.Build();

await appHost.RunAsync();