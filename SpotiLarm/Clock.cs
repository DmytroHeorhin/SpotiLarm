namespace SpotiLarm
{
    internal static class Clock
    {
        public static void WaitHalfAMinute()
        {
            Thread.Sleep(TimeSpan.FromMinutes(0.5));
        }

        public static void WaitUntill(DateTime time)
        {
            var timeOfDay = time.TimeOfDay;
            TimeSpan timeSpanToWait;

            var currentTimeOfDay = DateTime.Now.TimeOfDay;

            var isTimePassedToday = currentTimeOfDay > timeOfDay;

            if (isTimePassedToday)
            {
                timeSpanToWait = TimeSpan.FromHours(24) - currentTimeOfDay + timeOfDay;
            }
            else
            {
                timeSpanToWait = timeOfDay - currentTimeOfDay;
            }

            Console.WriteLine($"Timer is set for {timeSpanToWait:%h} hour(s) and {timeSpanToWait:%m} minute(s) from now.");
            Thread.Sleep(timeSpanToWait);
        }
    }
}
