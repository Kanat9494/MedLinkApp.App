namespace MedLinkApp.ViewModels;

internal class SetupViewModel : BaseViewModel, IQueryAttributable
{
    public SetupViewModel()
    {

    }

    private int _userId;
    public int UserId
    {
        get => _userId;
        set => SetProperty(ref _userId, value);
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        UserId = int.Parse(HttpUtility.UrlDecode(query["UserId"].ToString()));
    }
}
