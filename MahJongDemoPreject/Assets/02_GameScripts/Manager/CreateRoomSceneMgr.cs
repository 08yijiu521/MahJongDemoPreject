/************************************************************************
*	@brief		：CreateRoom场景管理类.
*		
*	@author		：李经纬
*	@copyright	：时光科技   2017
*	
*	@date		：2017年2月28日15:00:00
************************************************************************/
using UnityEngine;
using System.Collections;
using GameProtocol.Model;
using UnityEngine.SceneManagement;

public class CreateRoomSceneMgr : MonoBehaviour
{
    public static CreateRoomSceneMgr m_Instance = null;

    private void Awake()
    {
        m_Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        AudioManager.Instance.PlayBackgrounfAudio("playingInGame");
    }

    // Update is called once per frame
    void Update()
    {

    }

    //创建房间
    public void CreateRoom(RoomModel roomModel)
    {
        if (roomModel != null)
        {
            //由于是创建房间者，所以他为玩家东
            UserModel user = roomModel.DongUser;
            RoomInfoMgr.m_Instance.m_MySeat = MainSceneMger.PlayerSeat.PlayerEast;
            RoomInfoMgr.m_Instance.m_RoomID = roomModel.RoomID;
            RoomInfoMgr.m_Instance.m_PlayerEastID = user.UserID;
            RoomInfoMgr.m_Instance.m_PlayerEastName = user.UserName.ToString();
            RoomInfoMgr.m_Instance.m_PlayerEastCoin = user.UserCoin;
            RoomInfoMgr.m_Instance.m_PlayerEastDiamond = user.UserDiamond;
            RoomInfoMgr.m_Instance.m_PlayerEastIconID = user.UserIconNum;
            SceneManager.LoadScene("MainScene");  //创建房间
        }
        else
        {
            Debug.Log("获取失败");
        }
        InitRoomInfo(roomModel);
    }

    /// <summary>
    /// 加入房间
    /// </summary>
    /// <param name="roomModel"></param>
    public void JoinRoom(RoomModel roomModel)
    {
        if (string.IsNullOrEmpty(roomModel.Error))
        {

            //switch (roomModel.CurrentCount)
            //{
            //    case 1:
            //        RoomInfoMgr.m_Instance.m_MySeat = MainSceneMger.PlayerSeat.PlayerEast;
            //        break;
            //    case 2:
            //        RoomInfoMgr.m_Instance.m_MySeat = MainSceneMger.PlayerSeat.PlayerSouth;
            //        break;
            //    case 3:
            //        RoomInfoMgr.m_Instance.m_MySeat = MainSceneMger.PlayerSeat.PlayerWest;
            //        break;
            //    case 4:
            //        RoomInfoMgr.m_Instance.m_MySeat = MainSceneMger.PlayerSeat.PlayerNorth;
            //        break;
            //    default:
            //        break;
            //}
            int userId = PlayerMgr.m_Instance.UserID;

            if (userId==roomModel.DongUser.UserID)
            {
                SetSeat(roomModel.DongUser.Seat);
            }
            else if(userId==roomModel.NanUser.UserID)
            {
                SetSeat(roomModel.NanUser.Seat);
            }
            else if(userId==roomModel.XiUser.UserID)
            {
                SetSeat(roomModel.XiUser.Seat);
            }
            else if(userId==roomModel.BeiUser.UserID)
            {
                SetSeat(roomModel.BeiUser.Seat);
            }
            
            //if (roomModel.NanUser.UserID == -1)
            //{
            //    Debug.Log("PlayerSouth");
            //    RoomInfoMgr.m_Instance.m_MySeat = MainSceneMger.PlayerSeat.PlayerSouth;
            //}
            //else if(roomModel.XiUser.UserID == -1)
            //{
            //    Debug.Log("PlayerWest");
            //    RoomInfoMgr.m_Instance.m_MySeat = MainSceneMger.PlayerSeat.PlayerWest;
            //}
            //else if (roomModel.BeiUser.UserID == -1)
            //{
            //    Debug.Log("PlayerNorth");
            //    RoomInfoMgr.m_Instance.m_MySeat = MainSceneMger.PlayerSeat.PlayerNorth;
            //}
            InitRoomInfo(roomModel);
            SceneManager.LoadScene("MainScene");  //进入房间
    }
        else
        {
            CreateRoomViewMgr.m_Instance.OnJoinRoomBtnClick();
        }
    }

    void SetSeat(int seat)
    {
        switch (seat)
        {
            case 1:
                RoomInfoMgr.m_Instance.m_MySeat = MainSceneMger.PlayerSeat.PlayerEast;
                break;
            case 2:
                RoomInfoMgr.m_Instance.m_MySeat = MainSceneMger.PlayerSeat.PlayerSouth;
                break;
            case 3:
                RoomInfoMgr.m_Instance.m_MySeat = MainSceneMger.PlayerSeat.PlayerWest;
                break;
            case 4:
                RoomInfoMgr.m_Instance.m_MySeat = MainSceneMger.PlayerSeat.PlayerNorth;
                break;
            default:
                break;
        }
    }
    public void InitRoomInfo(RoomModel roomModel)
    {
        
        Debug.Log("RoomModel" + roomModel.DongUser);
        Debug.Log("RoomModel" + roomModel.NanUser);
        Debug.Log("RoomModel" + roomModel.XiUser);
        Debug.Log("RoomModel" + roomModel.BeiUser);
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
                    RoomInfoMgr.m_Instance.m_PlayerEastID = -1;
                    RoomInfoMgr.m_Instance.m_PlayerEastName = string.Empty;
                    RoomInfoMgr.m_Instance.m_PlayerEastCoin = -1;
                    RoomInfoMgr.m_Instance.m_PlayerEastDiamond = -1;
                    RoomInfoMgr.m_Instance.m_PlayerEastIconID = -1;
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
            else
            {
                RoomInfoMgr.m_Instance.m_PlayerEastID = -1;
                RoomInfoMgr.m_Instance.m_PlayerEastName = string.Empty;
                RoomInfoMgr.m_Instance.m_PlayerEastCoin = -1;
                RoomInfoMgr.m_Instance.m_PlayerEastDiamond = -1;
                RoomInfoMgr.m_Instance.m_PlayerEastIconID = -1;
            }

            if (Nanuser != null)
            {
                if (Nanuser.UserID <= 0)
                {
                    Debug.Log("当前玩家南不在，不显示玩家南头像");
                    RoomInfoMgr.m_Instance.m_PlayerSouthID = -1;
                    RoomInfoMgr.m_Instance.m_PlayerSouthName = string.Empty;
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
            else
            {
                Debug.Log("当前玩家南不在，不显示玩家南头像");
                RoomInfoMgr.m_Instance.m_PlayerSouthID = -1;
                RoomInfoMgr.m_Instance.m_PlayerSouthName = string.Empty;
                RoomInfoMgr.m_Instance.m_PlayerSouthCoin = -1;
                RoomInfoMgr.m_Instance.m_PlayerSouthDiamond = -1;
                RoomInfoMgr.m_Instance.m_PlayerSouthIconID = -1;
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
            else
            {
                RoomInfoMgr.m_Instance.m_PlayerWestID = -1;
                RoomInfoMgr.m_Instance.m_PlayerWestName = string.Empty;
                RoomInfoMgr.m_Instance.m_PlayerWestCoin = -1;
                RoomInfoMgr.m_Instance.m_PlayerWestDiamond = -1;
                RoomInfoMgr.m_Instance.m_PlayerWestIconID = -1;
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
            else
            {
                RoomInfoMgr.m_Instance.m_PlayerNorthID = -1;
                RoomInfoMgr.m_Instance.m_PlayerNorthName = string.Empty;
                RoomInfoMgr.m_Instance.m_PlayerNorthCoin = -1;
                RoomInfoMgr.m_Instance.m_PlayerNorthDiamond = -1;
                RoomInfoMgr.m_Instance.m_PlayerNorthIconID = -1;
            }
    }
}

    public void HavePlayerJoinRoom(RoomModel roomModel)
    {
        InitRoomInfo(roomModel);
    }


}
