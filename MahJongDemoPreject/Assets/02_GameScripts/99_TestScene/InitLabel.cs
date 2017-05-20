using UnityEngine;
using System.Collections;

public class InitLabel : MonoBehaviour {
	public UILabel EastUILabelPosX;
	public UILabel EastUILabelPosY;
	public UILabel EastUILabelPosZ;

	public UILabel EastUILabelRoX;
	public UILabel EastUILabelRoY;
	public UILabel EastUILabelRoZ;

	public UILabel NorthUILabelPosX;
	public UILabel NorthUILabelPosY;
	public UILabel NorthUILabelPosZ;

	public UILabel NorthUILabelRoX;
	public UILabel NorthUILabelRoY;
	public UILabel NorthUILabelRoZ;

	public UILabel WestUILabelPosX;
	public UILabel WestUILabelPosY;
	public UILabel WestUILabelPosZ;

	public UILabel WestUILabelRoX;
	public UILabel WestUILabelRoY;
	public UILabel WestUILabelRoZ;

	public UILabel SouthUILabelPosX;
	public UILabel SouthUILabelPosY;
	public UILabel SouthUILabelPosZ;

	public UILabel SouthUILabelRoX;
	public UILabel SouthUILabelRoY;
	public UILabel SouthUILabelRoZ;


	// Use this for initialization
	void Start () {
		EastUILabelPosX.text = PlayerPrefs.GetFloat ("EastPosX").ToString();
		EastUILabelPosY.text = PlayerPrefs.GetFloat ("EastPosY").ToString ();
		EastUILabelPosZ.text = PlayerPrefs.GetFloat ("EastPosZ").ToString ();

		EastUILabelRoX.text = PlayerPrefs.GetFloat ("EastRoX").ToString ();
		EastUILabelRoY.text = PlayerPrefs.GetFloat ("EastRoY").ToString ();
		EastUILabelRoZ.text = PlayerPrefs.GetFloat ("EastRoZ").ToString ();

		NorthUILabelPosX.text = PlayerPrefs.GetFloat ("NorthPosX").ToString ();
		NorthUILabelPosY.text = PlayerPrefs.GetFloat ("NorthPosY").ToString ();
		NorthUILabelPosZ.text = PlayerPrefs.GetFloat ("NorthPosZ").ToString ();

		NorthUILabelRoX.text = PlayerPrefs.GetFloat ("NorthRoX").ToString ();
		NorthUILabelRoY.text = PlayerPrefs.GetFloat ("NorthRoY").ToString ();
		NorthUILabelRoZ.text = PlayerPrefs.GetFloat ("NorthRoZ").ToString ();

		WestUILabelPosX.text = PlayerPrefs.GetFloat ("WestPosX").ToString ();
		WestUILabelPosY.text = PlayerPrefs.GetFloat ("WestPosY").ToString ();
		WestUILabelPosZ.text = PlayerPrefs.GetFloat ("WestPosZ").ToString ();

		WestUILabelRoX.text = PlayerPrefs.GetFloat ("WestRoX").ToString ();
		WestUILabelRoY.text = PlayerPrefs.GetFloat ("WestRoY").ToString ();
		WestUILabelRoZ.text = PlayerPrefs.GetFloat ("WestRoZ").ToString ();

		SouthUILabelPosX.text = PlayerPrefs.GetFloat ("SouthPosX").ToString ();
		SouthUILabelPosY.text = PlayerPrefs.GetFloat ("SouthPosY").ToString ();
		SouthUILabelPosZ.text = PlayerPrefs.GetFloat ("SouthPosZ").ToString ();

		SouthUILabelRoX.text = PlayerPrefs.GetFloat ("SouthRoX").ToString ();
		SouthUILabelRoY.text = PlayerPrefs.GetFloat ("SouthRoY").ToString ();
		SouthUILabelRoZ.text = PlayerPrefs.GetFloat ("SouthRoZ").ToString ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
