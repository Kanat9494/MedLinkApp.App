namespace MedLinkApp.ViewModels;

internal class AudioMessageViewModel : BaseViewModel
{
    public AudioMessageViewModel()
    {
        BackToChatCommand = new Command(OnBackToChatPage);
    }

    public Command BackToChatCommand { get; }

    private async void OnBackToChatPage()
    {
        await Shell.Current.GoToAsync(nameof(ChatPage));
    }
}
