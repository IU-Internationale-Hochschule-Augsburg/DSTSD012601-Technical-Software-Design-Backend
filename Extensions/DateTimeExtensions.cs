namespace Subscription_Control_Backend.Extensions;

public static class DateTimeExtensions
{
    /// <summary>
    /// Normalisiert einen Wert auf UTC, damit Npgsql ihn als timestamptz speichern kann.
    /// Lokale Werte werden umgerechnet, Werte ohne Zeitzonenangabe als UTC interpretiert.
    /// </summary>
    public static DateTime ToUtc(this DateTime value) => value.Kind switch
    {
        DateTimeKind.Utc => value,
        DateTimeKind.Local => value.ToUniversalTime(),
        _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
    };

    public static DateTime? ToUtc(this DateTime? value) => value?.ToUtc();
}