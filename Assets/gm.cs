using UnityEngine;
using System.Collections;

public class gm : MonoBehaviour {

	private int bowls = 0;
	private bool reset = false;
	private float time = 0f;
	private bool timeron = false;
	private int lastBowl = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (bowls > 0 && timeron == false && bowls % 2 == 0 && bowls != lastBowl) {
			timeron = true;
			lastBowl = bowls;
		}

		if (bowls % 2 == 1) {
			reset = false;
		}

		if (timeron == true) {
			time += Time.deltaTime;
		}
		if (time >= 5f) {
			time = 0f;
			timeron = false;
			reset = true;
		}
	}

	private int resets = 0;

	public bool canreset(){
		if (reset == true) {
			return true;
		}
		return false;
	}

	public void addBowl(){
		bowls += 1;
		Debug.Log ("Added bowl");
	}
}
