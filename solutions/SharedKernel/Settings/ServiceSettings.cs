namespace SharedKernel;

public sealed record ServiceSettings()
{
    public string ServiceName {get;init;} 
    public string ServiceVersion {get;init;}
    public string ServiceEnvironment {get;init;} 
    public string UI_URL {get;init;}
    public string GP_SSO_URL {get;init;}
}