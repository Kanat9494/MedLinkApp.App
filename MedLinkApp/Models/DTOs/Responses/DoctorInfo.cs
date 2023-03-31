namespace MedLinkApp.Models.DTOs.Responses;

public class DoctorInfo : BaseResponse
{
    public int DoctorId { get; set; }
    public string FullName { get; set; }
    public string AccountName { get; set; }
    public int WorkExperience { get; set; }
    public string AboutDoctor { get; set; }
    public string ProfileImg { get; set; }
    public string IsBusy { get; set; }
    public string IsOnline { get; set; }
}
