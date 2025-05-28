using System;
using System.Runtime.InteropServices;
using System.Text;

public class UsbEventService
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void UsbEventCallback(int eventType, IntPtr timeStamp, IntPtr message);

    [DllImport("edge_windows.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void register_event_callback(UsbEventCallback callback);

    [DllImport("edge_windows.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void start_usb_monitoring(byte[] dataPtr, UIntPtr dataLen, bool verify);

    [DllImport("edge_windows.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void stop_usb_monitoring();

    [DllImport("edge_windows.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool is_monitoring_active();

    private UsbEventCallback? _callback;

    public void StartMonitoring(string json, Action<int, string> onEvent, bool verify, int timeoutSeconds = 30)
    {
        _callback = new UsbEventCallback((int type, IntPtr ts, IntPtr msgPtr) =>
        {
            string msg = Marshal.PtrToStringAnsi(msgPtr) ?? "Unknown";
            onEvent?.Invoke(type, msg);
        });

        register_event_callback(_callback);
        byte[] data = Encoding.UTF8.GetBytes(json);
        start_usb_monitoring(data, (UIntPtr)data.Length, verify);

        int elapsed = 0;
        while (is_monitoring_active() && elapsed < timeoutSeconds)
        {
            Thread.Sleep(1000);
            elapsed++;
            if (elapsed % 5 == 0)
                onEvent?.Invoke(0, $"[INFO] Monitoring... ({elapsed}s)"); // eventType 0 = info
        }

        stop_usb_monitoring();
        onEvent?.Invoke(0, "Stopped monitoring."); // eventType 0 = info
    }
    public string GetEventTypeName(int eventType)
    {
        return eventType switch
        {
            1 => "DEVICE CONNECTED", // Working
            2 => "DEVICE DISCONNECTED",
            3 => "QRCODE SCANNED",
            4 => "DEVICE HKDF KEY",
            5 => "BLE CONNECTED", // Working
            6 => "BLE DISCONNECTED",
            7 => "BLE SENDING",
            8 => "BLE RECEIVING",
            9 => "DATA RECEIVED", // Working
            10 => "VERIFYING DATA",
            11 => "ERROR",
            _ => "UNKNOWN"
        };
    }
}
