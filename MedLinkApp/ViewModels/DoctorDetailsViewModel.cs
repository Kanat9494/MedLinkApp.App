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

    private bool _isDoctorOnline;
    public bool IsDoctorOnline
    {
        get => _isDoctorOnline;
        set => SetProperty(ref _isDoctorOnline, value);
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
        if (!Doctor.IsOnline.Equals("В сети"))
            await Shell.Current.DisplayAlert("Не в сети", "В данный момент выбранный врач не в сети", "Ок");
        else
        {
            if (!Doctor.IsBusy.Equals("Свободен"))
                await Shell.Current.DisplayAlert("Занят", "В данный момент выбранный врач консультирует другого пациента", "Ок");
            else
            {
                await Shell.Current.GoToAsync($"{nameof(ProductsPage)}?{nameof(ProductsViewModel.DoctorId)}={DoctorId}");
            }
        }
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
