namespace ClinicService;

public static class ClinicEndpoints{
    public static void AddClinicEndpoints(this IEndpointRouteBuilder app)
    {

        // Add Clinic
        app.ClinicAdd();

        
    }

    

   
}
