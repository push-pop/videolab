using MidiJack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace nateturley.midible
{
    public static class MidiMessageExt
    {
        public static MidiMessage ToMidiMessage(this string msg)
        {
            var status = msg.Split(new string[] { "(", ",", ")" }, StringSplitOptions.RemoveEmptyEntries);
            var msgType = status[0];
            int channel = 0;
            int noteNum = 0;
            int value = 0;

            if (status.Length > 1)
            {
                channel = int.Parse(status[1]);
                noteNum = int.Parse(status[2]);
                value = int.Parse(status[3]);
            }

            byte stat = 0;
            stat |= (byte)(channel - 1);

            if (msgType.Equals("NoteOn"))
            {
                stat |= (0x9 << 4);
            }
            else if (msgType.Equals("NoteOff"))
            {
                stat |= (0x8 << 4);
            }
            else if (msgType.Equals("Control"))
            {
                stat |= (0xB << 4);
            }
            else if (msgType.Equals("Start"))
            {
                stat = 0xFA;
            }
            else if (msgType.Equals("Stop"))
            {
                stat = 0xFC;
            }

            return new MidiMessage()
            {
                endpoint = 0,
                status = (byte)(stat),
                data1 = (byte)noteNum,
                data2 = (byte)value
            };
        }
    }

    public class AndroidMidiBleShim : MonoBehaviour
    {
        public const string PluginBundleID = "com.turley.nate.midible.BLEMidiDevice";

        public MidiSource MidiSource
        {
            get
            {
                if (_midiSource == null)
                    _midiSource = FindObjectOfType<MidiSource>();

                return _midiSource;
            }
        }

        [SerializeField]
        MidiSource _midiSource;

        AndroidMidiCallbacks _callbacks;

        public static Action<MidiBleDevice> OnDeviceAdded;
        public static Action<MidiMessage> OnMidiMessage;

        public List<MidiBleDevice> devices = new List<MidiBleDevice>();

        [SerializeField]
        float _scanTime = 10f;

        Coroutine _scanTimerRoutine;
        Coroutine _midiTestRoutine;

        AndroidJavaClass ajc;

        void Awake()
        {
            Debug.Log("Hello MidiBLE");

            _callbacks = new AndroidMidiCallbacks();

            _callbacks.OnDeviceFound += (device) =>
            {
                if (!devices.Contains(device))
                {
                    devices.Add(device);
                    if (OnDeviceAdded != null)
                        OnDeviceAdded.Invoke(device);
                }
            };

            _callbacks.OnMidiMessageReceived += (msg) =>
            {
                HandleMidiMessage(msg);
            };
        }

        public void StopDeviceSearch()
        {
            if (Application.platform.Equals(RuntimePlatform.Android))
                ajc.CallStatic("StopDeviceSearch");

            _scanTimerRoutine = null;
        }

        public void StartDeviceSearch()
        {
#if UNITY_EDITOR
            var testDev = new MidiBleDevice
            {
                Index = 0,
                Name = "TestDevice",
                Address = "025380:324:52"
            };

            OnDeviceAdded(testDev);
#endif

            if (_scanTimerRoutine != null)
                StopCoroutine(_scanTimerRoutine);

            _scanTimerRoutine = StartCoroutine(ScanTimer());

            if (Application.platform.Equals(RuntimePlatform.Android))
            {
                ajc = new AndroidJavaClass(PluginBundleID);
                ajc.CallStatic("SearchForDevices", _callbacks);
            }

            if (Application.platform.Equals(RuntimePlatform.WindowsEditor))
                StartCoroutine(DebugMidiRoutine());
        }

        IEnumerator ScanTimer()
        {
            yield return new WaitForSeconds(_scanTime);

            StopDeviceSearch();
        }

        IEnumerator DebugMidiRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(.1f);
                HandleMidiMessage("NoteOn(2, 53, 100)".ToMidiMessage());

                yield return new WaitForSeconds(.2f);
                HandleMidiMessage("NoteOn(1, 64, 34)".ToMidiMessage());

                yield return new WaitForSeconds(.2f);
                HandleMidiMessage("Stop()".ToMidiMessage());
            }
        }

        private void HandleMidiMessage(MidiMessage m)
        {
            if (OnMidiMessage != null)
                OnMidiMessage.Invoke(m);

            if (MidiSource != null)
                MidiSource.msgQueue.Enqueue(m);
        }

        internal void OpenConnection(MidiBleDevice newDevice)
        {
            Debug.Log("Attempting to open connection for " + newDevice.Name);

            if (Application.platform.Equals(RuntimePlatform.Android))
                ajc.CallStatic("OpenDevice", newDevice.Index, _callbacks);

#if UNITY_EDITOR
            StartTestMidi();
#endif
        }

        private void StartTestMidi()
        {
            if (_midiTestRoutine != null)
                StopCoroutine(_midiTestRoutine);

            _midiTestRoutine = StartCoroutine(DebugMidiRoutine());
        }
    }
}