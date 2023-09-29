namespace MedLinkApp.ViewModels;

internal class SettingsViewModel : BaseViewModel, IQueryAttributable
{
    public SettingsViewModel()
    {

    }

    private int _userId;
    public int UserId
    {
        get => _userId;
        set => SetProperty(ref _userId, value);
    }
    private bool _isNightTheme;
    public bool IsNightTheme
    {
        get => _isNightTheme;
        set => SetProperty(ref _isNightTheme, value);
    }


    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        UserId = int.Parse(HttpUtility.UrlDecode(query["UserId"].ToString()));
    }
}
