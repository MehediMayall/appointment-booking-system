namespace ClinicService;

public static class DoctorAddEndpoint{
    public static void DoctorAdd(this IEndpointRouteBuilder app) {

        // Token
        app.MapPost("/doctor/add", 
                [AllowAnonymous] async(IMediator mediator, 
                [FromBody] DoctorAddRequestDto newDoctor,  
                CancellationToken cancellationToken = default ) => 
            {

            
            return Results.Ok(await mediator.Send(new DoctorAddCommand(newDoctor), cancellationToken));
        })
        .Produces<DoctorAddResponseDto>(StatusCodes.Status200OK)
        .WithTags("Doctor")
        .WithSummary("Save a new Doctor")
        .WithOpenApi();

    }
}
