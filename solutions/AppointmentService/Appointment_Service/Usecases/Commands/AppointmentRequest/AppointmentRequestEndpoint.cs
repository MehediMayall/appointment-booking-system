namespace AppointmentService;

public static class AppointmentRequestEndpoint{
    public static void AppointmentRequest(this IEndpointRouteBuilder app) {

        // Token
        app.MapPost("/request", 
                [AllowAnonymous] async(IMediator mediator, 
                [FromBody] AppointmentRequestRequestDto newAppointment,  
                CancellationToken cancellationToken = default ) => 
            {
            return Results.Ok(await mediator.Send(new AppointmentRequestCommand(newAppointment), cancellationToken));
        })
        .Produces<AppointmentRequestResponseDto>(StatusCodes.Status200OK)
        .WithTags("Appointment")
        .WithSummary("Request for an Appointment")
        .WithOpenApi();

    }
}
