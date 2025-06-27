namespace ClinicService;


public static class GetAvailableSlotsEndpoint{

    public static void GetAvailableSlots(this IEndpointRouteBuilder app) {
     
        app.MapGet("/available/slots", [AllowAnonymous] async(
              IMediator mediator, CancellationToken cancellationToken = default ) =>
            {

            return Results.Ok(await mediator.Send(new GetAvailableSlotsQuery(), cancellationToken));
        })
        .Produces<Response<GetAvailableSlotsResponseDto>>(StatusCodes.Status200OK)
        .WithTags("Available Slots")
        .WithSummary("Get all available slots")
        .WithOpenApi();

    }

}