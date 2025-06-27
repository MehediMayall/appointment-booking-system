namespace ClinicService;


public static class IsSpecificSlotAvailableEndpoint{

    public static void IsSpecificSlotAvailable(this IEndpointRouteBuilder app) {
     
        app.MapGet("/slot/available/{slotid}", [AllowAnonymous] async(
              [FromRoute] Guid slotid,
              IMediator mediator, CancellationToken cancellationToken = default ) =>
            {

            return Results.Ok(await mediator.Send(new IsSpecificSlotAvailableQuery(slotid), cancellationToken));
        })
        .Produces<Response<IsSpecificSlotAvailableResponseDto>>(StatusCodes.Status200OK)
        .WithTags("Available Slots")
        .WithSummary("Get all available slots")
        .WithOpenApi();

    }

}