using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

	GameObject go;

	Vector3 startPos;
	Quaternion startRotation;

	// Use this for initialization
	void Start () {
		go = GameObject.Find("gm");
		startPos=transform.position;
		startRotation = transform.rotation;
		Debug.Log ("Stored position "+startPos);
	}

	// Update is called once per frame
	private bool canreset = true;
	void Update () {
		bool reseteh;
		reseteh = go.GetComponent<gm> ().canreset ();
		if (reseteh == true) {
			canreset = false;
			reset ();
		}
	}



	public void reset(){
		transform.position = startPos;
		transform.rotation = startRotation;
		transform.rigidbody.velocity = Vector3.zero;
		transform.rigidbody.angularVelocity = Vector3.zero;
	}
}
