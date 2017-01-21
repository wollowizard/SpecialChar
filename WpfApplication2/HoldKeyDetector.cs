using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WpfApplication2
{
    class HoldKeyDetector
    {

        private Action<int, string> holdingCallback = null;
        private static Action<int, string> holdingCallbackStatic = null;

        public HoldKeyDetector(Action<int, string> holdingCallback) {
            this.holdingCallback = holdingCallback;
            holdingCallbackStatic = holdingCallback;
        }


        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;

        static HoldKeyDetector(){
            SetHook(_proc);
        }
        
        private static LowLevelKeyboardProc _proc = HookCallback;

        private static IntPtr _hookID = IntPtr.Zero;
        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);


        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long Timestamp()
        {
            DateTime localDateTime, univDateTime;
            localDateTime = DateTime.Now;
            univDateTime = localDateTime.ToUniversalTime();
            return (long)(univDateTime - UnixEpoch).TotalMilliseconds;
        }


        
        private static int holdingKey = -1;
        private static long firstPressOfHoldingKey = -1;
        private static Boolean isHoldingKey = false;



        private static Boolean keyDown(int code) {

            if (code != holdingKey)
            {
                isHoldingKey = false;
                holdingKey = code;
                firstPressOfHoldingKey = Timestamp();
                return true;
            }
            else {

                if (isHoldingKey)
                    return false;

                long diff = Timestamp() - firstPressOfHoldingKey;
                if (diff > 1000) {
                    Console.WriteLine("Long pressed key " + code + " for ms " + diff);
                    isHoldingKey = true;
                    char c1 = (char)1;
                    string s = "";
                    s += c1;
                    holdingCallbackStatic(code, s);
                }
                return false;
            }
        }

        private static void keyUp(int code)
        {
            if (code == holdingKey) {
                holdingKey = -1;
                firstPressOfHoldingKey = -1;
                isHoldingKey = false;
            }
        }


        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
           
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Console.WriteLine("code " + vkCode);

                Boolean isFirstPressOfKey = keyDown(vkCode);

                Console.WriteLine("isFirstPressOfKey " + isFirstPressOfKey);
                if (!isFirstPressOfKey)
                    return (IntPtr)1;
                
                                               
            }


            if (nCode >= 0 && wParam == (IntPtr)WM_KEYUP)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                //Console.WriteLine(vkCode);
                keyUp(vkCode);
                Console.WriteLine("keyup " );
            }

            Console.WriteLine("letting go " + lParam);
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
        
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }

}
