using UnityEngine;
using System.Collections;
using Pose = Thalmic.Myo.Pose;

public class Snowball : MonoBehaviour {

	public Material snow;
	private Material basematerial;
	public GameObject myo = null;
	public Transform ball;
	private bool hasBall = false;
	GameObject go;

	void Start(){
		basematerial = renderer.material;
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.name == "Plane" && waitfornow == false) {
			Debug.Log ("Snowball"+other.gameObject.name);
			renderer.material = snow;
			hasBall = true;
		}
	}

	private Pose _lastPose = Pose.Unknown;
	private float timecount = 0f;
	private bool waitfornow = false;
	public Material stall = null;

	void Update(){

		if (waitfornow == true) {
			if(timecount > 5f){
				waitfornow = false;
				timecount = 0f;
				renderer.material = basematerial;
			}else{
				renderer.material = stall;
				timecount += Time.deltaTime;
				return;
			}
		}

		ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo> ();

		if (transform.position.y > 2.5 && hasBall == true) {
			renderer.material = basematerial;
			Instantiate(ball, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
			hasBall = false;
			waitfornow = true;

			go = GameObject.Find("gm");
			go.GetComponent<gm> ().addBowl ();

		}

		if (thalmicMyo.pose != _lastPose) {
			_lastPose = thalmicMyo.pose;
			if(thalmicMyo.pose == Pose.FingersSpread && hasBall == true){
				renderer.material = basematerial;
				Instantiate(ball, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
				hasBall = false;
				waitfornow = true;

				go = GameObject.Find("gm");
				go.GetComponent<gm> ().addBowl ();

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
