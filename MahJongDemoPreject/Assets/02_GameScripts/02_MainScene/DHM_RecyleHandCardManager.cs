using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DHM_RecyleHandCardManager : MonoBehaviour {
    public List<HandCardItem> _RecyleHandCardList = new List<HandCardItem>();
    public GameObject _chuPaiHand1 = null;
    public GameObject _chuPaiHand2 = null;
    public GameObject _handCardPrefab = null;

    //在HandCardManager中注册，用于给出牌动画注册chapai（）方法
    public delegate void ChuPaiCallBackDelegate(GameObject go);
    public event ChuPaiCallBackDelegate ChuPaiCallBackEvent;
    //
    [SerializeField]
    private float offSetX = -0.0340f;
    [SerializeField]
    private float offSetZ = 0.0460f;
    void Start () {
	}
    public void ChuPai(int id)
    {
        HandCardItem item = new HandCardItem();
        item._id = id;
        GameObject obj = (GameObject)Instantiate(_handCardPrefab);
        item._obj = RuleManager.UVoffSetWithReturn(id, obj);
    }
    public void ChuPai(HandCardItem item,bool isMoNi)
    {
        
        item._obj.layer = LayerMask.NameToLayer("ZhuoPai");
        RuleManager.m_instance.ResetHandCardColor(item._obj);
        Debug.Log("_RecyleHandCardList" + _RecyleHandCardList.Count);
        _RecyleHandCardList.Add(item);
        PlayChuPaiAnimation(isMoNi);
        AudioManager.Instance.PlayHandCardAudio(item._id);
    }
    public GameObject GetChuPaiWay()
    {
        int way = Random.Range(1, 3);
        if(way==1)
        {
            _chuPaiHand1 = ResourcesMgr.m_Instance.InstantiateGameObjectWithType("ChuPaiHand1", ResourceType.Hand);
            _chuPaiHand1.transform.position = this.transform.TransformPoint(-0.0809f, -0.0141f, 0.4405f);
            return _chuPaiHand1;
        }
        else 
        {
            _chuPaiHand2 = ResourcesMgr.m_Instance.InstantiateGameObjectWithType("ChuPaiHand2", ResourceType.Hand);
            _chuPaiHand2.transform.position = this.transform.TransformPoint(-0.1367f, -0.0131f, 0.505f);
            return _chuPaiHand2; 
        }
    }
    public GameObject GetHandCard(int id)
    {
        GameObject obj = null;
        for(int i = _RecyleHandCardList.Count-1;i >=0;i--)
        {
            if(_RecyleHandCardList[i]._id == id)
            {
                obj = _RecyleHandCardList[i]._obj;
                _RecyleHandCardList.RemoveAt(i);
            }
        }
        return obj;
    }
    public void PlayChuPaiAnimation(bool isMoNi)
    {
        Debug.LogWarning("DHM_RecyleHandCardManager+模拟摸牌的ID：" + _RecyleHandCardList[_RecyleHandCardList.Count - 1]);
        //设置手的偏移量
        GameObject hand = GetChuPaiWay();
        if(ChuPaiCallBackEvent!=null)
        {
            ChuPaiCallBackEvent(hand);
        }
        int index = _RecyleHandCardList.Count - 1;
        int row = index / 6;
        int col = index % 6;
        hand.transform.rotation = this.transform.rotation;
        hand.transform.Translate(offSetX * col, 0, offSetZ * row);
        DHM_HandAnimationCtr handCtr = hand.GetComponent<DHM_HandAnimationCtr>();
        handCtr.SetMoNiMoPai(isMoNi);
        //handCtr.Set_RecyleHandCardMgrPos(this.gameObject);
        handCtr.PlayChuPaiAnimation(_RecyleHandCardList[_RecyleHandCardList.Count - 1]);
        handCtr.chuPaiEndEvent += ChuPaiEndEventHandle;
        //hand.SendMessage("SetMoNiMoPai", isMoNi, SendMessageOptions.DontRequireReceiver);
        //hand.SendMessage("Set_RecyleHandCardMgrPos", this.gameObject, SendMessageOptions.DontRequireReceiver);
        //hand.SendMessage("PlayChuPaiAnimation", _RecyleHandCardList[_RecyleHandCardList.Count - 1], SendMessageOptions.RequireReceiver);

        GameManager.m_instance.m_ProState = this.transform.parent.GetComponent<DHM_CardManager>();
    }
    public void ChuPaiEndEventHandle()
    {
        GameObject obj = null;
        if (_RecyleHandCardList.Count==0)
        {
             obj = _RecyleHandCardList[0]._obj;
        }
        else
        {
            obj = _RecyleHandCardList[_RecyleHandCardList.Count - 1]._obj;
        }
        if (obj != null)
            obj.transform.SetParent(this.transform);
    }
    /// <summary>
    /// 删除桌牌上最后加入的牌
    /// </summary>
    public void DeleteCard()
    {
        Destroy(_RecyleHandCardList[_RecyleHandCardList.Count - 1]._obj);
        _RecyleHandCardList.RemoveAt(_RecyleHandCardList.Count - 1);
    }
    public void ResetInfo()
    {
        Transform[] trans = this.GetComponentsInChildren<Transform>();
        for (int i = trans.Length - 1; i >= 0; i--)
        {
            if (trans[i]!=this.transform)
            {
                Destroy(trans[i].gameObject);
            }
        }
        _RecyleHandCardList.Clear();
    }
}
