namespace MedLinkApp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();

        GenerateUIAsync();

        this.BindingContext = _viewModel = new HomeViewModel();
    }

    HomeViewModel _viewModel;
    protected override bool OnBackButtonPressed()
    {
        System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
        return false;
    }

    private void GenerateUIAsync()
    {
        App.Current.Dispatcher.Dispatch(async () =>
        {
            _viewModel.IsBusy = true;

            #region skeleton

            contentGrid.Add(new StackLayout
            {
                Children =
                {
                    new StackLayout
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.Center,
                        Orientation = StackOrientation.Horizontal,

                        Children =
                        {
                            new Border
                            {
                                Stroke = Color.FromArgb("#C49B33"),
                                Background = Color.FromArgb("#2B0B98"),
                                StrokeThickness = 4,
                                Padding = new Thickness(16, 8),
                                HorizontalOptions = LayoutOptions.Center,
                                StrokeShape = new RoundRectangle
                                {
                                    CornerRadius = new CornerRadius(40, 0, 0, 40)
                                },
                                WidthRequest = 150,
                                HeightRequest = 25
                            },

                            
                        }
                    }.Height(40).Margins(0, 0, 0, 0),
                }
            }/*.Bind(StackLayout.IsVisibleProperty, static (HomeViewModel vm) => vm.IsBusy),*/, 0, 0);


            #endregion

            await Task.Delay(500);
            _viewModel.accessToken = await SecureStorage.Default.GetAsync("UserAccessToken");


            await _viewModel.GetAllDoctors();

            _viewModel.IsBusy = false;
            //contentGrid.Add(new Label().Text("Test").Bind(Label.IsVisibleProperty,
            //    static (HomeViewModel vm) => !vm.IsBusy), 0, 0);

        });
    }
}