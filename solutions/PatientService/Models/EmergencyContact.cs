namespace PatientService;

public class EmergencyContact
{
    public string Name { get; set; }

    public string Relationship { get; set; }

    public string Phone { get; set; }

    public int Priority { get; set; } = 1;
}
