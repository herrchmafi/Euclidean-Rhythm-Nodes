using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EuclidNode : MonoBehaviour {
	public Vector2 stepsPulses;
	
	private List<bool> measure;
	
	private string rhythmString = "";
	
	private string euclidString = "";
	
	private bool isPlaying = false;
	public bool IsPlaying {
		get { return this.IsPlaying; }
	}
	
	private const float maxVolume = 100.0f;
	public float volume;
	
	public AudioClip audioClip;
	private AudioSource audioSource;
	private Metronome metronome;
	
	private Vector3 originalScale;
	
	public Vector2 displayTextOffset;
	public GUIStyle displayTextStyle;
	//Starts at index - 1 so first beat can be played
	//Beat offset is initiated while the metronome is already running will start at first index
	private int previousBeatRef = -1;
	private int beatOffset;
	
	#region Legacy
	
	void OnGUI(){
		string displayText = this.rhythmString + "\n" + this.euclidString;
		Vector3 point = (Vector3)Camera.main.WorldToScreenPoint(new Vector2(transform.position.x + this.displayTextOffset.x, transform.position.y + this.displayTextOffset.y));
		GUI.Label(new Rect(point.x, point.y, 50, 50), displayText, this.displayTextStyle);
		GUI.color = Color.blue;
	}
	
	#endregion
	
	#region Monodevelop
	// Use this for initialization
	void Start () {
		this.metronome = GameObject.FindGameObjectWithTag(SceneConstants.metronomeTag).GetComponent<Metronome>();
		this.audioSource = gameObject.GetComponent<AudioSource>();
		this.originalScale = transform.localScale;
		this.SetRhythm((int)this.stepsPulses.x, (int)this.stepsPulses.y);			
	}
	
	// Update is called once per frame
	void Update () {
		//Resizing nodes
		if (this.isPlaying) {
			float u = this.metronome.timer.Seconds / this.metronome.tempo; 
			//Resize Node Accordingly
			transform.localScale = Vector3.Lerp(transform.localScale, this.originalScale, u);
			//Check to see if measure at index has a pulse and if a beat has been played already for current index
			if (this.measure.Count != 0 
			&& (bool)this.measure[(this.metronome.Beat - this.beatOffset) % this.measure.Count] 
			&& this.previousBeatRef != (this.metronome.Beat - this.beatOffset)) {
				this.PlaySound();
				Vector3 targetScale = transform.localScale + Vector3.one * this.volume / maxVolume;
				transform.localScale = targetScale;
			}
			this.previousBeatRef = (this.metronome.Beat - this.beatOffset);
		}
	}
	
	#endregion
	
	#region Controller Functions
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
		if (v > u) {
			throw new System.ArgumentException("Steps May Not Exceed The Number Of Pulses");
		}
		this.previousBeatRef = -1;
		this.beatOffset = this.metronome.Beat;
		this.BuildRhythm(u, v);
		this.rhythmString = BuildRhythmString();
		this.euclidString = BuildEuclidString();
	}
	
	#endregion
	
	#region Helper Functions
	
	private void PlaySound() {
		this.audioSource.PlayOneShot(this.audioClip, volume);
	}
	
	#endregion
	
	#region Euclidean Algortihm Functions
	
	//http://cgm.cs.mcgill.ca/~godfried/publications/banff.pdf for implementation details
	private void BuildRhythm(int u, int v) {
		this.measure = new List<bool>();
		this.stepsPulses.x = u;
		this.stepsPulses.y = v;
		//if u divisible by v, then evenly distribute pulses in measure. No need to use algorithm in this case
		if (u == 0) {
			this.measure = new List<bool>();
			this.rhythmString = this.BuildRhythmString();
			this.euclidString = this.BuildEuclidString();
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
		//Auxilliary list will be used for the calculation
		List<List<bool>> auxMeasure = new List<List<bool>>();
		for (int i = 0; i < u; i++) {
			List<bool> nextSeg = new List<bool>();
			if (i < v) {
				nextSeg.Add(isVSmall);
			} else {
				nextSeg.Add(!isVSmall);
			}
			auxMeasure.Add(nextSeg);
		}
	
		int targetU = (isVSmall) ? u : u - (u - v);
		int targetV = (isVSmall) ? v : u - v;
		this.Euclidean(targetU, targetV, auxMeasure);
		
		//Concatenate subsequent segments generated from Euclidean to first segment
		List<bool> startSeg = auxMeasure[0];
		while (auxMeasure.Count > 1) {
			List<bool> nextSeg = auxMeasure[1];
			startSeg.AddRange(nextSeg);
			auxMeasure.RemoveAt(1);
		}
		this.measure = auxMeasure[0];
		
		//Invert back if v was greater than u / 2
		if (!isVSmall) {
			for (int i = 0; i < this.measure.Count; i++) {
				this.measure[i] = !(bool)this.measure[i];
			}	
		}
		this.rhythmString = this.BuildRhythmString();
		this.euclidString = this.BuildEuclidString();
	}
	
	private void Euclidean(int u, int v, List<List<bool>> auxMeasure) {
		if (v == 0) { return ; }
		//Concats starting segments to ending
		for (int i = 0; i < v; i++) {
			List<bool> endSeg = auxMeasure[auxMeasure.Count - 1];
			List<bool> startSeg = auxMeasure[i];
			startSeg.AddRange(endSeg);
			auxMeasure.RemoveAt(auxMeasure.Count - 1);
		}
		this.Euclidean(v, u % v, auxMeasure);
	}
	
	#endregion
	
	#region Display Text
	
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
	
	#endregion
	
}
