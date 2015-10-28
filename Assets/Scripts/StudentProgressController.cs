using UnityEngine;
using System.Collections;

public class StudentProgressController : MonoBehaviour {

	public static StudentProgressController studentProgress;

	// Use this for initialization
	void Start () {
		if (studentProgress == null) {
			DontDestroyOnLoad (gameObject);
			studentProgress = this;
		} else if (studentProgress != this) {
			Destroy(gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SaveStepsProgressToDB(){
		Debug.Log ("Se va a guardar el progreso de un paso...");
	}

}
