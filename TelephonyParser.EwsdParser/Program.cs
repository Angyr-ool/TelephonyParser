using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using TelephonyParser.EwsdModel.BusinessLogic.ExternalResourceServices;
using TelephonyParser.EwsdModel.BusinessLogic.ParseFileLogics;
using TelephonyParser.EwsdModel.BusinessLogic.ProcessFilesLogics;
using TelephonyParser.EwsdModel.DateTimeServices;
using TelephonyParser.EwsdModel.Settings;
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
    builder.RegisterType<ProcessEwsdFilesLogic>().As<IProcessEwsdFilesLogic>().SingleInstance();
    builder.RegisterType<ParseEwsdFileLogic>().As<IParseEwsdFileLogic>().SingleInstance();
    builder.RegisterType<EwsdExternalResourceService>().As<IEwsdExternalResourceService>().SingleInstance();
    //builder.RegisterType<EwsdSettings>().As<IEwsdSettings>().SingleInstance();

    // подключение файла appsettings
    builder.Register(
            _ => configurationRoot.GetSection(nameof(EwsdSettings)).Get<EwsdSettings>()!)
        .SingleInstance();
});

var appHost = hostAppBuilder.Build();

await appHost.RunAsync();