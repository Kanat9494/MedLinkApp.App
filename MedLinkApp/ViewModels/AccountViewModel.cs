namespace MedLinkApp.ViewModels;

internal class AccountViewModel : BaseViewModel
{
    public AccountViewModel()
    {
        IsLoading = true;
        RefreshAccountInfo = new Command(() =>
        {
            Task.Run(async () =>
            {
                await InitializeUser();
            }).GetAwaiter().OnCompleted(() =>
            {
                IsRefreshing = false;
            });
        });

        Task.Run(async () =>
        {
            _accessToken = await SecureStorage.Default.GetAsync("UserAccessToken");
            _userId = await SecureStorage.Default.GetAsync("UserId");

            await InitializeUser();
        });
    }

    private bool _isRefreshing;
    public bool IsRefreshing
    {
        get => _isRefreshing;
        set => SetProperty(ref _isRefreshing, value);
    }
    public Command RefreshAccountInfo { get; }
    private AuthenticateResponse _currentUser;
    public AuthenticateResponse CurrentUser
    {
        get => _currentUser;
        set => SetProperty(ref _currentUser, value);
    }
    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    string _accessToken;
    string _userId;

    public async Task InitializeUser()
    {
        try
        {
            var response = await ContentService.Instance(_accessToken).GetItemAsync<AuthenticateResponse>($"api/User/GetUser?userId={_userId}");

            if (response.StatusCode == 200)
                CurrentUser = response;

            IsLoading = false;
        }
        catch
        {
            IsLoading = false;
        }
    }
}
