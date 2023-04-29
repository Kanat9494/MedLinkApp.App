namespace MedLinkApp.Models.DTOs.Responses;

internal class PasswordReset
{
    public bool Success { get; set; }
    public int OneTimeCode { get; set; }
}
