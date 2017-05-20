using UnityEngine;
using System.Collections;
using GameProtocol;
using GameProtocol.Model;

public class GameManager : MonoBehaviour {
    public static GameManager m_instance = null;
    [Header("播放骰子的动画预设体")]
    public GameObject handBegin = null;//播放骰子的动画预设体
    [Header("骰子1点数：")]
    public int _number1 = 0;           //骰子1点数
    [Header("骰子2点数：")]
    public int _number2 = 0;           //骰子2点数
    public DHM_CardManager m_ProState;//上一回合
    public bool isGang = false;
    public bool islock = false;
    private bool isReset = false;
    public int m_CurrentCount = 1;//当前局数
    public int m_currentBanker;
    public GameState m_GameState = GameState.WAITING;

    public enum GameState
    {
        WAITING = 0,
        GAMESTART = 1
    }

    private void Awake()
    {
        m_instance = this;
    }
    private void Start()
    {
        MainViewMgr.m_Instance.SetGameCount(m_CurrentCount);
    }
    /// <summary>
    /// 开始游戏生成牌跺
    /// </summary>
    public void StartGame()
    {
        StartCoroutine(StartGameLogic());
        isReset = false;
        m_GameState = GameState.GAMESTART;

    }
    IEnumerator StartGameLogic()
    {
        MainViewMgr.m_Instance.CloseWaitInfo();
        MainViewMgr.m_Instance.ActiveImg_bkj();
        yield return new WaitForSeconds(1f);
        PlayMakerFSM fsm = GameObject.Find("mahjongTableManager").GetComponent<PlayMakerFSM>();
        fsm.SendEvent("GameStart");
        yield break;
    }
    /// <summary>
    /// 投掷骰子
    /// </summary>
    public void PlaySaiZi(int banker)
    {
        GameObject handBegin = ResourcesMgr.m_Instance.InstantiateGameObjectWithType("AnShaiZiHand", ResourceType.Hand);
        switch(banker)
        {
            case 1://东
                handBegin.transform.position = new Vector3(-0.101f, -0.015f, 0.607f);
                handBegin.transform.rotation = Quaternion.Euler(Vector3.zero);
                break;
            case 2://南
                handBegin.transform.position = new Vector3(-0.611f, -0.015f, -0.103f);
                handBegin.transform.rotation = Quaternion.Euler(new Vector3(0,-90,0));
                break;
            case 3://西
                handBegin.transform.position = new Vector3(0.106f, -0.015f, -0.613f);
                handBegin.transform.rotation = Quaternion.Euler(new Vector3(0,-180,0));
                break;
            case 4://北
                handBegin.transform.position = new Vector3(0.614f, -0.015f, 0.101f);
                handBegin.transform.rotation = Quaternion.Euler(new Vector3(0,90,0));
                break;
        }
        handBegin.GetComponent<Animation>().Play("ananniu");
    }
    public int GetDic1()
    {
        return _number1;
    }
    public int GetDic2()
    {
        return _number2;
    }
   
    /// <summary>
    /// 发牌和删除牌跺同时开始，都是在掷骰子动画播放完以后
    /// </summary>
    public void FaPai(FightModel fightModel)
    {
        //初始化骰子点数
        int[] dics = fightModel.Dice;
        if(dics.Length!=0)
        {
            _number1 = dics[0];
            _number2 = dics[1];
        }
        //播放动画
        Debug.Log("fightModel.Banker" + fightModel.Banker);
        PlaySaiZi(fightModel.Banker);
        //初始化玩家手牌
        PlayerManager.m_instance.m_EastPlayer.SetHandCardID(fightModel.DongHands);
        PlayerManager.m_instance.m_WestPlayer.SetHandCardID(fightModel.XiHands);
        PlayerManager.m_instance.m_SouthPlayer.SetHandCardID(fightModel.NanHands);
        PlayerManager.m_instance.m_NorthPlayer.SetHandCardID(fightModel.BeiHands);
        //设置当前玩家相机的culling Mask 1、2D只渲染自身的手牌 2、 3D不渲染自身的手和手牌
        switch(MainViewMgr.m_Instance.m_MySeat)
        {
            
            case MainSceneMger.PlayerSeat.PlayerEast:
                Debug.Log("初始化东家Layer");
                PlayerManager.m_instance.m_EastPlayer.SetLayer();
                break;
            case MainSceneMger.PlayerSeat.PlayerNorth:
                PlayerManager.m_instance.m_NorthPlayer.SetLayer();
                break;
            case MainSceneMger.PlayerSeat.PlayerSouth:
                PlayerManager.m_instance.m_SouthPlayer.SetLayer();
                break;
            case MainSceneMger.PlayerSeat.PlayerWest:
                PlayerManager.m_instance.m_WestPlayer.SetLayer();
                break;
        }
    }
  
    /// <summary>
    /// 当前玩家正式摸牌
    /// </summary>
    /// <param name="myseat"></param>
    public void MoPai(int seat,BoutModel boutModel)
    {
        //switch (seat)
        //{
        //    case 1:
        //        PlayerManager.m_instance.m_EastPlayer.MoPai(handCardId,isGang);
        //        Debug.Log("东家摸牌" + PlayerManager.m_instance.m_SouthPlayer.m_Player);
        //        PlayerManager.m_instance.m_EastPlayer.ActiveChuPaiState();
        //        break;
        //    case 2:
        //        PlayerManager.m_instance.m_SouthPlayer.MoPai(handCardId, isGang);
        //        Debug.Log("南家摸牌"+ PlayerManager.m_instance.m_SouthPlayer.m_Player);
        //        PlayerManager.m_instance.m_SouthPlayer.ActiveChuPaiState();
        //        break;
        //    case 3:
        //        PlayerManager.m_instance.m_WestPlayer.MoPai(handCardId, isGang);
        //        Debug.Log("西家摸牌" + PlayerManager.m_instance.m_SouthPlayer.m_Player);
        //        PlayerManager.m_instance.m_WestPlayer.ActiveChuPaiState();
        //        break;
        //    case 4:
        //        PlayerManager.m_instance.m_NorthPlayer.MoPai(handCardId, isGang);
        //        Debug.Log("北家摸牌" + PlayerManager.m_instance.m_SouthPlayer.m_Player);
        //        PlayerManager.m_instance.m_NorthPlayer.ActiveChuPaiState();
        //        break;
        //}
        //isGang = false;
        StartCoroutine(MoPaiLogic(seat, boutModel.CurrentPlate));
    }
    IEnumerator MoPaiLogic(int seat, int handCardId)
    {
        while (islock)
        {
            yield return new WaitForEndOfFrame();
        }
        islock = true;
        MainViewMgr.m_Instance.ActivePlayerSeatUI(RoomInfoMgr.m_Instance.m_MySeat, seat);
        switch (seat)
        {
            case 1:
                Debug.Log("东家摸牌" + PlayerManager.m_instance.m_SouthPlayer.m_Player);
                PlayerManager.m_instance.m_EastPlayer.MoPai(handCardId, isGang);
                
                PlayerManager.m_instance.m_EastPlayer.ActiveChuPaiState();
                break;
            case 2:
                Debug.Log("南家摸牌" + PlayerManager.m_instance.m_SouthPlayer.m_Player);
                PlayerManager.m_instance.m_SouthPlayer.MoPai(handCardId, isGang);
                
                PlayerManager.m_instance.m_SouthPlayer.ActiveChuPaiState();
                break;
            case 3:
                Debug.Log("西家摸牌" + PlayerManager.m_instance.m_SouthPlayer.m_Player);
                PlayerManager.m_instance.m_WestPlayer.MoPai(handCardId, isGang);
               
                PlayerManager.m_instance.m_WestPlayer.ActiveChuPaiState();
                break;
            case 4:
                Debug.Log("北家摸牌" + PlayerManager.m_instance.m_SouthPlayer.m_Player);
                PlayerManager.m_instance.m_NorthPlayer.MoPai(handCardId, isGang);
                
                PlayerManager.m_instance.m_NorthPlayer.ActiveChuPaiState();
                break;
        }
        isGang = false;
        islock = false;
        yield break;
    }
    /// <summary>
    /// 碰牌
    /// </summary>
    public void Peng(int seat, BoutModel boutModel)
    {
        //AudioManager.Instance.PlayEffectAudio("peng");
        //switch (seat)
        //{
        //    case 1:
        //        Debug.Log("东家碰牌了："+boutModel.CurrentPlate);
        //        PlayerManager.m_instance.m_EastPlayer.PengPai(seat,boutModel.CurrentPlate);
        //        PlayerManager.m_instance.m_EastPlayer.ActiveChuPaiState();
        //        break;
        //    case 2:
        //        PlayerManager.m_instance.m_SouthPlayer.PengPai(seat, boutModel.CurrentPlate);
        //        PlayerManager.m_instance.m_SouthPlayer.ActiveChuPaiState();
        //        Debug.Log("南家碰牌了："+ boutModel.CurrentPlate);
        //        break;
        //    case 3:
        //        PlayerManager.m_instance.m_WestPlayer.PengPai(seat, boutModel.CurrentPlate);
        //        PlayerManager.m_instance.m_WestPlayer.ActiveChuPaiState();
        //        Debug.Log("西家碰牌了："+ boutModel.CurrentPlate);
        //        break;
        //    case 4:
        //        PlayerManager.m_instance.m_NorthPlayer.PengPai(seat, boutModel.CurrentPlate);
        //        PlayerManager.m_instance.m_NorthPlayer.ActiveChuPaiState();
        //        Debug.Log("北家碰牌了："+ boutModel.CurrentPlate);
        //        break;
        //}
        StartCoroutine(PengLogic(seat, boutModel));
    }
    IEnumerator PengLogic(int seat, BoutModel boutModel)
    {
        while (islock)
        {
            yield return new WaitForEndOfFrame();
        }
        islock = true;
        AudioManager.Instance.PlayEffectAudio("peng");
        MainViewMgr.m_Instance.ActivePlayerSeatUI(RoomInfoMgr.m_Instance.m_MySeat, seat);
        switch (seat)
        {
            case 1:
                Debug.Log("东家碰牌了：" + boutModel.CurrentPlate);
                PlayerManager.m_instance.m_EastPlayer.PengPai(seat, boutModel.CurrentPlate);
                PlayerManager.m_instance.m_EastPlayer.ActiveChuPaiState();
                break;
            case 2:
                Debug.Log("南家碰牌了：" + boutModel.CurrentPlate);
                PlayerManager.m_instance.m_SouthPlayer.PengPai(seat, boutModel.CurrentPlate);
                PlayerManager.m_instance.m_SouthPlayer.ActiveChuPaiState();
                
                break;
            case 3:
                Debug.Log("西家碰牌了：" + boutModel.CurrentPlate);
                PlayerManager.m_instance.m_WestPlayer.PengPai(seat, boutModel.CurrentPlate);
                PlayerManager.m_instance.m_WestPlayer.ActiveChuPaiState();
                
                break;
            case 4:
                Debug.Log("北家碰牌了：" + boutModel.CurrentPlate);
                PlayerManager.m_instance.m_NorthPlayer.PengPai(seat, boutModel.CurrentPlate);
                PlayerManager.m_instance.m_NorthPlayer.ActiveChuPaiState();
                
                break;
        }
        islock = false;
        yield break;
}
    /// <summary>
    /// 杠牌
    /// </summary>
    public void Gang(int seat,BoutModel boutModel)
    {
        //isGang = true;//摸牌时需要从尾部删除
        //AudioManager.Instance.PlayEffectAudio("gang");
        //switch (seat)
        //{
        //    case 1:
        //        PlayerManager.m_instance.m_EastPlayer.GangPai(seat, boutModel);
        //        PlayerManager.m_instance.m_EastPlayer.ActiveChuPaiState(false);
        //        Debug.Log("东家杠牌了：" + boutModel.CurrentPlate);
        //        break;
        //    case 2:
        //        PlayerManager.m_instance.m_SouthPlayer.GangPai(seat, boutModel);
        //        PlayerManager.m_instance.m_SouthPlayer.ActiveChuPaiState(false);
        //        Debug.Log("南家杠牌了：" + boutModel.CurrentPlate);
        //        break;
        //    case 3:
        //        PlayerManager.m_instance.m_WestPlayer.GangPai(seat, boutModel);
        //        PlayerManager.m_instance.m_WestPlayer.ActiveChuPaiState(false);
        //        Debug.Log("西家杠牌了：" + boutModel.CurrentPlate);
        //        break;
        //    case 4:
        //        PlayerManager.m_instance.m_NorthPlayer.GangPai(seat, boutModel);
        //        PlayerManager.m_instance.m_NorthPlayer.ActiveChuPaiState(false);
        //        Debug.Log("北家杠牌了：" + boutModel.CurrentPlate);
        //        break;
        //}
        //int mySeat = (int)MainViewMgr.m_Instance.m_MySeat + 1;
        //if (mySeat==boutModel.CurrentBout)
        //{
        //    NetManager.m_Instance.SendMessage(Protocol.TYPE_FIGHT, 0, FightProtocol.GET_CREQ, null);
        //}
        StartCoroutine(GangLogic(seat, boutModel));
    }
    IEnumerator GangLogic(int seat, BoutModel boutModel)
    {
        while (islock)
        {
            yield return new WaitForEndOfFrame();
        }
        islock = true;
        isGang = true;//摸牌时需要从尾部删除
        AudioManager.Instance.PlayEffectAudio("gang");
        MainViewMgr.m_Instance.ActivePlayerSeatUI(RoomInfoMgr.m_Instance.m_MySeat, seat);
        switch (seat)
        {
            case 1:
                Debug.Log("东家杠牌了：" + boutModel.CurrentPlate);
                PlayerManager.m_instance.m_EastPlayer.GangPai(seat, boutModel);
                PlayerManager.m_instance.m_EastPlayer.ActiveChuPaiState(false);
                
                break;
            case 2:
                Debug.Log("南家杠牌了：" + boutModel.CurrentPlate);
                PlayerManager.m_instance.m_SouthPlayer.GangPai(seat, boutModel);
                PlayerManager.m_instance.m_SouthPlayer.ActiveChuPaiState(false);
                
                break;
            case 3:
                Debug.Log("西家杠牌了：" + boutModel.CurrentPlate);
                PlayerManager.m_instance.m_WestPlayer.GangPai(seat, boutModel);
                PlayerManager.m_instance.m_WestPlayer.ActiveChuPaiState(false);
                
                break;
            case 4:
                Debug.Log("北家杠牌了：" + boutModel.CurrentPlate);
                PlayerManager.m_instance.m_NorthPlayer.GangPai(seat, boutModel);
                PlayerManager.m_instance.m_NorthPlayer.ActiveChuPaiState(false);
                
                break;
        }
        int mySeat = (int)MainViewMgr.m_Instance.m_MySeat + 1;
        if (mySeat == boutModel.CurrentBout)
        {
            NetManager.m_Instance.SendMessage(Protocol.TYPE_FIGHT, 0, FightProtocol.GET_CREQ, null);
        }
        islock = false;
        yield break;
    }
    /// <summary>
    /// 胡牌
    /// </summary>
    public void Hu(int seat, BoutModel boutModel)
    {
        //AudioManager.Instance.PlayEffectAudio("hu");
        //switch (seat)
        //{
        //    case 1:
        //        PlayerManager.m_instance.m_EastPlayer.ActiveChuPaiState(false);
        //        PlayerManager.m_instance.m_EastPlayer.HuPai(boutModel.HuType,boutModel.CurrentPlate);
        //        Debug.Log("东家胡牌了：" + boutModel.CurrentPlate);
        //        break;
        //    case 2:
        //        PlayerManager.m_instance.m_SouthPlayer.ActiveChuPaiState(false);
        //        PlayerManager.m_instance.m_SouthPlayer.HuPai(boutModel.HuType, boutModel.CurrentPlate);
        //        Debug.Log("南家胡牌了：" + boutModel.CurrentPlate);
        //        break;
        //    case 3:
        //        PlayerManager.m_instance.m_WestPlayer.ActiveChuPaiState(false);
        //        PlayerManager.m_instance.m_WestPlayer.HuPai(boutModel.HuType, boutModel.CurrentPlate);
        //        Debug.Log("西家胡牌了：" + boutModel.CurrentPlate);
        //        break;
        //    case 4:
        //        PlayerManager.m_instance.m_NorthPlayer.ActiveChuPaiState(false);
        //        PlayerManager.m_instance.m_NorthPlayer.HuPai(boutModel.HuType, boutModel.CurrentPlate);
        //        Debug.Log("北家胡牌了：" + boutModel.CurrentPlate);
        //        break;
        //}
        //int mySeat = (int)MainViewMgr.m_Instance.m_MySeat + 1;
        //if (mySeat == boutModel.CurrentBout)
        //{
        //    Debug.Log("胡牌以后："+ mySeat);
        //    //NetManager.m_Instance.SendMessage(Protocol.TYPE_FIGHT, 0, FightProtocol.GET_CREQ, null);
        //}
        StartCoroutine(HuLogic(seat, boutModel));
    }
    IEnumerator HuLogic(int seat, BoutModel boutModel)
    {
        while (islock)
        {
            yield return new WaitForEndOfFrame();
        }
        islock = true;
        AudioManager.Instance.PlayEffectAudio("hu");
        MainViewMgr.m_Instance.ActivePlayerSeatUI(RoomInfoMgr.m_Instance.m_MySeat, seat);
        switch (seat)
        {
            case 1:
                Debug.Log("东家胡牌了：" + boutModel.CurrentPlate);
                PlayerManager.m_instance.m_EastPlayer.ActiveChuPaiState(false);
                PlayerManager.m_instance.m_EastPlayer.HuPai(boutModel.HuType, boutModel.CurrentPlate);
                
                break;
            case 2:
                Debug.Log("南家胡牌了：" + boutModel.CurrentPlate);
                PlayerManager.m_instance.m_SouthPlayer.ActiveChuPaiState(false);
                PlayerManager.m_instance.m_SouthPlayer.HuPai(boutModel.HuType, boutModel.CurrentPlate);
               
                break;
            case 3:
                Debug.Log("西家胡牌了：" + boutModel.CurrentPlate);
                PlayerManager.m_instance.m_WestPlayer.ActiveChuPaiState(false);
                PlayerManager.m_instance.m_WestPlayer.HuPai(boutModel.HuType, boutModel.CurrentPlate);
                
                break;
            case 4:
                Debug.Log("北家胡牌了：" + boutModel.CurrentPlate);
                PlayerManager.m_instance.m_NorthPlayer.ActiveChuPaiState(false);
                PlayerManager.m_instance.m_NorthPlayer.HuPai(boutModel.HuType, boutModel.CurrentPlate);
                
                break;
        }
        int mySeat = (int)MainViewMgr.m_Instance.m_MySeat + 1;
        if (mySeat == boutModel.CurrentBout)
        {
            Debug.Log("胡牌以后：" + mySeat);
            //NetManager.m_Instance.SendMessage(Protocol.TYPE_FIGHT, 0, FightProtocol.GET_CREQ, null);
        }
        islock = false;
        yield break;
    }
    public void Guo(BoutModel boutModel)
    {

        int mySeat = (int)MainViewMgr.m_Instance.m_MySeat + 1;
        if (mySeat == boutModel.CurrentBout)
        {
            NetManager.m_Instance.SendMessage(Protocol.TYPE_FIGHT, 0, FightProtocol.GET_CREQ, null);
        }
        PlayerManager.m_instance.m_EastPlayer.HideChuPaiState();
        PlayerManager.m_instance.m_SouthPlayer.HideChuPaiState();
        PlayerManager.m_instance.m_WestPlayer.HideChuPaiState();
        PlayerManager.m_instance.m_NorthPlayer.HideChuPaiState();
    }
    /// <summary>
    /// 出牌
    /// </summary>
    /// <param name="boutModel"></param>
    public void ChuPai(BoutModel boutModel)
    {
        switch(boutModel.CurrentBout)
        {
            case 11:
                Debug.Log("东家出牌：");
                //PlayerManager.m_instance.m_EastPlayer.ActiveChuPaiState();
                break;
            case 12:
                Debug.Log("南家出牌：");
                //PlayerManager.m_instance.m_SouthPlayer.ActiveChuPaiState();
                
                break;
            case 13:
                Debug.Log("西家出牌：");
                //PlayerManager.m_instance.m_WestPlayer.ActiveChuPaiState();
               
                break;
            case 14:
                Debug.Log("北家出牌：");
               // PlayerManager.m_instance.m_NorthPlayer.ActiveChuPaiState();
               
                break;
        }
    }
    /// <summary>
    /// 有人出牌
    /// </summary>
    /// <param name="boutModel"></param>
    public void SomeOneChuPai(int seat,int handCardID)
    {

        //islock = true;
        //if ((int)(MainViewMgr.m_Instance.m_MySeat + 1) == seat)
        //{
        //    Debug.Log("自身出的牌牌，不需要再模拟出牌");
        //    return;
        //}
        //switch (seat)
        //{
        //    case 1:
        //        PlayerManager.m_instance.m_EastPlayer.MoNiChuPai(handCardID);
        //        Debug.LogWarning("模拟东家出牌：");
        //        break;
        //    case 2:
        //        PlayerManager.m_instance.m_SouthPlayer.MoNiChuPai(handCardID);
        //        Debug.Log("模拟南家出牌：");
        //        break;
        //    case 3:
        //        PlayerManager.m_instance.m_WestPlayer.MoNiChuPai(handCardID);
        //        Debug.Log("模拟西家出牌：");
        //        break;
        //    case 4:
        //        PlayerManager.m_instance.m_NorthPlayer.MoNiChuPai(handCardID);
        //        Debug.Log("模拟北家出牌：");
        //        break;
        //}
        StartCoroutine(SomeOneChuPaiLogic(seat, handCardID));
    }
    IEnumerator SomeOneChuPaiLogic(int seat, int handCardID)
    {
        while (islock)
        {
            yield return new WaitForEndOfFrame();
        }
        islock = true;
        if ((int)(MainViewMgr.m_Instance.m_MySeat + 1) == seat)
        {
            islock = false;
            yield break;
        }
        switch (seat)
        {
            case 1:
                Debug.LogWarning("模拟东家出牌：");
                PlayerManager.m_instance.m_EastPlayer.MoNiChuPai(handCardID);
                break;
            case 2:
                Debug.Log("模拟南家出牌：");
                PlayerManager.m_instance.m_SouthPlayer.MoNiChuPai(handCardID);
                break;
            case 3:
                Debug.Log("模拟西家出牌：");
                PlayerManager.m_instance.m_WestPlayer.MoNiChuPai(handCardID);
                break;
            case 4:
                Debug.Log("模拟北家出牌：");
                PlayerManager.m_instance.m_NorthPlayer.MoNiChuPai(handCardID);
                break;
        }
        yield break;
    }
    public void FightEnd(int operation)
    {
        //0  有人胡，打完了
        //1  没人胡，打完了
        if (operation==0)
        {
            //
            if (MainViewMgr.m_Instance.m_MySeat.Equals(MainSceneMger.PlayerSeat.PlayerEast))
               NetManager.m_Instance.SendMessage(Protocol.TYPE_FIGHT, 0, FightProtocol.SETTLE_ACCOUNTS_CREQ, null);
        }
        else if(operation==1 )//流局
        {
            if (m_CurrentCount < 4)
                MainViewMgr.m_Instance.ActiveNext_Round();
            MainViewMgr.m_Instance.ActiveLiuJu(true);
        }
    }
    /// <summary>
    /// 对局结束
    /// </summary>
    public void GameEnd()
    {
        Debug.Log("对局结束");
        PlayerManager.m_instance.m_EastPlayer.HideChuPaiState();
        PlayerManager.m_instance.m_SouthPlayer.HideChuPaiState();
        PlayerManager.m_instance.m_WestPlayer.HideChuPaiState();
        PlayerManager.m_instance.m_NorthPlayer.HideChuPaiState();
    }
    public void RePlay()
    {
        //PlayerManager.m_instance.m_WestPlayer
        if (!isReset)
        {
            isReset = true;
            DeletePai.m_instance.ClearList();
            PlayerManager.m_instance.m_EastPlayer.RePlay();
            PlayerManager.m_instance.m_SouthPlayer.RePlay();
            PlayerManager.m_instance.m_WestPlayer.RePlay();
            PlayerManager.m_instance.m_NorthPlayer.RePlay();
            //m_CurrentCount++;
            MainViewMgr.m_Instance.SetGameCount(++m_CurrentCount);
            GameEnd();
            ClearPaiDuo( GameObject.Find("tableslot_up").transform,GameObject.Find("tableslot_up 1").transform);
            ClearPaiDuo(GameObject.Find("tableslot_right").transform, GameObject.Find("tableslot_right 1").transform);
            ClearPaiDuo(GameObject.Find("tableslot_left").transform, GameObject.Find("tableslot_left 1").transform);
            ClearPaiDuo(GameObject.Find("tableslot_down").transform, GameObject.Find("tableslot_down 1").transform);
        }
    }
    void ClearPaiDuo(Transform target,Transform bro)
    {
        Transform[] trans = target.GetComponentsInChildren<Transform>();
        for (int i = trans.Length - 1; i >= 0; i--)
        {
            if (trans[i] != target && trans[i]!=bro)
            {
                Destroy(trans[i].gameObject);
            }
        }
    }
    public void Next_Round(FightModel fightModel)
    {
        RoomModel roomModel = new RoomModel();

        roomModel.DongUser = new UserModel();
        roomModel.DongUser.IsReady = fightModel.DongReady;

        roomModel.NanUser = new UserModel();
        roomModel.NanUser.IsReady = fightModel.NanReady;

        roomModel.XiUser = new UserModel();
        roomModel.XiUser.IsReady = fightModel.XiReady;

        roomModel.BeiUser = new UserModel();
        roomModel.BeiUser.IsReady = fightModel.BeiReady;

        MainViewMgr.m_Instance.initWaitInfo(roomModel);
        if(fightModel.DongReady && fightModel.NanReady && fightModel.XiReady && fightModel.BeiReady)
        {
            if(RoomInfoMgr.m_Instance.m_MySeat.Equals(MainSceneMger.PlayerSeat.PlayerEast))
            {
                NetManager.m_Instance.SendMessage(Protocol.TYPE_FIGHT, RoomInfoMgr.m_Instance.m_RoomID, FightProtocol.START_CREQ, null);
            }
        }
    }
}
