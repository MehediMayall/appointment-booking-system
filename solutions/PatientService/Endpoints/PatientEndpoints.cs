namespace PatientService;

public static class PatientEndpoints{
    public static void AddPatientEndpoints(this IEndpointRouteBuilder app)
    {

        // Add Patient
        app.PatientAdd();

        
    }

    

   
}
