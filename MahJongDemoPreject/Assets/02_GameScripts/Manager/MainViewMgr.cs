using UnityEngine;
using System.Collections;
using GameProtocol;
using GameProtocol.Model;
using UnityEngine.SceneManagement;
public class MainViewMgr : MonoBehaviour {
    public static MainViewMgr m_Instance = null;


    public UILabel Label_RoomID;                //当前房间号
    public MainSceneMger.PlayerSeat m_MySeat;   //当前玩家座位

    public GameObject obj_IconDown;             //位于下方玩家的头像OBJ
    public UISprite Img_IconDown;               //位于下方的玩家头像图片
    public UILabel Label_Down;                  //位于下方的玩家金币Label
    public GameObject obj_IconLeft;             //位于左方玩家的头像OBJ
    public UISprite Img_IconLeft;               //位于左方的玩家头像图片
    public UILabel Label_Left;                  //位于左方的玩家金币Label
    public GameObject obj_IconUp;             //位于上方玩家的头像OBJ
    public UISprite Img_IconUp;               //位于上方的玩家头像图片
    public UILabel Label_Up;                  //位于上方的玩家金币Label
    public GameObject obj_IconRight;             //位于右方玩家的头像OBJ
    public UISprite Img_IconRight;               //位于右方的玩家头像图片
    public UILabel Label_Right;                  //位于右方的玩家金币Label
    public GameObject Img_HeadEffectDown;         //位于下方的头像特效
    public GameObject Img_HeadEffectLeft;         //位于下方的头像特效
    public GameObject Img_HeadEffectUp;         //位于下方的头像特效
    public GameObject Img_HeadEffectRight;         //位于下方的头像特效

    public UIButton Btn_StartGame;              //开始游戏按钮
    public UIButton btn_Exit;                   //退出游戏按钮
    public GameObject Wait_Left;                //左方玩家的等待状态
    public GameObject Wait_Right;               //右方玩家的等待状态
    public GameObject Wait_Down;                //下方玩家的等待状态
    public GameObject Wait_Up;                  //上方玩家的等待状态
    public GameObject Img_bkj = null;
    [Header("定缺")]
    public GameObject Btn_DingQueParent;       //定缺按钮
    public UISprite Img_DQDown;                //位于玩家下方的定缺图标
    public UISprite Img_DQRight;               //位于玩家右方的定缺图标
    public UISprite Img_DQUp;                  //位于玩家上方的定缺图标
    public UISprite Img_DQLeft;                //位于玩家左方的定缺图标

    [Header("碰杠胡按钮")]
    public GameObject Btn_HuPeng;
    public GameObject Btn_HuGang;
    public GameObject Btn_Peng;
    public GameObject Btn_Gang;
    public GameObject Btn_Hu;

    public UIButton btn_WeChatRequire;
    public Transform m_CenterContain;
    [Header("流局")]
    public GameObject LiuJu = null;

    [Header("实时流水和提示按钮")]
    public UIButton Btn_realTimeScore;
    public UIButton Btn_Tip;
    private void Awake()
    {
        m_Instance = this;
    }
    void Start()
    {
        InitBtn();
        _InitMainUI();
    }
    void OnEnable()
    {
        obj_IconRight.SetActive(false);
        obj_IconDown.SetActive(false);
        obj_IconLeft.SetActive(false);
        obj_IconUp.SetActive(false);
    }
    void InitBtn()
    {
        UIEventListener.Get(btn_WeChatRequire.gameObject).onClick = (go) =>
        {
            BtnWeChatRequireClick(go);
        };
    }
    /// <summary>
    /// 初始化玩家头像
    /// </summary>
    /// <param name="IconNum"></param>
    /// <returns></returns>
    public string InitPlayerIcon(int IconNum)
    {
        switch (IconNum)
        {
            case 1:
                return "Img_tx01";
            case 2:
                return "Img_tx02";
            case 3:
                return "Img_tx03";
            case 4:
                return "Img_tx04";
            default:
                Debug.Log("错误玩家头像ID！");
                return null;
        }
    }
    void BtnWeChatRequireClick(GameObject go)
    {
            GameObject obj = ResourcesMgr.m_Instance.GetGameObject("Prefab/MainScene/WeChatInvite");
            obj.transform.SetParent(m_CenterContain);
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
    }
    /// <summary>
    /// 初始化场景UI
    /// </summary>
    public void _InitMainUI()
    {
        
        if (RoomInfoMgr.m_Instance != null)
        {
            Label_RoomID.text = RoomInfoMgr.m_Instance.m_RoomID.ToString();
            m_MySeat = RoomInfoMgr.m_Instance.m_MySeat;
            Debug.LogWarning("我的座位是：" + m_MySeat);
            if(RoomInfoMgr.m_Instance.m_PlayerEastID >0 && RoomInfoMgr.m_Instance.m_PlayerSouthID > 0 && RoomInfoMgr.m_Instance.m_PlayerWestID > 0 && RoomInfoMgr.m_Instance.m_PlayerNorthID > 0)//四家到齐，显示 开始游戏按钮
            {
                btn_WeChatRequire.gameObject.SetActive(false);
                Btn_StartGame.gameObject.SetActive(true);//显示开始游戏按钮
                //btn_Exit.gameObject.SetActive(false);
            }
            else
            {
                btn_WeChatRequire.gameObject.SetActive(true);
                Btn_StartGame.gameObject.SetActive(false);//显示开始游戏按钮
            }
            switch (m_MySeat)
            {
                case MainSceneMger.PlayerSeat.PlayerEast:
                    Debug.Log("当前玩家为玩家东，初始化玩家东UI");
                    if (RoomInfoMgr.m_Instance.m_PlayerEastID <= 0)
                    {
                        Debug.Log("玩家东不在，还打个屁麻将！");
                        return;
                    }
                    else
                    {
                        obj_IconDown.SetActive(true);   //下方头像显示
                        Img_IconDown.spriteName = InitPlayerIcon(RoomInfoMgr.m_Instance.m_PlayerEastIconID);   //初始化玩家下方头像
                        Label_Down.text = RoomInfoMgr.m_Instance.m_PlayerEastCoin.ToString();
                    }
                    //初始化右边玩家头像
                    if (RoomInfoMgr.m_Instance.m_PlayerSouthID <= 0)
                    {
                        Debug.Log("玩家南不在，隐藏右边头像！");
                        obj_IconRight.SetActive(false);
                        
                    }
                    else
                    {
                        Debug.Log("****"  + RoomInfoMgr.m_Instance.m_PlayerSouthID);
                        obj_IconRight.SetActive(true);   //右边方头像显示
                        Img_IconRight.spriteName = InitPlayerIcon(RoomInfoMgr.m_Instance.m_PlayerSouthIconID);   //初始化玩家右边头像
                        Label_Right.text = RoomInfoMgr.m_Instance.m_PlayerSouthCoin.ToString();
                    }
                    //初始化上边玩家头像
                    if (RoomInfoMgr.m_Instance.m_PlayerWestID <= 0)
                    {
                        Debug.Log("玩家西不在，隐藏上边头像！");
                        obj_IconUp.SetActive(false);

                    }
                    else
                    {
                        obj_IconUp.SetActive(true);   //上边方头像显示
                        Img_IconUp.spriteName = InitPlayerIcon(RoomInfoMgr.m_Instance.m_PlayerWestIconID);   //初始化玩家上边头像
                        Label_Up.text = RoomInfoMgr.m_Instance.m_PlayerWestCoin.ToString();
                    }
                    //初始化左边玩家头像
                    if (RoomInfoMgr.m_Instance.m_PlayerNorthID <= 0)
                    {
                        Debug.Log("玩家北不在，隐藏左边头像！");
                        obj_IconLeft.SetActive(false);

                    }
                    else
                    {
                        obj_IconLeft.SetActive(true);   //左边方头像显示
                        Img_IconLeft.spriteName = InitPlayerIcon(RoomInfoMgr.m_Instance.m_PlayerNorthIconID);   //初始化玩家左边头像
                        Label_Left.text = RoomInfoMgr.m_Instance.m_PlayerNorthCoin.ToString();
                    }
                    break;
                case MainSceneMger.PlayerSeat.PlayerSouth:
                    Debug.Log("当前玩家为玩家南，初始化玩家南UI");
                    if (RoomInfoMgr.m_Instance.m_PlayerEastID <= 0)
                    {
                        Debug.Log("玩家东不在，还打个屁麻将！");
                        return;
                    }
                    else
                    {
                        obj_IconLeft.SetActive(true);   //左方头像显示
                        Img_IconLeft.spriteName = InitPlayerIcon(RoomInfoMgr.m_Instance.m_PlayerEastIconID);   //初始化玩家左方头像
                        Label_Left.text = RoomInfoMgr.m_Instance.m_PlayerEastCoin.ToString();
                    }
                    //初始化下边玩家头像
                    if (RoomInfoMgr.m_Instance.m_PlayerSouthID <= 0)
                    {
                        Debug.Log("玩家南不在，隐藏下边头像！");
                        obj_IconDown.SetActive(false);
                    }
                    else
                    {
                        obj_IconDown.SetActive(true);   //下边方头像显示
                        Img_IconDown.spriteName = InitPlayerIcon(RoomInfoMgr.m_Instance.m_PlayerSouthIconID);   //初始化玩家下边头像
                        Label_Down.text = RoomInfoMgr.m_Instance.m_PlayerSouthCoin.ToString();
                    }
                    //初始化右边玩家头像
                    if (RoomInfoMgr.m_Instance.m_PlayerWestID <= 0)
                    {
                        Debug.Log("玩家西不在，隐藏右边头像！");
                        obj_IconRight.SetActive(false);

                    }
                    else
                    {
                        obj_IconRight.SetActive(true);   //右边方头像显示
                        Img_IconRight.spriteName = InitPlayerIcon(RoomInfoMgr.m_Instance.m_PlayerWestIconID);   //初始化玩家右边头像
                        Label_Right.text = RoomInfoMgr.m_Instance.m_PlayerWestCoin.ToString();
                    }
                    //初始化上边玩家头像
                    if (RoomInfoMgr.m_Instance.m_PlayerNorthID <= 0)
                    {
                        Debug.Log("玩家北不在，隐藏上边头像！");
                        obj_IconUp.SetActive(false);

                    }
                    else
                    {
                        obj_IconUp.SetActive(true);   //上边方头像显示
                        Img_IconUp.spriteName = InitPlayerIcon(RoomInfoMgr.m_Instance.m_PlayerNorthIconID);   //初始化玩家上边头像
                        Label_Up.text = RoomInfoMgr.m_Instance.m_PlayerNorthCoin.ToString();
                    }
                    break;
                case MainSceneMger.PlayerSeat.PlayerWest:
                    Debug.Log("当前玩家为玩家西，初始化玩家西UI");
                    if (RoomInfoMgr.m_Instance.m_PlayerEastID <= 0)
                    {
                        Debug.Log("玩家东不在，还打个屁麻将！");
                        return;
                    }
                    else
                    {
                        obj_IconUp.SetActive(true);   //上方头像显示
                        Img_IconUp.spriteName = InitPlayerIcon(RoomInfoMgr.m_Instance.m_PlayerEastIconID);   //初始化玩家上方头像
                        Label_Up.text = RoomInfoMgr.m_Instance.m_PlayerEastCoin.ToString();
                    }
                    //初始化左边玩家头像
                    if (RoomInfoMgr.m_Instance.m_PlayerSouthID <= 0)
                    {
                        Debug.Log("玩家南不在，隐藏左边头像！");
                        obj_IconLeft.SetActive(false);

                    }
                    else
                    {
                        obj_IconLeft.SetActive(true);   //左边方头像显示
                        Img_IconLeft.spriteName = InitPlayerIcon(RoomInfoMgr.m_Instance.m_PlayerSouthIconID);   //初始化玩家左边头像
                        Label_Left.text = RoomInfoMgr.m_Instance.m_PlayerSouthCoin.ToString();
                    }
                    //初始化下边玩家头像
                    if (RoomInfoMgr.m_Instance.m_PlayerWestID <= 0)
                    {
                        Debug.Log("玩家西不在，隐藏下边头像！");
                        obj_IconDown.SetActive(false);

                    }
                    else
                    {
                        obj_IconDown.SetActive(true);   //下边方头像显示
                        Img_IconDown.spriteName = InitPlayerIcon(RoomInfoMgr.m_Instance.m_PlayerWestIconID);   //初始化玩家下边头像
                        Label_Down.text = RoomInfoMgr.m_Instance.m_PlayerWestCoin.ToString();
                    }
                    //初始化右边玩家头像
                    if (RoomInfoMgr.m_Instance.m_PlayerNorthID <= 0)
                    {
                        Debug.Log("玩家北不在，隐藏右边头像！");
                        obj_IconRight.SetActive(false);

                    }
                    else
                    {
                        obj_IconRight.SetActive(true);   //上边方头像显示
                        Img_IconRight.spriteName = InitPlayerIcon(RoomInfoMgr.m_Instance.m_PlayerNorthIconID);   //初始化玩家上边头像
                        Label_Right.text = RoomInfoMgr.m_Instance.m_PlayerNorthCoin.ToString();
                    }
                    break;
                case MainSceneMger.PlayerSeat.PlayerNorth:
                    Debug.Log("当前玩家为玩家北，初始化玩家北UI");
                    if (RoomInfoMgr.m_Instance.m_PlayerEastID <= 0)
                    {
                        Debug.Log("玩家东不在，还打个屁麻将！");
                        return;
                    }
                    else
                    {
                        obj_IconRight.SetActive(true);   //右方头像显示
                        Img_IconRight.spriteName = InitPlayerIcon(RoomInfoMgr.m_Instance.m_PlayerEastIconID);   //初始化玩家右方头像
                        Label_Right.text = RoomInfoMgr.m_Instance.m_PlayerEastCoin.ToString();
                    }
                    //初始化上方玩家头像
                    if (RoomInfoMgr.m_Instance.m_PlayerSouthID <= 0)
                    {
                        Debug.Log("玩家南不在，隐藏上边头像！");
                        obj_IconUp.SetActive(false);

                    }
                    else
                    {
                        obj_IconUp.SetActive(true);   //上边方头像显示
                        Img_IconUp.spriteName = InitPlayerIcon(RoomInfoMgr.m_Instance.m_PlayerSouthIconID);   //初始化玩家上边头像
                        Label_Up.text = RoomInfoMgr.m_Instance.m_PlayerSouthCoin.ToString();
                    }
                    //初始化左边玩家头像
                    if (RoomInfoMgr.m_Instance.m_PlayerWestID <= 0)
                    {
                        Debug.Log("玩家西不在，隐藏左边头像！");
                        obj_IconLeft.SetActive(false);

                    }
                    else
                    {
                        obj_IconLeft.SetActive(true);   //左边方头像显示
                        Img_IconLeft.spriteName = InitPlayerIcon(RoomInfoMgr.m_Instance.m_PlayerWestIconID);   //初始化玩家左边头像
                        Label_Left.text = RoomInfoMgr.m_Instance.m_PlayerWestCoin.ToString();
                    }
                    //初始化下边玩家头像
                    if (RoomInfoMgr.m_Instance.m_PlayerNorthID <= 0)
                    {
                        Debug.Log("玩家北不在，隐藏下边头像！");
                        obj_IconDown.SetActive(false);

                    }
                    else
                    {
                        obj_IconDown.SetActive(true);   //下边方头像显示
                        Img_IconDown.spriteName = InitPlayerIcon(RoomInfoMgr.m_Instance.m_PlayerNorthIconID);   //初始化玩家下边头像
                        Label_Down.text = RoomInfoMgr.m_Instance.m_PlayerNorthCoin.ToString();
                    }
                    break;
                default:
                    Debug.Log("错误座位！");
                    break;
            }
        }
    }

    /// <summary>
    /// 玩家点击退出按钮
    /// </summary>
    public void OnButtonExitChick()
    {
        Debug.Log("玩家点击退出按钮！");
        AudioManager.Instance.PlayEffectAudio("ui_click");
        //if (m_MySeat == MainSceneMger.PlayerSeat.PlayerEast)
        //{
        //    NetManager.m_Instance.SendMessage(Protocol.TYPE_ROOM, 0, RoomProtocol.DESTROY_CREQ, RoomInfoMgr.m_Instance.m_RoomID);
        //}
        //else
        //{
        if (GameManager.m_instance.m_GameState == GameManager.GameState.WAITING)
        {
            if (m_MySeat == MainSceneMger.PlayerSeat.PlayerEast)
            {
                NetManager.m_Instance.SendMessage(Protocol.TYPE_ROOM, 0, RoomProtocol.DISSOLVE_CREQ, null);
            }
            else
            {
                NetManager.m_Instance.SendMessage(Protocol.TYPE_ROOM, 0, RoomProtocol.LEAVE_CREQ, RoomInfoMgr.m_Instance.m_RoomID);
            }
        }
        else
        {
            NetManager.m_Instance.SendMessage(Protocol.TYPE_FIGHT,0,FightProtocol.DESTROY_FIGHT_CREQ,null);
        }
       
        //}
        //btn_Exit.GetComponent<Collider>().enabled = false;

        SceneManager.LoadScene("CreateRoomScene");
    }
    /// <summary>
    /// 刷新房间信息
    /// </summary>
    /// <param name="roomModel"></param>
    public void InitRoomInfo(RoomModel roomModel)
    {
        if (string.IsNullOrEmpty(roomModel.Error))
        {
            Debug.Log("加入房间成功");
            RoomInfoMgr.m_Instance.m_RoomID = roomModel.RoomID;
            UserModel Donguser = roomModel.DongUser;
            UserModel Nanuser = roomModel.NanUser;
            UserModel Xiuser = roomModel.XiUser;
            UserModel Beiuser = roomModel.BeiUser;
            if (Donguser != null)
            {
                if (Donguser.UserID <= 0)
                {
                    Debug.Log("玩家东不在，还打个屁麻将！");
                    return;
                }
                else
                {
                    Debug.Log("当前玩家东存在，初始化及显示玩家东头像");
                    RoomInfoMgr.m_Instance.m_PlayerEastID = Donguser.UserID;
                    RoomInfoMgr.m_Instance.m_PlayerEastName = Donguser.UserName;
                    RoomInfoMgr.m_Instance.m_PlayerEastCoin = Donguser.UserCoin;
                    RoomInfoMgr.m_Instance.m_PlayerEastDiamond = Donguser.UserDiamond;
                    RoomInfoMgr.m_Instance.m_PlayerEastIconID = Donguser.UserIconNum;
                }
            }

            if (Nanuser != null)
            {
                if (Nanuser.UserID <= 0)
                {
                    Debug.Log("当前玩家南不在，不显示玩家南头像");
                    RoomInfoMgr.m_Instance.m_PlayerSouthID = -1;
                    RoomInfoMgr.m_Instance.m_PlayerSouthName = null;
                    RoomInfoMgr.m_Instance.m_PlayerSouthCoin = -1;
                    RoomInfoMgr.m_Instance.m_PlayerSouthDiamond = -1;
                    RoomInfoMgr.m_Instance.m_PlayerSouthIconID = -1;
                }
                else
                {
                    Debug.Log("当前玩家南存在，初始化及显示玩家南头像");
                    //保存玩家南的信息
                    RoomInfoMgr.m_Instance.m_PlayerSouthID = Nanuser.UserID;
                    RoomInfoMgr.m_Instance.m_PlayerSouthName = Nanuser.UserName;
                    RoomInfoMgr.m_Instance.m_PlayerSouthCoin = Nanuser.UserCoin;
                    RoomInfoMgr.m_Instance.m_PlayerSouthDiamond = Nanuser.UserDiamond;
                    RoomInfoMgr.m_Instance.m_PlayerSouthIconID = Nanuser.UserIconNum;
                }
            }

            if (Xiuser != null)
            {
                if (Xiuser.UserID <= 0)
                {
                    Debug.Log("当前玩家西不在，不显示玩家西头像");
                    RoomInfoMgr.m_Instance.m_PlayerWestID = -1;
                    RoomInfoMgr.m_Instance.m_PlayerWestName = null;
                    RoomInfoMgr.m_Instance.m_PlayerWestCoin = -1;
                    RoomInfoMgr.m_Instance.m_PlayerWestDiamond = -1;
                    RoomInfoMgr.m_Instance.m_PlayerWestIconID = -1;
                }
                else
                {
                    Debug.Log("当前玩家西存在，初始化及显示玩家西头像");
                    //保存玩家西的信息
                    RoomInfoMgr.m_Instance.m_PlayerWestID = Xiuser.UserID;
                    RoomInfoMgr.m_Instance.m_PlayerWestName = Xiuser.UserName;
                    RoomInfoMgr.m_Instance.m_PlayerWestCoin = Xiuser.UserCoin;
                    RoomInfoMgr.m_Instance.m_PlayerWestDiamond = Xiuser.UserDiamond;
                    RoomInfoMgr.m_Instance.m_PlayerWestIconID = Xiuser.UserIconNum;
                }
            }

            if (Beiuser != null)
            {
                if (Beiuser.UserID <= 0)
                {
                    Debug.Log("当前玩家北不在，不显示玩家北头像");
                    RoomInfoMgr.m_Instance.m_PlayerNorthID = -1;
                    RoomInfoMgr.m_Instance.m_PlayerNorthName = null;
                    RoomInfoMgr.m_Instance.m_PlayerNorthCoin = -1;
                    RoomInfoMgr.m_Instance.m_PlayerNorthDiamond = -1;
                    RoomInfoMgr.m_Instance.m_PlayerNorthIconID = -1;
                }
                else
                {
                    Debug.Log("当前玩家北存在，初始化及显示玩家北头像");
                    //保存玩家北的信息
                    RoomInfoMgr.m_Instance.m_PlayerNorthID = Beiuser.UserID;
                    RoomInfoMgr.m_Instance.m_PlayerNorthName = Beiuser.UserName;
                    RoomInfoMgr.m_Instance.m_PlayerNorthCoin = Beiuser.UserCoin;
                    RoomInfoMgr.m_Instance.m_PlayerNorthDiamond = Beiuser.UserDiamond;
                    RoomInfoMgr.m_Instance.m_PlayerNorthIconID = Beiuser.UserIconNum;
                }
            }
    }
}
    
    /// <summary>
    /// 向服务器发送信息，开始游戏，将自身的等待状态开启
    /// </summary>
    /// <param name="go"></param>
    public void Button_StartGameClick(GameObject go)
    {
        //发信息给服务器
        NetManager.m_Instance.SendMessage(Protocol.TYPE_ROOM, RoomInfoMgr.m_Instance.m_RoomID, RoomProtocol.READY_CREQ, null);
        //FightProtocol.START_CREQ
        //等待中
        //Wait_Down.SetActive(true);
        Btn_StartGame.gameObject.SetActive(false);
        AudioManager.Instance.PlayEffectAudio("ui_click");
    }
   
    /// <summary>
    /// //激活开始游戏按钮
    /// </summary>
    public void StartGame()
    {
       
    }
    /// <summary>
    /// 根据哪个玩家点击了开始游戏，设置等待中状态
    /// </summary>
    /// <param name="type"></param>
    public void initWaitInfo(RoomModel roomModel)
    {
        Debug.Log("测试等待状态");
        UserModel Donguser = roomModel.DongUser;
        UserModel Nanuser = roomModel.NanUser;
        UserModel Xiuser = roomModel.XiUser;
        UserModel Beiuser = roomModel.BeiUser;
       

        if (RoomInfoMgr.m_Instance!=null)
        {
            switch(m_MySeat)
            {
                case MainSceneMger.PlayerSeat.PlayerEast:
                    {
                        if (Donguser!=null)
                        {
                            
                                if(Donguser.IsReady)
                                {
                                    Wait_Down.SetActive(true);
                                }
                            
                        }
                        if (Nanuser != null)
                        {
                            
                                if (Nanuser.IsReady)
                                {
                                    Wait_Right.SetActive(true);
                                }
                            
                        }
                        if (Xiuser != null)
                        {
                            
                                if (Xiuser.IsReady)
                                {
                                    Wait_Up.SetActive(true);
                                }
                            
                        }
                        if (Beiuser != null)
                        {
                            
                                if (Beiuser.IsReady)
                                {
                                    Wait_Left.SetActive(true);
                                }
                            
                        }
                        break;
                    }
                case MainSceneMger.PlayerSeat.PlayerSouth:
                    {
                        //将
                        if (Donguser != null)
                        {
                            
                                if (Donguser.IsReady)
                                {
                                    Wait_Left.SetActive(true);
                                }
                            
                        }
                        if (Nanuser != null)
                        {
                            
                                if (Nanuser.IsReady)
                                {
                                    Wait_Down.SetActive(true);
                                }
                            
                        }
                        if (Xiuser != null)
                        {
                           
                                if (Xiuser.IsReady)
                                {
                                    Wait_Right.SetActive(true);
                                }
                            
                        }
                        if (Beiuser != null)
                        {
                            
                                if (Beiuser.IsReady)
                                {
                                    Wait_Up.SetActive(true);
                                }
                            
                        }
                        break;
                    }
                case MainSceneMger.PlayerSeat.PlayerWest:
                    {
                        if (Donguser != null)
                        {
                           
                                if (Donguser.IsReady)
                                {
                                    Wait_Up.SetActive(true);
                                }
                            
                        }
                        if (Nanuser != null)
                        {
                            
                                if (Nanuser.IsReady)
                                {
                                    Wait_Left.SetActive(true);
                                }
                            
                        }
                        if (Xiuser != null)
                        {
                           
                                if (Xiuser.IsReady)
                                {
                                    Wait_Down.SetActive(true);
                                }
                            
                        }
                        if (Beiuser != null)
                        {
                            
                                if (Beiuser.IsReady)
                                {
                                    Wait_Right.SetActive(true);
                                }
                            
                        }
                        break;
                    }
                case MainSceneMger.PlayerSeat.PlayerNorth:
                    {
                        if (Donguser != null)
                        {
                            
                                if (Donguser.IsReady)
                                {
                                    Wait_Right.SetActive(true);
                                }
                            
                        }
                        if (Nanuser != null)
                        {
                           
                                if (Nanuser.IsReady)
                                {
                                    Wait_Up.SetActive(true);
                                }
                            
                        }
                        if (Xiuser != null)
                        {
                            
                                if (Xiuser.IsReady)
                                {
                                    Wait_Left.SetActive(true);
                                }
                            
                        }
                        if (Beiuser != null)
                        {
                            
                                if (Beiuser.IsReady)
                                {
                                    Wait_Down.SetActive(true);
                                }
                            
                        }
                        break;
                    }
            }
            Wait_Down.GetComponentInChildren<UISprite>().spriteName = "Img_Waiting";
            Wait_Right.GetComponentInChildren<UISprite>().spriteName = "Img_Waiting";
            Wait_Up.GetComponentInChildren<UISprite>().spriteName = "Img_Waiting";
            Wait_Left.GetComponentInChildren<UISprite>().spriteName = "Img_Waiting";
        }
    }
    public void CloseWaitInfo()
    {
        Wait_Down.gameObject.SetActive(false);
        Wait_Left.gameObject.SetActive(false);
        Wait_Right.gameObject.SetActive(false);
        Wait_Up.gameObject.SetActive(false);
        Debug.Log("CloseWaitInfo");
        if (m_MySeat.Equals(MainSceneMger.PlayerSeat.PlayerEast))
        {
            NetManager.m_Instance.SendMessage(Protocol.TYPE_FIGHT, RoomInfoMgr.m_Instance.m_RoomID, FightProtocol.START_CREQ, null);
        }
        ActiveImg_bkj();
    }
    public void ActiveImg_bkj()
    {
        //btn_WeChatRequire.gameObject.SetActive(false);
        Img_bkj.SetActive(true);
        Invoke("HideImg_bkj", 1f);
    }
    void HideImg_bkj()
    {
        Img_bkj.SetActive(false);
        //btn_WeChatRequire.gameObject.SetActive(true);
    }
    /// <summary>
    /// 关闭所有的 等待中。。
    /// </summary>
    public void HideWaitInfo()
    {
        Wait_Down.SetActive(false);
        Wait_Up.SetActive(false);
        Wait_Left.SetActive(false);
        Wait_Right.SetActive(false);
    }
    #region  定缺
    /// <summary>
    /// 激活定缺按钮
    /// </summary>
    public void ActiveDingQueBtn()
    {

        Btn_DingQueParent.gameObject.SetActive(true);
        UIButton[] btns = Btn_DingQueParent.GetComponentsInChildren<UIButton>();
        foreach(var btn in btns)
        {
            UIEventListener listener = UIEventListener.Get(btn.gameObject);
            if (listener != null)
            {
                listener.onClick = null;
                listener.onClick = BtnDingQueClick;
            }
        }
    }
    /// <summary>
    /// 定缺按钮回调方法
    /// </summary>
    /// <param name="go"></param>
    public void BtnDingQueClick(GameObject go)
    {
        Debug.Log("定缺");
        AudioManager.Instance.PlayEffectAudio("ui_click");
        Btn_DingQueParent.gameObject.SetActive(false);
        string name = go.name;
        switch(name)
        {
            case "Wan":
                //向服务器发送消息
                NetManager.m_Instance.SendMessage(Protocol.TYPE_FIGHT, 0, FightProtocol.QUE_CREQ, 1);
                DingQue(RuleManager.DingQueType.WAN);
                break;
            case "Tiao":
                NetManager.m_Instance.SendMessage(Protocol.TYPE_FIGHT, 0, FightProtocol.QUE_CREQ, 2);
                DingQue(RuleManager.DingQueType.TIAO);
                break;
            case "Tong":
                NetManager.m_Instance.SendMessage(Protocol.TYPE_FIGHT, 0, FightProtocol.QUE_CREQ, 3);
                DingQue(RuleManager.DingQueType.TONG);
                break;
        }
        
    }
    void DingQue(RuleManager.DingQueType type)
    {
        switch(m_MySeat)
        {
            case MainSceneMger.PlayerSeat.PlayerEast:
                PlayerManager.m_instance.m_EastPlayer.DingQue(type);
                break;
            case MainSceneMger.PlayerSeat.PlayerWest:
                PlayerManager.m_instance.m_WestPlayer.DingQue(type);
                break;
            case MainSceneMger.PlayerSeat.PlayerSouth:
                PlayerManager.m_instance.m_SouthPlayer.DingQue(type);
                break;
            case MainSceneMger.PlayerSeat.PlayerNorth:
                PlayerManager.m_instance.m_NorthPlayer.DingQue(type);
                break;
        }
    }
    /// <summary>
    /// 初始化 定缺等待中 信息
    /// </summary>
    /// <param name="fightModel"></param>
    public void InitDingQueWaitingInfo(FightModel fightModel)
    {
        switch (m_MySeat)
        {
            case MainSceneMger.PlayerSeat.PlayerEast:
                if (fightModel.DongQue > 0)
                    Wait_Down.SetActive(true);
                if (fightModel.NanQue > 0)
                    Wait_Right.SetActive(true);
                if (fightModel.XiQue > 0)
                    Wait_Up.SetActive(true);
                if (fightModel.BeiQue > 0)
                    Wait_Left.SetActive(true);
                break;
            case MainSceneMger.PlayerSeat.PlayerWest:
                if (fightModel.DongQue > 0)
                    Wait_Up.SetActive(true);
                if (fightModel.NanQue > 0)
                    Wait_Left.SetActive(true);
                if (fightModel.XiQue > 0)
                    Wait_Down.SetActive(true);
                if (fightModel.BeiQue > 0)
                    Wait_Right.SetActive(true);
                break;
            case MainSceneMger.PlayerSeat.PlayerSouth:
                if (fightModel.DongQue > 0)
                    Wait_Left.SetActive(true);
                if (fightModel.NanQue > 0)
                    Wait_Down.SetActive(true);
                if (fightModel.XiQue > 0)
                    Wait_Right.SetActive(true);
                if (fightModel.BeiQue > 0)
                    Wait_Up.SetActive(true);
                break;
            case MainSceneMger.PlayerSeat.PlayerNorth:
                if (fightModel.DongQue > 0)
                    Wait_Right.SetActive(true);
                if (fightModel.NanQue > 0)
                    Wait_Up.SetActive(true);
                if (fightModel.XiQue > 0)
                    Wait_Left.SetActive(true);
                if (fightModel.BeiQue > 0)
                    Wait_Down.SetActive(true);
                break;
        }
        Wait_Down.GetComponentInChildren<UISprite>().spriteName = "Img_dqz";
        Wait_Right.GetComponentInChildren<UISprite>().spriteName = "Img_dqz";
        Wait_Up.GetComponentInChildren<UISprite>().spriteName = "Img_dqz";
        Wait_Left.GetComponentInChildren<UISprite>().spriteName = "Img_dqz";
    }
    /// <summary>
    /// 初始化定缺信息
    /// </summary>
    /// <param name="fightModel"></param>
    public void InitDingQueImg(FightModel fightModel)
    {
        HideWaitInfo();
        switch (m_MySeat)
        {
            case MainSceneMger.PlayerSeat.PlayerEast:
                PlayerManager.m_instance.m_SouthPlayer.DingQue(GetDQType(fightModel.NanQue));
                PlayerManager.m_instance.m_WestPlayer.DingQue(GetDQType(fightModel.XiQue));
                PlayerManager.m_instance.m_NorthPlayer.DingQue(GetDQType(fightModel.BeiQue));
                if (fightModel.DongQue > 0)
                {
                    Img_DQDown.gameObject.SetActive(true);
                    Img_DQDown.spriteName = GetImg_DQName(fightModel.DongQue);
                }
                if (fightModel.NanQue > 0)
                {
                    Img_DQRight.gameObject.SetActive(true);
                    Img_DQRight.spriteName = GetImg_DQName(fightModel.NanQue);
                }
                if (fightModel.XiQue > 0)
                {
                    Img_DQUp.gameObject.SetActive(true);
                    Img_DQUp.spriteName = GetImg_DQName(fightModel.XiQue);
                }
                if (fightModel.BeiQue > 0)
                {
                    Img_DQLeft.gameObject.SetActive(true);
                    Img_DQLeft.spriteName = GetImg_DQName(fightModel.BeiQue);
                }
                break;
            case MainSceneMger.PlayerSeat.PlayerWest:
                PlayerManager.m_instance.m_EastPlayer.DingQue(GetDQType(fightModel.DongQue));
                PlayerManager.m_instance.m_SouthPlayer.DingQue(GetDQType(fightModel.NanQue));
                PlayerManager.m_instance.m_NorthPlayer.DingQue(GetDQType(fightModel.BeiQue));
                if (fightModel.DongQue > 0)
                {
                    Img_DQUp.gameObject.SetActive(true);
                    Img_DQUp.spriteName = GetImg_DQName(fightModel.DongQue);
                }
                if (fightModel.NanQue > 0)
                {
                    Img_DQLeft.gameObject.SetActive(true);
                    Img_DQLeft.spriteName = GetImg_DQName(fightModel.NanQue);
                }
                if (fightModel.XiQue > 0)
                {
                    Img_DQDown.gameObject.SetActive(true);
                    Img_DQDown.spriteName = GetImg_DQName(fightModel.XiQue);
                }
                if (fightModel.BeiQue > 0)
                {
                    Img_DQRight.gameObject.SetActive(true);
                    Img_DQRight.spriteName = GetImg_DQName(fightModel.BeiQue);
                }
                break;
            case MainSceneMger.PlayerSeat.PlayerSouth:
                PlayerManager.m_instance.m_EastPlayer.DingQue(GetDQType(fightModel.DongQue));
                PlayerManager.m_instance.m_WestPlayer.DingQue(GetDQType(fightModel.XiQue));
                PlayerManager.m_instance.m_NorthPlayer.DingQue(GetDQType(fightModel.BeiQue));
                if (fightModel.DongQue > 0)
                {
                    Img_DQLeft.gameObject.SetActive(true);
                    Img_DQLeft.spriteName = GetImg_DQName(fightModel.DongQue);
                }
                if (fightModel.NanQue > 0)
                {
                    Img_DQDown.gameObject.SetActive(true);
                    Img_DQDown.spriteName = GetImg_DQName(fightModel.NanQue);
                }
                if (fightModel.XiQue > 0)
                {
                    Img_DQRight.gameObject.SetActive(true);
                    Img_DQRight.spriteName = GetImg_DQName(fightModel.XiQue);
                }
                if (fightModel.BeiQue > 0)
                {
                    Img_DQUp.gameObject.SetActive(true);
                    Img_DQUp.spriteName = GetImg_DQName(fightModel.BeiQue);
                }
                break;
            case MainSceneMger.PlayerSeat.PlayerNorth:
                PlayerManager.m_instance.m_EastPlayer.DingQue(GetDQType(fightModel.DongQue));
                PlayerManager.m_instance.m_SouthPlayer.DingQue(GetDQType(fightModel.NanQue));
                PlayerManager.m_instance.m_WestPlayer.DingQue(GetDQType(fightModel.XiQue));
                if (fightModel.DongQue > 0)
                {
                    Img_DQRight.gameObject.SetActive(true);
                    Img_DQRight.spriteName = GetImg_DQName(fightModel.DongQue);
                }
                if (fightModel.NanQue > 0)
                {
                    Img_DQUp.gameObject.SetActive(true);
                    Img_DQUp.spriteName = GetImg_DQName(fightModel.NanQue);
                }
                if (fightModel.XiQue > 0)
                {
                    Img_DQLeft.gameObject.SetActive(true);
                    Img_DQLeft.spriteName = GetImg_DQName(fightModel.XiQue);
                }
                if (fightModel.BeiQue > 0)
                {
                    Img_DQDown.gameObject.SetActive(true);
                    Img_DQDown.spriteName = GetImg_DQName(fightModel.BeiQue);
                }
                break;
        }
        //定缺结束,东家发信息
        if (fightModel.Banker == ((int)m_MySeat + 1))
        {
            //if (m_MySeat.Equals(MainSceneMger.PlayerSeat.PlayerEast))
            Debug.Log("fightModel.Banker"+ fightModel.Banker);
            NetManager.m_Instance.SendMessage(Protocol.TYPE_FIGHT, 0, FightProtocol.GET_CREQ, null);
        }
    }
    RuleManager.DingQueType GetDQType(int id)
    {
        switch (id)
        {
            case 1:
                return  RuleManager.DingQueType.WAN;
                break;
            case 2:
                return  RuleManager.DingQueType.TIAO;
                break;
            case 3:
                return  RuleManager.DingQueType.TONG;
                break;
        }
        return RuleManager.DingQueType.None;
    }
    string GetImg_DQName(int id)
    {
        string str = string.Empty;
        switch (id)
        {
            case 1:
                str = "Img_wan";
                break;
            case 2:
                str = "Img_tiao";
                break;
            case 3:
                str = "Img_tong";
                break;
        }
        return str;
    }
    #endregion
    #region  碰、杠、胡
    void ActiveState()
    {
        ActivePlayerSeatUI(RoomInfoMgr.m_Instance.m_MySeat, (int)m_MySeat + 1);
        switch (m_MySeat)
        {
            case MainSceneMger.PlayerSeat.PlayerEast:
                PlayerManager.m_instance.m_EastPlayer.ActiveChuPaiState(false);
                break;
            case MainSceneMger.PlayerSeat.PlayerSouth:
                PlayerManager.m_instance.m_SouthPlayer.ActiveChuPaiState(false);
                break;
            case MainSceneMger.PlayerSeat.PlayerWest:
                PlayerManager.m_instance.m_WestPlayer.ActiveChuPaiState(false);
                break;
            case MainSceneMger.PlayerSeat.PlayerNorth:
                PlayerManager.m_instance.m_NorthPlayer.ActiveChuPaiState(false);
                break;
        }
    }

    /// <summary>
    /// 激活杠牌选择按钮
    /// </summary>
    /// <param name="isMingGang"></param>
    /// <param name="boutModel"></param>
    public void ActiveGangBtn(bool isMingGang, BoutModel boutModel)
    {
        ActiveState();
        //激活按钮包括 杠 过
        Btn_Gang.SetActive(true);
        if (isMingGang)
        {
            boutModel.GangType = 1;//明杠
        }
        else  //需要区分是否是加杠
        {
            boutModel.GangType = 2;//暗杠
        }
        //暗杠
        boutModel.LittleCanon = GetUpstream();
        foreach (UIButton btn in Btn_Gang.GetComponentsInChildren<UIButton>())
        {
            UIEventListener.Get(btn.gameObject).onClick = (go) =>
            {
                BtnPengGangHuGuoClick(go, boutModel.CurrentPlate, boutModel);
            };
        }
    }
    public void ActivePengBtn(BoutModel boutModel)
    {
        ActiveState();
        //激活按钮包括 碰 过
        Btn_Peng.SetActive(true);
        foreach (UIButton btn in Btn_Peng.GetComponentsInChildren<UIButton>())
        {
            UIEventListener.Get(btn.gameObject).onClick = (go) =>
            {
                BtnPengGangHuGuoClick(go, boutModel.CurrentPlate,boutModel);
            };
        }
    }

    public void ActiveHuBtn(bool isZiMo, BoutModel boutModel)
    {
        ActiveState();
        //激活按钮包括 胡 过
        Btn_Hu.SetActive(true);
        if(isZiMo)
        {
            //2 自摸
            boutModel.HuType = 2;
        }
        else
        {
            //1 点炮
            boutModel.HuType = 1;
            boutModel.LittleCanon = GetUpstream(); //1,2,4,5
        }
        
        foreach (UIButton btn in Btn_Hu.GetComponentsInChildren<UIButton>())
        {
            UIEventListener.Get(btn.gameObject).onClick = (go) =>
            {
                BtnPengGangHuGuoClick(go, boutModel.CurrentPlate,boutModel);
            };
        }
    }
    /// <summary>
    /// 得到上家
    /// </summary>
    /// <returns></returns>
    int GetUpstream()
    {
        if ((GameManager.m_instance.m_ProState == null))
            return -1;
            
            if (GameManager.m_instance.m_ProState.Equals(PlayerManager.m_instance.m_EastPlayer))
            {
                return 1;
            }
            else if (GameManager.m_instance.m_ProState.Equals(PlayerManager.m_instance.m_SouthPlayer))
            {
                return 2;
            }
            else if (GameManager.m_instance.m_ProState.Equals(PlayerManager.m_instance.m_WestPlayer))
            {
                return 3;
            }
            else
            {
                return 4;
            }
    }
    public void ActiveHuPengBtn(BoutModel boutModel)
    {
        ActiveState();
        //激活按钮包括 胡 过
        Btn_HuPeng.SetActive(true);
        //1 点炮
        boutModel.HuType = 1;
        boutModel.LittleCanon = GetUpstream();
        foreach (UIButton btn in Btn_HuPeng.GetComponentsInChildren<UIButton>())
        {
            UIEventListener.Get(btn.gameObject).onClick = (go) =>
            {
                BtnPengGangHuGuoClick(go, boutModel.CurrentPlate,boutModel);
            };
        }
    }
    public void ActiveHuGangBtn(bool isZiMo,bool isMingGang,BoutModel boutModel)
    {
        ActiveState();
        //激活按钮包括 胡 过
        Btn_HuGang.SetActive(true);
        if (isZiMo)
        {
            //2 自摸
            boutModel.HuType = 2;
        }
        else
        {
            //1 点炮
            boutModel.HuType = 1;
        }
        if (isMingGang)
        {
            boutModel.GangType = 1;//明杠
        }
        else  //需要区分是否是加杠
        {
            boutModel.GangType = 2;//暗杠
        }
        boutModel.LittleCanon = GetUpstream();
        foreach (UIButton btn in Btn_HuGang.GetComponentsInChildren<UIButton>())
        {
            UIEventListener.Get(btn.gameObject).onClick = (go) =>{
                BtnPengGangHuGuoClick(btn.gameObject, boutModel.CurrentPlate,boutModel);
                    
            };
        }
    }
    public void BtnPengGangHuGuoClick(GameObject go ,int m_currentId, BoutModel boultModel = null)
    {
        Btn_Gang.SetActive(false);
        Btn_Hu.SetActive(false);
        Btn_Peng.SetActive(false);
        Btn_HuGang.SetActive(false);
        Btn_HuPeng.SetActive(false);
        string name = go.gameObject.name;
        switch(name)
        {
            case "Button_Peng":
                //方位、牌
                NetManager.m_Instance.SendMessage(Protocol.TYPE_FIGHT, ((int)m_MySeat)+1, FightProtocol.PENG_CREQ, boultModel);
                Debug.Log("碰的牌+"+boultModel.CurrentPlate);
                Btn_Peng.SetActive(false);
                break;
            case "Button_Gang":
                //需要检测明杠还是暗杠
                NetManager.m_Instance.SendMessage(Protocol.TYPE_FIGHT, ((int)m_MySeat) + 1, FightProtocol.GANG_CREQ, boultModel);
                Debug.Log("杠的牌+" + boultModel.CurrentPlate);
                Btn_Gang.SetActive(false);
                break;
            case "Button_Hu":
                NetManager.m_Instance.SendMessage(Protocol.TYPE_FIGHT, ((int)m_MySeat) + 1, FightProtocol.HU_CREQ, boultModel);
                Debug.Log("胡的牌+" + boultModel.CurrentPlate);
                Btn_Hu.SetActive(false);
                break;
            case "Button_Guo":
                boultModel.CurrentBout = GetUpstream();
                NetManager.m_Instance.SendMessage(Protocol.TYPE_FIGHT, ((int)m_MySeat) + 1, FightProtocol.GUO_CREQ, boultModel);
                Debug.Log("过");
                
               
                break;
        }
    }


    #endregion
    #region 结算界面
    public void InitScoreResultWindow(ScoreModel scoreModel)
    {
       GameObject obj = ResourcesMgr.m_Instance.GetGameObject("Prefab/MainScene/DuiJuContainer");
        obj.transform.SetParent(GameObject.Find("CenterContainer").transform);
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
        obj.GetComponent<DuiJuContainerWindowCtr>().InitScoreResult(scoreModel);
        btn_Exit.gameObject.SetActive(true);
    }
    public void AvtiveResultScoreWindow()
    {
        GameObject obj = ResourcesMgr.m_Instance.GetGameObject("Prefab/MainScene/ResultScore/ResultScoreWindow");
        obj.transform.SetParent(m_CenterContain);
        //obj.transform.localPosition = new Vector3(3, -30, 0);
        obj.transform.localPosition = new Vector3(0, 0, 0);
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;

        RoomInfoMgr roomInfo = new RoomInfoMgr();
        RoomInfoMgr.m_Instance = roomInfo;
        roomInfo.m_PlayerEastName = "zzzz";
        roomInfo.m_PlayerEastIconID = 1;
        roomInfo.m_PlayerSouthName = "sss";
        roomInfo.m_PlayerSouthIconID = 2;
        roomInfo.m_PlayerWestName = "xxx";
        roomInfo.m_PlayerWestIconID = 3;
        roomInfo.m_PlayerNorthName = "nnn";
        roomInfo.m_PlayerNorthIconID = 4;
        roomInfo.m_MySeat = MainSceneMger.PlayerSeat.PlayerWest;

        obj.GetComponent<ResultScoreWindowCtr>().InitResultScoreWindow(roomInfo, 34);
    }
    #endregion
    #region 流局
    public void ActiveLiuJu(bool isActive)
    {
        LiuJu.SetActive(isActive);
    }
    public void ActiveNext_Round()
    {
        GameObject obj = Instantiate( Resources.Load("Prefab/MainScene/Btn_Next") as GameObject);
        obj.transform.SetParent(m_CenterContain);
        obj.transform.localPosition = new Vector3(0,-180,0);
        obj.transform.localScale = Vector3.one;
        obj.transform.localRotation =Quaternion.Euler(Vector3.zero);
    }
    public void SetGameCount(int count)
    {
        this.transform.parent.GetComponentInChildren<Img_DuiJuViewCtr>().SetGameCount(count);
    }
    #endregion
    #region
    public void ActivePlayerSeatUI(MainSceneMger.PlayerSeat m_MySeat,int m_ActiveSeat)
    {
        switch (m_MySeat)
        {
            case MainSceneMger.PlayerSeat.PlayerEast:
                switch (m_ActiveSeat)
                {
                    case 1:
                        Debug.Log("当前为东家回合");
                        Img_HeadEffectDown.SetActive(true);
                        Img_HeadEffectRight.SetActive(false);
                        Img_HeadEffectUp.SetActive(false);
                        Img_HeadEffectLeft.SetActive(false);
                        break;
                    case 2:
                        Debug.Log("当前为南家回合");
                        Img_HeadEffectDown.SetActive(false);
                        Img_HeadEffectRight.SetActive(true);
                        Img_HeadEffectUp.SetActive(false);
                        Img_HeadEffectLeft.SetActive(false);
                        break;
                    case 3:
                        Debug.Log("当前为西家回合");
                        Img_HeadEffectDown.SetActive(false);
                        Img_HeadEffectRight.SetActive(false);
                        Img_HeadEffectUp.SetActive(true);
                        Img_HeadEffectLeft.SetActive(false);
                        break;
                    case 4:
                        Debug.Log("当前为北家回合");
                        Img_HeadEffectDown.SetActive(false);
                        Img_HeadEffectRight.SetActive(false);
                        Img_HeadEffectUp.SetActive(false);
                        Img_HeadEffectLeft.SetActive(true);
                        break;
                    default:
                        Debug.Log("错误座位！");
                        break;
                }
                break;
            case MainSceneMger.PlayerSeat.PlayerSouth:
                switch (m_ActiveSeat)
                {
                    case 1:
                        Debug.Log("当前为东家回合");
                        Img_HeadEffectDown.SetActive(false);
                        Img_HeadEffectRight.SetActive(false);
                        Img_HeadEffectUp.SetActive(false);
                        Img_HeadEffectLeft.SetActive(true);
                        break;
                    case 2:
                        Debug.Log("当前为南家回合");
                        Img_HeadEffectDown.SetActive(true);
                        Img_HeadEffectRight.SetActive(false);
                        Img_HeadEffectUp.SetActive(false);
                        Img_HeadEffectLeft.SetActive(false);
                        break;
                    case 3:
                        Debug.Log("当前为西家回合");
                        Img_HeadEffectDown.SetActive(false);
                        Img_HeadEffectRight.SetActive(true);
                        Img_HeadEffectUp.SetActive(false);
                        Img_HeadEffectLeft.SetActive(false);
                        break;
                    case 4:
                        Debug.Log("当前为北家回合");
                        Img_HeadEffectDown.SetActive(false);
                        Img_HeadEffectRight.SetActive(false);
                        Img_HeadEffectUp.SetActive(true);
                        Img_HeadEffectLeft.SetActive(false);
                        break;
                    default:
                        Debug.Log("错误座位！");
                        break;
                }
                break;
            case MainSceneMger.PlayerSeat.PlayerWest:
                switch (m_ActiveSeat)
                {
                    case 1:
                        Debug.Log("当前为东家回合");
                        Img_HeadEffectDown.SetActive(false);
                        Img_HeadEffectRight.SetActive(false);
                        Img_HeadEffectUp.SetActive(true);
                        Img_HeadEffectLeft.SetActive(false);
                        break;
                    case 2:
                        Debug.Log("当前为南家回合");
                        Img_HeadEffectDown.SetActive(false);
                        Img_HeadEffectRight.SetActive(false);
                        Img_HeadEffectUp.SetActive(false);
                        Img_HeadEffectLeft.SetActive(true);
                        break;
                    case 3:
                        Debug.Log("当前为西家回合");
                        Img_HeadEffectDown.SetActive(true);
                        Img_HeadEffectRight.SetActive(false);
                        Img_HeadEffectUp.SetActive(false);
                        Img_HeadEffectLeft.SetActive(false);
                        break;
                    case 4:
                        Debug.Log("当前为北家回合");
                        Img_HeadEffectDown.SetActive(false);
                        Img_HeadEffectRight.SetActive(true);
                        Img_HeadEffectUp.SetActive(false);
                        Img_HeadEffectLeft.SetActive(false);
                        break;
                    default:
                        Debug.Log("错误座位！");
                        break;
                }
                break;
            case MainSceneMger.PlayerSeat.PlayerNorth:
                switch (m_ActiveSeat)
                {
                    case 1:
                        Debug.Log("当前为东家回合");
                        Img_HeadEffectDown.SetActive(false);
                        Img_HeadEffectRight.SetActive(true);
                        Img_HeadEffectUp.SetActive(false);
                        Img_HeadEffectLeft.SetActive(false);
                        break;
                    case 2:
                        Debug.Log("当前为南家回合");
                        Img_HeadEffectDown.SetActive(false);
                        Img_HeadEffectRight.SetActive(false);
                        Img_HeadEffectUp.SetActive(true);
                        Img_HeadEffectLeft.SetActive(false);
                        break;
                    case 3:
                        Debug.Log("当前为西家回合");
                        Img_HeadEffectDown.SetActive(false);
                        Img_HeadEffectRight.SetActive(false);
                        Img_HeadEffectUp.SetActive(false);
                        Img_HeadEffectLeft.SetActive(true);
                        break;
                    case 4:
                        Debug.Log("当前为北家回合");
                        Img_HeadEffectDown.SetActive(true);
                        Img_HeadEffectRight.SetActive(false);
                        Img_HeadEffectUp.SetActive(false);
                        Img_HeadEffectLeft.SetActive(false);
                        break;
                    default:
                        Debug.Log("错误座位！");
                        break;
                }
                break;
            default:
                Debug.Log("错误座位！");
                break;
        }
    }
    #endregion

    #region 实时流水和提示按钮
    public void Btn_realTimeScoreClick()
    {
        GameObject obj = ResourcesMgr.m_Instance.GetGameObject("Prefab/MainScene/ResultScore/ResultScoreWindow");
        obj.transform.SetParent(m_CenterContain);
        //obj.transform.localPosition = new Vector3(3, -30, 0);
        obj.transform.localPosition = new Vector3(0, 0, 0);
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;

        RoomInfoMgr roomInfo = new RoomInfoMgr();
        RoomInfoMgr.m_Instance = roomInfo;
        roomInfo.m_PlayerEastName = "zzzz";
        roomInfo.m_PlayerEastIconID = 1;
        roomInfo.m_PlayerSouthName = "sss";
        roomInfo.m_PlayerSouthIconID = 2;
        roomInfo.m_PlayerWestName = "xxx";
        roomInfo.m_PlayerWestIconID = 3;
        roomInfo.m_PlayerNorthName = "nnn";
        roomInfo.m_PlayerNorthIconID = 4;
        roomInfo.m_MySeat = MainSceneMger.PlayerSeat.PlayerWest;
        obj.GetComponent<ResultScoreWindowCtr>().InitResultScoreWindow(roomInfo, 34);

        //ScoreModel scoreModel = new ScoreModel();
        //scoreModel.Round = 3;
        //scoreModel.RoundOne = new System.Collections.Generic.Dictionary<int, int>();
        //scoreModel.RoundTwo = new System.Collections.Generic.Dictionary<int, int>();
        //scoreModel.RoundThree = new System.Collections.Generic.Dictionary<int, int>();
        //scoreModel.RoundFour = new System.Collections.Generic.Dictionary<int, int>();
        //for (int i = 1; i <= 4; i++)
        //{
        //    scoreModel.RoundOne.Add(i, i * 1000);
        //    scoreModel.RoundTwo.Add(i, i * 1200);
        //    scoreModel.RoundThree.Add(i, 0);
        //    scoreModel.RoundFour.Add(i, 0);
        //}

        //obj.GetComponent<DuiJuContainerWindowCtr>().InitScoreResult(scoreModel);

        //NetManager.m_Instance.SendMessage(Protocol.TYPE_FIGHT, 0, FightProtocol.GET_CURRENT_ACCOUNTS_CREQ, null);
    }
    public void ActiveRealTimeScoreWindow(ScoreModel scoreModel)
    {
        GameObject obj = ResourcesMgr.m_Instance.GetGameObject("Prefab/MainScene/ResultScore/DuiJuContainer");
        obj.transform.SetParent(m_CenterContain);
        obj.transform.localPosition = new Vector3(3, -30, 0);
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;
        obj.GetComponent<DuiJuContainerWindowCtr>().InitScoreResult(scoreModel);
    }
   
    public void Btn_TipClick()
    {

    }
    #endregion
}
