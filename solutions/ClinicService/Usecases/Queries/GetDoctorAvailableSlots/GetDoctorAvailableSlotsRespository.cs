namespace ClinicService;


public interface IGetDoctorAvailableSlotsRepository
{
    Task<IEnumerable<DoctorAvailableSlotsDto>> GetDoctorAvailableSlots(Guid ClinicId, string Specialization);
    Task<IEnumerable<DoctorAvailableSlotsDto>> GetClinicDoctorAvailableSlots(string Specialization);

}

public sealed class GetDoctorAvailableSlotsRepository : IGetDoctorAvailableSlotsRepository
{

    private readonly IClinicDbConnection _db;

    public GetDoctorAvailableSlotsRepository(IClinicDbConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<DoctorAvailableSlotsDto>> GetClinicDoctorAvailableSlots(string Specialization) =>
        await _db.Connection.QueryWithLoggingAsync<DoctorAvailableSlotsDto>(
            @"SELECT s.id, c.name, c.address, c.phone_number as PhoneNumber, s.clinic_id as ClinicId, s.doctor_id as DoctorId, 
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
        
        
    public async Task<IEnumerable<DoctorAvailableSlotsDto>> GetAvailableSlots(string Specialization) =>
        await _db.Connection.QueryWithLoggingAsync<DoctorAvailableSlotsDto>(
        @"SELECT s.id, c.name, c.address, c.phone_number as PhoneNumber, s.clinic_id as ClinicId, s.doctor_id as DoctorId, 
            d.first_name as FirstName, d.last_name as LastName, d.specialization, 
            s.start_time as StartTime, s.end_time as EndTime 
            FROM public.slots s
                LEFT JOIN public.clinics c on s.clinic_id = c.id
                LEFT JOIN public.doctors d on s.doctor_id = d.id
            WHERE  s.is_booked = false AND 
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

}
