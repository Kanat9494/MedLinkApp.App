namespace MedLinkApp.ViewModels.CustomControls;

internal class DoctorDetailsPopupViewModel : BaseViewModel
{
    public DoctorDetailsPopupViewModel()
    {
        IsLoading = true;
        Products = new ObservableCollection<Product>();
        OpenChat = new Command(async () => await OnOpenChat());

        Task.Run(async () =>
        {
            accessToken = await SecureStorage.Default.GetAsync("UserAccessToken");

            await LoadProducts();
        }).GetAwaiter().OnCompleted(() =>
        {
            IsLoading = false;
        });
    }

    public ObservableCollection<Product> Products { get; set; }
    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public Command OpenChat { get; }

    string accessToken;

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

    private async Task OnOpenChat()
    {
        await Shell.Current.GoToAsync($"{nameof(ChatPage)}");
        //await Navigation.PushAsync(new HomePage());
        //await Navigation.PushAsync(new ChatPage(), true);
    }
}
