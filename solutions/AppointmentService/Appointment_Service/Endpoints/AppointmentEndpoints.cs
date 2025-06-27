namespace AppointmentService;

public static class AppointmentEndpoints{
    public static void AddAppoinmentEndpoints(this IEndpointRouteBuilder app)
    {

        // Confirm Appointment
        app.AppointmentConfirm();

        // Request Appointment
        app.AppointmentRequest();
        
    }

    

   
}
