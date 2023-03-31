namespace MedLinkApp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class DoctorDetailsPage : ContentPage
{
    public DoctorDetailsPage()
    {
        InitializeComponent();

        this.BindingContext = new DoctorDetailsViewModel();
    }

    //protected override void OnAppearing()
    //{
    //    base.OnAppearing();
    //    //Application.Current.MainPage = new DoctorDetailsPage();
    //    Shell.SetTabBarIsVisible(this, false);// set false in second page, set true in first page
    //}
}
