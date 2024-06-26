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
                    }.Font(size: 19).Text("������ �����"),
                    new Switch
                    {
                        HorizontalOptions = LayoutOptions.EndAndExpand,
                        VerticalOptions = LayoutOptions.Center
                    }.Bind(Switch.IsToggledProperty, static (SettingsViewModel vm) => vm.IsNightTheme)
                }
            }.Margins(10, 10, 10, 10));

            contentSL.Add(CreateBoxView());

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
                    }.Font(size: 19).Text("������ �����"),
                    new Switch
                    {
                        HorizontalOptions = LayoutOptions.EndAndExpand,
                        VerticalOptions = LayoutOptions.Center
                    }.Bind(Switch.IsToggledProperty, static (SettingsViewModel vm) => vm.IsNightTheme)
                }
            }.Margins(10, 10, 10, 10));

            contentSL.Add(CreateBoxView());

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
                    }.Font(size: 19).Text("������ �����"),
                    new Switch
                    {
                        HorizontalOptions = LayoutOptions.EndAndExpand,
                        VerticalOptions = LayoutOptions.Center
                    }.Bind(Switch.IsToggledProperty, static (SettingsViewModel vm) => vm.IsNightTheme)
                }
            }.Margins(10, 10, 10, 10));

            contentSL.Add(CreateBoxView());

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
                    }.Font(size: 19).Text("������ �����"),
                    new Switch
                    {
                        HorizontalOptions = LayoutOptions.EndAndExpand,
                        VerticalOptions = LayoutOptions.Center
                    }.Bind(Switch.IsToggledProperty, static (SettingsViewModel vm) => vm.IsNightTheme)
                }
            }.Margins(10, 10, 10, 10));

            contentSL.Add(CreateBoxView());

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
                    }.Font(size: 19).Text("������ �����"),
                    new Switch
                    {
                        HorizontalOptions = LayoutOptions.EndAndExpand,
                        VerticalOptions = LayoutOptions.Center
                    }.Bind(Switch.IsToggledProperty, static (SettingsViewModel vm) => vm.IsNightTheme)
                }
            }.Margins(10, 10, 10, 10));

            contentSL.Add(CreateBoxView());

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
                    }.Font(size: 19).Text("������ �����"),
                    new Switch
                    {
                        HorizontalOptions = LayoutOptions.EndAndExpand,
                        VerticalOptions = LayoutOptions.Center
                    }.Bind(Switch.IsToggledProperty, static (SettingsViewModel vm) => vm.IsNightTheme)
                }
            }.Margins(10, 10, 10, 10));

            contentSL.Add(CreateBoxView());

            for(int i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                    await Task.Delay(1000);
                else
                {
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
                    }.Font(size: 19).Text("������ �����"),
                    new Switch
                    {
                        HorizontalOptions = LayoutOptions.EndAndExpand,
                        VerticalOptions = LayoutOptions.Center
                    }.Bind(Switch.IsToggledProperty, static (SettingsViewModel vm) => vm.IsNightTheme)
                }
                    }.Margins(10, 10, 10, 10));

                    contentSL.Add(CreateBoxView());
                }
            }

        });
    }

    BoxView CreateBoxView()
    {
        return new BoxView
        {
            HorizontalOptions = LayoutOptions.FillAndExpand,
            Color = Colors.Purple
        }.Margins(0, 0, 0, 10).Height(1);
    }
}