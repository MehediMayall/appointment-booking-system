namespace PatientService;

using System;
using System.Collections.Generic;

public class MedicalRecord
{
    public DateTime RecordDate { get; set; }
    public string RecordType { get; set; }
    public string Description { get; set; }
    public string DoctorId { get; set; }
    public List<string> AttachmentUrls { get; set; } = new List<string>();
}