using Microsoft.AspNetCore.SignalR.Client;

namespace MedLinkApp.ViewModels;

[QueryProperty(nameof(DoctorId), "DoctorId")]
internal class ProductsViewModel : BaseViewModel
{
    public ProductsViewModel()
    {
        IsLoading = true;
        Products = new ObservableCollection<Product>();

        ProductTapped = new Command<Product>(async (product) =>
        {
            await OnProductSelected(product);
        });

        Task.Run(async () =>
        {
            accessToken = await SecureStorage.Default.GetAsync("UserAccessToken");

            await LoadProducts();
        }).GetAwaiter().OnCompleted(() =>
        {
            IsLoading = false;
        });
    }

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

    string accessToken;

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
                await Shell.Current.GoToAsync($"{nameof(ChatPage)}");
            }
        }
    }

    async Task ConsultationConfirmed()
    {
        await Shell.Current.GoToAsync($"{nameof(ChatsPage)}");
    }
}
