using System.Runtime.InteropServices;

namespace SharedKernel;

public static class StartupMessage{
    public static void SetStartupMessage(IConfiguration configuration) {
        
        ServiceSettings serviceSettings = configuration.GetSection("ServiceSettings").Get<ServiceSettings>() 
            ?? new()
            { 
                ServiceName = Assembly.GetEntryAssembly().GetName().Name, 
                ServiceVersion = Assembly.GetEntryAssembly().GetName().Version.ToString() ,
            };
        
        // Startup Message
        var startupMessage = $"{serviceSettings.ServiceName} has been started. App Version: {serviceSettings.ServiceVersion}. {GetEnvironmentDetails()}";

        Console.WriteLine($"\n\n******************************* {serviceSettings.ServiceName.ToUpper()} **********************************************");
        Log.Information(startupMessage);
        Console.WriteLine("*****************************************************************************************\n\n");
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
            Processor Count: {Environment.ProcessorCount}";
    }
}