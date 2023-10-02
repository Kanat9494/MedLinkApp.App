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
            contentSL.Add(new Image
            {
                Aspect = Aspect.AspectFill
            }.Source("https://picsum.photos/id/1/200/300").Height(400).Margins(0, 0, 0, 25));

            await Task.Delay(1000);

            contentSL.Add(new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    new Label
                    {
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        TextColor = Colors.Purple,
                        VerticalOptions = LayoutOptions.Center
                    }.Font(size: 19).Text("Ночной режим"),
                    new Switch
                    {
                        HorizontalOptions = LayoutOptions.EndAndExpand,
                        VerticalOptions = LayoutOptions.Center
                    }.Bind(Switch.IsToggledProperty, static (SettingsViewModel vm) => vm.IsNightTheme)
                }
            }.Margins(10, 10, 10, 10));

        });
    }
}