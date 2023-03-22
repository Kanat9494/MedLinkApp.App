namespace MedLinkApp.ViewModels;

internal class LoginViewModel : INotifyPropertyChanged
{
    public LoginViewModel()
    {
        IsLoading = false;
        LoginCommand = new Command(async () => await OnLogin());
    }

    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged();
        }
    }

    private string _userName;
    public string UserName
    {
        get => _userName;
        set
        {
            _userName = value;
            OnPropertyChanged();
        }
    }

    private string _password;
    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
        }
    }

    public Command LoginCommand { get; }

    private async Task OnLogin()
    {
        IsLoading = true;
        if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
        {
            await Shell.Current.DisplayAlert("Пустые значения",
                "Пожалуйста введите логин и пароль для входа!", "Ок");
            IsLoading = false;
        }
        else
        {
            CurrentUser = new AuthenticateResponse()
            {
                StatusCode = 200
            };

            await Task.Delay(3000);

            if (CurrentUser.StatusCode == 200)
            {
                await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            }
            else
            {
                await Shell.Current.DisplayAlert("Не удалось войти в систему",
                    "У вас не получилось", "Ок");
                IsLoading = false;
            }
        }
    }

    private AuthenticateResponse _currentUser;
    public AuthenticateResponse CurrentUser
    {
        get => _currentUser;
        set
        {
            _currentUser = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
