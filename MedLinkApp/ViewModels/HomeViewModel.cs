
namespace MedLinkApp.ViewModels;

public class HomeViewModel : INotifyPropertyChanged
{
    public HomeViewModel()
    {
        IsLoading = true;
        Categories = new ObservableCollection<Category>();
        Doctors = new ObservableCollection<Doctor>();
        DoctorTapped = new Command<int>(OnDoctorSelected);

        

        Task.Run(async () =>
        {
            await LoadCategories();
            await GetAllDoctors();
            accessToken = await SecureStorage.Default.GetAsync("UserAccessToken");
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

    string accessToken;

    public ObservableCollection<Doctor> Doctors { get; set; }
    public ObservableCollection<Category> Categories { get; set; }

    public Command<int> DoctorTapped { get; set; }

    private async Task LoadCategories()
    {
        try
        {
            var response = await ContentService.Instance(accessToken).LoadCategories();

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
            var response = await ContentService.Instance(accessToken).GetAllDoctors();

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

    private async void OnDoctorSelected(int doctorId)
    {
        if (doctorId == null)
            return;

        await Shell.Current.GoToAsync($"{nameof(DoctorDetailsPage)}?{nameof(DoctorDetailsViewModel.DoctorId)}={doctorId}");
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
