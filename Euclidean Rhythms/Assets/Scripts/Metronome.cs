using UnityEngine;
using System.Collections; 

public class Metronome : MonoBehaviour {
	
	public Timer timer;
	//In seconds per beat
	private float tempoAdjustRate = 0.05f;
	public float TempoAdjustRate {
		get { return this.tempoAdjustRate; }
	}
	
	private float maxTempo = 0.05f;
	public float MaxTempo {
		get { return this.maxTempo; }
	}
	
	private float minTempo = 1.0f;
	public float MinTempo {
		get { return this.minTempo; }
	}
	
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
	// Use this for initialization
	void Start () {
		this.timer = new Timer();
		this.baseTempo = this.tempo;
	}
	
	// Update is called once per frame
	void Update () {
		this.timer.Update();
		if (this.timer.IsTiming) {
			if (this.timer.TimeElapsed > tempo) {
				beat++;
				this.timer.Reset();
			}
		}
	}
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
	
}
