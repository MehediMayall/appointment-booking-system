namespace  ClinicService;

public sealed class RedisKeys {

    public static string GetBillingKey(Guid PlayerId, Guid PricingSlotId) => $"billing:{PlayerId}:{PricingSlotId}";
    

}