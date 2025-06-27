namespace ClinicService;

public static class ClinicEndpoints{
    public static void AddClinicEndpoints(this IEndpointRouteBuilder app)
    {

        // Add Clinic
        app.ClinicAdd();


        // Get specific clinic and specialization available slots  
        app.GetDoctorAvailableSlots();

        // Search
        app.SearchAvailableSlots();

        // Get all available slots
        app.GetAvailableSlots();

        // Is specific slot available
        app.IsSpecificSlotAvailable();

        
    }

    

   
}
