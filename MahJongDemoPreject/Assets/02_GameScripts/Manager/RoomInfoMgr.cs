/************************************************************************
*	@brief		：房间信息管理类.
*		
*	@author		：李经纬
*	@copyright	：时光科技   2017
*	
*	@date		：2017年2月28日13:50:57
************************************************************************/
using UnityEngine;
using System.Collections;

public class RoomInfoMgr : MonoBehaviour {
    public static RoomInfoMgr m_Instance = null;

    [System.NonSerialized]
    public int m_RoomID;            //房间号

    [System.NonSerialized]
    public MainSceneMger.PlayerSeat m_MySeat;     //当前玩家座位

    [System.NonSerialized]
    public int m_PlayerEastID;      //玩家东的ID

    [System.NonSerialized]
    public string m_PlayerEastName; //玩家东的name

    [System.NonSerialized]
    public int m_PlayerEastCoin;              //玩家东的金币

    [System.NonSerialized]
    public int m_PlayerEastDiamond;      //玩家东的钻石

    [System.NonSerialized]
    public int m_PlayerEastIconID;      //玩家东的头像编号

    [System.NonSerialized]
    public int m_PlayerSouthID;      //玩家南的ID

    [System.NonSerialized]
    public string m_PlayerSouthName; //玩家南的name

    [System.NonSerialized]
    public int m_PlayerSouthCoin;              //玩家南的金币

    [System.NonSerialized]
    public int m_PlayerSouthDiamond;      //玩家南的钻石

    [System.NonSerialized]
    public int m_PlayerSouthIconID;      //玩家南的头像编号

    [System.NonSerialized]
    public int m_PlayerWestID;      //玩家西的ID

    [System.NonSerialized]
    public string m_PlayerWestName; //玩家西的name

    [System.NonSerialized]
    public int m_PlayerWestCoin;              //玩家西的金币

    [System.NonSerialized]
    public int m_PlayerWestDiamond;      //玩家西的钻石

    [System.NonSerialized]
    public int m_PlayerWestIconID;      //玩家西的头像编号

    [System.NonSerialized]
    public int m_PlayerNorthID;      //玩家北的ID

    [System.NonSerialized]
    public string m_PlayerNorthName; //玩家北的name

    [System.NonSerialized]
    public int m_PlayerNorthCoin;              //玩家北的金币

    [System.NonSerialized]
    public int m_PlayerNorthDiamond;      //玩家北的钻石

    [System.NonSerialized]
    public int m_PlayerNorthIconID;      //玩家北的头像编号


    private void Awake()
    {
        m_Instance = this;
    }


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
