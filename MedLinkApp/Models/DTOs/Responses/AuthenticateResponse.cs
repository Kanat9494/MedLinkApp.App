namespace MedLinkApp.Models.DTOs.Responses;

public class AuthenticateResponse : BaseResponse
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string UserEmail { get; set; }
    public string AccessToken { get; set; }
}
