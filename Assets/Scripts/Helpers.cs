using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class Helpers
{
    private static List<char> alphabet = new List<char> {
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k','l', 'm',
        'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
    };

    private static float RangeNumber(float oldValue, float oldMin, float oldMax, float newMin, float newMax)
    {
        return (((oldValue - oldMin) * (newMax - newMin)) / (oldMax - oldMin)) + newMin;
    }

    public static float ConvertMonthToStrength(int topMonth)
    {
        // Clamp from 1 - 4
        return RangeNumber(topMonth + 1, 1, 12, 1, 4);
    }

    public static float ConvertTotalPlaysToRoughness(int totalPlays, int maxPlays)
    {
        // Clamp to 1 - 5
        return RangeNumber(totalPlays, 1, maxPlays, 1, 5);
    }

    public static Vector3 ConvertIdToCoord(string id)
    {
        var idSanitized = new Regex("[0-9]").Replace(id, string.Empty).ToLower();

        var x = alphabet.FindIndex(i => i == idSanitized[0]) / 2;
        var y = alphabet.FindIndex(i => i == idSanitized[1]) / 2;
        var z = alphabet.FindIndex(i => i == idSanitized[2]) / 2;

        return new Vector3(x, y, z);
    }
}