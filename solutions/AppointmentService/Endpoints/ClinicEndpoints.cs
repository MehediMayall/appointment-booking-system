namespace AppointmentService;

public static class AppointmentEndpoints{
    public static void AddAppointmentEndpoints(this IEndpointRouteBuilder app)
    {

        // Add Appointment
        app.AppointmentBook();

        
    }

    

   
}
