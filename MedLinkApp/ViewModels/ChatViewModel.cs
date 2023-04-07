using MedLinkApp.Helpers;
using System.Net;
using System.Threading;

namespace MedLinkApp.ViewModels;

internal class ChatViewModel : BaseViewModel
{
    public ChatViewModel()
    {
        WaitingForDoctor = true;
        ContentIsVisible = false;
        _isTimerRunning = false;
        IsImageVisible = false;

        Task.Run(async () =>
        {
            accessToken = await SecureStorage.Default.GetAsync("UserAccessToken");

            _senderName = await SecureStorage.Default.GetAsync("UserName");
            _receiverName = await SecureStorage.Default.GetAsync("DoctorAccountName");
        }).Wait();

        hubConnection = new HubConnectionBuilder()
            .WithUrl(MedLinkConstants.SERVER_ROOT_URL + $"/chatHub/{_senderName}")
            .Build();

        Messages = new ObservableCollection<Message>();


        Task.Run(async () =>
        {
            await Connect();

            DoctorFullName = await SecureStorage.Default.GetAsync("DoctorFullName");

            await SendConfirmMessage();
        }).GetAwaiter().OnCompleted(() =>
        {

        });

        SendMessage = new Command(async () =>
        {
            await OnSendMessage();
        });

        OpenAudioMessagePage = new Command(ToAudioMessagePage);
        OpenPhotoMessagePage = new Command(PickImage);
        OpenPhotoMessageCommand = new Command<string>(async (imageUrl) => await OnOpenPhotoMessage(imageUrl));

        hubConnection.Closed += async (error) =>
        {
            await Task.Delay(5000);
            await Connect();
        };

        //hubConnection.On<string, string, string>("ReceiveMessage", (senderName, receiverName, message) =>
        //{
        //    if (_isConfirmed)
        //    {
        //        if (!_isTimerRunning)
        //        {
        //            StartCountDownTimer();
        //            _isTimerRunning = !_isTimerRunning;
        //        }

        //        SendLocalMessage(message);
        //    }
        //    else
        //        Task.Run(async () => await ConsultationConfirmed());
        //});

        hubConnection.On<string, string, string>("ReceiveMessage", (senderName, receiverName, jsonMessage) =>
        {
            try
            {
                var message = JsonConvert.DeserializeObject<Message>(jsonMessage);
                if (_isConfirmed)
                {
                    if (!_isTimerRunning)
                    {
                        StartCountDownTimer();
                        _isTimerRunning = !_isTimerRunning;
                    }

                    SendLocalMessage(message);
                }
                else
                    Task.Run(async () => await ConsultationConfirmed());
            }
            catch
            {

            }
        });
    }

    string accessToken;

    HubConnection hubConnection;
    public Command SendMessage { get; }
    public Command OpenAudioMessagePage { get; }
    public Command OpenPhotoMessagePage { get; }
    public Command<string> OpenPhotoMessageCommand { get; }

    private string _sendingMessage;
    public string SendingMessage
    {
        get => _sendingMessage;
        set => SetProperty(ref _sendingMessage, value);
    }
    private string _senderName;
    private string _receiverName;

    private string _chatTimer;
    public string ChatTimer
    {
        get => _chatTimer;
        set => SetProperty(ref _chatTimer, value);
    }

    string cTimer;
    DateTime endTime;
    System.Timers.Timer timer;

    private bool _contentIsVisible;
    public bool ContentIsVisible
    {
        get => _contentIsVisible;
        set => SetProperty(ref _contentIsVisible, value);
    }

    private bool _waitingForDoctor;
    public bool WaitingForDoctor
    {
        get => _waitingForDoctor;
        set => SetProperty(ref _waitingForDoctor, value);
    }

    private string _doctorFullName;
    public string DoctorFullName
    {
        get => _doctorFullName;
        set => SetProperty(ref _doctorFullName, value);
    }

    private bool _isConfirmed;
    private bool _isTimerRunning;

    public bool _isImageVisible;
    public bool IsImageVisible
    {
        get => _isImageVisible;
        set => SetProperty(ref _isImageVisible, value);
    }

    public ObservableCollection<Message> Messages { get; set; }

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

        if ((timeSpan.TotalMilliseconds < 0) || (timeSpan.TotalMilliseconds < 1000))
            timer.Stop();
    }

    async Task OnSendMessage()
    {
        try
        {
            
            var message = new Message()
            {
                SenderName = _senderName,
                ReceiverName = _receiverName,
                Content = SendingMessage,
                //ImageUrl = "https://www.google.com/images/logos/ps_logo2.png"

            };
            var serializedMessage = JsonConvert.SerializeObject(message);
            await hubConnection.InvokeAsync("SendMessage", _senderName, _receiverName, serializedMessage);

            //это лишнее убрал, чтобы не отправлять сообщение 2 раза
            SendLocalMessage(message);
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

    async Task SendConfirmMessage()
    {
        try
        {
            await hubConnection.InvokeAsync("SendMessage", _senderName, _receiverName, JsonConvert.SerializeObject(new Message
            {
                SenderName = _senderName,
                ReceiverName = _receiverName,
                Content = MedLinkConstants.CONFIRM_MESSAGE
            }));

            //await hubConnection.InvokeAsync("SendMessage", _senderName, _receiverName, SendingMessage);
        }
        catch (Exception ex)
        {
            
        }
    }

    private async Task ConsultationConfirmed()
    {
        _isConfirmed = true;

        WaitingForDoctor = false;
        await Task.Delay(500);
        ContentIsVisible = true;

        StartCountDownTimer();
    }

    private void SendLocalMessage(Message message)
    {
        if (string.IsNullOrEmpty(message.Content))
            return;


        #region сохранение фото в локальном хранилище
        //if (message.ImageUrl != null)
        //{
        //    Task.Run(async () =>
        //    {
        //        var imabeBytes = await FileHelper.DownloadImageBytesAsync(message.ImageUrl);
        //        if (imabeBytes != null)
        //        {
        //            var c = await FileHelper.SaveFileAsync(imabeBytes);
        //            message.ImageUrl = c;
        //        }
        //    }).Wait();
        //}
        #endregion

        Messages.Add(message);

        SendingMessage = string.Empty;
    }

    private async void ToAudioMessagePage()
    {
        await Shell.Current.GoToAsync(nameof(AudioMessagePage));
    }

    private async Task OnOpenPhotoMessage(string imageUrl)
    {
        await Shell.Current.GoToAsync($"{nameof(ImageBrowsePage)}?{nameof(ImageBrowseViewModel.ImageUrl)}={imageUrl}");
    }



    #region SendImage

    async void PickImage()
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Выберите изображение",
            FileTypes = FilePickerFileType.Images
        });

        if (result == null)
            return;

        var stream = await result.OpenReadAsync();

        var imageBytes = FileHelper.StreamTyByte(stream);
        string accessToken = await SecureStorage.Default.GetAsync("UserAccessToken");

        var imageUrl = MedLinkConstants.FILE_BASE_PATH + "/" + await FileService.UploadFile(imageBytes, accessToken);

        //await OnSendMessage("test", "tomy", "тестовый месседж", $"{MedLinkConstants.FILE_BASE_PATH}/{filePath}");
        await SendImageMessage(imageUrl);
    }

    async Task SendImageMessage(string imageUrl)
    {
        try
        {
            var message = new Message()
            {
                SenderName = _senderName,
                ReceiverName = _receiverName,
                Content = "Фото",
                ImageUrl = imageUrl
            };
            var serializedMessage = JsonConvert.SerializeObject(message);
            await hubConnection.InvokeAsync("SendMessage", _senderName, _receiverName, serializedMessage);

            //это лишнее убрал, чтобы не отправлять сообщение 2 раза
            SendLocalMessage(message);
        }
        catch (Exception ex)
        {

        }
    }


    #endregion
}
