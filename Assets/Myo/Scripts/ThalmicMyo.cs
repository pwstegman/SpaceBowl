using UnityEngine;
using System.Collections;

using Arm = Thalmic.Myo.Arm;
using XDirection = Thalmic.Myo.XDirection;
using VibrationType = Thalmic.Myo.VibrationType;
using Pose = Thalmic.Myo.Pose;

// Represents a Myo armband. Myo's orientation is made available through transform.localRotation, and other properties
// like the current pose are provided explicitly below. All spatial data about Myo is provided following Unity
// coordinate system conventions (the y axis is up, the z axis is forward, and the coordinate system is left-handed).
public class ThalmicMyo : MonoBehaviour {

    // True if and only if Myo has detected that it is on an arm.
    public bool armRecognized;

    // The current arm that Myo is being worn on. An arm of Unknown means that Myo is unable to detect the arm
    // (e.g. because it's not currently being worn).
    public Arm arm;

    // The current direction of Myo's +x axis relative to the user's arm. A xDirection of Unknown means that Myo is
    // unable to detect the direction (e.g. because it's not currently being worn).
    public XDirection xDirection;

    // The current pose detected by Myo. A pose of Unknown means that Myo is unable to detect the pose (e.g. because
    // it's not currently being worn).
    public Pose pose = Pose.Unknown;

    // Myo's current accelerometer reading, representing the acceleration due to force on the Myo armband in units of
    // g (roughly 9.8 m/s^2) and following Unity coordinate system conventions.
    public Vector3 accelerometer;

    // Myo's current gyroscope reading, representing the angular velocity about each of Myo's axes in degrees/second
    // following Unity coordinate system conventions.
    public Vector3 gyroscope;

    // True if and only if this Myo armband has paired successfully, at which point it will provide data and a
    // connection with it will be maintained when possible.
    public bool isPaired {
        get { return _myo != null; }
    }

    // Vibrate the Myo with the provided type of vibration, e.g. VibrationType.Short or VibrationType.Medium.
    public void Vibrate (VibrationType type) {
        _myo.Vibrate (type);
    }

    void Start() {
    }

    void Update() {
        lock (_lock) {
            armRecognized = _myoArmRecognized;
            arm = _myoArm;
            xDirection = _myoXDirection;
            if (_myoQuaternion != null) {
                transform.localRotation = new Quaternion(_myoQuaternion.Y, _myoQuaternion.Z, -_myoQuaternion.X, -_myoQuaternion.W);
            }
            if (_myoAccelerometer != null) {
                accelerometer = new Vector3(_myoAccelerometer.Y, _myoAccelerometer.Z, -_myoAccelerometer.X);
            }
            if (_myoGyroscope != null) {
                gyroscope = new Vector3(_myoGyroscope.Y, _myoGyroscope.Z, -_myoGyroscope.X);
            }
            pose = _myoPose;
        }
    }

    void myo_OnArmRecognized(object sender, Thalmic.Myo.ArmRecognizedEventArgs e) {
        lock (_lock) {
            _myoArmRecognized = true;
            _myoArm = e.Arm;
            _myoXDirection = e.XDirection;
        }
    }

    void myo_OnArmLost(object sender, Thalmic.Myo.MyoEventArgs e) {
        lock (_lock) {
            _myoArmRecognized = false;
            _myoArm = Arm.Unknown;
            _myoXDirection = XDirection.Unknown;
        }
    }

    void myo_OnOrientationData(object sender, Thalmic.Myo.OrientationDataEventArgs e) {
        lock (_lock) {
            _myoQuaternion = e.Orientation;
        }
    }

    void myo_OnAccelerometerData(object sender, Thalmic.Myo.AccelerometerDataEventArgs e) {
        lock (_lock) {
            _myoAccelerometer = e.Accelerometer;
        }
    }

    void myo_OnGyroscopeData(object sender, Thalmic.Myo.GyroscopeDataEventArgs e) {
        lock (_lock) {
            _myoGyroscope = e.Gyroscope;
        }
    }

    void myo_OnPoseChange(object sender, Thalmic.Myo.PoseEventArgs e) {
        lock (_lock) {
            _myoPose = e.Pose;
        }
    }

    public Thalmic.Myo.Myo internalMyo {
        get { return _myo; }
        set {
            if (_myo != null) {
                _myo.ArmRecognized -= myo_OnArmRecognized;
                _myo.ArmLost -= myo_OnArmLost;
                _myo.OrientationData -= myo_OnOrientationData;
                _myo.AccelerometerData -= myo_OnAccelerometerData;
                _myo.GyroscopeData -= myo_OnGyroscopeData;
                _myo.PoseChange -= myo_OnPoseChange;
            }
            _myo = value;
            if (value != null) {
                value.ArmRecognized += myo_OnArmRecognized;
                value.ArmLost += myo_OnArmLost;
                value.OrientationData += myo_OnOrientationData;
                value.AccelerometerData += myo_OnAccelerometerData;
                value.GyroscopeData += myo_OnGyroscopeData;
                value.PoseChange += myo_OnPoseChange;
            }
        }
    }

    private Object _lock = new Object();

    private bool _myoArmRecognized = false;
    private Arm _myoArm = Arm.Unknown;
    private XDirection _myoXDirection = XDirection.Unknown;
    private Thalmic.Myo.Quaternion _myoQuaternion = null;
    private Thalmic.Myo.Vector3 _myoAccelerometer = null;
    private Thalmic.Myo.Vector3 _myoGyroscope = null;
    private Pose _myoPose = Pose.Unknown;

    private Thalmic.Myo.Myo _myo;
}
