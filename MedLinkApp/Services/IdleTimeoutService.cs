namespace MedLinkApp.Services;

internal class IdleTimeoutService
{
    private const int IDLE_TIMEOUT_MINUTES = 10;
    private const int IDLE_TIMEOUT_SECONDS = 10000;
    private static readonly TimeSpan IdleTimeout = TimeSpan.FromSeconds(IDLE_TIMEOUT_SECONDS);

    private bool _isTimerRunning;
    private Timer _idleTimer;

    public void StartTimer()
    {
        if (_isTimerRunning)
            return;

        App.Current.Dispatcher.StartTimer(IdleTimeout, OnIdleTimeoutElapsed);
        _isTimerRunning = true;
    }

    public void ResetTimer()
    {
        _isTimerRunning = false;
    }

    private bool OnIdleTimeoutElapsed()
    {
        App.Current.Dispatcher.Dispatch(async () =>
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        });

        return false;
    }
}
