namespace MedLinkApp.ViewModels;

class DoctorDetailsViewModel : BaseViewModel, IQueryAttributable
{
    public DoctorDetailsViewModel()
    {
        Doctor = new DoctorInfo();
        Consultation = new Command(OnConsultation);
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

    public Command Consultation { get; set; }

    async Task GetDoctorInfo()
    {
        //var response = await ContentService.Instance().GetDoctorInfo(DoctorId);
        var response = await ContentService.Instance().GetItemAsync<DoctorInfo, int>($"api/Doctors/GetDoctor/{DoctorId}");
        await SecureStorage.Default.SetAsync("DoctorId", DoctorId.ToString());

        if (response.StatusCode == 200)
            Doctor = response;
    }

    private async void OnConsultation()
    {
        //double userBalance = double.Parse(await SecureStorage.Default.GetAsync("UserBalance"));

        var page = new DoctorDetailsPage();
        //page.Show(new );
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
