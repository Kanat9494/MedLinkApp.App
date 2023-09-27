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
                                Stroke = Colors.Transparent,
                                Background = Color.FromArgb("#C8C8C8"),
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                StrokeShape = new RoundRectangle
                                {
                                    CornerRadius = new CornerRadius(6, 6, 6, 6)
                                },
                                HeightRequest = 25
                            }.Margins(0, 0, 10, 0),

                            new Border
                            {
                                Stroke = Colors.Transparent,
                                StrokeShape = new RoundRectangle
                                {
                                    CornerRadius = new CornerRadius(6, 6, 6, 6)
                                },
                                Background = Color.FromArgb("#C8C8C8"),
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.EndAndExpand
                            }.Height(25).Width(25).Margins(0, 0, 10, 0),
                            
                        }
                    }.Height(40).Margins(10, 0, 0, 0),

                    new Border
                    {
                        Stroke = Colors.Transparent,
                        Background = Color.FromArgb("#C8C8C8"),
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        StrokeShape = new RoundRectangle
                        {
                            CornerRadius = new CornerRadius(6, 6, 6, 6)
                        },
                        HeightRequest = 20
                    }.Margins(10, 0, 10, 0).Width(300),
                }
            }.Bind(StackLayout.IsVisibleProperty, static (HomeViewModel vm) => vm.IsBusy), 0, 0);

            contentGrid.Add(new Border
            {
                Stroke = Colors.Transparent,
                Background = Color.FromArgb("#C8C8C8"),
                HorizontalOptions = LayoutOptions.StartAndExpand,
                StrokeShape = new RoundRectangle
                {
                    CornerRadius = new CornerRadius(6, 6, 6, 6)
                },
                HeightRequest = 20
            }.Margins(10, 0, 0, 0).Width(220).Bind(Border.IsVisibleProperty, static (HomeViewModel vm) => vm.IsBusy), 0, 1);

            contentGrid.Add(new StackLayout
            {
                Children =
                {
                   new StackLayout
                    {
                        Orientation = StackOrientation.Vertical,

                        Children =
                        {
                            ListViewSkeleton(),
                            ListViewSkeleton(),
                            ListViewSkeleton(),
                        }
                    }
                }
            }.Margins(5, 0, 0, 0).Bind(StackLayout.IsVisibleProperty, static (HomeViewModel vm) => vm.IsBusy), 0, 2);


            #endregion

            await Task.Delay(2000);
            _viewModel.accessToken = await SecureStorage.Default.GetAsync("UserAccessToken");


            await _viewModel.GetAllDoctors();

            #region content

            _viewModel.IsBusy = false;
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
                            new Label
                            {
                                HorizontalOptions = LayoutOptions.Start,
                                VerticalOptions = LayoutOptions.Center
                            }.Text("MedLink").Font(size: 24, bold: true, italic: true, family: "RegularFont"),
                            new Image
                            {
                                VerticalOptions = LayoutOptions.Center,
                                Source = "bell_icon.png",
                                HorizontalOptions = LayoutOptions.EndAndExpand
                            }.Height(25).Width(25)
                        }
                    },

                    new Label
                    {
                        TextColor = Color.FromArgb("#00e600")
                    }.Text("Ваше здоровье в надежных руках").Font(bold: true, family: "RegularFont")
                }
            }.Margins(10, 10, 10, 0).Bind(StackLayout.IsVisibleProperty, static (HomeViewModel vm) => !vm.IsBusy), 0, 0);

            contentGrid.Add(new Label().Text("Наши доктора").Font(bold: true, size: 18, family: "RegularFont").Margins(10, 10, 10, 0)
                .Bind(Label.IsVisibleProperty, static (HomeViewModel vm) => !vm.IsBusy), 0, 1);

            #endregion

        });
    }

    private StackLayout ListViewSkeleton()
    {
        return new StackLayout
        {
            Orientation = StackOrientation.Horizontal,

            Children =
            {
                new Border
                {
                    Stroke = Colors.Transparent,
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = new CornerRadius(10, 10, 10, 10),
                    },
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.Center,
                    Background = Color.FromArgb("#C8C8C8")
                }.Width(130).Margins(0, 0, 10, 0).Height(160),

                new StackLayout
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Children =
                    {
                        new Border
                        {
                            Stroke = Colors.Transparent,
                            Background = Color.FromArgb("#C8C8C8"),
                            StrokeShape = new RoundRectangle
                            {
                                CornerRadius = new CornerRadius(6, 6, 6, 6),
                            },
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                        }.Height(25),
                        new Border
                        {
                            Stroke = Colors.Transparent,
                            Background = Color.FromArgb("#C8C8C8"),
                            StrokeShape = new RoundRectangle
                            {
                                CornerRadius = new CornerRadius(6, 6, 6, 6),
                            },
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                        }.Height(25),
                        new Border
                        {
                            Stroke = Colors.Transparent,
                            Background = Color.FromArgb("#C8C8C8"),
                            StrokeShape = new RoundRectangle
                            {
                                CornerRadius = new CornerRadius(6, 6, 6, 6),
                            },
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                        }.Height(25),
                    }

                }.Margins(0, 0, 0, 0)
            }
        }.Margins(10, 0, 10, 8);
    }
}