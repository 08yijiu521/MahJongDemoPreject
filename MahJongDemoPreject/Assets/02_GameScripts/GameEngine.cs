using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using GameProtocol;

/// <summary>
/// 客户端核心全局对象.(提供所有Mgr的接口,提供资源加载接口,提供View显示和管理接口)
/// </summary>
public class GameEngine : MonoBehaviour {
    public static GameEngine m_Instance = null;

    /// <summary>
    /// 网络管理器
    /// </summary>
    public NetManager m_NetMgr;

    void Awake()
    {
        m_Instance = this;
    }
    /// <summary>
    /// 进入游戏，连接服务器
    /// </summary>
    void Start()
    {
        Debug.Log("进入游戏，连接服务器,暂时未连接，以后需连接时点这里");
        //ConnectTo();
    }

    /// <summary>
    /// 连接服务器
    /// </summary>
    public void ConnectTo()
    {
        m_NetMgr.Connect();
    }

    /// <summary>
    /// 断开连接
    /// </summary>
    public void CloseSocket()
    {
        m_NetMgr.Close();
    }
    
    /// <summary>
    /// 销毁
    /// </summary>
    void OnDestroy()
    {
        m_NetMgr.Close();
    }

}
