namespace MedLinkApp.ViewModels;

internal class ChatViewModel : BaseViewModel
{
    public ChatViewModel()
    {
        WaitingForDoctor = true;
        ContentIsVisible = false;
        _isTimerRunning = false;

        Messages = new ObservableCollection<Message>();
        hubConnection = new HubConnectionBuilder()
            .WithUrl(MedLinkConstants.SERVER_ROOT_URL + "/chatHub")
            .Build();

        Task.Run(async () =>
        {
            accessToken = await SecureStorage.Default.GetAsync("UserAccessToken");

            senderId = int.Parse(await SecureStorage.Default.GetAsync("UserId"));
            receiverId = int.Parse(await SecureStorage.Default.GetAsync("DoctorId"));

            await Connect();

            await SendConfirmMessage();
        }).GetAwaiter().OnCompleted(() =>
        {

        });

        SendMessage = new Command(async () =>
        {
            await OnSendMessage();
        });

        hubConnection.Closed += async (error) =>
        {
            await Task.Delay(5000);
            await Connect();
        };

        hubConnection.On<int, int, string>("ReceiveMessage", (senderId, receiverId, message) =>
        {
            if (_isConfirmed)
            {
                if (!_isTimerRunning)
                {
                    StartCountDownTimer();
                    _isTimerRunning = !_isTimerRunning;
                }

                SendLocalMessage(senderId, receiverId, message);
            }
            else
                Task.Run(async () => await ConsultationConfirmed());
        });
    }

    int senderId, receiverId;

    string accessToken;

    HubConnection hubConnection;
    public Command SendMessage { get; }

    private string _message;
    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }
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

    private bool _isConfirmed;
    private bool _isTimerRunning;

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
            await hubConnection.InvokeAsync("SendMessage", senderId, receiverId, Message);

            SendLocalMessage(senderId, receiverId, Message);
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
            await hubConnection.InvokeAsync("SendMessage", senderId, 
                receiverId, MedLinkConstants.CONFIRM_MESSAGE);
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

    private void SendLocalMessage(int senderId, int receiverId, string message)
    {
        Message = string.Empty;

        Messages.Insert(0, new Message
        {
            Content = message
        });
    }
}
