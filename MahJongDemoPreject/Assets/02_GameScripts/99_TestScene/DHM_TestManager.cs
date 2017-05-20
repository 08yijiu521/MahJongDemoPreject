using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DHM_TestManager : MonoBehaviour {

    public static DHM_TestManager instance;

    public List<GameObject> _handCardList = new List<GameObject>();
    private int[] idArray = null;//ID数组

    void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// 将手牌添加到list容器
    /// </summary>
    /// <param name="go"></param>
    public void PushHandCard(GameObject go)
    {
        _handCardList.Add(go);
    }
    /// <summary>
    /// 
    /// 问题：如何获取 id 数组
    /// 
    /// </summary>
    public void TestUV()
    {
        //idArray = new int[] {1,21,3,4,13,15,16,17,18,21,23,24,25 };
        string str = "1,21,3,4,13,15,16,17,18,21,23,24,25";
        ParseString(str);
        TestUVOffSet(idArray);
        HideHandCard();
        StartCoroutine(ActiveHandCard());
       
    }
    /// <summary>
    /// 解析字符串，得到id数组
    /// </summary>
    /// <param name="str"></param>
    public void ParseString(string str)
    {
        string[] array = str.Split(',');
        idArray = new int[array.Length];
        for(int i = 0;i < array.Length;i++)
        {
            idArray[i] = int.Parse(array[i]);
        }
    }
    /// <summary>
    /// 根据下标返回ID
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public int GetIdFromArrayAtIndex(int index)
    {
        if(idArray.Length!=0 && index <idArray.Length)
        {
            return idArray[index];
        }
        return -1;
    }
    public void TestUVOffSet(int [] IdArray)
    {
        for(int i = 0;i < IdArray.Length;i++)
        {
            //int temp = IdArray[i];
            //int UVy = temp / 10;
            //int UVx = temp % 10;
            //_handCardList[12-i].GetComponent<Renderer>().materials[1].mainTextureOffset = new Vector2((UVx-1)*0.11f, -UVy*0.18f);
            UVoffSet(IdArray[i],_handCardList[12-i]);
        }
    }

    /// <summary>
    /// 根据ID，给指定的牌进行贴图的UV偏移
    /// 1-9 ：  万
    /// 11-19： 条
    /// 21-29： 筒
    /// </summary>
    /// <param name="handCardId"></param>
    /// <param name="handCard"></param>
    public  void UVoffSet(int handCardId, GameObject handCard)
    {
        //    int UVy = handCardId / 10;
        //    int UVx = handCardId % 10;
        //if(UVy==0)
        //{
        //    UVy = 1;
        //}
        //else if(UVy==1)
        //{
        //    UVy = 0;
        //}
        //    handCard.GetComponent<Renderer>().materials[1].mainTextureOffset = new Vector2((UVx - 1) * 0.1068f, -UVy * 0.168f);
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
    #region  对ID数组进行排序
    /// <summary>
    ///将手牌的ID排序，更新手牌UV偏移
    /// </summary>
    /// <param name="array"></param>
    /// <returns></returns>
    IEnumerator SortHandCard()
    {
        yield return new WaitForSeconds(0.2f);
        SortList(idArray);
        TestUVOffSet(idArray);
        yield break;
    }
    /// <summary>
    /// 将将指定数组排序
    /// </summary>
    /// <param name="array"></param>
    public void SortList(int[] array)
    {
        Quick_Sort(array,0,array.Length-1);
    }
    /// <summary>
    /// 将ID排序
    /// </summary>
    public void SortList()
    {
        Quick_Sort(idArray, 0, idArray.Length - 1);
    }
    void Quick_Sort(int[] array, int first, int last)
    {
        if (first < last)
        {
            int key = array[first];
            int low = first;
            int hight = last;
            while (low < hight)
            {
                while (low < hight && array[hight] >= key)
                {
                    hight--;
                }
                while (low < hight && array[low] <= key)
                {
                    low++;
                }
                int temp = array[low];
                array[low] = array[hight];
                array[hight] = temp;
            }
            array[first] = array[low];
            array[low] = key;
            Quick_Sort(array, first, low - 1);
            Quick_Sort(array, low + 1, last);
        }
    }
    #endregion
    /// <summary>
    /// 隐藏手牌
    /// </summary>
    public void HideHandCard()
    {
        for (int i = 0; i < _handCardList.Count; i++)
        {
            _handCardList[i].SetActive(false);
        }
    }
    /// <summary>
    /// 激活手牌，四张一组激活
    /// </summary>
    /// <returns></returns>
    public IEnumerator ActiveHandCard()
    {
        int count = 0;
        GameObject obj = new GameObject();
        Transform handcardparent = GameObject.Find("HandCardPlace").transform;
        obj.transform.SetParent(handcardparent);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.rotation = handcardparent.rotation;
        for (int i = _handCardList.Count-1; i >= 0;i--)
        {
            _handCardList[i].SetActive(true);
            _handCardList[i].transform.SetParent(obj.transform);
            if((++count)%4 == 0)
            {
                //obj.transform.Rotate(-90, 0, 0,Space.World);
                yield return StartCoroutine(RotateTo(obj.transform, new Vector3(90, 0, 0)));
                Transform[] tran = obj.GetComponentsInChildren<Transform>();
                for (int j = 0;j<tran.Length;j++)
                {
                    if (!tran[j].gameObject.Equals(obj.gameObject))
                        tran[j].transform.SetParent(handcardparent);

                }
                yield return new WaitForSeconds(0.5f);
                obj.transform.Rotate(-90, 0, 0, Space.World);
            }
            else if(i%13==0)
            {
                
                yield return StartCoroutine(RotateTo(obj.transform, new Vector3(90, 0, 0)));
                _handCardList[i].transform.SetParent(handcardparent);
                //_handCardList[i].transform.Rotate(-90, 0, 0);
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForFixedUpdate();
        }
        Destroy(obj.gameObject);
        //handcardparent.Rotate(-90, 0, 0);
        foreach(var item in _handCardList)
        {
            item.transform.Rotate(90, 0, 0);
        }
        yield return StartCoroutine(SortHandCard());
        handcardparent.Rotate(90, 0, 0);
        yield break;
    }
    /// <summary>
    ///清空数组
    /// </summary>
    public void ClearHandCardList()
    {
        _handCardList.Clear();
    }
    /// <summary>
    /// 旋转到指定的角度
    /// </summary>
    /// <param name="target"></param>
    /// <param name="targetDirection"></param>
    /// <returns></returns>
    IEnumerator RotateTo(Transform target,Vector3 targetDirection)
    {
        Vector3 temp = target.TransformDirection(Vector3.forward).normalized;
        while (true)
        {
            if (Vector3.Dot(temp, target.TransformDirection(Vector3.forward).normalized) <= Mathf.Cos(Mathf.PI * 90 / 180))
            {
                yield break;
            }
            target.Rotate( Vector3.Lerp(Vector3.zero, targetDirection, Time.deltaTime*3));
            yield return new WaitForFixedUpdate();
        }
    }
}
