using System;

namespace Core.Tools
{
    public static class TimeConvertionTools
    {
        private const string TIME_FORMAT = "0";
        private const float SECS_IN_MINUTE = 60f;
        private const float MINUTES_IN_HOUR = 60f;
        private const float SECS_IN_HOUR = 3600f;

        public static string ConvertSeconds(float seconds)
        {
            var minutes = (int)(seconds / SECS_IN_MINUTE);
            var hours = (int)(minutes / MINUTES_IN_HOUR);
            var sec = seconds - ((minutes * SECS_IN_MINUTE) + (hours * SECS_IN_HOUR)); 

            if (hours > 0)
            {
                return $"{hours.ToString(TIME_FORMAT)}h {minutes.ToString(TIME_FORMAT)}min";
            }
            else if (minutes > 0)
            {
                return $"{minutes.ToString(TIME_FORMAT)}min {sec.ToString(TIME_FORMAT)}sec";
            }
            else
            {
                return $"{sec.ToString(TIME_FORMAT)}sec left";
            }
        }
    }
}