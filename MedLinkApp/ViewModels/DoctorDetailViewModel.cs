namespace MedLinkApp.ViewModels;

[QueryProperty(nameof(DoctorId), nameof(DoctorId))]
public class DoctorDetailViewModel : INotifyPropertyChanged
{
    public DoctorDetailViewModel()
    {

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

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
