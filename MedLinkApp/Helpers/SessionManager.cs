namespace MedLinkApp.Helpers;

internal class SessionManager
{
    internal SessionManager()
    {
    }

    static readonly Lazy<SessionManager> lazy = new Lazy<SessionManager>(() => new SessionManager());
    internal static SessionManager Instance { get => lazy.Value; }
}
