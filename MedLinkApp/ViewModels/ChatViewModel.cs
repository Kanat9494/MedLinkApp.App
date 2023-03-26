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

        SendMessage = new Command<string>(async (message) =>
        {
            await OnSendMessage(message);
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
    }

    HubConnection hubConnection;
    public Command<string> SendMessage { get; }
    public string Message { get; set; }

    public ObservableCollection<Message> Messages { get; set; }

    async Task OnSendMessage(string message)
    {
        try
        {
            await hubConnection.InvokeAsync("SendMessage", Message);
            Message = string.Empty;
        }
        catch (Exception ex)
        {

        }
    }

    async Task Connect()
    {
        await hubConnection.StartAsync();
    }

    async Task Disconnect()
    {
        await hubConnection.StopAsync();
    }
}
