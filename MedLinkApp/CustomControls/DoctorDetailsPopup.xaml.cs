namespace MedLinkApp.CustomControls;

public partial class DoctorDetailsPopup : Popup
{
	public DoctorDetailsPopup()
	{
		InitializeComponent();

		BindingContext = new DoctorDetailsPopupViewModel();
	}
}