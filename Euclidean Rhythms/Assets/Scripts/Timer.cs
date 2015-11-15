using UnityEngine;
using System.Collections;

public class Timer {
	private float timeElapsed = 0.0f;
	public float TimeElapsed {
		get { return this.timeElapsed; }
		set { this.timeElapsed = value; }
	}
	private bool isTiming = false;
	public bool IsTiming {
		get { return this.isTiming; }
	}
	
	public void Update() {
		if (this.isTiming) {
			timeElapsed += Time.deltaTime;
		}
	}
	public void Start() {
		this.isTiming = true;
	}
	
	public void Stop() {
		this.isTiming = false;
		this.Reset();
	}
	
	public void Pause() {
		this.isTiming = false;
	}
	
	public void Reset() {
		this.timeElapsed = 0.0f;
	}
	
}
