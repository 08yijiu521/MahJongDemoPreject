using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RuleManager : MonoBehaviour {
    public static RuleManager m_instance = null;
    private void Awake()
    {
        m_instance = this;
    }
    public enum DingQueType
    {
        WAN,  //万
        TIAO, //条
        TONG, //筒
        None
    }
	
    /// <summary>
    /// 定缺,缺一门
    /// </summary>
    /// <param name="type">指定的需要定缺的类型</param>
    /// <param name="_handCardList">指定的需要定缺的数组</param>
    public void DingQue(DingQueType type,List<HandCardItem> _handCardList)
    {
        int low = -1;
        int hight = -1;
        switch (type)
        {
            case DingQueType.WAN:
                low = 0;
                hight = 9;
                break;
            case DingQueType.TIAO:
                low = 10;
                hight = 19;
                break;
            case DingQueType.TONG:
                low = 20;
                hight = 29;
                break;
        }
        if (low != -1 || hight != -1)
        {
            for (int i = _handCardList.Count - 1; i >= 0; i--)
            {
                HandCardItem item = _handCardList[i];
                if (item._id >= low && item._id <= hight)
                {
                    Debug.Log("定缺");
                    item._obj.GetComponent<Renderer>().materials[1].color = new Color(0.286f, 0.286f, 0.286f, 1f);
                }
            }
        }
    }
    public HandCardItem DingQue(DingQueType type,HandCardItem target)
    {
        int low = -1;
        int hight = -1;
        switch (type)
        {
            case DingQueType.WAN:
                low = 0;
                hight = 9;
                break;
            case DingQueType.TIAO:
                low = 10;
                hight = 19;
                break;
            case DingQueType.TONG:
                low = 20;
                hight = 29;
                break;
        }
        if (low != -1 || hight != -1)
        {
            if (target._id >= low && target._id <= hight)
            {
                target._obj.GetComponent<Renderer>().materials[1].color = new Color(0.286f, 0.286f, 0.286f, 1f);
            }
        }
        return target;
    }
    /// <summary>
    /// 根据ID，给指定的牌进行贴图的UV偏移
    /// 1-9 ：  万
    /// 11-19： 条
    /// 21-29： 筒
    /// </summary>
    /// <param name="handCardId"></param>
    /// <param name="handCard"></param>
    public void UVoffSet(int handCardId, GameObject handCard)
    {
        UVoffSetWithReturn(handCardId, handCard);
    }
    public static GameObject UVoffSetWithReturn(int handCardId, GameObject handCard)
    {
        int UVy = handCardId / 10;
        int UVx = handCardId % 10;
        if (UVy == 0)
        {
            UVy = 1;
        }
        else if (UVy == 1)
        {
            UVy = 0;
        }
        handCard.GetComponent<Renderer>().materials[1].mainTextureOffset = new Vector2((UVx - 1) * 0.1068f, -UVy * 0.168f);
        return handCard;
    }
    public void ResetHandCardColor(GameObject obj)
    {
        obj.GetComponent<Renderer>().materials[1].color = Color.white;
    }
}
