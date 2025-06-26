namespace PatientService;

public static class PatientAddEndpoint{
    public static void PatientAdd(this IEndpointRouteBuilder app) {

        // Token
        app.MapPost("/patient/add", 
                [AllowAnonymous] async(IMediator mediator, 
                [FromBody] PatientAddRequestDto newPatient,  
                CancellationToken cancellationToken = default ) => 
            {

            return Results.Ok(await mediator.Send(new PatientAddCommand(newPatient), cancellationToken));
        })
        .Produces<PatientAddResponseDto>(StatusCodes.Status200OK)
        .WithTags("Patient")
        .WithSummary("Save a new Patient")
        .WithOpenApi();

    }
}
