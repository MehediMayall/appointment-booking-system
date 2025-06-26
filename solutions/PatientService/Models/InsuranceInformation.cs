namespace PatientService;

using System;

public class InsuranceInformation
{
    public string Provider { get; set; }

    public string PolicyNumber { get; set; }

    public string GroupNumber { get; set; }
    public DateTime? ExpirationDate { get; set; }
}
