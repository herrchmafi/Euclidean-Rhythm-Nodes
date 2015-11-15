using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RhythmNodeController : MonoBehaviour {
	public float baseRhythmTime = 4.9f;
	private int currentRhythm = 0;
	
	private GameObject[] rhythmNodeObjects;
	
	private Timer timer;
	private Metronome metronomeRef;
	
	#region Monodevelop
	// Use this for initialization
	void Start () {
		this.rhythmNodeObjects = GameObject.FindGameObjectsWithTag(SceneConstants.rhythmNodeTag);
		this.metronomeRef = GameObject.FindGameObjectWithTag(SceneConstants.metronomeTag).GetComponent<Metronome>();
		this.timer = new Timer();
	}
	
	// Update is called once per frame
	void Update () {
		if (this.timer.IsTiming) {
			this.timer.Update();
		}
	}
	
	#endregion
	
	#region Controller Functions
	
	public void Begin () {
		this.timer.Start();
		this.metronomeRef.Begin();
		foreach (GameObject rhythmNodeObject in this.rhythmNodeObjects) {
			rhythmNodeObject.GetComponent<EuclidNode>().Begin();
		}
		GameObject.Destroy(GameObject.FindGameObjectWithTag(SceneConstants.startButtonTag));
		
	}
	public void Stop () {
		this.currentRhythm = -1;
		this.timer.Stop();
		this.metronomeRef.Stop();
		foreach (GameObject rhythmNodeObject in this.rhythmNodeObjects) {
			rhythmNodeObject.GetComponent<EuclidNode>().Stop();
		}
	}
	public void Pause() {
		this.timer.Pause();
		this.metronomeRef.Pause();
		foreach (GameObject rhythmNodeObject in this.rhythmNodeObjects) {
			rhythmNodeObject.GetComponent<EuclidNode>().Pause();
		}
	}
	
	public void Reset() {
		this.timer.Reset();
		this.metronomeRef.Reset();
		foreach (GameObject rhythmNodeObject in this.rhythmNodeObjects) {
			rhythmNodeObject.GetComponent<EuclidNode>().Reset();
		}
	}
	
	#endregion
	
}
