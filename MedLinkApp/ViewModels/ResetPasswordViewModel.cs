namespace MedLinkApp.ViewModels;

internal class ResetPasswordViewModel : BaseViewModel
{
    public ResetPasswordViewModel()
    {
        IsReset = false;
        IsBusy = false;
        oneTimeCode = 0;

        SendCommand = new Command(OnSend);
        CheckCommand = new Command(OnCheck);
    }

    int oneTimeCode;

    public Command SendCommand { get; }
    public Command CheckCommand { get; }

    private string _userName;
    public string UserName
    {
        get => _userName;
        set => SetProperty(ref _userName, value);
    }
    private bool _isReset;
    public bool IsReset
    {
        get => _isReset;
        set => SetProperty(ref _isReset, value);
    }
    private string _checkOneTimeCode;
    public string CheckOneTimeCode
    {
        get => _checkOneTimeCode;
        set => SetProperty(ref _checkOneTimeCode, value);
    }
    private string _passwordNew;
    public string PasswordNew
    {
        get => _passwordNew;
        set => SetProperty(ref _passwordNew, value);
    }
    private bool _isBusy;
    public bool IsBusy 
    { 
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    private async void OnSend()
    {
        IsBusy = true;
        var isSuccess = await ContentService.Instance("").GetItemDataAsync<PasswordReset>($"api/Authentication/ResetPassword?userName={UserName}&isUser=true");

        if (!isSuccess.Success)
        {
            await Shell.Current.DisplayAlert("Ошибка", "Произошла неизвестная ошибка. Возможно при регистрации вы не указали email." +
                "Если вам не удается решить вашу проблему, пожалуйста, обратитесь в службу поддержки - 996708362166", "Ок");
            return;
        }

        oneTimeCode = isSuccess.OneTimeCode;

        await Shell.Current.DisplayAlert("Отлично", "На вашу почту отправлено" +
                "сообщение с кодом, для сброса пароля", "Ок");
        IsReset = true;
        IsBusy = false;
    }

    private async void OnCheck()
    {
        if (string.IsNullOrEmpty(CheckOneTimeCode))
        {
            await Shell.Current.DisplayAlert("Пустое значение", "Введите одноразовый код", "Ок");
            return;
        }

        if (int.Parse(CheckOneTimeCode) != oneTimeCode)
        {
            await Shell.Current.DisplayAlert("Ошибка", "Введенный вами одноразовый код не совпадает, с кодом, отправленным вам", "Ок");

            return;
        }

        if (string.IsNullOrEmpty(PasswordNew))
        {
            await Shell.Current.DisplayAlert("Ошибка", "Пароль не может быть пустым", "Ок");

            return;
        }

        var userRP = new UserResetPassword
        {
            UserName = UserName,
            PasswordNew = PasswordNew,
            IsUser = true
        };
        await ContentService.Instance("").PutItemAsync(userRP, "api/Authentication/UpdatePassword");

        await Shell.Current.DisplayAlert("Отлично", "Ваш пароль успешно изменен на новый", "Ок");

        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }
}
