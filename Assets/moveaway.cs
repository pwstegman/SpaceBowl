using UnityEngine;
using System.Collections;

public class moveaway : MonoBehaviour {

	// Use this for initialization
	void Start () {
		rigidbody.AddForce(new Vector3(0,25,1000));
	}
	
	// Update is called once per frame
	void Update () {

	}
}
