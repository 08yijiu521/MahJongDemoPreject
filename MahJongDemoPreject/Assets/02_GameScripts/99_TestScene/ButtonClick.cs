using UnityEngine;
using System.Collections;

public class ButtonClick : MonoBehaviour {

	public UILabel XPositionInput;
	public UILabel YPositionInput;
	public UILabel ZPositionInput;
	public UILabel XRotationInput;
	public UILabel YRotationInput;
	public UILabel ZRotationInput;

	void OnClick(){
		Camera.main.transform.position = new Vector3 (float.Parse(XPositionInput.text),float.Parse(YPositionInput.text),float.Parse(ZPositionInput.text));
		Camera.main.transform.eulerAngles = new Vector3 (float.Parse (XRotationInput.text), float.Parse (YRotationInput.text), float.Parse (ZRotationInput.text));
		Debug.Log( this.gameObject.GetComponentInChildren<UILabel>().text);
		switch (this.gameObject.GetComponentInChildren<UILabel>().text) {
		case "东家":
			SaveData ("East");
			break;
		case "北家":
			SaveData ("North");
			break;
		case "西家":
			SaveData ("West");
			break;
		case "南家":
			SaveData ("South");
			break;
		default:
			break;
		}
	}

	private void SaveData(string name){
		PlayerPrefs.SetFloat (name+"PosX", float.Parse (XPositionInput.text));
		PlayerPrefs.SetFloat (name+"PosY", float.Parse (YPositionInput.text));
		PlayerPrefs.SetFloat (name+"PosZ", float.Parse (ZPositionInput.text));

		PlayerPrefs.SetFloat (name+"RoX", float.Parse (XRotationInput.text));
		PlayerPrefs.SetFloat (name+"RoY", float.Parse (YRotationInput.text));
		PlayerPrefs.SetFloat (name+"RoZ", float.Parse (ZRotationInput.text));
	}
}
