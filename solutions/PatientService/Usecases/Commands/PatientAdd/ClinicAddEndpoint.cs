namespace PatientService;

public static class PatientAddEndpoint{
    public static void PatientAdd(this IEndpointRouteBuilder app) {

        // Token
        app.MapPost("/patient/add", 
                [AllowAnonymous] async(IMediator mediator, 
                [FromBody] PatientAddRequestDto newClinic,  
                CancellationToken cancellationToken = default ) => 
            {

            return Results.Ok(await mediator.Send(new PatientAddCommand(newClinic), cancellationToken));
        })
        .Produces<PatientAddResponseDto>(StatusCodes.Status200OK)
        .WithTags("Patient")
        .WithSummary("Save a new Patient")
        .WithOpenApi();

    }
}
