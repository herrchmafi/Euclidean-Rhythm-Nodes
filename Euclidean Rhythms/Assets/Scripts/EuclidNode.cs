using UnityEngine;
using System.Collections;

public class EuclidNode : MonoBehaviour {
	public Vector2 stepsPulses;
	
	private ArrayList measure;
	
	private string rhythmString = "";
	public string RhythmString {
		get { return this.rhythmString; }
	}
	
	private string euclidString;
	public string EuclidString {
		get { return this.euclidString; }
	}
	
	private bool isPlaying = false;
	public bool IsPlaying {
		get { return this.IsPlaying; }
	}
	
	public float volume;
	
	public AudioClip audioClip;
	private AudioSource audioSource;
	private Metronome metronome;

	private Vector3 originalScale;
	
	//Starts at index - 1 so first beat can be played
	//Beat offset is initiated while the metronome is already running will start at first index
	private int previousBeatRef = -1, beatOffset;
	
	// Use this for initialization
	void Start () {
		this.metronome = GameObject.FindGameObjectWithTag("Metronome").GetComponent<Metronome>();
		this.audioSource = gameObject.GetComponent<AudioSource>();
		this.originalScale = transform.localScale;
					
	}
	
	// Update is called once per frame
	void Update () {
		//Resizing nodes
		if (this.isPlaying) {
			float u = this.metronome.timer.TimeElapsed / this.metronome.tempo; 
			transform.localScale = Vector3.Lerp(transform.localScale, this.originalScale, u);
			//Check to see if measure at index has a pulse and if beat has been played already for current index
			if (this.measure.Count != 0 && (bool)this.measure[(this.metronome.Beat - this.beatOffset) % this.measure.Count] && this.previousBeatRef != (this.metronome.Beat - this.beatOffset)) {
				PlaySound();
				Vector3 targetScale = transform.localScale + Vector3.one * volume / 100.0f;
				transform.localScale = targetScale;
			}
			this.previousBeatRef = (this.metronome.Beat - this.beatOffset);
		}
	}
	
	public void Begin() {
		this.isPlaying = true;
		this.beatOffset = this.metronome.Beat;
	}
	
	public void Pause() {
		this.isPlaying = false;
	}
	
	public void Stop() {
		this.isPlaying = false;
		this.Reset();
	}
	
	public void Reset() {
		this.previousBeatRef = -1;
		this.beatOffset = 0;
	}
	
	public void SetRhythm(int u, int v) {
		this.previousBeatRef = -1;
		this.beatOffset = this.metronome.Beat;
		BuildRhythm(u, v);
		this.rhythmString = BuildRhythmString();
		this.euclidString = BuildEuclidString();
	}
	
	private void PlaySound() {
		this.audioSource.PlayOneShot(this.audioClip, volume);
	}
	
	private void BuildRhythm(int u, int v) {
		this.measure = new ArrayList();
		this.stepsPulses.x = u;
		this.stepsPulses.y = v;
		//if u divisible by v, then evenly distribute pulses in measure
		if (u == 0) {
			this.measure = new ArrayList();
			this.rhythmString = BuildRhythmString();
			this.euclidString = BuildEuclidString();
			return;
		}
		if (u % v == 0) {
			for (int i = 0; i < u; i++) {
				if (i % (u / v) == 0) {
					this.measure.Add(true);
				} else {
					this.measure.Add(false);
				}
			}
			this.rhythmString = BuildRhythmString();
			this.euclidString = BuildEuclidString();	
			return;
		} 
		
		//if v is greater than u / 2, then you must invert the values
		bool isVSmall = (v < u / 2);
		//Initial setup for measure (Series of 1's followed by series of 0's)
		for (int i = 0; i < u; i++) {
			ArrayList note = new ArrayList();
			if (i < v) {
				note.Add(isVSmall);
			} else {
				note.Add(!isVSmall);
			}
			this.measure.Add(note);
		}
	
		int targetU = (isVSmall) ? u : u - (u - v);
		int targetV = (isVSmall) ? v : u - v;
		Euclidean(targetU, targetV);
		
		//Concatenate subsequent segments generated from Euclidean to first segment
		ArrayList startSeg = (ArrayList)this.measure[0];
		while (this.measure.Count > 1) {
			ArrayList nextSeg = (ArrayList)this.measure[1];
			startSeg.AddRange(nextSeg);
			this.measure.RemoveAt(1);
		}
		ArrayList temp = (ArrayList)this.measure[0];
		this.measure = temp;
		
		//Invert back if v was greater than u / 2
		if (!isVSmall) {
			for (int i = 0; i < this.measure.Count; i++) {
				this.measure[i] = !(bool)this.measure[i];
			}	
		}
		this.rhythmString = BuildRhythmString();
		this.euclidString = BuildEuclidString();
	}
	
	private void Euclidean(int u, int v) {
		if (v == 0) { return ; }
		//Concats starting segments to ending
		for (int i = 0; i < v; i++) {
			ArrayList endSeg = (ArrayList)this.measure[this.measure.Count - 1];
			ArrayList startSeg = (ArrayList)this.measure[i];
			startSeg.AddRange(endSeg);
			this.measure.RemoveAt(this.measure.Count - 1);
		}
		Euclidean(v, u % v);
	}
	
	private string BuildRhythmString() {
		string rhythmString =  "[ ";
		for (int i = 0; i < this.measure.Count; i++) {
			int boolInt = ((bool)this.measure[i]) ? 1 : 0;
			rhythmString += boolInt.ToString() + " ";
		}
		rhythmString += "]";
		return rhythmString;
	}
	
	private string BuildEuclidString () {
		return "(" + this.stepsPulses.x + ", " + this.stepsPulses.y + ")";
	}
	
}
