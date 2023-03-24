namespace MedLinkApp.Models.DTOs.Responses;

public class DoctorResponse
{
    public int DoctorId { get; set; }
    public string DoctorFullName { get; set; }
    public int WorkExperience { get; set; }
    public string DoctorProfileImg { get; set; }
    public string IsBusy { get; set; }
}
