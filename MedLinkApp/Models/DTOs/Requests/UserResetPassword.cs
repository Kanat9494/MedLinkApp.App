namespace MedLinkApp.Models.DTOs.Requests;

internal class UserResetPassword
{
    public string UserName { get; set; }
    public string PasswordNew { get; set; }    
    public bool IsUser { get; set; }
}
