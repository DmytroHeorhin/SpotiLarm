using System.Diagnostics;

namespace SpotiLarm
{
    public static class SystemSound
    {
        public static void DecreaseVolume() 
        { 
            CallSetVolumeUtil("20");
        }

        public static void IncreaseVolume() 
        { 
            CallSetVolumeUtil("+1");
        }

        private static void CallSetVolumeUtil(string argument)
        {
            Process.Start(new ProcessStartInfo("SetVol.exe", argument) { CreateNoWindow = true });
        }
    }
}