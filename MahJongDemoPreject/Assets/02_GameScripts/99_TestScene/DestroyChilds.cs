using UnityEngine;
using System.Collections;

public class DestroyChilds : MonoBehaviour {



	public void DestroyChild(){
		foreach (Transform item in this.gameObject.transform) {
			Destroy (item.gameObject);
		}	
	}
}
