namespace ClinicService;


public static class SearchAvailableSlotsEndpoint{

    public static void SearchAvailableSlots(this IEndpointRouteBuilder app) {
     
        app.MapGet("/search/available/slots/{searchText}", [AllowAnonymous] async([FromRoute] string searchText,  IMediator mediator, CancellationToken cancellationToken = default ) => {

            return Results.Ok(await mediator.Send(
                new SearchAvailableSlotsQuery(
                    new SearchAvailableSlotsRequestDto() { SearchText = searchText }
            ), cancellationToken));
        })
        .Produces<Response<SearchAvailableSlotsResponseDto>>(StatusCodes.Status200OK)
        .WithTags("Available Slots")
        .WithSummary("Search available slots by specialization, first name, last name, clinic name")
        .WithOpenApi();

    }

}