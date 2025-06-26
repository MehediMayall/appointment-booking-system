namespace  AppointmentService;

public sealed class RedisKeys {

    public static string GetNewAppointmentKey(Guid AppointmentId) => $"new-appointment:{AppointmentId}";
    

}