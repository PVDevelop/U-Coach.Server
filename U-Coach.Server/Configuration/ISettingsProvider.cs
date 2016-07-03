namespace PVDevelop.UCoach.Server.Configuration
{
    /// <summary>
    /// Предоставляет настройки
    /// </summary>
    public interface ISettingsProvider<T>
    {
        T Settings { get; }
    }
}
