
namespace MedLinkApp.ViewModels;

public class HomeViewModel : INotifyPropertyChanged
{
    public HomeViewModel()
    {
        IsLoading = true;
        Categories = new ObservableCollection<Category>();

        Task.Run(async () =>
        {
            await LoadCategories();
        }).GetAwaiter().OnCompleted(() =>
        {
            IsLoading = false;
        });
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

    private async Task LoadCategories()
    {
        try
        {
            var response = await ContentService.Instance().LoadCategories();

            if (response != null)
            {
                //Categories = new ObservableCollection<Category>(response);
                foreach (var category in response)
                {
                    Categories.Add(category);
                }
            }
        }
        catch(Exception ex) 
        {

        }
    }

    public ObservableCollection<Category> Categories { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
