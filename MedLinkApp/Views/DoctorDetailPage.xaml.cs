namespace MedLinkApp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class DoctorDetailPage : ContentPage
{
    public DoctorDetailPage()
    {
        InitializeComponent();

        this.BindingContext = new DoctorDetailViewModel();
    }
}
