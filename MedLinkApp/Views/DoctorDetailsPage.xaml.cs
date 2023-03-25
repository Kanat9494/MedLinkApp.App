namespace MedLinkApp.Views
{
    public partial class DoctorDetailsPage : ContentPage
    {
        public DoctorDetailsPage()
        {
            InitializeComponent();

            this.BindingContext = new DoctorDetailsViewModel();
        }

        public void OnTest(object sender, EventArgs e)
        {
            this.ShowPopup(new DoctorDetailsPopup());
        }
    }
}
