using UnityEngine;
using System.Collections;

public class PlayHandAnimation : MonoBehaviour {

	public GameObject handBegin;
	private UILabel label1;
	private UILabel label2;

	// Use this for initialization
	void Start () {
		label1 = GameObject.Find ("NumLabel1").GetComponent<UILabel> ();
		label2 = GameObject.Find ("NumLabel2").GetComponent<UILabel> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick(){
		if (int.Parse(label1.text) > 6) {
			return;
		}
		if (int.Parse(label2.text) > 6) {
			return;
		}
		if (int.Parse(label1.text) < 0) {
			return;
		}
		if (int.Parse(label2.text) < 0) {
			return;
		}
		Instantiate (handBegin);
		handBegin.transform.position = new Vector3 (-0.141f,0f,0.638f);
		handBegin.GetComponent<Animation> ().Play("ananniu");
	}
}
