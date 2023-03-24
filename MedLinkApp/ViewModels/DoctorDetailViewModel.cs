using System.Web;

namespace MedLinkApp.ViewModels;

//[QueryProperty(nameof(DoctorId), nameof(DoctorId))]
public class DoctorDetailViewModel : IQueryAttributable, INotifyPropertyChanged
{
    public DoctorDetailViewModel()
    {
        Doctor = new DoctorInfo();
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

    async Task GetDoctorInfo()
    {
        var response = await ContentService.Instance().GetDoctorInfo(DoctorId);
        if (response.StatusCode == 200)
        {
            Doctor = response;
        }
        else
        {
            await Shell.Current.DisplayAlert("Информация о докторе", response.ResponseMessage, "Ок");
        }
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
