/************************
 * Title：
 * Function：
 * 	－ 微信邀请好友
 * Used By：canvas
 * Author：qwt
 * Date：2017.3.8
 * Version：1.0
 * Description：
 *
************************/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeChatInvite : MonoBehaviour {

    public GameObject InvitePanel;

    [SerializeField]
    WeChatPluginLiteScript m_pluginObject;    
    [SerializeField]
    Texture2D[] m_Textures;

    private string sixRoomNum ;

    void Start()
    {
        sixRoomNum = RoomInfoMgr.m_Instance.m_RoomID.ToString();
    }
    public void ShareImage()
    {
        m_pluginObject.Prepare();
        m_pluginObject.m_isMoments = true;
        m_pluginObject.m_ShareText = "《3D麻将》好友房间号：" + sixRoomNum + "，快来加入吧！";
        m_pluginObject.m_Textures = new Texture2D[1] { m_Textures[0] };
        m_pluginObject.Share();
    }

    public void CloseInvitePanel()
    {
        this.transform.GetChild(0).GetComponent<TweenScale>().PlayReverse();
        Destroy(InvitePanel,0.5f);
    }
}
