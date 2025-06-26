namespace  GPlay.Auth.Services;

public sealed class RedisKeys {

    public static string GetPatientMobileKey(string Mobile) => $"patient:{Mobile}";
    public static string GetNewPatientKey(string Mobile) => $"newpatient:{Mobile}";
    

}