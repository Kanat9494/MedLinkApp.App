namespace MedLinkApp.Services;

public class LoginService : ILoginService
{
    public LoginService()
    {

    }

    private static LoginService _instance;
    public static LoginService GetInstance()
    {
        if (_instance == null)
            _instance = new LoginService();

        return _instance;
    }

    public async Task<AuthenticateResponse> AuthenticateUser(string userName, string password)
    {
        var requestUser = new AuthenticationRequest()
        {
            UserName = userName,
            Password = password
        };

        using (HttpClient httpClient  = new HttpClient())
        {
            httpClient.BaseAddress = new Uri(MedLinkConstants.SERVER_ROOT_URL);

            var content = new StringContent(JsonConvert.SerializeObject(requestUser), Encoding.UTF8, "application/json");

            try
            {
                var response = await httpClient.PostAsync("api/Authentication/AuthenticateUser", content);

                var jsonResult = await response.Content.ReadAsStringAsync();

                var authenticatedUser = JsonConvert.DeserializeObject<AuthenticateResponse>(jsonResult);

                return authenticatedUser;
            }
            catch (Exception ex)
            {
                return new AuthenticateResponse
                {
                    StatusCode = 500,
                    ResponseMessage = ex.Message,
                };
            }
        }
    }
}
