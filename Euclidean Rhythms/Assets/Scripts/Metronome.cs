using UnityEngine;
using System.Collections; 

public class Metronome : MonoBehaviour {
	
	public Timer timer;
	//In seconds per beat
	public float tempoAdjustRate = 0.05f;

	public float maxTempo;
	
	public float minTempo;
	
	public float tempo, baseTempo;
	
	private int beat = 0;
	public int Beat {
		get { return this.beat; }
		set { this.beat = value; }
	}
	
	private bool isTicking = false;
	public bool IsTicking {
		get { return this.isTicking; }
		set { this.isTicking = value; }
	}
	
	#region Monodevelop
	// Use this for initialization
	void Start () {
		this.timer = new Timer();
		this.baseTempo = this.tempo;
	}
	
	// Update is called once per frame
	void Update () {
		if (this.timer.IsTiming) {
			this.timer.Update();
			if (this.timer.Seconds > tempo) {
				beat++;
				this.timer.Reset();
			}
		}
	}
	
	#endregion
	
	#region Controller Functions
	
	public void Begin() {
		this.timer.Start();
		this.isTicking = true;
	}
	
	public void Pause() {
		this.timer.Pause();
		this.isTicking = false;
	}
	
	public void Stop() {
		this.timer.Stop();
		this.timer.Reset();
		this.beat = 0;
		this.isTicking = false;
	}
	
	public void Reset() {
		this.timer.Reset();
		this.beat = 0;
		this.tempo = baseTempo;
	}
	
	public void SpeedUp() {
		this.tempo -= tempoAdjustRate;
		if (tempo < this.maxTempo) {
			this.tempo = this.maxTempo;
		}
	}
	
	public void SlowDown() {
		this.tempo += tempoAdjustRate;
		if (this.tempo > this.minTempo) {
			this.tempo = this.minTempo;
		}
	}
	
	#endregion
	
}
