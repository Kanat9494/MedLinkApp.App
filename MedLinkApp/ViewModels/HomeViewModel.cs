namespace MedLinkApp.ViewModels;

internal class HomeViewModel : BaseViewModel
{
    public HomeViewModel()
    {
        IsBusy = true;
        Categories = new ObservableCollection<Category>();
        Doctors = new ObservableCollection<Doctor>();
        DoctorTapped = new Command<int>(OnDoctorSelected);

        RefreshPageCommand = new Command(() =>
        {
            Task.Run(async () =>
            {
                await GetAllDoctors();
            }).GetAwaiter().OnCompleted(() =>
            {
                IsRefreshing = false;
            });
        });

        Task.Run(async () =>
        {
            accessToken = await SecureStorage.Default.GetAsync("UserAccessToken");

            await LoadCategories();
            await GetAllDoctors();
        }).GetAwaiter().OnCompleted(() =>
        {
            IsBusy = false;
        });
    }

    string accessToken;


    public ICommand RefreshPageCommand { get; }


    
    private bool _isRefreshing;
    public bool IsRefreshing
    {
        get => _isRefreshing;
        set => SetProperty(ref _isRefreshing, value);
    }


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
            var response = await ContentService.Instance(accessToken).GetItemsAsync<Doctor>("api/Doctors/GetAllDoctors");

            if (response != null )
            {
                Doctors.Clear();
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
}
