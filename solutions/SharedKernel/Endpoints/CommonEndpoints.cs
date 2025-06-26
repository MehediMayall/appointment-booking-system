using System.Runtime.InteropServices;

namespace SharedKernel;

public static class CommonEndpoints{
    public static void AddCommonEndpoints(
        this WebApplication app, IConfiguration configuration) {

        ServiceSettings serviceSettings = configuration.GetSection("ServiceSettings").Get<ServiceSettings>();
        LogAndThrowException.IfNull(serviceSettings, $"ServiceSettings not found in appsettings.json");


        string welcome = $"Welcome to {serviceSettings.ServiceName} Service. Version: {serviceSettings.ServiceVersion}. Environment: {serviceSettings.ServiceEnvironment}. Current System date & time:{DateTime.Now}. Machine Name:  {Environment.MachineName}";


        app.MapGet("/welcome", async() => { return Results.Ok(welcome); })
        .Produces(StatusCodes.Status200OK).WithTags("Others").WithOpenApi();

        app.MapGet("/", async() => { return Results.Ok(welcome); })
        .Produces(StatusCodes.Status200OK).WithTags("Others").WithOpenApi();
    }

    private static string GetEnvironmentDetails() {
        return $@"
            Machine Name: {Environment.MachineName}
            OS: {RuntimeInformation.OSDescription}
            CPU: {RuntimeInformation.ProcessArchitecture}
            .NET Core Version: {RuntimeInformation.FrameworkDescription}
            CLR Version: {RuntimeInformation.FrameworkDescription}
            OS Platform: {RuntimeInformation.OSDescription}
            OS Version: {Environment.OSVersion.VersionString}
            OS Architecture: {RuntimeInformation.OSArchitecture}
            OS Description: {RuntimeInformation.OSDescription}
            User Name: {Environment.UserName}
            User Domain: {Environment.UserDomainName}
            Working Directory: {Environment.CurrentDirectory}
            Process Id: {Environment.ProcessId}
            Process Path: {Environment.ProcessPath}
            CPU Usage: {Environment.CpuUsage}
            Processor Count: {Environment.ProcessorCount}
        ";
    }
}