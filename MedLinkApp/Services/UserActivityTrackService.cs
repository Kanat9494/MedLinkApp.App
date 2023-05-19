namespace MedLinkApp.Services;

internal class UserActivityTrackService
{
    internal UserActivityTrackService()
    {
        _sessionExpirationTime = 0;
        cancelTokenSource = new CancellationTokenSource();
        cancelToken = cancelTokenSource.Token;
    }

    static readonly Lazy<UserActivityTrackService> _lazy = new Lazy<UserActivityTrackService>(() => new UserActivityTrackService());
    internal CancellationTokenSource cancelTokenSource;
    internal CancellationToken cancelToken;
    int _sessionExpirationTime = 0;

    internal static UserActivityTrackService Instance { get => _lazy.Value; }
    bool IsSessionActive { get; set; }

    internal void StartSession()
    {
        Task userActivityTask = new Task(async () =>
        {
            await Task.Delay(5000);

            while (true)
            {
                if (cancelToken.IsCancellationRequested)
                    break;

                _sessionExpirationTime++;

                if (_sessionExpirationTime > 3)
                    await StopSession();

                await Task.Delay(5000);
            }
        }, cancelToken);

        userActivityTask.Start();
    }

    async Task StopSession()
    {
        cancelTokenSource.Cancel();
        cancelTokenSource.Dispose();

        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }

    internal void RestartSession()
    {
        _sessionExpirationTime = 0;
    }
}
