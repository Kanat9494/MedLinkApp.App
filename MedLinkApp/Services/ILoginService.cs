namespace MedLinkApp.Services;

public interface ILoginService 
{
    Task<AuthenticateResponse> AuthenticateUser(string userName, string password);
}
