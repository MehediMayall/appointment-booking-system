namespace ClinicService;


public interface IGetDoctorAvailableSlotsRepository
{
    Task<IEnumerable<DoctorAvailableSlotsDto>> GetDoctorAvailableSlots(Guid ClinicId, string Specialization);
    Task<IEnumerable<DoctorAvailableSlotsDto>> GetClinicDoctorAvailableSlots(string Specialization);
    Task<IEnumerable<AvailableSlotsDto>> GetAvailableSlots();

    Task<bool> IsSpecificSlotAvailable(Guid SlotId);

    Task UpdateSlot(Guid SlotId);

}

public sealed class GetDoctorAvailableSlotsRepository : IGetDoctorAvailableSlotsRepository
{

    private readonly IClinicDbConnection _db;

    public GetDoctorAvailableSlotsRepository(IClinicDbConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<AvailableSlotsDto>> GetAvailableSlots() =>
        await _db.Connection.QueryWithLoggingAsync<AvailableSlotsDto>(
        @"SELECT s.id, c.name as ClinicName, c.address, c.phone_number as PhoneNumber, s.clinic_id as ClinicId, s.doctor_id as DoctorId, 
            d.first_name as FirstName, d.last_name as LastName, d.specialization, 
            s.start_time as StartTime, s.end_time as EndTime 
            FROM public.slots s
                LEFT JOIN public.clinics c on s.clinic_id = c.id
                LEFT JOIN public.doctors d on s.doctor_id = d.id
            WHERE  s.is_booked = false AND 
                s.is_active = true
            LIMIT 10000;
        ");

    public async Task<IEnumerable<DoctorAvailableSlotsDto>> GetClinicDoctorAvailableSlots(string Specialization) =>
        await _db.Connection.QueryWithLoggingAsync<DoctorAvailableSlotsDto>(
            @"SELECT s.id, c.name as ClinicName, c.address, c.phone_number as PhoneNumber, s.clinic_id as ClinicId, s.doctor_id as DoctorId, 
            d.first_name as FirstName, d.last_name as LastName, d.specialization, 
            s.start_time as StartTime, s.end_time as EndTime 
            FROM public.slots s
                LEFT JOIN public.clinics c on s.clinic_id = c.id
                LEFT JOIN public.doctors d on s.doctor_id = d.id
            WHERE LOWER(d.specialization) = @Specialization AND
                s.is_booked = false AND 
                s.is_active = true
            LIMIT 100;
        ", new { Specialization }
        );




    public async Task<IEnumerable<DoctorAvailableSlotsDto>> GetDoctorAvailableSlots(Guid ClinicId, string Specialization) =>
        await _db.Connection.QueryWithLoggingAsync<DoctorAvailableSlotsDto>(
            @"SELECT s.id, s.clinic_id as ClinicId, s.doctor_id as DoctorId, 
            d.first_name as FirstName, d.last_name as LastName, d.specialization, 
            s.start_time as StartTime, s.end_time as EndTime 
            FROM public.slots s
                LEFT JOIN public.clinics c on s.clinic_id = c.id
                LEFT JOIN public.doctors d on s.doctor_id = d.id
            WHERE 
                s.clinic_id = @ClinicId AND
                LOWER(d.specialization) = @Specialization AND
                s.is_booked = false AND 
                s.is_active = true
            LIMIT 100;
        ", new { ClinicId, Specialization }
        );


    public async Task<bool> IsSpecificSlotAvailable(Guid slotId)
    {
        const string query = @"
                SELECT EXISTS (
                    SELECT 1
                    FROM public.slots s
                    WHERE 
                        s.id = @slotId AND
                        s.is_booked = false AND 
                        s.is_active = true
                );";

        return await _db.Connection.QueryFirstOrDefaultAsync<bool>(query, new { slotId });
    }
        
    public async Task UpdateSlot(Guid SlotId) =>
        await _db.Connection.ExecuteAsync(
            @"Update public.slots 
                SET is_booked = true
                WHERE id = @SlotId;
        ", new { SlotId }
        );
         

}
