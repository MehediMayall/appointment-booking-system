namespace AppointmentService;

public static class AppointmentBookEndpoint{
    public static void AppointmentBook(this IEndpointRouteBuilder app) {

        // Token
        app.MapPost("/Appointment/add", 
                [AllowAnonymous] async(IMediator mediator, 
                [FromBody] AppointmentBookRequestDto newAppointment,  
                CancellationToken cancellationToken = default ) => 
            {

            Log.Information($"New Appointment:  {newAppointment.Name}");
            return Results.Ok(await mediator.Send(new AppointmentBookCommand(newAppointment), cancellationToken));
        })
        .Produces<AppointmentBookResponseDto>(StatusCodes.Status200OK)
        .WithTags("Appointment")
        .WithSummary("Save a new Appointment")
        .WithOpenApi();

    }
}
