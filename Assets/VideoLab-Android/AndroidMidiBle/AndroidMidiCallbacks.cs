using MidiJack;
using System;
using UnityEngine;

namespace nateturley.midible
{
    public class AndroidMidiCallbacks : AndroidJavaProxy
    {
        public const string CallbackBundleID = "com.turley.nate.midible.MidiCallback";

        public AndroidMidiCallbacks() : base(CallbackBundleID) { }

        public Action<MidiMessage> OnMidiMessageReceived;

        public Action<MidiBleDevice> OnDeviceFound;
        public Action<MidiBleDevice> OnDeviceOpened;
        public Action<MidiBleDevice> OnDeviceClosed;

        public Action<string> OnError;
        public Action<string> OnMessage;

        public void SendMidiMessage(string msg)
        {
            if (OnMidiMessageReceived != null)
                OnMidiMessageReceived.Invoke(msg.ToMidiMessage());
        }

        public void DeviceAdded(string msg)
        {
            var device = JsonUtility.FromJson<MidiBleDevice>(msg);
            Debug.LogFormat("[AndroidMidiCallback] OnDeviceAdded:{0}", device.Name);

            if (OnDeviceFound != null)
                OnDeviceFound.Invoke(device);
        }

        public void DeviceOpened(string msg)
        {
            var device = JsonUtility.FromJson<MidiBleDevice>(msg);
            Debug.LogFormat("[AndroidMidiCallback] Device Opened:{0}", device.Name);

            if (OnDeviceOpened != null)
                OnDeviceOpened.Invoke(device);

        }

        //Currently this callback will never get called
        public void DeviceClosed(string msg)
        {
            var device = JsonUtility.FromJson<MidiBleDevice>(msg);
            Debug.LogFormat("[AndroidMidiCallback] Device Closed:{0}", device.Name);

            if (OnDeviceClosed != null)
                OnDeviceClosed.Invoke(device);
        }

        //Currently this callback will never be called
        public void Error(string msg)
        {
            Debug.LogErrorFormat("[AndroidMidiCallback] Got Error:{0}", msg);
        }

        //Currently this callback will never be called
        public void SendMessage(string msg)
        {
            Debug.LogFormat("[AndroidMidiCallback] Got Message:{0}", msg);
        }
    }
}