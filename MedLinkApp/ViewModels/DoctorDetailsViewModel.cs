namespace MedLinkApp.ViewModels;

class DoctorDetailsViewModel : IQueryAttributable, INotifyPropertyChanged
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
        set
        {
            _doctorId = value;
            OnPropertyChanged();
        }
    }

    private DoctorInfo _doctor;
    public DoctorInfo Doctor
    {
        get => _doctor;
        set
        {
            _doctor = value;
            OnPropertyChanged();
        }
    }

    public Command Consultation { get; set; }

    async Task GetDoctorInfo()
    {
        //var response = await ContentService.Instance().GetDoctorInfo(DoctorId);
        var response = await ContentService.Instance().GetItemAsync<DoctorInfo, int>(DoctorId, "api/Doctors/GetDoctor/");
        if (response.StatusCode == 200)
        {
            Doctor = response;
        }
        else
        {
            await Shell.Current.DisplayAlert("Информация о докторе", response.ResponseMessage, "Ок");
        }
    }

    private async void OnConsultation()
    {
        //double userBalance = double.Parse(await SecureStorage.Default.GetAsync("UserBalance"));
        //var page = new DoctorDetailsBottomSheet();
        //page.ShowHandle = true;
        //page.Show();
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
