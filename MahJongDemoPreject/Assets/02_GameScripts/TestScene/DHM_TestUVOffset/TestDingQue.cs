using UnityEngine;
using System.Collections;

public class TestDingQue : MonoBehaviour {

    public UIButton tong = null;
    public UIButton wan = null;
    public UIButton tiao = null;
    public DHM_HandCardManager hh;
	// Use this for initialization
	void Start () {
        UIEventListener.Get(tong.gameObject).onClick = TongEvent;
        UIEventListener.Get(wan.gameObject).onClick = WanEvent;
        UIEventListener.Get(tiao.gameObject).onClick = TiaoEvent;
    }
	
	public void TongEvent(GameObject go)
    {
        hh.DingQue(RuleManager.DingQueType.TONG);
    }
    public void WanEvent(GameObject go)
    {
        hh.DingQue(RuleManager.DingQueType.WAN);
    }
    public void TiaoEvent(GameObject go)
    {
        hh.DingQue(RuleManager.DingQueType.TIAO);
    }
}
