namespace MedLinkApp.ViewModels;

[QueryProperty(nameof(ProductPrice), "ProductPrice")]
internal class OfferViewModel : BaseViewModel
{
    public OfferViewModel()
    {
        WaitingForDoctor = true;
        checkOfferCount = 0;

        CancelCommand = new Command(OnCancel);
    }

    string _accessToken;
    string _abortMessage;
    string _senderName;
    string _receiverName;
    int _offerId;
    short checkOfferCount;
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

    void SetOffer()
    {
        Task setOfferTask = new Task(async () =>
        {
            await Task.Delay(2000);

            while (true)
            {
                if (cancelToken.IsCancellationRequested)
                    break;

                if (checkOfferCount == 5)
                    break;

                var offer = await ContentService.Instance(_accessToken).GetItemAsync<Offer>($"api/Offers/GetOffer?receiverName={_senderName}");


                checkOfferCount++;
                await Task.Delay(5000);
            }
        }, cancelToken);

        setOfferTask.Start();
    }

    async Task<bool> CheckOffer(Offer offer)
    {
        if (offer != null)
        {
            if (offer.StatusCode == 200)
            {
                if (offer.StatusCode == 1)
                {
                    _receiverName = offer.SenderName;
                    await SecureStorage.Default.SetAsync("ReceiverName", offer.SenderName);
                    _productPrice = offer.ProductPrice;
                    _offerId = offer.OfferId;

                }

                await ContentService.Instance(_accessToken).GetItemDataAsync<bool>($"api/Offers/DeleteOffer?offerId={_offerId}");

                return false;
            }

            return false;
        }

        return false;
    }
}
