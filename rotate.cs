using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roateScreen
{
    class Program
    {
        static void Main(string[] args)
        {
            int direction = NativeMethods.DMDO_DEFAULT;

            if ( args.Length != 0 )
            {
                if (args[0].Equals(@"up", StringComparison.OrdinalIgnoreCase))
                    direction = NativeMethods.DMDO_DEFAULT;
                else if (args[0].Equals(@"down", StringComparison.OrdinalIgnoreCase))
                    direction = NativeMethods.DMDO_180;
                else if (args[0].Equals(@"left", StringComparison.OrdinalIgnoreCase))
                    direction = NativeMethods.DMDO_270;
                else if (args[0].Equals(@"right", StringComparison.OrdinalIgnoreCase))
                    direction = NativeMethods.DMDO_90;
            }

            NativeMethods.RotateTo( direction );
        }
    }

    public struct DEVMODE
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string dmDeviceName;

        public short dmSpecVersion;
        public short dmDriverVersion;
        public short dmSize;
        public short dmDriverExtra;
        public int dmFields;
        public int dmPositionX;
        public int dmPositionY;
        public int dmDisplayOrientation;
        public int dmDisplayFixedOutput;
        public short dmColor;
        public short dmDuplex;
        public short dmYResolution;
        public short dmTTOption;
        public short dmCollate;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string dmFormName;

        public short dmLogPixels;
        public short dmBitsPerPel;
        public int dmPelsWidth;
        public int dmPelsHeight;
        public int dmDisplayFlags;
        public int dmDisplayFrequency;
        public int dmICMMethod;
        public int dmICMIntent;
        public int dmMediaType;
        public int dmDitherType;
        public int dmReserved1;
        public int dmReserved2;
        public int dmPanningWidth;
        public int dmPanningHeight;
    };

    public class NativeMethods
    {

        // PInvoke declaration for EnumDisplaySettings Win32 API
        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        public static extern int EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);

        // PInvoke declaration for ChangeDisplaySettings Win32 API
        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        public static extern int ChangeDisplaySettings(ref DEVMODE lpDevMode, int dwFlags);

        // helper for creating an initialized DEVMODE structure
        public static DEVMODE CreateDevmode()
        {
            DEVMODE dm = new DEVMODE();
            dm.dmDeviceName = new String(new char[32]);
            dm.dmFormName = new String(new char[32]);
            dm.dmSize = (short)Marshal.SizeOf(dm);
            return dm;
        }

        public static void RotateTo(int direction)
        {
            DEVMODE dm = CreateDevmode();
            EnumDisplaySettings(null, ENUM_CURRENT_SETTINGS, ref dm);

            if ( ( ( ( direction == DMDO_DEFAULT) || ( direction == DMDO_180) ) && ( dm.dmPelsHeight > dm.dmPelsWidth ) ) ||
                 ( ( ( direction == DMDO_270 )    || ( direction == DMDO_90 ) ) && ( dm.dmPelsHeight < dm.dmPelsWidth ) ) )
            {
                int temp = dm.dmPelsHeight;
                dm.dmPelsHeight = dm.dmPelsWidth;
                dm.dmPelsWidth = temp;
            }

            dm.dmDisplayOrientation = direction;
            ChangeDisplaySettings(ref dm, 0);
        }

        // constants
        public const int ENUM_CURRENT_SETTINGS = -1;
        public const int DISP_CHANGE_SUCCESSFUL = 0;
        public const int DISP_CHANGE_BADDUALVIEW = -6;
        public const int DISP_CHANGE_BADFLAGS = -4;
        public const int DISP_CHANGE_BADMODE = -2;
        public const int DISP_CHANGE_BADPARAM = -5;
        public const int DISP_CHANGE_FAILED = -1;
        public const int DISP_CHANGE_NOTUPDATED = -3;
        public const int DISP_CHANGE_RESTART = 1;
        public const int DMDO_DEFAULT = 0;
        public const int DMDO_90 = 1;
        public const int DMDO_180 = 2;
        public const int DMDO_270 = 3;
    }
}
