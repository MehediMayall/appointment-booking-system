namespace ClinicService;

public static class DoctorSlotAddEndpoint{
    public static void DoctorSlotAdd(this IEndpointRouteBuilder app) {

        // Token
        app.MapPost("/doctor/slot/add", 
                [AllowAnonymous] async(IMediator mediator, 
                [FromBody] DoctorSlotAddRequestDto newDoctorSlot,  
                CancellationToken cancellationToken = default ) => 
            {

            
            return Results.Ok(await mediator.Send(new DoctorSlotAddCommand(newDoctorSlot), cancellationToken));
        })
        .Produces<DoctorSlotAddResponseDto>(StatusCodes.Status200OK)
        .WithTags("Slot")
        .WithSummary("Save a new DoctorSlot")
        .WithOpenApi();

    }
}
