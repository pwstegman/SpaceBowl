using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Thalmic.Myo
{
    public class Myo
    {
        private readonly Hub _hub;
        private IntPtr _handle;

        internal Myo(Hub hub, IntPtr handle)
        {
            Debug.Assert(handle != IntPtr.Zero, "Cannot construct Myo instance with null pointer.");

            _hub = hub;
            _handle = handle;
        }

        public event EventHandler<MyoEventArgs> Connected;

        public event EventHandler<MyoEventArgs> Disconnected;

        public event EventHandler<ArmRecognizedEventArgs> ArmRecognized;

        public event EventHandler<MyoEventArgs> ArmLost;

        public event EventHandler<PoseEventArgs> PoseChange;

        public event EventHandler<OrientationDataEventArgs> OrientationData;

        public event EventHandler<AccelerometerDataEventArgs> AccelerometerData;

        public event EventHandler<GyroscopeDataEventArgs> GyroscopeData;

        public event EventHandler<RssiEventArgs> Rssi;

        internal Hub Hub
        {
            get { return _hub; }
        }

        internal IntPtr Handle
        {
            get { return _handle; }
        }

        public void Vibrate(VibrationType type)
        {
            libmyo.vibrate(_handle, (libmyo.VibrationType)type, IntPtr.Zero);
        }

        public void RequestRssi()
        {
            libmyo.request_rssi(_handle, IntPtr.Zero);
        }

        internal void HandleEvent(libmyo.EventType type, DateTime timestamp, IntPtr evt)
        {
            switch (type)
            {
                case libmyo.EventType.Connected:
                    if (Connected != null)
                    {
                        Connected(this, new MyoEventArgs(this, timestamp));
                    }
                    break;

                case libmyo.EventType.Disconnected:
                    if (Disconnected != null)
                    {
                        Disconnected(this, new MyoEventArgs(this, timestamp));
                    }
                    break;

                case libmyo.EventType.ArmRecognized:
                    if (ArmRecognized != null)
                    {
                        Arm arm = (Arm)libmyo.event_get_arm(evt);
                        XDirection xDirection = (XDirection)libmyo.event_get_x_direction(evt);

                        ArmRecognized(this, new ArmRecognizedEventArgs(this, timestamp, arm, xDirection));
                    }
                    break;

                case libmyo.EventType.ArmLost:
                    if (ArmLost != null)
                    {
                        ArmLost(this, new MyoEventArgs(this, timestamp));
                    }
                    break;

                case libmyo.EventType.Orientation:
                    if (AccelerometerData != null)
                    {
                        float x = libmyo.event_get_accelerometer(evt, 0);
                        float y = libmyo.event_get_accelerometer(evt, 1);
                        float z = libmyo.event_get_accelerometer(evt, 2);

                        var accelerometer = new Vector3(x, y, z);
                        AccelerometerData(this, new AccelerometerDataEventArgs(this, timestamp, accelerometer));
                    }
                    if (GyroscopeData != null)
                    {
                        float x = libmyo.event_get_gyroscope(evt, 0);
                        float y = libmyo.event_get_gyroscope(evt, 1);
                        float z = libmyo.event_get_gyroscope(evt, 2);

                        var gyroscope = new Vector3(x, y, z);
                        GyroscopeData(this, new GyroscopeDataEventArgs(this, timestamp, gyroscope));
                    }
                    if (OrientationData != null)
                    {
                        float x = libmyo.event_get_orientation(evt, libmyo.OrientationIndex.X);
                        float y = libmyo.event_get_orientation(evt, libmyo.OrientationIndex.Y);
                        float z = libmyo.event_get_orientation(evt, libmyo.OrientationIndex.Z);
                        float w = libmyo.event_get_orientation(evt, libmyo.OrientationIndex.W);

                        var orientation = new Quaternion(x, y, z, w);
                        OrientationData(this, new OrientationDataEventArgs(this, timestamp, orientation));
                    }
                    break;

                case libmyo.EventType.Pose:
                    if (PoseChange != null)
                    {
                        var pose = (Pose)libmyo.event_get_pose(evt);
                        PoseChange(this, new PoseEventArgs(this, timestamp, pose));
                    }
                    break;

                case libmyo.EventType.Rssi:
                    if (Rssi != null)
                    {
                        var rssi = libmyo.event_get_rssi(evt);
                        Rssi(this, new RssiEventArgs(this, timestamp, rssi));
                    }
                    break;
            }
        }
    }

    public enum Arm
    {
        Right,
        Left,
        Unknown
    }

    public enum XDirection
    {
        TowardWrist,
        TowardElbow,
        Unknown
    }

    public enum VibrationType
    {
        Short,
        Medium,
        Long
    }
}
