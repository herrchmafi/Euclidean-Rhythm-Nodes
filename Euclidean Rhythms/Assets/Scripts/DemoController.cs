using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DemoController : MonoBehaviour {
	private ArrayList brownBeats, analogBeats, hitBeats, synthBeats, bassBeats;
	private EuclidNode brownNodeRef, analogNodeRef, hitNodeRef, synthNodeRef, bassNodeRef;
	private Timer timer;
	private Metronome metronomeRef;
	private ArrayList nodeRefs;
	private bool isRunning = false;
	private int currentRhythm = 0;
	private float baseRhythmTime = 4.9f, currentRhythmTime;
	// Use this for initialization
	void Start () {
		this.brownBeats = new ArrayList();
		//Not sure if literals for this
		this.brownBeats.Add(new Vector2(16, 4));
		
		this.analogBeats = new ArrayList();
		this.analogBeats.Add(new Vector2(16, 7));
		
		this.hitBeats = new ArrayList();
		this.hitBeats.Add(new Vector2(16, 10));
		
		this.synthBeats = new ArrayList();
		this.synthBeats.Add(new Vector2(16, 6));
		
		this.bassBeats = new ArrayList();
		this.bassBeats.Add(new Vector2(16, 1));
		
		this.brownNodeRef = GameObject.FindGameObjectWithTag("BrownNode").GetComponent<EuclidNode>();
		this.analogNodeRef = GameObject.FindGameObjectWithTag("AnalogNode").GetComponent<EuclidNode>();
		this.hitNodeRef = GameObject.FindGameObjectWithTag("HitNode").GetComponent<EuclidNode>();
		this.synthNodeRef = GameObject.FindGameObjectWithTag("SynthNode").GetComponent<EuclidNode>();
		this.bassNodeRef = GameObject.FindGameObjectWithTag("BassNode").GetComponent<EuclidNode>();
		
		this.nodeRefs = new ArrayList();
		this.nodeRefs.Add(this.brownNodeRef);
		this.nodeRefs.Add(this.analogNodeRef);
		this.nodeRefs.Add(this.hitNodeRef);
		this.nodeRefs.Add(this.synthNodeRef);
		this.nodeRefs.Add(this.bassNodeRef);
		
		this.metronomeRef = GameObject.FindGameObjectWithTag("Metronome").GetComponent<Metronome>();
		this.currentRhythmTime = (this.metronomeRef.tempo /this.metronomeRef.baseTempo) * this.baseRhythmTime;
		this.timer = new Timer();
	}
	
	// Update is called once per frame
	void Update () {
		this.timer.Update();
		if (this.isRunning) {
			this.currentRhythmTime = (this.metronomeRef.tempo /this.metronomeRef.baseTempo) * this.baseRhythmTime;
			/*if (this.timer.TimeElapsed > this.currentRhythmTime) {
				this.currentRhythm++;
				this.timer.Reset();
				SetRhythms();
			}*/
		}
	}
	
	public void Begin () {
		this.isRunning = true;
		this.timer.Start();
		this.metronomeRef.Begin();
		SetRhythms();
		foreach (EuclidNode node in this.nodeRefs) {
			node.Begin();
		}
		//For now
		Button buttonRef = GameObject.FindGameObjectWithTag("StartButton").GetComponent<Button>();
		GameObject.Destroy(GameObject.FindGameObjectWithTag("StartButton"));
		
	}
	public void Stop () {
		this.currentRhythm = -1;
		this.isRunning = false;
		this.timer.Stop();
		this.metronomeRef.Stop();
		foreach (EuclidNode node in this.nodeRefs) {
			node.Stop();
		}
	}
	public void Pause() {
		this.isRunning = false;
		this.timer.Pause();
		this.metronomeRef.Pause();
		foreach (EuclidNode node in this.nodeRefs) {
			node.Pause();
		}
	}
	
	public void Reset() {
		this.timer.Reset();
		this.metronomeRef.Reset();
		foreach (EuclidNode node in this.nodeRefs) {
			node.Reset();
		}
	}
	
	private void SetRhythms () {
		Vector2 rhythmRef = (Vector2)this.brownBeats[this.currentRhythm % this.brownBeats.Count];
		this.brownNodeRef.SetRhythm((int)rhythmRef.x, (int)rhythmRef.y);
		rhythmRef = (Vector2)this.analogBeats[this.currentRhythm % this.analogBeats.Count];
		this.analogNodeRef.SetRhythm((int)rhythmRef.x, (int)rhythmRef.y);
		rhythmRef = (Vector2)this.hitBeats[this.currentRhythm % this.hitBeats.Count];
		this.hitNodeRef.SetRhythm((int)rhythmRef.x, (int)rhythmRef.y);
		rhythmRef = (Vector2)this.synthBeats[this.currentRhythm % this.synthBeats.Count];
		this.synthNodeRef.SetRhythm((int)rhythmRef.x, (int)rhythmRef.y);
		rhythmRef = (Vector2)this.bassBeats[this.currentRhythm % this.bassBeats.Count];
		this.bassNodeRef.SetRhythm((int)rhythmRef.x, (int)rhythmRef.y);
	}
}
