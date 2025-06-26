namespace AppointmentService;

public static class AppointmentEndpoints{
    public static void AddAppoinmentEndpoints(this IEndpointRouteBuilder app)
    {

        // Add
        app.AppointmentConfirm();
        
    }

    

   
}
