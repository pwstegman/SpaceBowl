using UnityEngine;
using System.Collections;

public class moveaway : MonoBehaviour {

	// Use this for initialization
	void Start () {

		Debug.Log (transform.position);
		
		DestroyObject(gameObject,6);

		float xPos = transform.position.x;
		Debug.Log (xPos);

		if (xPos < 0.5f && xPos > -0.5f) {
			xPos = xPos / 3f;
		}

		rigidbody.AddForce(new Vector3(xPos*1000,25,3000));
	}
	
	// Update is called once per frame
	void Update () {
		rigidbody.AddForce(new Vector3(0,0,500*Time.deltaTime));
	}
}
