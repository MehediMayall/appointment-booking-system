namespace AppointmentService;

public static class AppointmentConfirmEndpoint{
    public static void AppointmentConfirm(this IEndpointRouteBuilder app) {

        // Token
        app.MapPost("/confirm", 
                [AllowAnonymous] async(IMediator mediator, 
                [FromBody] AppointmentConfirmRequestDto newAppointment,  
                CancellationToken cancellationToken = default ) => 
            {
            return Results.Ok(await mediator.Send(new AppointmentConfirmCommand(newAppointment), cancellationToken));
        })
        .Produces<AppointmentConfirmResponseDto>(StatusCodes.Status200OK)
        .WithTags("Appointment")
        .WithSummary("Save a new Appointment")
        .WithOpenApi();

    }
}
