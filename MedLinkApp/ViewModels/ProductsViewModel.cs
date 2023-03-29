using Microsoft.AspNetCore.SignalR.Client;

namespace MedLinkApp.ViewModels;

[QueryProperty(nameof(DoctorId), "DoctorId")]
internal class ProductsViewModel : BaseViewModel
{
    public ProductsViewModel()
    {
        hubConnection = new HubConnectionBuilder()
            .WithUrl(MedLinkConstants.SERVER_ROOT_URL + "/chatHub")
            .Build();

        IsLoading = true;
        IsWaitingDoctor = false;
        Products = new ObservableCollection<Product>();

        ProductTapped = new Command<Product>(async (product) =>
        {
            await OnProductSelected(product);
        });

        Task.Run(async () =>
        {
            accessToken = await SecureStorage.Default.GetAsync("UserAccessToken");

            await LoadProducts();
            await Connect();
        }).GetAwaiter().OnCompleted(() =>
        {
            IsLoading = false;
        });

        hubConnection.Closed += async (error) =>
        {
            await Task.Delay(5000);
            await Connect();
        };

        hubConnection.On<string>("ReceiveMessage", (message) =>
        {
            Message = message;

            Task.Run(async () => await ConsultationConfirmed());
            //if (Message.Equals(MedLinkConstants.CONFIRM_MESSAGE))
            //{
            //    Task.Run(async () =>
            //    {
            //        await ConsultationConfirmed();
            //    });
            //}
        });
    }

    string Message { get; set; }

    private int _doctorId;
    public int DoctorId
    {
        get => _doctorId;
        set => SetProperty(ref _doctorId, value);
    }

    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    private bool _isWaitingDoctor;
    public bool IsWaitingDoctor
    {
        get => _isWaitingDoctor;
        set => SetProperty(ref _isWaitingDoctor, value);
    }

    string accessToken;

    HubConnection hubConnection;

    public ObservableCollection<Product> Products { get; set; }

    public Command<Product> ProductTapped { get; }

    public async Task LoadProducts()
    {
        try
        {
            var response = await ContentService.Instance(accessToken).GetItemsAsync<Product>("api/Product/GetProducts");

            if (response != null)
            {
                foreach (var item in response)
                    Products.Add(item);
            }
        }
        catch (Exception ex)
        {

        }
    }

    private async Task OnProductSelected(Product product)
    {
        if (product == null)
            return;

        int userId = int.Parse(await SecureStorage.Default.GetAsync("UserId"));

        double userBalance = await ContentService.Instance(accessToken).GetItemDataAsync<double, double>($"api/User/GetUserBalance/{userId}");

        if (userBalance == 0)
            await Shell.Current.DisplayAlert("Недостаточно средств", "У вас не хватает средств для консультации, " +
                "пожалуйста пополните баланс", "Ок");
        else
        {
            if (userBalance < product.Price)
                await Shell.Current.DisplayAlert("Недостаточно средств", "У вас не хватает средств для консультации, " +
                    "пожалуйста пополните баланс", "Ок");
            else
            {
                IsWaitingDoctor = true;
                await SendConfirmMessage();
            }
        }
    }

    async Task SendConfirmMessage()
    {
        try
        {
            await hubConnection.InvokeAsync("SendMessage", MedLinkConstants.CONFIRM_MESSAGE);
        }
        catch (Exception ex)
        {

        }
    }

    async Task Connect()
    {
        try
        {
            await hubConnection.StartAsync();
        }
        catch (Exception ex)
        {

        }
    }

    async Task Disconnect()
    {
        await hubConnection.StopAsync();
    }

    async Task ConsultationConfirmed()
    {
        await Shell.Current.GoToAsync($"{nameof(ProductsPage)}?{nameof(ProductsViewModel.DoctorId)}={DoctorId}");
    }
}
