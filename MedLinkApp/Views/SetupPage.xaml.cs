namespace MedLinkApp.Views;

public partial class SetupPage : ContentPage
{
	public SetupPage()
	{
		InitializeComponent();

		GenerateUIAsync();

		BindingContext = _viewModel = new SetupViewModel();
	}

	SetupViewModel _viewModel;

	private void GenerateUIAsync()
	{
		App.Current.Dispatcher.Dispatch(async () =>
		{
			await Task.Delay(1000);

            contentSL.Add(new StackLayout
			{
				Orientation = StackOrientation.Horizontal,
				Children =
				{
					new Label().Text("Test")
				}
			}.Margins(10, 10, 10, 10));

		});
	}
}