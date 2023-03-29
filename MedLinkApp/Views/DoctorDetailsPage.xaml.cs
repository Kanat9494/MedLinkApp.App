namespace MedLinkApp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class DoctorDetailsPage : ContentPage
{
    public DoctorDetailsPage()
    {
        InitializeComponent();

        this.BindingContext = new DoctorDetailsViewModel();
    }
}
