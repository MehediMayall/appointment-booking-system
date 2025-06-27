using Appointment_Contracts;
using MassTransit;

namespace ClinicService;


// This Consumer is to Update available slots for every appointment booked
public sealed class AppointmentBookedConsumer(IGetDoctorAvailableSlotsRepository _repo, IHybridCacheService _hybridCache) : IConsumer<AppointmentBooked>
{

    // Step 1: Update Slot as booked
    // Step 2: Get all available lots
    // Step 3: Save to HybridCache
    public async Task Consume(ConsumeContext<AppointmentBooked> context)
    {
        try
        {
            AppointmentBooked appointment = context.Message;

            // Update Slot as booked
            await _repo.UpdateSlot(appointment.SlotId);

            // Get all available lots
            var availableSlots = await _repo.GetAvailableSlots();

            // Save to HybridCache
            await _hybridCache.SetAsync(
                RedisKeys.GetAvailableSlotsKey(),
                availableSlots.Where(t=>t.Id != appointment.SlotId)
            );



        }
        catch (Exception ex)
        {
            Log.Error(ex.GetAllExceptions());
        }

    }
}