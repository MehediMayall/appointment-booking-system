namespace ClinicService;

public static class ClinicAddEndpoint{
    public static void ClinicAdd(this IEndpointRouteBuilder app) {

        // Token
        app.MapPost("/clinic/add", 
                [AllowAnonymous] async(IMediator mediator, 
                [FromBody] ClinicAddRequestDto newClinic,  
                CancellationToken cancellationToken = default ) => 
            {

            Log.Information($"New Clinic:  {newClinic.Name}");
            return Results.Ok(await mediator.Send(new ClinicAddCommand(newClinic), cancellationToken));
        })
        .Produces<ClinicAddResponseDto>(StatusCodes.Status200OK)
        .WithTags("Clinic")
        .WithSummary("Save a new Clinic")
        .WithOpenApi();

    }
}
