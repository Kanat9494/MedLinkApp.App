﻿namespace MedLinkApp.ViewModels;

internal class LoginViewModel : BaseViewModel
{
    public LoginViewModel()
    {
        IsLoading = false;
        LoginCommand = new Command(async () => await OnLogin());

        UserName = "test";
        Password = "1234";
    }

    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    private string _userName;
    public string UserName
    {
        get => _userName;
        set => SetProperty(ref _userName, value);
    }

    private string _password;
    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value );
    }

    private AuthenticateResponse _currentUser;
    public AuthenticateResponse CurrentUser
    {
        get => _currentUser;
        set => SetProperty(ref _currentUser, value);
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
}
