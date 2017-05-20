/************************
 * Title：
 * Function：
 * 	－ 检查输入的加入房间号码
 * Used By：JoinRoomNumManager
 * Author：qwt
 * Date：2017.2.23
 * Version：1.0
 * Description：
 *
************************/
using UnityEngine;
using System.Collections;
using GameProtocol;

public class CheckRoomNum : MonoBehaviour {

    public static CheckRoomNum instance;

    public string SixRoomNum;

    private bool isCheckRoomNumFinish = false;

    ArrayList numArrayList = new ArrayList();
    
    void Awake()
    {
        instance = this;
    }

	void Update () {
        ShowRoomNum();
        if (!isCheckRoomNumFinish)
        {
            CheckRoomNumRight();
        }
	}

    //得到输入房间号码
    public void GetRoomNum(int num)
    {
        if (numArrayList.Count<6)
        {
            numArrayList.Add(num);
        }        
    }

    //重置房间号码
    public void ResetRoomNum()
    {
        numArrayList.Clear();
        for (int i = 0; i < 6; i++)
        {
            JoinRoomNum.instance.RoomNum.GetChild(i).GetChild(0).GetComponent<UILabel>().text = "";
        }
    }

    //删除房间号码
    public void DeleteRoomNum()
    {
        if (numArrayList.Count==0)
        {
            return;
        }     
        JoinRoomNum.instance.RoomNum.GetChild(numArrayList.Count-1).GetChild(0).GetComponent<UILabel>().text = "";
        numArrayList.RemoveAt(numArrayList.Count - 1);
    }

    //显示所输入的房间号
    public void ShowRoomNum()
    {
        for (int i = 0; i < numArrayList.Count; i++)
        {            
                JoinRoomNum.instance.RoomNum.GetChild(i).GetChild(0).GetComponent<UILabel>().text = numArrayList[i].ToString();
        }
    }

    //检查输入的房间号是否正确
    public void CheckRoomNumRight()
    {
        if (numArrayList.Count==6)
        {
            foreach (int roomNum in numArrayList)
            {
                string[] strs = new string[numArrayList.Count];
                for (int i = 0; i < numArrayList.Count; i++)
                {
                    strs[i] = numArrayList[i].ToString();
                    SixRoomNum = string.Join("", strs);                    
                }
            }
            NetManager.m_Instance.SendMessage(Protocol.TYPE_ROOM, int.Parse(SixRoomNum), RoomProtocol.JOIN_CREQ, null);
            isCheckRoomNumFinish = true;
            Destroy(CreateRoomViewMgr.m_Instance.joinRoomWindow);
        }
    }
}
