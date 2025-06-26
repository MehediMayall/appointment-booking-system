using Microsoft.Extensions.Caching.Hybrid;

namespace ClinicService;

public record GetDoctorAvailableSlotsQuery() : IRequest<Response<GetDoctorAvailableSlotsResponseDto>>{}
public sealed class GetDoctorAvailableSlotsQueryHandler(IGetDoctorAvailableSlotsRepository _repo) : IRequestHandler<GetDoctorAvailableSlotsQuery, Response<GetDoctorAvailableSlotsResponseDto>>
{
 


    // Step 1: Get user using email/mobile and password
    // Step 2: Generate Token from that user
    // Step 3: Return GetDoctorAvailableSlots Response
    public async Task<Response<GetDoctorAvailableSlotsResponseDto>> Handle(GetDoctorAvailableSlotsQuery request, CancellationToken cancellationToken)
    {
        

        // Step 3: Return GetDoctorAvailableSlots Response
        return GetDoctorAvailableSlotsResponseDto();
    }





}