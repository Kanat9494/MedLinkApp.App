using System.Diagnostics;

namespace MedLinkApp.Helpers;

internal class SessionManager
{
    internal SessionManager()
    {
        _sessionExpirationTime = 0;
        cancelTokenSource = new CancellationTokenSource();
        cancelToken = cancelTokenSource.Token;
    }

    static readonly Lazy<SessionManager> lazy = new Lazy<SessionManager>(() => new SessionManager());
    internal static SessionManager Instance { get => lazy.Value; }
    bool IsSessionActive { get; set; }
    internal CancellationTokenSource cancelTokenSource;
    internal CancellationToken cancelToken;

    int _sessionExpirationTime = 0;

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
