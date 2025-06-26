namespace ClinicService;


public static class GetDoctorAvailableSlotsEndpoint{
    /// <summary>
    /// Get logged user profile
    /// </summary>
    /// <returns>Firstname, lastname, email, mobile and email</returns>
    public static void GetDoctorAvailableSlots(this IEndpointRouteBuilder app) {
     
        app.MapGet("/user/profile", [Authorize] async(  IMediator mediator, CancellationToken cancellationToken = default ) => {
            return Results.Ok(await mediator.Send(new GetDoctorAvailableSlotsQuery(), cancellationToken));
        })
        .Produces<Response<GetDoctorAvailableSlotsResponseDto>>(StatusCodes.Status200OK)
        .WithTags("Profile")
        .WithSummary("Get an user account information")
        .WithOpenApi();

    }

}