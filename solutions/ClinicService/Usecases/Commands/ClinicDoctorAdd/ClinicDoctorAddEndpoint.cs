namespace ClinicService;

public static class ClinicDoctorAddEndpoint{
    public static void ClinicDoctorAdd(this IEndpointRouteBuilder app) {

        // Token
        app.MapPost("/clinic/doctor/add", 
                [AllowAnonymous] async(IMediator mediator, 
                [FromBody] ClinicDoctorAddRequestDto newClinicDoctor,  
                CancellationToken cancellationToken = default ) => 
            {

            
            return Results.Ok(await mediator.Send(new ClinicDoctorAddCommand(newClinicDoctor), cancellationToken));
        })
        .Produces<ClinicDoctorAddResponseDto>(StatusCodes.Status200OK)
        .WithTags("Clinic Doctor")
        .WithSummary("Save a new ClinicDoctor")
        .WithOpenApi();

    }
}
