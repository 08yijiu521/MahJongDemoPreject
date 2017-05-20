/************************************************************************
*	@brief		：角色管理器(所有角色数据的保存和处理类).
*		
*	@author		：李经纬
*	@copyright	：时光科技 2017
*	
*	@date		：2017年2月23日13:32:27
************************************************************************/
using UnityEngine;
using System.Collections;
using GameProtocol.Model;

public class PlayerMgr : MonoBehaviour {

    public static PlayerMgr m_Instance = null;

    #region 玩家数据接口
    [System.NonSerialized]
    private int m_UserID;              //用户ID

    [System.NonSerialized]
    private int m_PlayerCoin;          //金币

    [System.NonSerialized]
    private int m_PlayerDiamond;       //钻石

    [System.NonSerialized]
    private string m_PlayerName;       //用户名

    [System.NonSerialized]
    private int m_IconNum;             //玩家头像ID

    #endregion


    #region 数据操作接口
    //用户ID
    public int UserID
    {
        get { return m_UserID; }
        set { m_UserID = value; }
    }

    //玩家用户名
    public string PlayerName
    {
        get { return m_PlayerName; }
        set { m_PlayerName = value; }
    }

    //玩家金币
    public int PlayerCoin
    {
        get { return m_PlayerCoin; }
        set { m_PlayerCoin = value; }
    }

    //玩家钻石
    public int PlayerDiamond
    {
        get { return m_PlayerDiamond; }
        set { m_PlayerDiamond = value; }
    }

    //玩家头像
    public int IconNum
    {
        get { return m_IconNum; }
        set { m_IconNum = value; }
    }

    /// <summary>
    /// 初始化玩家信息
    /// </summary>
    public void InitPlayerInfo(UserModel userModel)
    {
        if (userModel.UserID == -1)
        {
            Debug.Log("ID不存在");
        }
        UserID = userModel.UserID;
        PlayerName = userModel.UserName;
        PlayerCoin = userModel.UserCoin;
        PlayerDiamond = userModel.UserDiamond;
        IconNum = userModel.UserIconNum;

        CreateRoomViewMgr.m_Instance._Init();
    }
    #endregion

    private void Awake()
    {
        m_Instance = this;
    }
    public void ResetInfo()
    {

    }
}
