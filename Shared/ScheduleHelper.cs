using System.Globalization;

namespace Services
{
    public class ScheduleHelper()
    {
        public static void ValidateSchedule(int hour, int minute)
        {
            if (hour < 0 || hour > 23 || minute < 0 || minute > 59)
            {
                throw new ArgumentOutOfRangeException("hour", "La hora debe estar entre 0 y 23, y los minutos entre 0 y 59.");
            }
        }

        public static string FormatTimeSpan(TimeSpan timeSpan)
        {
            return timeSpan.ToString("hh\\:mm") + "h"; ;
        }

        public static string FormatHourMinute(int hour, int minute)
        {
            return $"{hour:D2}:{minute:D2}h";
        }

        public static TimeSpan ParseTimeSpan(string timeString)
        {
            // Eliminar la "h" al final de la cadena
            string timeWithoutH = timeString.Replace("h", "");

            // Intentar analizar la cadena con el formato @"hh\:mm"
            if (TimeSpan.TryParseExact(timeWithoutH, @"hh\:mm", CultureInfo.InvariantCulture, out TimeSpan timeSpan))
            {
                return timeSpan;
            }
            else
            {
                // Si el análisis falla, lanzar una excepción
                throw new FormatException($"Formato de hora inválido. Debe ser hh:mm -/-> {timeString} <-> {timeWithoutH}");
            }
        }

        public static (int hour, int minute) SplitHourMinute(string timeString)
        {
            var timeSpan = ParseTimeSpan(timeString);
            return (timeSpan.Hours, timeSpan.Minutes);
        }
    }
}
