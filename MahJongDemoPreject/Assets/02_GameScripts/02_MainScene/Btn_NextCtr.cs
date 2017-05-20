using UnityEngine;
using System.Collections;
using GameProtocol;
public class Btn_NextCtr : MonoBehaviour {

	// Use this for initialization
	void Start () {
        UIButton[] btns = GetComponentsInChildren<UIButton>();
        foreach(var btn in btns)
        {
            UIEventListener.Get(btn.gameObject).onClick = (go) =>
             {
                 BtnClick(go);
             };
        }
	}
	void BtnClick(GameObject go)
    {
        string name = go.name;
        switch(name)
        {
            case "Btn_Next(Clone)":

                NetManager.m_Instance.SendMessage(Protocol.TYPE_FIGHT, 0, FightProtocol.NEXT_ROUND_CREQ, null);
                go.SetActive(false);
                MainViewMgr.m_Instance.ActiveLiuJu(false);
                break;
        }
    }
	
}
