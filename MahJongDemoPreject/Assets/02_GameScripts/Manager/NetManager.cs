/************************************************************************
*	@brief		：客户端网络管理(连接/发送/接收等事件提供接口).
*		
*	@author		：李经纬
*	@copyright	：时光科技 2017
*	
*	@date		：2017年2月21日15:00:00
************************************************************************/
using UnityEngine;
using System.Collections;
using GameProtocol;
using GameProtocol.Model;
using UnityEngine.SceneManagement;

//NetManager继承自MonoBehaviour必须当一个GameObject来使用.
public class NetManager : MonoBehaviour
{

    private ConnectServer m_Client;

    public static NetManager m_Instance = null;

    public GameObject obj_LoginMessage;

    private void Awake()
    {
        m_Instance = this;
    }

    void _init()
    {
        m_Client = new ConnectServer();
    }
    //重新初始化. 会断开当前连接需要重新连接.
    public void ReInit()
    {
        _init();
    }
    //连接目标
    public void Connect()
    {
        ReInit();
        m_Client.OnClickConnect();
    }

    //发送信息
    public void SendMessage(byte type, int area, int command, object message)
    {
        m_Client.write(type, area, command, message);
    }

    //关闭当前连接
    public void Close()
    {
        if (m_Client != null)
        {
            m_Client.Close();
            m_Client = null;
        }
    }

    void Update()
    {
        if (m_Client != null)
        {
            if (m_Client.messages.Count > 0)
            {
                //SocketModel model = ConnectServer.m_Instance.messages.Dequeue();
                SocketModel model = m_Client.messages.Dequeue();
                switch (model.type)
                {
                    //用户登陆
                    case Protocol.TYPE_LOGIN:
                        Debug.Log("TYPE_LOGIN");
                        //login.MessageReceive(model);
                        switch (model.command)
                        {
                            case LoginProtocol.LOGIN_SRES:
                                Debug.Log("LOGIN_SRES");
                                LoginSceneMgr.m_Instance.login(model.GetMessage<int>());
                                break;
                            case LoginProtocol.REG_SRES:
                                LoginSceneMgr.m_Instance.reg(model.GetMessage<int>());
                                break;
                            case 99:
                                //txt_recieve.text = model.message.ToString();
                                Debug.Log("收到服务器消息：" + model.message.ToString());
                                break;
                        }
                        break;
                    //用户信息
                    case Protocol.TYPE_USER:
                        //user.MessageReceive(model);
                        Debug.Log("收到服务器消息：TYPE_USER");
                        switch (model.command)
                        {
                            
                            case UserProtocol.INFO_SRES:
                                PlayerMgr.m_Instance.InitPlayerInfo(model.GetMessage<UserModel>());
                                break;
                        }
                        break;
                    //房间有关消息
                    case Protocol.TYPE_ROOM:
                        Debug.Log("TYPE_ROOM");
                        switch (model.command)
                        {
                            //创建房间
                            case RoomProtocol.CREATE_SRES:
                                CreateRoomSceneMgr.m_Instance.CreateRoom(model.GetMessage<RoomModel>());
                                break;
                            //有新朋友加入
                            case RoomProtocol.JOIN_BRO:
                                Debug.Log("RoomProtocol.JOIN_BRO");
                                CreateRoomSceneMgr.m_Instance.HavePlayerJoinRoom(model.GetMessage<RoomModel>());
                                MainViewMgr.m_Instance._InitMainUI();//初始化UI
                                break;
                            //
                            case RoomProtocol.JOIN_SRES:
                                Debug.Log("RoomProtocol.JOIN_SRES");
                                CreateRoomSceneMgr.m_Instance.JoinRoom(model.GetMessage<RoomModel>());
                                if (MainViewMgr.m_Instance != null)
                                {
                                    MainViewMgr.m_Instance._InitMainUI();//初始化UI
                                }
                                break;
                            case RoomProtocol.LEAVE_SRES:
                                Debug.Log("有玩家离开游戏");
                                //if (MainViewMgr.m_Instance != null)
                                //{
                                    //MainViewMgr.m_Instance._InitMainUI();//初始化UI
                                    SceneManager.LoadScene("CreateRoomScene");
                                //}
                                break;
                            case RoomProtocol.LEAVE_BRO://广播信息
                                Debug.Log("有玩家离开游戏");
                                if (MainViewMgr.m_Instance != null)
                                {
                                    MainViewMgr.m_Instance.InitRoomInfo(model.GetMessage<RoomModel>());//刷新房间信息
                                    MainViewMgr.m_Instance._InitMainUI();//初始化UI
                                }
                                break;
                            case RoomProtocol.DISSOLVE_BRO:
                                if (RoomInfoMgr.m_Instance.m_MySeat != MainSceneMger.PlayerSeat.PlayerEast)
                                    SceneManager.LoadScene("CreateRoomScene");
                                Debug.Log("DISSOLVE_BRO");
                                obj_LoginMessage.GetComponent<LoginMessageInit>().InitBox("房间已解散");
                                GameObject uiRoot = GameObject.Find("UI Root");
                                NGUITools.AddChild(uiRoot, obj_LoginMessage);
                                break;
                            case RoomProtocol.READY_BRO:
                                {
                                    //将某个玩家的准备中激活
                                    Debug.Log("准备开始游戏");
                                    MainViewMgr.m_Instance.initWaitInfo(model.GetMessage<RoomModel>());

                                    break;
                                }
                            case RoomProtocol.FINISH_BRO:
                                {
                                    //开始发牌
                                    Debug.Log("FINISH_BRO:准备完毕！");
                                    //MainViewMgr.m_Instance.CloseWaitInfo(model.GetMessage<RoomModel>());
                                    //MainSceneMger.m_Instance.StartGame(model.GetMessage<RoomModel>());
                                    //GameManager.m_instance.StartGame(model.GetMessage<RoomModel>());
                                    GameManager.m_instance.StartGame();
                                    break;
                                }
                            
                               
                        } 
                        break;

                    case Protocol.TYPE_FIGHT:
                        Debug.Log("TYPE_FIGHT");
                        switch (model.command)
                        {
                            case FightProtocol.START_BRO:
                                {
                                    //主逻辑---开始游戏：掷骰子、发牌、定缺、摸牌
                                    Debug.Log("START_BRO"+":"+model.type);
                                    GameManager.m_instance.FaPai(model.GetMessage<FightModel>());
                                    break;
                                }
                            case FightProtocol.QUE_WAIT_BRO://定缺准备中
                                Debug.Log("定缺准备中");
                                MainViewMgr.m_Instance.InitDingQueWaitingInfo(model.GetMessage<FightModel>());
                                break;
                            case FightProtocol.QUE_FINISH_BRO://定缺准备完毕
                                Debug.Log("定缺完毕");
                                MainViewMgr.m_Instance.InitDingQueImg(model.GetMessage<FightModel>());
                                break;
                            case FightProtocol.GET_BRO://摸牌
                                Debug.Log("GET_BRO_____________________________");
                                GameManager.m_instance.MoPai(model.area,model.GetMessage<BoutModel>());
                                break;
                            case FightProtocol.GET_CAN_GANG_SERS://暗杠
                                Debug.Log("GET_CAN_GANG_SERS_____________________________");
                                MainViewMgr.m_Instance.ActiveGangBtn(false,model.GetMessage<BoutModel>());
                                break;
                            case FightProtocol.GET_CAN_HU_SERS://自摸
                                Debug.Log("GET_CAN_HU_SERS_____________________________");
                                MainViewMgr.m_Instance.ActiveHuBtn(true,model.GetMessage<BoutModel>());
                                break;
                            case FightProtocol.GET_CAN_HU_AND_GANG_SERS:
                                MainViewMgr.m_Instance.ActiveHuGangBtn(true,false, model.GetMessage<BoutModel>());
                                break;
                            case FightProtocol.GET_NONE_SERS://摸牌后既没有杠、也没有胡
                                //激活自身的出牌状态
                                Debug.Log("GET_NONE_SERS_____________________________");
                                GameManager.m_instance.ChuPai(model.GetMessage<BoutModel>());
                                break;
                            case FightProtocol.PENG_BRO://有人碰牌
                                Debug.Log("PENG_BRO_____________________________");
                                GameManager.m_instance.Peng(model.area, model.GetMessage<BoutModel>());

                                break;
                            case FightProtocol.GANG_BRO://有人杠牌
                                Debug.Log("GANG_BRO_____________________________");
                                GameManager.m_instance.Gang(model.area, model.GetMessage<BoutModel>());
                                break;
                            case FightProtocol.HU_BRO://有人胡牌
                                Debug.Log("HU_BRO_____________________________");
                                GameManager.m_instance.Hu(model.area, model.GetMessage<BoutModel>());
                                break;
                            case FightProtocol.PUT_BRO://客户端出牌请求的广播
                                Debug.Log("PUT_BRO_____________________________");
                                GameManager.m_instance.SomeOneChuPai(model.area,model.GetMessage<int>());
                                break;
                            case FightProtocol.GUO_BRO://客户端过请求的广播
                                Debug.Log("GUO_BRO_____________________________");
                                GameManager.m_instance.Guo(model.GetMessage<BoutModel>());
                                break;
                            case FightProtocol.PUT_CAN_PENG_SERS://碰其他玩家，需要检测是否处于等待状态
                                Debug.Log("PUT_CAN_PENG_SERS_____________________________");
                                MainViewMgr.m_Instance.ActivePengBtn(model.GetMessage<BoutModel>());
                                break;
                            case FightProtocol.PUT_CAN_GANG_SERS://明杠其他玩家，需要解析检测是否处于等待状态
                                Debug.Log("PUT_CAN_GANG_SERS_____________________________");
                                MainViewMgr.m_Instance.ActiveGangBtn(true,model.GetMessage<BoutModel>());
                                break;
                            case FightProtocol.PUT_CAN_HU_SERS://胡其他玩家
                                Debug.Log("PUT_CAN_HU_SERS_____________________________");
                                MainViewMgr.m_Instance.ActiveHuBtn(false,model.GetMessage<BoutModel>());
                                break;
                            case FightProtocol.PUT_CAN_HU_AND_PENG_SERS://有人碰、胡
                                MainViewMgr.m_Instance.ActiveHuPengBtn(model.GetMessage<BoutModel>());
                                break;
                            case FightProtocol.PUT_CAN_HU_AND_GANG_SERS://有人杠、胡
                                MainViewMgr.m_Instance.ActiveHuGangBtn(false,true,model.GetMessage<BoutModel>());
                                break;
                            case FightProtocol.FIGHT_END_BRO://对局结束
                                Debug.Log("FIGHT_END_____________________________");
                                //model.area=0 没有参数
                                Debug.Log("结束游戏");
                                //结算请求
                                if (MainViewMgr.m_Instance.m_MySeat.Equals(MainSceneMger.PlayerSeat.PlayerEast))
                                    SendMessage(Protocol.TYPE_FIGHT, 0, FightProtocol.SETTLE_ACCOUNTS_CREQ, null);
                                break;
                            case FightProtocol.PLATE_END_BRO://结算界面
                                Debug.Log("PLATE_END_BRO_____________________________");
                                //0  有人胡，打完了
                                //1  没人胡，打完了
                                GameManager.m_instance.FightEnd(model.GetMessage<int>());
                                break;
                            case FightProtocol.SETTLE_ACCOUNTS_BRO://流水
                                Debug.Log("SETTLE_ACCOUNTS_BRO");
                                GameManager.m_instance.GameEnd();
                                MainViewMgr.m_Instance.ActiveNext_Round();
                                MainViewMgr.m_Instance.InitScoreResultWindow(model.GetMessage<ScoreModel>());
                                
                                break;
                            case FightProtocol.ROUND_DISSOLVE_BRO://结算
                                Debug.Log("ROUND_DISSOLVE_BRO");
                                GameManager.m_instance.GameEnd();
                                MainViewMgr.m_Instance.InitScoreResultWindow(model.GetMessage<ScoreModel>());
                                
                                break;
                            case FightProtocol.NEXT_ROUND_BRO://下一局
                                GameManager.m_instance.RePlay();
                                Debug.Log("NEXT_ROUND_BRO");
                                GameManager.m_instance.Next_Round(model.GetMessage<FightModel>());
                                break;
                            case FightProtocol.DESTROY_FIGHT_BRO://解散房间，退出游戏，返回创建房间场景
                                SceneManager.LoadScene("CreateRoomScene");
                                Debug.Log("DESTROY_FIGHT_BRO");
                                obj_LoginMessage.GetComponent<LoginMessageInit>().InitBox("房间已解散");
                                GameObject uiRoot = GameObject.Find("UI Root");
                                NGUITools.AddChild(uiRoot, obj_LoginMessage);
                                break;
                            case FightProtocol.GET_CURRENT_ACCOUNTS_CREQ://获取实时流水
                                MainViewMgr.m_Instance.ActiveRealTimeScoreWindow(model.GetMessage<ScoreModel>());
                                break;
                        }
                        break;
                }
            }
        }
    }

}
