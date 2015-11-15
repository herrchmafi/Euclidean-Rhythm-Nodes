using UnityEngine;
using System.Collections;

public class Timer {
	private float seconds = 0.0f;
	public float Seconds {
		get { return this.seconds; }
		set { this.seconds = value; }
	}
	private bool isTiming = false;
	public bool IsTiming {
		get { return this.isTiming; }
	}
	
	public void Update() {
		if (this.isTiming) {
			seconds += Time.deltaTime;
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
		this.seconds = 0.0f;
	}
	
}
