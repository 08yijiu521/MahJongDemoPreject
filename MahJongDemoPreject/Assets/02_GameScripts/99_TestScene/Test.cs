using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {
    public UILabel _label = null;
    public UIButton btn1 = null;
    public UIButton btn2 = null;
	void Start () {
        //UIButton[] btns = GetComponentsInChildren<UIButton>();
        //foreach(var btn in btns)
        //{
        //    UIEventListener.Get(btn.gameObject).onClick += ClickEvent;
        //}
        UILabel  _label = GetComponentInChildren<UILabel>();
        UIEventListener.Get(btn1.gameObject).onClick = ClickEvent;
        UIEventListener.Get(btn2.gameObject).onClick = ClickEvent2;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void ClickEvent(GameObject go)
    {
        string tableName = "mahjongTable_001";

        DHM_ResourcesManager.Instance().GetGameObject(tableName);
    }
    void ClickEvent2(GameObject go)
    {
        string tableName = "mahjongTable_002";

        DHM_ResourcesManager.Instance().GetGameObject(tableName);
    }

}
