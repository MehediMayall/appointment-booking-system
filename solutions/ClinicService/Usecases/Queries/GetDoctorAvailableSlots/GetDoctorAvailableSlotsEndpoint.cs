namespace ClinicService;


public static class GetDoctorAvailableSlotsEndpoint{

    public static void GetDoctorAvailableSlots(this IEndpointRouteBuilder app) {
     
        app.MapGet("/available/slots/{clinicId}/{specialization}", [AllowAnonymous] async(
              [FromRoute] Guid clinicId,
              [FromRoute] string specialization,
              IMediator mediator, CancellationToken cancellationToken = default ) =>
            {

            var requestDto = new GetDoctorAvailableSlotsRequestDto() { ClinicId = clinicId, Specialization = specialization };
            
            return Results.Ok(await mediator.Send(new GetDoctorAvailableSlotsQuery(requestDto), cancellationToken));
        })
        .Produces<Response<GetDoctorAvailableSlotsResponseDto>>(StatusCodes.Status200OK)
        .WithTags("Available Slots")
        .WithSummary("Get specific clinic and specialization available slots")
        .WithOpenApi();

    }

}