using UnityEngine;
using System.Collections;

public abstract class Scene: MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}	

	public abstract void OnStateIn(Scene last);
	public abstract void OnStateOut(Scene next);
}
