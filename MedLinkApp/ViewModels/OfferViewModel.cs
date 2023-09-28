

namespace MedLinkApp.ViewModels;

[QueryProperty(nameof(ProductPrice), "ProductPrice")]
internal class OfferViewModel : BaseViewModel
{
    public OfferViewModel()
    {
        WaitingForDoctor = true;
        checkOfferCount = 0;
        cancelTokenSource = new CancellationTokenSource();
        cancelToken = cancelTokenSource.Token;
        _saveOfferCount = 0;

        Task.Run(async () =>
        {
            _userId = int.Parse(await SecureStorage.Default.GetAsync("UserId"));
            _accessToken = await SecureStorage.Default.GetAsync("UserAccessToken");
            _senderName = await SecureStorage.Default.GetAsync("UserName");
            _receiverName = await SecureStorage.Default.GetAsync("DoctorAccountName");
        }).Wait();

        Task.Run(async () =>
        {
            await SetOffer();
        });

        CancelCommand = new AsyncRelayCommand(OnCancel);
    }

    string _accessToken;
    string _senderName;
    string _receiverName;
    short checkOfferCount;
    short _saveOfferCount;
    CancellationTokenSource cancelTokenSource;
    CancellationToken cancelToken;
    int _userId;

    public ICommand CancelCommand { get; }

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
    private int _offerId;
    public int OfferId
    {
        get => _offerId;
        set => SetProperty(ref _offerId, value);
    }

    async Task OnCancel()
    {
        cancelTokenSource.Cancel();
        cancelTokenSource.Dispose();

        await DeleteOffer(OfferId);

        await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
    }

    async Task SetOffer()
    {
        try
        {
            await Task.Delay(2000);

            var isSaved = await SaveOffer();
            if (!isSaved)
            {
                await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            }

            Task setOfferTask = new Task(async () =>
            {
                await Task.Delay(2000);

                while (true)
                {
                    if (cancelToken.IsCancellationRequested)
                        break;

                    if (checkOfferCount == 5)
                        await OnCancel();

                    var offer = await ContentService.Instance(_accessToken).GetItemAsync<Offer>($"api/Offers/GetOffer?receiverName={_senderName}");
                    var isConfirmed = await CheckOffer(offer);
                    if (isConfirmed)
                    {
                        var message = "Консультация";
                        await DeleteOffer(OfferId);
                        await ContentService.Instance(_accessToken).GetItemDataAsync<bool>($"api/Balance/WithdrawalUserBalance?id={_userId}&" +
                            $"balance={ProductPrice}&description={message}&recipient={_receiverName}");

                        App.Current.Dispatcher.Dispatch(async () =>
                        {
                            await Shell.Current.GoToAsync(nameof(ChatPage));
                        });
                        break;
                    }

                    checkOfferCount++;
                    await Task.Delay(5000);
                }
            }, cancelToken);

            setOfferTask.Start();
        }
        catch 
        {
            cancelTokenSource.Cancel();
            cancelTokenSource.Dispose();
        }
    }

    async Task<bool> CheckOffer(Offer offer)
    {
        if (offer != null)
        {
            if (offer.StatusCode == 200)
            {
                if (offer.IsConfirmed == 1)
                {

                    await DeleteOffer(offer.OfferId);

                    cancelTokenSource.Cancel();
                    cancelTokenSource.Dispose();
                    return true;
                }

                await DeleteOffer(offer.OfferId);


                return false;
            }

            return false;
        }

        return false;
    }

    async Task<bool> SaveOffer()
    {
        try
        {
            while (true)
            {
                var offer = new Offer
                {
                    SenderName = _senderName,
                    ReceiverName = _receiverName,
                    ProductPrice = _productPrice,
                    IsConfirmed = 1
                };
                OfferId = await ContentService.Instance(_accessToken).PostItemAsync(offer, "api/Offers/SetOffer");

                if (OfferId > 0)
                {
                    return true;
                }
                

                if (_saveOfferCount == 5)
                {
                    App.Current.Dispatcher.Dispatch(async () =>
                    {
                        await Shell.Current.DisplayAlert("Непредвиденная ошибка", "Что-то пошло не так, попробуйте снова!", "Ок");
                    });

                    cancelTokenSource.Cancel();
                    cancelTokenSource.Dispose();
                    return false;
                }

                _saveOfferCount++;

                await Task.Delay(5000);
            }
        }
        catch 
        {
            return false;
        }
    }

    private async Task DeleteOffer(int? offerId)
    {
        if (offerId != null && offerId > 0)
            await ContentService.Instance(_accessToken).DeleteItemAsync($"api/Offers/DeleteOffer?offerId={offerId}");
    }
}
