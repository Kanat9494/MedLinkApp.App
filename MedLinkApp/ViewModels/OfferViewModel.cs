namespace MedLinkApp.ViewModels;

[QueryProperty(nameof(ProductPrice), "ProductPrice")]
internal class OfferViewModel : BaseViewModel
{
    public OfferViewModel()
    {
        WaitingForDoctor = true;

        CancelCommand = new Command(OnCancel);
    }

    string accessToken;
    string _abortMessage;
    string _senderName;
    string _receiverName;
    int _offerId;
    CancellationTokenSource cancelTokenSource;
    CancellationToken cancelToken;

    public Command CancelCommand { get; }

    private bool _waitingForDoctor;
    public bool WaitingForDoctor
    {
        get => _waitingForDoctor;
        set => SetProperty(ref _waitingForDoctor, value);
    }
    private double _productPrice;
    public double ProductPrice
    {
        get => _productPrice;
        set => SetProperty(ref _productPrice, value);
    }

    async void OnCancel()
    {
        cancelTokenSource.Cancel();
        cancelTokenSource.Dispose();

        await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
    }
}
