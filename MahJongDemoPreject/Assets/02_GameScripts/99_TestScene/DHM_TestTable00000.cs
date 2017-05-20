using UnityEngine;
using System.Collections;

public class DHM_TestTable00000 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void PrintInfo(GameObject go)
    {
        Debug.Log("Table---------------------------------------------------"+go.name);
    }
    public void PrintInfo(string go)
    {
        Debug.Log("Table name---------------------------------------------------" + go);
    }
}
