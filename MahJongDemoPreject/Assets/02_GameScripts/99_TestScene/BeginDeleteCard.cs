using UnityEngine;
using System.Collections;

public class BeginDeleteCard : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// 
	/// </summary>
	public void SureDelete(string label1,string label2){
		int dicsum = int.Parse (label1) + int.Parse (label2);

		int mod = dicsum % 4;

		if (mod == 2) {
			
		}
	}
}
