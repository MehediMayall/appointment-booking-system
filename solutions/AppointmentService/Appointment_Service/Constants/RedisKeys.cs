namespace  AppointmentService;

public sealed class RedisKeys
{

    public static string GetNewAppointmentKey(Guid AppointmentId) => $"new-appointment:{AppointmentId}";
    public static string GetRecentlyAppointedKey(Guid SlotId) => $"recently-appointment:{SlotId}";
    public static string GetAvailableSlotsKey() => $"available_slots";
    

}