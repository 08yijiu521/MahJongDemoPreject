/************************
 * Title：
 * Function：
 * 	－ 控制面板的切换
 * Used By：UIManager
 * Author：qwt
 * Date：2017.2.24
 * Version：1.0
 * Description：
 *
************************/
using UnityEngine;
using System.Collections;
using GameProtocol;

public class CreateRoomViewMgr : MonoBehaviour {
    public GameObject uiRoot;                       //UIRoot
    public GameObject joinRoomWindowPrefab;         //加入房间窗口
    public UIButton Btn_CloseRoomNumPanel;          //关闭加入房间窗口按钮
    public UIButton Btn_CreateRoom;                 //创建房间按钮
    public UIButton Btn_JoinRoom;                   //加入房间按钮        
    public UILabel Label_UserName;                  //玩家用户名
    public UILabel Label_Diamond;                   //玩家钻石数显示
    public UISprite Img_UserIcon;                     //玩家头像

    public GameObject joinRoomWindow;

    public bool isClick = false;
    public static CreateRoomViewMgr m_Instance = null;

    private void Awake()
    {
        m_Instance = this;
    }

    void Start () {

        #region 创建房间场景按键相应事件
        EventDelegate Btn_CreateRoomEvent = new EventDelegate(this, "OnCreateRoomBtnClick");
        EventDelegate Btn_JoinRoomEvent = new EventDelegate(this, "OnJoinRoomBtnClick");
        EventDelegate Btn_CloseRoomNumPanelEvent = new EventDelegate(this, "OnCloseRoomNumPanelClick");

        Btn_CreateRoom.onClick.Add(Btn_CreateRoomEvent);
        Btn_JoinRoom.onClick.Add(Btn_JoinRoomEvent);
        Btn_CloseRoomNumPanel.onClick.Add(Btn_CloseRoomNumPanelEvent);
        #endregion
        NetManager.m_Instance.SendMessage(Protocol.TYPE_USER, 0, UserProtocol.INFO_CREQ, null);
    }


    /// <summary>
    /// 创建房间场景UI初始化
    /// </summary>
    public void _Init()
    {
        Debug.Log("_Init");
        Label_UserName.text = PlayerMgr.m_Instance.PlayerName.ToString();
        Label_Diamond.text = PlayerMgr.m_Instance.PlayerDiamond.ToString();
        Img_UserIcon.spriteName = InitPlayerIcon(PlayerMgr.m_Instance.IconNum);
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

    //创建房间按钮点击事件
    public void OnCreateRoomBtnClick()
    {
        Debug.Log("创建房间按钮被按下");
        AudioManager.Instance.PlayEffectAudio("ui_click");
        //向服务器发送创建房间请求
        NetManager.m_Instance.SendMessage(Protocol.TYPE_ROOM, 0, RoomProtocol.CREATE_CREQ, null);
        Btn_CreateRoom.GetComponent<Collider>().enabled = false;
    }

    //加入房间按钮点击事件
    public void OnJoinRoomBtnClick()
    {
        AudioManager.Instance.PlayEffectAudio("ui_click");
        //显示输入房间面板
        joinRoomWindow = NGUITools.AddChild(uiRoot, joinRoomWindowPrefab);
        joinRoomWindow.GetComponent<TweenScale>().PlayForward();
        //将创建房间，加入房间按钮禁用掉
        Btn_CreateRoom.GetComponent<Collider>().enabled = false;
        Btn_JoinRoom.GetComponent<Collider>().enabled = false;
    }

    //关闭输入房间号码面板按钮点击事件
    public void OnCloseRoomNumPanelClick()
    {
        AudioManager.Instance.PlayEffectAudio("ui_click");
        joinRoomWindow.GetComponent<TweenScale>().PlayReverse();
        Destroy(joinRoomWindow, 0.5f);
        Btn_CreateRoom.GetComponent<Collider>().enabled = true;
        Btn_JoinRoom.GetComponent<Collider>().enabled = true;
    }
}
