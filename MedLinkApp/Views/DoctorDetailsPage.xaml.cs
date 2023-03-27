namespace MedLinkApp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class DoctorDetailsPage : ContentPage
{
    public DoctorDetailsPage()
    {
        InitializeComponent();

        this.BindingContext = new DoctorDetailsViewModel();
    }

    public async void OnConsultation_Clicked(object sender, EventArgs e)
    {
        this.ShowPopup(new DoctorDetailsPopup());
    }
}
