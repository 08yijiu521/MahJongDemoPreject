using UnityEngine;
using System.Collections;
using GameProtocol.Model;
using GameProtocol;
using UnityEngine.SceneManagement;

public class LoginViewMgr : MonoBehaviour {

    //public UIButton m_QQLoginBtn;

    //public UIButton m_WeChatLoginBtn;
    public UIInput input_Account;       //玩家账号输入框

    public UIInput input_Password;
    public UIButton btnLogin;
    public UIButton btnRegin;

    public bool isOnline = false;       //表示玩家当前是否处于在线状态（临时）

    private void Start()
    {
        OnOnlineButtonClick();
    }
    /// <summary>
    /// 玩家登陆
    /// </summary>
    public void OnButtonLoginClick()
    {
        Debug.Log("登陆按钮被按下");
        AudioManager.Instance.PlayEffectAudio("ui_click");
        //(临时)
        if (isOnline)
        {
            if (input_Account.value.Length == 0 || input_Account.value.Length > 15)
            {
                Debug.Log("账号不合法");
                return;
            }
            if (input_Password.value.Length == 0 || input_Password.value.Length > 15)
            {
                Debug.Log("密码不合法");
                return;
            }           
            //验证通过则进行消息发送
            AccountModel model = new AccountModel();
            model.name = input_Account.value;
            model.password = input_Password.value;            
            NetManager.m_Instance.SendMessage(Protocol.TYPE_LOGIN, 0, LoginProtocol.LOGIN_CREQ, model);
        }
        else
        {
            //当前属于离线状态，直接登陆游戏
            SceneManager.LoadScene("CreateRoomScene");
        }
        //btnLogin.GetComponent<Collider>().enabled = false;
    }

    /// <summary>
    /// 玩家注册
    /// </summary>
    public void OnButtonRegisterClick()
    {
        Debug.Log("注册按钮被按下");
        AudioManager.Instance.PlayEffectAudio("ui_click");
        if (input_Account.value.Length == 0 || input_Account.value.Length > 15)
        {
            Debug.Log("账号不合法");
            return;
        }
        if (input_Password.value.Length == 0 || input_Password.value.Length > 15)
        {
            Debug.Log("密码不合法");
            return;
        }
        //验证通过则进行消息发送
        AccountModel model = new AccountModel();
        model.name = input_Account.value;
        model.password = input_Password.value;
        NetManager.m_Instance.SendMessage(Protocol.TYPE_LOGIN, 0, LoginProtocol.REG_CREQ, model);
        //btnRegin.GetComponent<Collider>().enabled = false;
    }

    /// <summary>
    /// QQ登陆
    /// </summary>
    public void OnQQButtonClick()
    {
        Debug.Log("QQ登陆按钮按下");
        string message = "QQ";
        GameEngine.m_Instance.m_NetMgr.SendMessage(message);
    }

    /// <summary>
    /// 微信登陆
    /// </summary>
    public void OnWeChatButtonClick()
    {
        Debug.Log("微信登陆按钮按下");
        string message = "WeChat";
        GameEngine.m_Instance.m_NetMgr.SendMessage(message);
    }


    //临时按钮，点击在线按钮连接服务器
    public void OnOnlineButtonClick()
    {
        Debug.Log("在线按钮按下");
        isOnline = true;
        GameEngine.m_Instance.ConnectTo();

    }

    //临时按钮，点击离线按钮连接服务器
    public void OnOfflineButtonClick()
    {        
        Debug.Log("离线按钮按下");
        isOnline = false;
        GameEngine.m_Instance.CloseSocket();
    }
}
