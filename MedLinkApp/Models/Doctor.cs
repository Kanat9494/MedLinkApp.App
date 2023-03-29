namespace MedLinkApp.Models;

public class Doctor
{
    public int DoctorId { get; set; }
    public string FullName { get; set; }
    public int WorkExperience { get; set; }
    public string ProfileImg { get; set; }
    public string IsBusy { get; set; }
    public string IsOnline { get; set; }
}
