using System;
using System.Runtime.InteropServices;

namespace Leap.Util
{
  public enum Direction : int { Invalid, Up, Down };
  public enum Status : int { Invalid, ErrorCannotAccessImages, Idle, SwipeBegin, SwipeUpdate, SwipeComplete, SwipeAbort, InfoQueueEmpty };

  [StructLayout(LayoutKind.Sequential)]
  public struct SystemWipeInfo
  {
    public Direction Direction;
    public Status Status;
    public float Progress;
  }

  public class SystemWipeRecognizerNative
  {
#   if UNITY_STANDALONE_OSX
      const CallingConvention LeapCallingConvention = CallingConvention.Cdecl;
#   else
      const CallingConvention LeapCallingConvention = CallingConvention.StdCall;
#   endif

    [UnmanagedFunctionPointer(LeapCallingConvention)]
    public delegate void CallbackSystemWipeInfoDelegate(SystemWipeInfo systemWipeInfo);

    [DllImport("SystemWipeRecognizerDll", CallingConvention = LeapCallingConvention)]
    public static extern void SetSystemWipeRecognizerCallback(IntPtr property);

    [DllImport("SystemWipeRecognizerDll", CallingConvention = LeapCallingConvention)]
    public static extern void EnableSystemWipeRecognizer();

    [DllImport("SystemWipeRecognizerDll", CallingConvention = LeapCallingConvention)]
    public static extern void DisableSystemWipeRecognizer();

    [DllImport("SystemWipeRecognizerDll", CallingConvention = LeapCallingConvention)]
    public static extern bool WasLastImageAccessOk();

    [DllImport("SystemWipeRecognizerDll", CallingConvention = LeapCallingConvention)]
    public static extern int GetFrameCount();

    [DllImport("SystemWipeRecognizerDll", CallingConvention = LeapCallingConvention)]
    public static extern SystemWipeInfo GetNextSystemWipeInfo();
  }
}
