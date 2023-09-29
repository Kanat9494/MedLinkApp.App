namespace MedLinkApp.Views;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();

        GenerateUIAsync();

        BindingContext = _viewModel = new SettingsViewModel();
    }

    SettingsViewModel _viewModel;

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
                    new Label
                    {
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        TextColor = Colors.Purple
                    }.Font(size: 19)
                }
            }.Margins(10, 10, 10, 10));

        });
    }
}