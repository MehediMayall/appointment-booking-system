namespace SharedKernel;

public sealed record JWTSettings {
    public string Key {get;init;}
    public string Issuer {get;init;}
    public string Audience {get;init;}
    public int AccessExpirationInMinute {get;init;}
    public int RefreshExpirationInMinute {get;init;}
}