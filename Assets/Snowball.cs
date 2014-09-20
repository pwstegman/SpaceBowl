using UnityEngine;
using System.Collections;
using Pose = Thalmic.Myo.Pose;

public class Snowball : MonoBehaviour {

	public Material snow;
	private Material basematerial;
	public GameObject myo = null;
	public Transform ball;
	private bool hasBall = false;

	void Start(){
		basematerial = renderer.material;
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.name == "Plane") {
			Debug.Log ("Snowball"+other.gameObject.name);
			renderer.material = snow;
			hasBall = true;
		}
	}

	private Pose _lastPose = Pose.Unknown;

	void Update(){
		ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo> ();
		if (thalmicMyo.pose != _lastPose) {
			_lastPose = thalmicMyo.pose;
			if(thalmicMyo.pose == Pose.FingersSpread && hasBall == true){
				renderer.material = basematerial;
				Instantiate(ball, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
				hasBall = false;
			}
			// Vibrate the Myo armband when a fist is made.
			/*if (thalmicMyo.pose == Pose.Fist) {
                thalmicMyo.Vibrate (VibrationType.Medium);

            // Change material when wave in, wave out or thumb to pinky poses are made.
            } else if (thalmicMyo.pose == Pose.WaveIn) {
                renderer.material = waveInMaterial;
            } else if (thalmicMyo.pose == Pose.WaveOut) {
                renderer.material = waveOutMaterial;
            } else if (thalmicMyo.pose == Pose.ThumbToPinky) {
                renderer.material = thumbToPinkyMaterial;
            }*/
		}
	}
}
