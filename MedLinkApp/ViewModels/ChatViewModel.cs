namespace MedLinkApp.ViewModels;

internal class ChatViewModel : BaseViewModel
{
    public ChatViewModel()
    {

    }

    string _accessToken;
    string _abortMessage;
    string _senderName;
    string _receiverName;
    string cTimer;
    DateTime endTime;
    System.Timers.Timer timer;
    FirebaseClient _firebaseClient;

    public ObservableCollection<Message> Messages { get; set; }

    public ICommand SendMessage { get; }
    public ICommand OpenAuidioMessagePage { get; }
    public ICommand OpenPhotoMessagePage { get; }
    public ICommand OpenPhotoMessageCommand { get; }
    public ICommand AbortChatCommand { get; }

    private string _sendingMessage;
    public string SendingMessage
    {
        get => _sendingMessage;
        set => SetProperty(ref _sendingMessage, value);
    }
    private string _chatTimer;
    public string ChatTimer
    {
        get => _chatTimer;
        set => SetProperty(ref _chatTimer, value);
    }
    private string _doctorFullName;
    public string DoctorFullName
    {
        get => _doctorFullName;
        set => SetProperty(ref _doctorFullName, value);
    }

    void StartCountDownTimer()
    {
        timer = new System.Timers.Timer();
        endTime = DateTime.Now.AddMinutes(5);
        timer.Elapsed += ChatTimerTick;
        TimeSpan timeSpan = endTime - DateTime.Now;
        cTimer = timeSpan.ToString("m' Minutes 's' Seconds'");
        timer.Start();
    }

    void ChatTimerTick(object sender, EventArgs e)
    {
        TimeSpan timeSpan = endTime - DateTime.Now;

        cTimer = timeSpan.ToString("m':'s' '");

        App.Current.Dispatcher.Dispatch(() =>
        {
            ChatTimer = cTimer;
        });

        if ((timeSpan.TotalMinutes == 0) || (timeSpan.TotalMilliseconds < 1000))
        {
            timer.Stop();
            Task.Run(async () =>
            {
                //DisconnectFirebase();
                await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            });
        }
    }

    async Task OnSendMessage()
    {
        try
        {
            var message = new Message
            {
                SenderName = _senderName,
                ReceiverName = _receiverName,
                Content = SendingMessage
            };

            await ContentService.Instance(_accessToken).PostItemAsync<Message>(message, "api/Messages/SendMessage");
        }
        catch (Exception ex) { }
    }
}


//Чат для firebase
//internal class ChatViewModel : BaseViewModel
//{
//    public ChatViewModel()
//    {
//        _abortMessage = "Вы отмененили";
//        firebaseClient = new FirebaseClient("https://medlinkchat-default-rtdb.europe-west1.firebasedatabase.app/");

//        Task.Run(async () =>
//        {
//            accessToken = await SecureStorage.Default.GetAsync("UserAccessToken");
//            _senderName = await SecureStorage.Default.GetAsync("UserName");
//            _receiverName = await SecureStorage.Default.GetAsync("DoctorAccountName");
//            DoctorFullName = await SecureStorage.Default.GetAsync("DoctorFullName");
//        }).Wait();

//        Messages = new ObservableCollection<Message>();

//        var collectionOfMessages = firebaseClient
//            .Child("Messages")
//            .OrderByPriority()
//            .LimitToLast(1)
//            .AsObservable<Message>()
//            .Where(m => m.Object.ReceiverName == _senderName && m.Object.SenderName == _receiverName)
//            .Subscribe((item) =>
//            {
//                if (item.Object != null)
//                {
//                    Messages.Add(item.Object);
//                }
//            });

//        SendMessage = new Command(async () =>
//        {
//            await OnSendMessage();
//        });

//        OpenAudioMessagePage = new Command(ToAudioMessagePage);
//        OpenPhotoMessagePage = new Command(PickImage);
//        OpenPhotoMessageCommand = new Command<string>(async (imageUrl) => await OnOpenPhotoMessage(imageUrl));
//        AbortChatCommand = new Command(OnAbortChat);

//        StartCountDownTimer();
//    }

//    string accessToken;
//    string _abortMessage;
//    string _senderName;
//    string _receiverName;
//    string cTimer;
//    DateTime endTime;
//    System.Timers.Timer timer;
//    FirebaseClient firebaseClient;

//    public Command SendMessage { get; }
//    public Command OpenAudioMessagePage { get; }
//    public Command OpenPhotoMessagePage { get; }
//    public Command<string> OpenPhotoMessageCommand { get; }
//    public Command AbortChatCommand { get; }

//    private string _sendingMessage;
//    public string SendingMessage
//    {
//        get => _sendingMessage;
//        set => SetProperty(ref _sendingMessage, value);
//    }
//    private string _chatTimer;
//    public string ChatTimer
//    {
//        get => _chatTimer;
//        set => SetProperty(ref _chatTimer, value);
//    }
    
//    private string _doctorFullName;
//    public string DoctorFullName
//    {
//        get => _doctorFullName;
//        set => SetProperty(ref _doctorFullName, value);
//    }

//    public ObservableCollection<Message> Messages { get; set; }

//    void StartCountDownTimer()
//    {
//        timer = new System.Timers.Timer();
//        endTime = DateTime.Now.AddMinutes(5);
//        timer.Elapsed += ChatTimerTick;
//        TimeSpan timeSpan = endTime - DateTime.Now;
//        cTimer = timeSpan.ToString("m' Minutes 's' Seconds'");
//        timer.Start();
//    }

//    void ChatTimerTick(object sender, EventArgs e)
//    {
//        TimeSpan timeSpan = endTime - DateTime.Now;

//        cTimer = timeSpan.ToString("m':'s' '");

//        App.Current.Dispatcher.Dispatch(() =>
//        {
//            ChatTimer = cTimer;
//        });

//        if ((timeSpan.TotalMinutes == 0) || (timeSpan.TotalMilliseconds < 1000))
//        {
//            timer.Stop();
//            Task.Run(async () =>
//            {
//                DisconnectFirebase();
//                await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
//            });
//        }
//    }

//    async Task OnSendMessage()
//    {
//        try
//        {
//            var message = new Message()
//            {
//                SenderName = _senderName,
//                ReceiverName = _receiverName,
//                Content = SendingMessage,
//            };

//            var serializedMessage = JsonConvert.SerializeObject(message);

//            await firebaseClient.Child("Messages").PostAsync(serializedMessage);

//            SendLocalMessage(message);
//        }
//        catch (Exception ex) { }
//    }

//    void DisconnectFirebase()
//    {
//        if (firebaseClient != null)
//            firebaseClient.Dispose();
//    }

//    private void SendLocalMessage(Message message)
//    {
//        if (string.IsNullOrEmpty(message.Content))
//            return;

//        #region сохранение фото в локальном хранилище
//        //if (message.ImageUrl != null)
//        //{
//        //    Task.Run(async () =>
//        //    {
//        //        var imabeBytes = await FileHelper.DownloadImageBytesAsync(message.ImageUrl);
//        //        if (imabeBytes != null)
//        //        {
//        //            var c = await FileHelper.SaveFileAsync(imabeBytes);
//        //            message.ImageUrl = c;
//        //        }
//        //    }).Wait();
//        //}
//        #endregion

//        Messages.Add(message);

//        SendingMessage = string.Empty;
//    }

//    private async void ToAudioMessagePage()
//        => await Shell.Current.GoToAsync(nameof(AudioMessagePage));

//    private async void OnAbortChat()
//    {
//        await Shell.Current.DisplayAlert("Отмена", _abortMessage, "Ок");
//        DisconnectFirebase();
//        await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
//    }

//    private async Task OnOpenPhotoMessage(string imageUrl)
//        => await Shell.Current.GoToAsync($"{nameof(ImageBrowsePage)}?{nameof(ImageBrowseViewModel.ImageUrl)}={imageUrl}");

//    #region SendImage
//    async void PickImage()
//    {
//        var result = await FilePicker.PickAsync(new PickOptions
//        {
//            PickerTitle = "Выберите изображение",
//            FileTypes = FilePickerFileType.Images
//        });

//        if (result == null)
//            return;

//        var stream = await result.OpenReadAsync();

//        var imageBytes = FileHelper.StreamTyByte(stream);
//        string accessToken = await SecureStorage.Default.GetAsync("UserAccessToken");

//        var imageUrl = MedLinkConstants.FILE_BASE_PATH + "/" + await FileService.UploadFile(imageBytes, accessToken);

//        //await OnSendMessage("test", "tomy", "тестовый месседж", $"{MedLinkConstants.FILE_BASE_PATH}/{filePath}");
//        await SendImageMessage(imageUrl);
//    }

//    async Task SendImageMessage(string imageUrl)
//    {
//        try
//        {
//            var message = new Message()
//            {
//                SenderName = _senderName,
//                ReceiverName = _receiverName,
//                Content = "Фото",
//                ImageUrl = imageUrl
//            };
//            var serializedMessage = JsonConvert.SerializeObject(message);
//            await firebaseClient.Child("Messages").PostAsync(serializedMessage);

//            SendLocalMessage(message);
//        }
//        catch (Exception ex)
//        {

//        }
//    }
//    #endregion
//}

//Расскомментировать, для SignalR или собственного чат сервера
//internal class ChatViewModel : BaseViewModel
//{
//    public ChatViewModel()
//    {
//        WaitingForDoctor = true;
//        ContentIsVisible = false;
//        _isTimerRunning = false;
//        _abortMessage = "Ваш запрос отменен";

//        Task.Run(async () =>
//        {
//            accessToken = await SecureStorage.Default.GetAsync("UserAccessToken");

//            _senderName = await SecureStorage.Default.GetAsync("UserName");
//            _receiverName = await SecureStorage.Default.GetAsync("DoctorAccountName");
//        }).Wait();

//        hubConnection = new HubConnectionBuilder()
//            .WithUrl(MedLinkConstants.SERVER_ROOT_URL + $"/chatHub/{_senderName}")
//            .Build();

//        Messages = new ObservableCollection<Message>();


//        Task.Run(async () =>
//        {
//            await Connect();

//            DoctorFullName = await SecureStorage.Default.GetAsync("DoctorFullName");

//            await SendConfirmMessage();
//        }).GetAwaiter().OnCompleted(() =>
//        {

//        });

//        SendMessage = new Command(async () =>
//        {
//            await OnSendMessage();
//        });

//        OpenAudioMessagePage = new Command(ToAudioMessagePage);
//        OpenPhotoMessagePage = new Command(PickImage);
//        OpenPhotoMessageCommand = new Command<string>(async (imageUrl) => await OnOpenPhotoMessage(imageUrl));
//        AbortChatCommand = new Command(OnAbortChat);

//        hubConnection.Closed += async (error) =>
//        {
//            await Task.Delay(4000);
//            await Connect();
//        };

//        hubConnection.On<string, string>("ReceiveRejectMessage", (senderName, receiverName) =>
//        {
//            _abortMessage = "Доктор отклонил ваш запрос, попробуйте еще раз";
//            OnAbortChat();
//        });

//        hubConnection.On<string, string, double>("ReceiveConfirmMessage", (senderName, receiverName, prive) =>
//        {
//            Task.Run(async () => await ConsultationConfirmed());
//        });

//        hubConnection.On<string, string, string>("ReceiveMessage", (senderName, receiverName, jsonMessage) =>
//        {
//            try
//            {
//                var message = JsonConvert.DeserializeObject<Message>(jsonMessage);
//                if (!_isTimerRunning)
//                {
//                    StartCountDownTimer();
//                    _isTimerRunning = !_isTimerRunning;
//                }

//                SendLocalMessage(message);
//            }
//            catch
//            {

//            }
//        });
//    }

//    string accessToken;
//    string _abortMessage;

//    HubConnection hubConnection;
//    public Command SendMessage { get; }
//    public Command OpenAudioMessagePage { get; }
//    public Command OpenPhotoMessagePage { get; }
//    public Command<string> OpenPhotoMessageCommand { get; }
//    public Command AbortChatCommand { get; }

//    private string _sendingMessage;
//    public string SendingMessage
//    {
//        get => _sendingMessage;
//        set => SetProperty(ref _sendingMessage, value);
//    }
//    private string _senderName;
//    private string _receiverName;

//    private string _chatTimer;
//    public string ChatTimer
//    {
//        get => _chatTimer;
//        set => SetProperty(ref _chatTimer, value);
//    }

//    string cTimer;
//    DateTime endTime;
//    System.Timers.Timer timer;

//    private bool _contentIsVisible;
//    public bool ContentIsVisible
//    {
//        get => _contentIsVisible;
//        set => SetProperty(ref _contentIsVisible, value);
//    }

//    private bool _waitingForDoctor;
//    public bool WaitingForDoctor
//    {
//        get => _waitingForDoctor;
//        set => SetProperty(ref _waitingForDoctor, value);
//    }

//    private string _doctorFullName;
//    public string DoctorFullName
//    {
//        get => _doctorFullName;
//        set => SetProperty(ref _doctorFullName, value);
//    }
//    private double _productPrice;

//    public double ProductPrice
//    {
//        get => _productPrice;
//        set => SetProperty(ref _productPrice, value);
//    }

//    private bool _isTimerRunning;
//    public ObservableCollection<Message> Messages { get; set; }

//    void StartCountDownTimer()
//    {
//        timer = new System.Timers.Timer();
//        endTime = DateTime.Now.AddMinutes(5);
//        timer.Elapsed += ChatTimerTick;
//        TimeSpan timeSpan = endTime - DateTime.Now;
//        cTimer = timeSpan.ToString("m' Minutes 's' Seconds'");
//        timer.Start();
//    }

//    void ChatTimerTick(object sender, EventArgs e)
//    {
//        TimeSpan timeSpan = endTime - DateTime.Now;

//        cTimer = timeSpan.ToString("m':'s' '");

//        App.Current.Dispatcher.Dispatch(() =>
//        {
//            ChatTimer = cTimer;
//        });

//        if ((timeSpan.TotalMinutes == 0) || (timeSpan.TotalMilliseconds < 1000))
//            timer.Stop();
//    }

//    async Task OnSendMessage()
//    {
//        try
//        {
//            var message = new Message()
//            {
//                SenderName = _senderName,
//                ReceiverName = _receiverName,
//                Content = SendingMessage,
//                //ImageUrl = "https://www.google.com/images/logos/ps_logo2.png"

//            };
//            var serializedMessage = JsonConvert.SerializeObject(message);
//            await hubConnection.InvokeAsync("SendMessage", _senderName, _receiverName, serializedMessage);

//            //это лишнее убрал, чтобы не отправлять сообщение 2 раза
//            SendLocalMessage(message);
//        }
//        catch (Exception ex)
//        {

//        }
//    }

//    async Task Connect()
//    {
//        try
//        {
//            await hubConnection.StartAsync();
//        }
//        catch (Exception ex)
//        {

//        }
//    }

//    async Task Disconnect()
//    {
//        await hubConnection.StopAsync();
//    }

//    async Task SendConfirmMessage()
//    {
//        try
//        {
//            await hubConnection.InvokeAsync("SendConfirmMessage", _senderName, _receiverName, ProductPrice);
//        }
//        catch (Exception ex)
//        {

//        }
//    }

//    private async Task ConsultationConfirmed()
//    {
//        WaitingForDoctor = false;
//        await Task.Delay(500);
//        ContentIsVisible = true;
//    }

//    private void SendLocalMessage(Message message)
//    {
//        if (string.IsNullOrEmpty(message.Content))
//            return;


//        #region сохранение фото в локальном хранилище
//        //if (message.ImageUrl != null)
//        //{
//        //    Task.Run(async () =>
//        //    {
//        //        var imabeBytes = await FileHelper.DownloadImageBytesAsync(message.ImageUrl);
//        //        if (imabeBytes != null)
//        //        {
//        //            var c = await FileHelper.SaveFileAsync(imabeBytes);
//        //            message.ImageUrl = c;
//        //        }
//        //    }).Wait();
//        //}
//        #endregion

//        Messages.Add(message);

//        SendingMessage = string.Empty;
//    }

//    private async void ToAudioMessagePage()
//    {
//        await Shell.Current.GoToAsync(nameof(AudioMessagePage));
//    }

//    private async void OnAbortChat()
//    {
//        await Shell.Current.DisplayAlert("Отмена", _abortMessage, "Ок");
//        await Disconnect();
//        await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
//    }

//    private async Task OnOpenPhotoMessage(string imageUrl)
//    {
//        await Shell.Current.GoToAsync($"{nameof(ImageBrowsePage)}?{nameof(ImageBrowseViewModel.ImageUrl)}={imageUrl}");
//    }



//    #region SendImage

//    async void PickImage()
//    {
//        var result = await FilePicker.PickAsync(new PickOptions
//        {
//            PickerTitle = "Выберите изображение",
//            FileTypes = FilePickerFileType.Images
//        });

//        if (result == null)
//            return;

//        var stream = await result.OpenReadAsync();

//        var imageBytes = FileHelper.StreamTyByte(stream);
//        string accessToken = await SecureStorage.Default.GetAsync("UserAccessToken");

//        var imageUrl = MedLinkConstants.FILE_BASE_PATH + "/" + await FileService.UploadFile(imageBytes, accessToken);

//        //await OnSendMessage("test", "tomy", "тестовый месседж", $"{MedLinkConstants.FILE_BASE_PATH}/{filePath}");
//        await SendImageMessage(imageUrl);
//    }

//    async Task SendImageMessage(string imageUrl)
//    {
//        try
//        {
//            var message = new Message()
//            {
//                SenderName = _senderName,
//                ReceiverName = _receiverName,
//                Content = "Фото",
//                ImageUrl = imageUrl
//            };
//            var serializedMessage = JsonConvert.SerializeObject(message);
//            await hubConnection.InvokeAsync("SendMessage", _senderName, _receiverName, serializedMessage);

//            //это лишнее убрал, чтобы не отправлять сообщение 2 раза
//            SendLocalMessage(message);
//        }
//        catch (Exception ex)
//        {

//        }
//    }


//    #endregion
//}
