// namespace PatientService;

// public interface IPatientAddRepository 
// {
//     Task<Boolean> HasPatientWithMobile(string Mobile);
// }

// public sealed class PatientAddRepository(IMongoRepository<Patient> _repo) : IPatientAddRepository
// {

//     public async Task<Boolean> HasPatientWithMobile(string Mobile) =>
//         await _repo.Find(x => x.ContactInfo.Phone == Mobile).AnyAsync();

// }