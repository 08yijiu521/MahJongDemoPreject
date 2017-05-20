/************************
 * Title：
 * Function：
 * 	－ 得到输入加入房间的号码
 * Used By：JoinRoomNumManager
 * Author：qwt
 * Date：2017.2.23
 * Version：1.0
 * Description：
 *
************************/
using UnityEngine;
using System.Collections;

public class JoinRoomNum : MonoBehaviour {

    public static JoinRoomNum instance;
    [HideInInspector]
    public Transform NumGrid;
    [HideInInspector]
    public Transform RoomNum;

    void Awake()
    {
        instance = this;
    }

	void Start () {
        RoomNum = GameObject.Find("RoomNum").transform;
        NumGrid = GameObject.Find("NumGrid").transform;
        EventDelegate NumBtnEvent = new EventDelegate(this, "OnNumBtnClick");
        //给数字按钮添加点击事件
        foreach (Transform item in NumGrid.transform)
        {            
            item.GetComponent<UIButton>().onClick.Add(NumBtnEvent);
        }
	}

    public void OnNumBtnClick()
    {
        //获取当前点击的物体
        AudioManager.Instance.PlayEffectAudio("ui_click");
        GameObject currentObj= UICamera.currentTouch.current;
        string str= currentObj.name.Substring(7, 1);
        switch (str)
        {
            case "0": 
                CheckRoomNum.instance.GetRoomNum(0);
                break;
            case "1":
                CheckRoomNum.instance.GetRoomNum(1);
                break;
            case "2":
                CheckRoomNum.instance.GetRoomNum(2);
                break;
            case "3":
                CheckRoomNum.instance.GetRoomNum(3);
                break;
            case "4":
                CheckRoomNum.instance.GetRoomNum(4);
                break;
            case "5":
                CheckRoomNum.instance.GetRoomNum(5);
                break;
            case "6":
                CheckRoomNum.instance.GetRoomNum(6);
                break;
            case "7":
                CheckRoomNum.instance.GetRoomNum(7);
                break;
            case "8":
                CheckRoomNum.instance.GetRoomNum(8);
                break;
            case "9":
                CheckRoomNum.instance.GetRoomNum(9);
                break;
            case "R":
                CheckRoomNum.instance.ResetRoomNum();
                break;
            case "D":
                CheckRoomNum.instance.DeleteRoomNum();
                break;
            default:
                break;
        }
    }
}
