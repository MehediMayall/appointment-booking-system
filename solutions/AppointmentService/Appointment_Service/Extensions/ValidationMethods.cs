using System.Text.RegularExpressions;
 
namespace AppointmentService;

public static class ValidationMethods 
{
    public static bool BeAValidGuid(Guid guid) {
        if (string.IsNullOrEmpty(guid.ToString()))
            return false;

        // Regex to match a valid Guid format
        var regex = new Regex(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$");
        return regex.IsMatch(guid.ToString());
    }
}