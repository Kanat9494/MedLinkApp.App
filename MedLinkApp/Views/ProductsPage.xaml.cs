namespace MedLinkApp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ProductsPage : ContentPage
{
	public ProductsPage()
	{
		InitializeComponent();

		this.BindingContext = new ProductsViewModel();
	}

    //protected override bool OnBackButtonPressed()
    //{
    //    return true;
    //}
}