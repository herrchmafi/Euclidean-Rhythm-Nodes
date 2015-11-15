using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NodeSubText : MonoBehaviour {
	
	public enum SoundType { BROWN, SYNTH, BASS, HIT, ANALOG }
	public SoundType soundType;
	private Text textRef;
	private EuclidNode rhythmRef;
	// Use this for initialization
	void Start () {
		this.rhythmRef = gameObject.GetComponent<EuclidNode>();
		string targetSound = "";
		switch (this.soundType) {
			case SoundType.BROWN:
				targetSound = "BrownText";
				break;
			case SoundType.SYNTH:
				targetSound = "SynthText";
				break;
			case SoundType.BASS:
				targetSound = "BassText";
				break;
			case SoundType.HIT:
				targetSound = "HitText";
				break;
			case SoundType.ANALOG:
				targetSound = "AnalogText";
				break;
		}
		this.textRef = GameObject.FindGameObjectWithTag(targetSound).GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		this.textRef.text = this.rhythmRef.EuclidString + "\n"  + this.rhythmRef.RhythmString;
	}
}
