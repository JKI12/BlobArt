using System.Globalization;
using UnityEngine.UI;

public static class UIManager
{
    private static string template = "Song: {0}\nPopular Month: {1}\nTotal Plays: {2}";

    public static void UpdateLabel(Text label, Track track)
    {
        label.text = string.Format(template, track.Title, CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(track.Month + 1), track.Count);
    }
}
