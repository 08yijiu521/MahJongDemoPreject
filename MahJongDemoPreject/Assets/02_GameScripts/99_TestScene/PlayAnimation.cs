using UnityEngine;
using System.Collections;

public class PlayAnimation : MonoBehaviour {

	public void PlayAnm(){
		this.transform.GetComponent<Animation> ().Play("hupai");
		Debug.Log ("播放");
	}
}
