namespace MedLinkApp.ViewModels;

class DoctorDetailsViewModel : BaseViewModel, IQueryAttributable
{
    public DoctorDetailsViewModel()
    {
        Doctor = new DoctorInfo();
        Consultation = new Command(async () => 
        {
            await OnConsultation();
        });

        Task.Run(async () =>
        {
            accessToken = await SecureStorage.Default.GetAsync("UserAccessToken");
        }).GetAwaiter();
    }

    private int _doctorId;
    public int DoctorId
    {
        get => _doctorId;
        set => SetProperty(ref _doctorId, value);
    }

    private DoctorInfo _doctor;
    public DoctorInfo Doctor
    {
        get => _doctor;
        set => SetProperty(ref _doctor, value);
    }

    string accessToken;

    public Command Consultation { get; set; }

    async Task GetDoctorInfo()
    {
        //var response = await ContentService.Instance().GetDoctorInfo(DoctorId);
        var response = await ContentService.Instance(accessToken).GetItemAsync<DoctorInfo, int>($"api/Doctors/GetDoctor/{DoctorId}");
        await SecureStorage.Default.SetAsync("DoctorId", DoctorId.ToString());

        if (response.StatusCode == 200)
            Doctor = response;
        else if (response.StatusCode == 401)
            await Shell.Current.GoToAsync($"..//{nameof(LoginPage)}");
    }

    private async Task OnConsultation()
    {
        await Shell.Current.GoToAsync($"{nameof(ProductsPage)}?{nameof(ProductsViewModel.DoctorId)}={DoctorId}");
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        DoctorId = int.Parse(HttpUtility.UrlDecode(query["DoctorId"].ToString()));

        Task.Run(async () =>
        {
            await GetDoctorInfo();
        }).GetAwaiter().OnCompleted(() =>
        {

        });
    }
}
