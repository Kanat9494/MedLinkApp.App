﻿namespace MedLinkApp.ViewModels;

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
            CurrentUser = await LoginService.GetInstance().AuthenticateUser(userName: UserName, password: Password);

            //await Task.Delay(3000);

            if (CurrentUser.StatusCode == 200)
            {
                await SecureStorage.Default.SetAsync("UserAccessToken", CurrentUser.AccessToken);
                await SecureStorage.Default.SetAsync("UserId", CurrentUser.UserId.ToString());
                await SecureStorage.Default.SetAsync("UserBalance", CurrentUser.UserBalance.ToString());

                await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            }
            else
            {
                await Shell.Current.DisplayAlert("Не удалось войти в систему",
                    $"{CurrentUser.ResponseMessage}", "Ок");
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
