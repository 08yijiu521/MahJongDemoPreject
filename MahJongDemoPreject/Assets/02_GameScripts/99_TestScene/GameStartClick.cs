using UnityEngine;
using System.Collections;
using PlayMaker;

public class GameStartClick : MonoBehaviour {

	public PlayMakerFSM fsm;
	private GameObject[] gameArray;

	void OnClick(){
		gameArray = GameObject.FindGameObjectsWithTag ("Paiduo");
		if (gameArray == null) {
			return;
		} else {
			foreach (GameObject paiduo in gameArray) {
				Destroy (paiduo);
			}
		}
		fsm.SendEvent ("GameStart");
	}
}
