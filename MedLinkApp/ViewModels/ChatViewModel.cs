namespace MedLinkApp.ViewModels;

internal class ChatViewModel : BaseViewModel
{
    public ChatViewModel()
    {
        Messages = new ObservableCollection<Message>();
        hubConnection = new HubConnectionBuilder()
            .WithUrl(MedLinkConstants.SERVER_ROOT_URL + "/chatHub")
            .Build();

        Task.Run(async () =>
        {
            await Connect();
        }).Wait();

        SendMessage = new Command(async () =>
        {
            await OnSendMessage();
        });

        hubConnection.Closed += async (error) =>
        {
            await Task.Delay(5000);
            await Connect();
        };

        hubConnection.On<string>("ReceiveMessage", (message) =>
        {
            Messages.Add(new Message() { Content = message});
        });

        StartCountDownTimer();
    }

    HubConnection hubConnection;
    public Command SendMessage { get; }
    public string Message { get; set; }
    private string _chatTimer;
    public string ChatTimer
    {
        get => _chatTimer;
        set => SetProperty(ref _chatTimer, value);
    }

    string cTimer;
    DateTime endTime;
    System.Timers.Timer timer;

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

    public ObservableCollection<Message> Messages { get; set; }

    async Task OnSendMessage()
    {
        try
        {
            await hubConnection.InvokeAsync("SendMessage", Message);
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
}
