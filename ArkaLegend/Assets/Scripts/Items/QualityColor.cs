using System.Collections.Generic;

public static class QualityColor
{
    private static Dictionary<Quality, string> quality2colorMap = new Dictionary<Quality, string> {
        {Quality.Common, "#FFFFFF"}, {Quality.Uncommon, "#00FF00"},
        {Quality.Rare, "#0000FF"}, {Quality.Epic, "#FF00FF"},
        {Quality.Legend, "#FFFF00"}
    };

    public static Dictionary<Quality, string> Quality2colorMap
    {
        get
        {
            return quality2colorMap;
        }
    }
}