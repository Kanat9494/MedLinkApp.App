namespace MedLinkApp.Helpers;

internal class SessionManager
{
    internal SessionManager()
    {
        this.SessionDuration = TimeSpan.FromSeconds(10);
        this._sessionExpirationTime = DateTime.FromFileTimeUtc(0);
    }

    static readonly Lazy<SessionManager> lazy = new Lazy<SessionManager>(() => new SessionManager());
    internal static SessionManager Instance { get => lazy.Value; }
    internal TimeSpan SessionDuration;
    internal EventHandler OnSessionExpired;

    DateTime _sessionExpirationTime;

    internal bool IsSessionActive { get; private set; }

    public async Task StartTrackSessionAsync()
    {
        this.IsSessionActive = true;

        ExtendSession();

        await StartSessionTimerAsync();
    }

    public void EndTrackSession()
    {
        this.IsSessionActive = false;
        this._sessionExpirationTime = DateTime.FromFileTimeUtc(0);
    }

    public void ExtendSession()
    {
        if (this.IsSessionActive == false)
            return;

        this._sessionExpirationTime = DateTime.Now.Add(this.SessionDuration);
    }

    async Task StartSessionTimerAsync()
    {
        if (IsSessionActive == false)
            return;

        while (DateTime.Now < this._sessionExpirationTime)
        {
            await Task.Delay(1000);
        }

        if (this.IsSessionActive && this.OnSessionExpired != null)
        {
            IsSessionActive = false;
            OnSessionExpired.Invoke(this, null);
        }
    }

    internal async Task LogoutAsync()
        => await Shell.Current.GoToAsync($"\\{nameof(LoginPage)}");
}
