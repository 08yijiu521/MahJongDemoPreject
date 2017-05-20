using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameProtocol;
using GameProtocol.Model;
using HighlightingSystem;
public class DHM_CardManager : MonoBehaviour {
    [Header("手牌管理")]
    private DHM_HandCardManager _handCardMgr = null;
    [Header("桌牌管理")]
    private DHM_RecyleHandCardManager _recyleCardMgr = null;
    [Header("碰牌区管理")]
    private PengGangManager _pengGangMgr = null;
    private DHM_HandAnimationCtr _handAnimationCtr = null;
    [SerializeField]
    private GameObject m_Tip = null;
    public DHM_HandCardManager.PlayerType m_Player;
    public Highlighter highLighter;
    public Color m_highLighteColerMin;
    public Color m_highLighteColerMax;
    GameObject tip;
	void Start () {
         tip = Instantiate(m_Tip);
        tip.SetActive(false);
        if (_handCardMgr == null)
        {
            _handCardMgr = GetComponentInChildren<DHM_HandCardManager>();
        }
        if(_recyleCardMgr==null)
        {
            _recyleCardMgr = GetComponentInChildren<DHM_RecyleHandCardManager>();
        }
        if(_pengGangMgr==null)
        {
            _pengGangMgr = GetComponentInChildren<PengGangManager>();
        }
        if(_handAnimationCtr==null)
        {
            _handAnimationCtr = GetComponentInChildren<DHM_HandAnimationCtr>();
        }
        if(_handCardMgr!=null && _recyleCardMgr!=null)
        {
            _handCardMgr.chuPaiEvent += _recyleCardMgr.ChuPai;
            _recyleCardMgr.ChuPaiCallBackEvent += _handCardMgr.ChuPaiCallBackEventHandle;
        }
        //initHighLight();
	}
    void initHighLight()
    {
        GameObject obj = null;
        switch (m_Player)
        {
            case DHM_HandCardManager.PlayerType.East:
                obj =  GameObject.Find("eastlight");
                m_highLighteColerMin = new Color(1, 0.92f, 0.016f,0);
                m_highLighteColerMax = Color.yellow;
                break;
            case DHM_HandCardManager.PlayerType.West:
                obj = GameObject.Find("westlight");
                m_highLighteColerMin = new Color(0, 0, 1, 0);
                m_highLighteColerMax = Color.blue;
                break;
            case DHM_HandCardManager.PlayerType.North:
                obj = GameObject.Find("northlight");
                m_highLighteColerMin = new Color(1, 0, 0, 0);
                m_highLighteColerMax = Color.red;
                break;
            case DHM_HandCardManager.PlayerType.South:
                obj = GameObject.Find("southlight");
                m_highLighteColerMin = new Color(1, 0.92f, 0.016f, 0);
                m_highLighteColerMax = Color.yellow;
                break;
        }
        highLighter = obj.GetComponent<Highlighter>();
        if (highLighter == null)
            highLighter = obj.AddComponent<Highlighter>();
    }
    /// <summary>
    /// 发牌
    /// </summary>
    /// <param name="idArray">手牌的ID数组</param>
	public void FaPai()
    {
        _handCardMgr.FaPai();

    }
    #region 摸牌
    /// <summary>
    /// 摸牌
    /// 1 正常情况下的摸牌
    /// 2 杠的时候摸牌需要从尾部删除
    /// 3 碰的时候不摸，只打
    /// </summary>
    /// <param name="id">摸到的牌的ID</param>
    public void MoPai(int id,bool isGang = false)
    {
       
        _handCardMgr.SetMoHandCard(id);
        if(isGang)
        {
            DeletePai.m_instance.Deletepai_Gang();
        }
        else
        {
            DeletePai.m_instance.Deletepai();
        }
    }
    public void HideChuPaiState()
    {
        if(highLighter==null)
        {
            initHighLight();
        }
        highLighter.FlashingOff();
        _handCardMgr.IsState = false;
    }
    public void ActiveChuPaiState(bool isState = true)
    {
        Debug.Log("**********************************************我的回合，我是："+ m_Player);
        if (GameManager.m_instance.m_ProState != null)
        {
            Debug.Log("上一回合是："+ GameManager.m_instance.m_ProState.m_Player);
            GameManager.m_instance.m_ProState.HideChuPaiState();
        }
        if (highLighter == null)
        {
            initHighLight();
        }
        highLighter.FlashingOn(m_highLighteColerMin, m_highLighteColerMax);
        _handCardMgr.IsState = isState;
    }
    #endregion
    #region 设置手牌的ID
    /// <summary>
    /// 设置手牌的ID
    /// </summary>
    /// <param name="handCardIdList"></param>
    public void SetHandCardID(List<int> handCardIdList)
    {
        _handCardMgr.SetIDArray(handCardIdList);
    }
    public void SetLayer()
    {
        _handCardMgr.SetLayer(LayerMask.NameToLayer("Self"));
    }
    #endregion
    #region 定缺牌型
    /// <summary>
    /// 定缺牌型
    /// </summary>
    /// <param name="type">定缺的类型</param>
    public void DingQue(RuleManager.DingQueType type)
    {
        _handCardMgr._dingQueType = type;        
        _handCardMgr.DingQue(type); //1、手牌重新排序：需要定缺的id + 30、排序ID、定缺过的ID - 30、刷新uv,将定缺后的牌放置在最后
        RuleManager.m_instance.DingQue(type, _handCardMgr._handCardList);//2、定缺：改变颜色
    }
    #endregion
    public void PengPai(int seat,int id)
    {
        //手牌删除2张
        _handCardMgr.Peng(id);
        //桌牌删一张
        if (GameManager.m_instance.m_ProState != null)
            GameManager.m_instance.m_ProState._recyleCardMgr.DeleteCard();
        //碰牌区生成3张，特效
        _pengGangMgr.Peng(GetSeat(seat), id);
        
    }
    public void GangPai(int seat,BoutModel boutModel)
    {
        if(boutModel.GangType==1)//明杠
        {
            MingBar(seat, boutModel.CurrentPlate);
        }
        else if(boutModel.GangType==2)//暗杠
        {
            DarkBar(seat, boutModel.CurrentPlate);
        }
        else if(boutModel.GangType==3)//加杠
        {
            AddBar(seat,boutModel.CurrentPlate);
        }

    }
    //明杠
    public void MingBar(int seat,int id)
    {
        //手牌删除3张
        _handCardMgr.Gang(id,false);
        //某一位玩家的桌牌删除一张
        if (GameManager.m_instance.m_ProState != null)
            GameManager.m_instance.m_ProState._recyleCardMgr.DeleteCard();
        //碰牌区生成四张牌。特效
        _pengGangMgr.Gang(GetSeat(seat), id,false);
        //激活手牌管理，摸
    }
    //暗杠
    public void DarkBar(int seat,int id)
    {
        //手牌删除4张
        _handCardMgr.Gang(id, true);
        //碰牌区生成四张牌。特效
        _pengGangMgr.Gang(GetSeat(seat), id, true);
        //激活手牌管理，摸
    }
    //加杠
    public void AddBar(int seat,int id)
    {
        //手牌删除3张
        _handCardMgr.RemoveMoHandCard(id);
        //碰牌区生成1张牌。特效
        _pengGangMgr.CreateWanGangCard(id);
        //激活手牌管理，摸

    }
    /// <summary>
    /// 模拟摸牌
    /// </summary>
    /// <param name="id"></param>
    public void MoNiChuPai(int id)
    {
        _handCardMgr.MoNiChuPai(id);
    }
    /// <summary>
    /// 胡牌
    /// </summary>
    /// <param name="id"></param>
    public void HuPai(int huType,int id)
    {
        if(huType==1)
        {
            _handCardMgr.HuPai(id);
            if (GameManager.m_instance.m_ProState != null)
                GameManager.m_instance.m_ProState._recyleCardMgr.DeleteCard();
        }
        else if(huType==2)
        {
            ZiMo(id);
        }
        HideChuPaiState();
    }
    public void ZiMo(int id)
    {
        _handCardMgr.RemoveMoHandCard(id);
        _handCardMgr.HuPai(id);
    }
    /// <summary>
    /// 根据自身的位置获取方位
    /// </summary>
    /// <returns></returns>
    OtherSeat GetSeat(int currentSeat)
    {
        OtherSeat seat = OtherSeat.None;
        switch (currentSeat)
        {
            case 1:
                {
                    if (GameManager.m_instance.m_ProState.Equals(PlayerManager.m_instance.m_SouthPlayer))
                    {
                        seat = OtherSeat.Down;
                    }
                    if (GameManager.m_instance.m_ProState.Equals(PlayerManager.m_instance.m_WestPlayer))
                    {
                        seat = OtherSeat.Face;
                    }
                    if (GameManager.m_instance.m_ProState.Equals(PlayerManager.m_instance.m_NorthPlayer))
                    {
                        seat = OtherSeat.Up;
                    }
                    break;
                }

            case 2:
                {
                    if (GameManager.m_instance.m_ProState.Equals(PlayerManager.m_instance.m_EastPlayer))
                    {
                        seat = OtherSeat.Up;
                    }
                    if (GameManager.m_instance.m_ProState.Equals(PlayerManager.m_instance.m_WestPlayer))
                    {
                        seat = OtherSeat.Down;
                    }
                    if (GameManager.m_instance.m_ProState.Equals(PlayerManager.m_instance.m_NorthPlayer))
                    {
                        seat = OtherSeat.Face;
                    }
                    break;
                }
            case 3:
                {
                    if (GameManager.m_instance.m_ProState.Equals(PlayerManager.m_instance.m_EastPlayer))
                    {
                        seat = OtherSeat.Face;
                    }
                    if (GameManager.m_instance.m_ProState.Equals(PlayerManager.m_instance.m_SouthPlayer))
                    {
                        seat = OtherSeat.Up;
                    }
                    if (GameManager.m_instance.m_ProState.Equals(PlayerManager.m_instance.m_NorthPlayer))
                    {
                        seat = OtherSeat.Down;
                    }
                    break;
                }
            case 4:
                {
                    if (GameManager.m_instance.m_ProState.Equals(PlayerManager.m_instance.m_EastPlayer))
                    {
                        seat = OtherSeat.Down;
                    }
                    if (GameManager.m_instance.m_ProState.Equals(PlayerManager.m_instance.m_SouthPlayer))
                    {
                        seat = OtherSeat.Face;
                    }
                    if (GameManager.m_instance.m_ProState.Equals(PlayerManager.m_instance.m_WestPlayer))
                    {
                        seat = OtherSeat.Up;
                    }
                    break;
                }
        }
        return seat;
    }
    public void RePlay()
    {
        _handCardMgr.ResetInfo();
        _recyleCardMgr.ResetInfo();
        _pengGangMgr.ResetPengGangInfo();
        ShowRemainCard.instance.ResetCardCount();
        ShowRemainCard.instance.HideCardCount();
    }
}
