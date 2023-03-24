
namespace MedLinkApp.ViewModels;

public class HomeViewModel : INotifyPropertyChanged
{
    public HomeViewModel()
    {
        IsLoading = true;
        Categories = new ObservableCollection<Category>();
        Doctors = new ObservableCollection<DoctorResponse>();

        Task.Run(async () =>
        {
            await LoadCategories();
            await GetAllDoctors();
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

    public ObservableCollection<DoctorResponse> Doctors { get; set; }

    private async Task LoadCategories()
    {
        try
        {
            var response = await ContentService.Instance().LoadCategories();

            if (response != null)
            {
                //Categories = new ObservableCollection<Category>(response);
                foreach (var category in response)
                    Categories.Add(category);
            }
        }
        catch(Exception ex) 
        {

        }
    }

    private async Task GetAllDoctors()
    {
        try
        {
            var response = await ContentService.Instance().GetAllDoctors();

            if (response != null )
            {
                foreach (var doctor in response)
                    Doctors.Add(doctor);
            }
        }
        catch (Exception ex)
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
