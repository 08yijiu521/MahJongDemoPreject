using UnityEngine;
using System.Collections;

public class Init : MonoBehaviour {

	private UILabel NumLabel1;
	private UILabel NumLabel2;

	// Use this for initialization
	void Start () {
		NumLabel1 = GameObject.Find ("NumLabel1").GetComponent<UILabel> ();
		NumLabel2 = GameObject.Find ("NumLabel2").GetComponent<UILabel> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int Dic1(){
		return int.Parse(NumLabel1.text);
	}

	public int Dic2(){
		return int.Parse(NumLabel2.text);
	}
}
