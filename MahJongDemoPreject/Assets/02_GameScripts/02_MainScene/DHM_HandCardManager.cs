using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameProtocol;
using GameProtocol.Model;
[System.Serializable]
public class HandCardItem
{
    public int _id;
    public GameObject _obj;
}
public class DHM_HandCardManager : MonoBehaviour {
    public List<HandCardItem> _handCardList = new List<HandCardItem>();//手牌数组
    private List<int> idArray = null;           //ID数组
    public float offSetX = 0.034f;          //每张手牌x轴的偏移量
    public GameObject currentObj = null;    //当前点击的手牌
    public GameObject _handCardPrefab = null;//手牌预设体
    private Transform _HandCardPlace = null; //手牌放置父节点
    private int newIndex;                   //摸牌要插入的下标
    private int oldIndex;                   //打出去的牌的下标

    private HandCardItem _MoHand = null;    //摸牌
    [SerializeField]
    private Transform _MoHandPos;           //摸牌的位置

    string strChaPaiHand = "ChaPaiHand";    //插牌动画的名字前缀

    public enum PlayerType
    {
        East,
        West,
        South,
        North,
        None
    }
    public string tagValue = string.Empty;           //当前玩家的tag值
    public PlayerType _currentType = PlayerType.None;//当前玩家类型
    public RuleManager.DingQueType _dingQueType = RuleManager.DingQueType.None;//当前玩家选择的定缺类型

    public bool isState = false;//是否是当前玩家回合
    public LayerMask m_handCard_layer;//手牌的层
    public Camera camera_3D;//3D相机，不渲染自身的手牌和插排动画
    public Camera camera_2D;//2D相机，只渲染手牌

    public bool isPeng = false;
    public delegate void ChuPaiDelegate(HandCardItem item,bool isMoNi);
    public event ChuPaiDelegate chuPaiEvent;

    public GameObject _huPaiHand = null;
    public GameObject _huEffect = null;
    //2017-0317-添加，每次碰杠胡，出生点左移,只能移动三次，当下一局开始时，需要回到初始位置
    private int m_pengOrGangMoveCount = 0;
    private Vector3 m_HandCardPlace_StartPos;
    private Vector3 m_HandCardMgr_StartPos;
    Transform huPaiSpawn;
   [SerializeField]
    private Material m_shouMaterial;
    //
    public bool IsState 
    {
        get { return isState; }
        set { isState = value; }
    }
    private static bool isDQ = false;
    /// <summary>
    /// 重置信息
    /// </summary>
    public void ResetInfo()
    {
        m_pengOrGangMoveCount = 0;
        _HandCardPlace.position = m_HandCardPlace_StartPos;
        _HandCardPlace.localRotation = Quaternion.identity;
        this.transform.position = m_HandCardMgr_StartPos;
        isDQ = false;
        IsState = false;
        isPeng = false;
        newIndex = -1;
        oldIndex = -1;
        idArray.Clear();
        _handCardList.Clear();
        Transform[] trans = _HandCardPlace.GetComponentsInChildren<Transform>();
        for(int i =trans.Length-1 ;i >=0;i--)
        {
            if (trans[i]!=_HandCardPlace)
            {
                Destroy(trans[i].gameObject);
            }
        }
        if (_MoHand != null)
        {
            if (_MoHand._obj != null)
                Destroy(_MoHand._obj);
        }
        _MoHand = null;
        currentObj = null;
        if (huPaiSpawn != null)
        {
            Transform[] tranArray = huPaiSpawn.GetComponentsInChildren<Transform>();
            for (int i = tranArray.Length - 1; i >= 0; i--)
            {
                if (tranArray[i] != huPaiSpawn)
                {
                    Destroy(tranArray[i].gameObject);
                }
            }
        }
    }
    private void Awake()
    {
        m_handCard_layer = LayerMask.NameToLayer("HandCard");//默认都能显示
    }
    void Start () {
        _HandCardPlace =  transform.parent.FindChild("HandCardPlace").transform;
        InitTagValue();
        camera_3D = Camera.main;
        camera_2D = camera_3D.transform.Find("Camera").GetComponent<Camera>();
        m_HandCardPlace_StartPos = _HandCardPlace.position;
        m_HandCardMgr_StartPos = this.transform.position;
         huPaiSpawn = this.transform.parent.Find("HuPaiSpwan");
    }
    void InitTagValue()
    {
         tagValue = string.Empty;
        switch (_currentType)
        {
            case PlayerType.East:
                tagValue = "East";
                break;
            case PlayerType.West:
                tagValue = "West";
                break;
            case PlayerType.South:
                tagValue = "South";
                break;
            case PlayerType.North:
                tagValue = "North";
                break;
        }
    }
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera_2D.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, m_handCard_layer))
            {
                if(hit.collider.gameObject.CompareTag(tagValue))
                {
                    if (hit.collider.gameObject.Equals(currentObj) && isState)//当前回合才能出牌
                    {
                        isState = false;
                        currentObj = null;
                        SendHandCard(hit.collider.gameObject);
                    }
                    else
                    {
                        SelectHandCard(hit.collider.gameObject);
                    }
                }
            }
            else if(!Physics.Raycast(ray, out hit, m_handCard_layer))
            {
                if(currentObj!=null)
                {
                    SelectHandCard(currentObj);
                }
            }

        }
    }
    public void SetLayer(LayerMask layer)
    {
        m_handCard_layer = layer;//2d相机显示自身手牌，3D不显示
    }
    /// <summary>
    /// 选择一张手牌
    /// </summary>
    /// <param name="target"></param>
    void SelectHandCard(GameObject target)
    {
        AudioManager.Instance.PlayEffectAudio("select");
        if(currentObj!=null)
        {
            currentObj.transform.Translate(0,-0.02f,0);
            currentObj = null;
        }
        else
        {
            currentObj = target;
            currentObj.transform.Translate(0, 0.02f, 0);
        }
    }
    /// <summary>
    /// 打牌
    /// </summary>
    /// <param name="target"></param>
    void SendHandCard(GameObject target)
    {
        oldIndex = GetIndexByObj(target);//打出去的牌所在下标
        if (oldIndex != -1)
        {
            if(chuPaiEvent!=null)
            {
                if (_handCardList[oldIndex]._obj==null)
                {
                    Debug.LogError("打出去的手牌是:"+ target+"ID:"+ _handCardList[oldIndex]._id);
                }
                chuPaiEvent(_handCardList[oldIndex], false);
            }
        }
        else//直接打出 摸到的牌，不需要插牌，打完继续摸一张
        {
            if (chuPaiEvent != null)
            {
                chuPaiEvent(_MoHand,false);
            }
        }
    }
    /// <summary>
    /// 模拟摸牌，只知道ID，不知道位置
    /// </summary>
    /// <param name="id"></param>
    public void MoNiChuPai(int id)
    {
        Debug.LogWarning("模拟打牌的ID："+id);
        for(int i = 0;i < _handCardList.Count;i++)
        {
            if(id==_handCardList[i]._id && _handCardList[i]._obj!=null)
            {
                oldIndex = GetIndexByObj(_handCardList[i]._obj);//打出去的牌所在下标
                chuPaiEvent(_handCardList[i],true);
                return;
            }
        }
        chuPaiEvent(_MoHand,true);
        oldIndex = -1;
    }
    public int GetIndexByItem(HandCardItem targetItem)
    {
        int index = -1;
        int key = int.MaxValue;
        int DQ_Start_index = -1;
        bool isGetDQIndex = false;
        for (int i = 0; i < _handCardList.Count;i++)
        {
            if (CheckIsDingQue(_handCardList[i]._id) && !isGetDQIndex)
            {
                DQ_Start_index = i;
                isGetDQIndex = true;
            }
            if (targetItem._id == _handCardList[i]._id)
            {
                index = i;
                break;
            }
            else if(targetItem._id<_handCardList[i]._id && key > _handCardList[i]._id)
            {
                index = i;
                key = _handCardList[i]._id;
            }
        }
         if(CheckIsDingQue(targetItem._id) && DQ_Start_index==-1)
            {
                index = _handCardList.Count;
            }
         else if(CheckIsDingQue(targetItem._id) && DQ_Start_index!=-1)
        {
            if(index<DQ_Start_index)
            {
                index = _handCardList.Count;
            }
        }
        if(index==-1)//新牌是最大的，需要在最末尾
        {
            if (!CheckIsDingQue(targetItem._id) && DQ_Start_index != -1)
            {
                index = DQ_Start_index;
            }
             
            else
            {
                index = _handCardList.Count;
            }
        }
        return index;//
    }
    public int GetIndexByObj(GameObject obj)
    {
        int index = -1;
        for(int i = 0;i < _handCardList.Count;i++)
        {
            if (_handCardList[i]._obj.Equals(obj))
                index = i;
        }
        return index;
    }

    public void PushToList(int id,GameObject obj)
    {
        HandCardItem item = new HandCardItem();
        item._id = id;
        item._obj = obj;
        _handCardList.Add(item);
    }
    public void ClearList()
    {
        _handCardList.Clear();
    }
    /// <summary>
    /// 插牌，被出牌动画的帧事件调用
    /// </summary>
    public void chapai()
    {
        if (isPeng)//碰牌以后，直接打牌，不需要摸牌，也不能插牌
        {
            if (oldIndex != -1)
                _handCardList.RemoveAt(oldIndex);
            else
            {
                _MoHand = null;
            }
            isPeng = false;
            UpdateHandCard();
            
            GameManager.m_instance.islock = false;
            Debug.LogWarning("碰以后打出的牌：" + GameManager.m_instance.islock);
            return;
        }
        else if (oldIndex != -1 && _MoHand!=null)//如果需要插牌，则执行插牌
        {
            newIndex = GetIndexByItem(_MoHand);
            if (newIndex > oldIndex)
            {
                newIndex--;
            }
            if(newIndex==oldIndex && newIndex==13)
            {
                newIndex--;
            }
            ChaPai(newIndex, _MoHand._obj);
        }
        else if(oldIndex==-1)
        {
            GameManager.m_instance.islock = false;
            Debug.LogWarning("打出莫的牌：" + GameManager.m_instance.islock);
        }
        else
        {
            GameManager.m_instance.islock = false;
            Debug.LogWarning("默认打开开关：" + GameManager.m_instance.islock);
        }
    }
    /// <summary>
    /// 移动手牌
    /// </summary>
    public  void MoveHandCard()
    {
        if (newIndex == oldIndex)
        {
        }
        else if (newIndex < oldIndex)
        {
            //newIndex~oldIndex，数组中后移，手牌上右移
            GameObject obj = new GameObject("tempParent");
            obj.transform.SetParent(_HandCardPlace);//获取父节点的方式有问题
            obj.transform.localPosition = Vector3.zero;
            obj.transform.rotation = _HandCardPlace.rotation;
            for (int i = oldIndex - 1; i >= newIndex; i--)
            {
                _handCardList[i + 1] = _handCardList[i];
                _handCardList[i + 1]._obj.transform.SetParent(obj.transform);
            }
            obj.transform.Translate(-offSetX, 0, 0);
            Transform[] tran = obj.GetComponentsInChildren<Transform>();
            for (int j = 0; j < tran.Length; j++)
            {
                if (!tran[j].gameObject.Equals(obj.gameObject))
                {
                    tran[j].transform.SetParent(_HandCardPlace);//获取父节点的方式有问题
                }

            }
            Destroy(obj);
        }
        else if (newIndex > oldIndex)
        {

            //oldIndex~list.count:数组前移，牌桌上的手牌左移，item入数组尾部
            GameObject obj = new GameObject("tempParent");
            obj.transform.SetParent(_HandCardPlace);//获取父节点的方式有问题
            obj.transform.localPosition = Vector3.zero;
            obj.transform.rotation = _HandCardPlace.rotation;
            for (int i = oldIndex; i < newIndex; i++)
            {
                _handCardList[i] = _handCardList[i + 1];
                _handCardList[i]._obj.transform.SetParent(obj.transform);
            }
            obj.transform.Translate(offSetX, 0, 0);
            Transform[] tran = obj.GetComponentsInChildren<Transform>();
            for (int j = 0; j < tran.Length; j++)
            {
                if (!tran[j].gameObject.Equals(obj.gameObject))
                {
                    tran[j].transform.SetParent(_HandCardPlace);//获取父节点的方式有问题
                }
            }
            Destroy(obj);
        }
        _handCardList[newIndex] = _MoHand;
    }
   
   /// <summary>
   /// 插牌
   /// </summary>
   /// <param name="needIndex">需要插入的位置</param>
   /// <param name="obj">需要插入的对象</param>
    public void ChaPai(int needIndex,GameObject obj)
    {
        //创建手，设置手的位置，即，确定要插入的位置
        //拿起摸到的牌
        //将摸到的牌放在手上
        //将手牌左移或者右移
        Debug.Log("插牌下标：" + newIndex);
        int handIndex = 13 - (_handCardList.Count-needIndex)+1;
        string name = strChaPaiHand + handIndex.ToString();
        GameObject hand =ResourcesMgr.m_Instance.InstantiateGameObjectWithType(name, ResourceType.Hand);
        if (m_handCard_layer==(LayerMask.NameToLayer("Self")))
        {
            foreach (var tran in hand.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                tran.material = ResourcesMgr.m_Instance.M_transparent;
            }
        }
        else
        {
            foreach (var tran in hand.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                tran.material = m_shouMaterial;
            }
        }
        hand.transform.rotation = _MoHandPos.rotation;
        hand.transform.position = _MoHandPos.TransformPoint(-0.011f, -0.0229f, 0.257f);

        DHM_HandAnimationCtr handAniCtr = hand.GetComponent<DHM_HandAnimationCtr>();
        if(handAniCtr==null)
        {
            hand.SendMessage("PlayChaPaiAnimation", obj, SendMessageOptions.RequireReceiver);
        }
        handAniCtr.PlayChaPaiAnimation(obj);
        handAniCtr.moveHandEvent += MoveHandCard;
        handAniCtr.chaPaiEndEvent += ChaPaiEndEventHandle;
        //handAniCtr.Set_HandCardPlacePos(_HandCardPlace.gameObject);
    }
    void ChaPaiEndEventHandle(DHM_HandAnimationCtr hand)
    {
        hand.moveHandEvent -= MoveHandCard;
        hand.chaPaiEndEvent -= ChaPaiEndEventHandle;
        float x = _handCardList.Count / 2.0f - newIndex;
        _MoHand._obj.transform.SetParent(_HandCardPlace);
        _MoHand._obj.transform.localPosition = Vector3.zero;
        _MoHand._obj.transform.localRotation = Quaternion.Euler(Vector3.zero);
        _MoHand._obj.transform.Translate(offSetX * x, 0, 0);
        _MoHand = null;
        ResourcesMgr.m_Instance.RemoveGameObject(hand.gameObject);
        GameManager.m_instance.islock = false;
        Debug.LogWarning("ChaPaiEndEventHandle结束：" + GameManager.m_instance.islock);
    }
    /// <summary>
    /// 碰牌
    /// </summary>
    /// <param name="id">需要碰牌的ID</param>
    public void Peng(int id)
    {
        if(m_pengOrGangMoveCount<3)
        {
            m_pengOrGangMoveCount++;
            _HandCardPlace.transform.Translate(1.5f * offSetX, 0, 0);//---------------测试-------------------
            this.transform.Translate(1.5f * offSetX, 0, 0);//---------------测试-------------------
        }
        isPeng = true;
        RemoveGameObj(id, 2);
        
    }
    /// <summary>
    /// 杠
    /// </summary>
    /// <param name="id"></param>
    /// <param name="isDarkGang">是否是暗杠</param>
    public void Gang(int id,bool isDarkGang)
    {
        if(m_pengOrGangMoveCount<3)
        {
            m_pengOrGangMoveCount++;
            _HandCardPlace.transform.Translate(2f * offSetX, 0, 0);//---------------测试-------------------
            this.transform.Translate(2f * offSetX, 0, 0);//---------------测试-------------------
        }
        if (isDarkGang)
        {
            RemoveGameObj(id, 3);
            RemoveMoHandCard(id);
        }
        else
        {
            RemoveGameObj(id, 3);
        }
        
    }
    public void RemoveMoHandCard(int id)
    {
        Destroy(_MoHand._obj);
        _MoHand = null;
    }
    public void RemoveGameObj(int id,int Number)
    {
        int count = 0;
        for (int i = _handCardList.Count - 1; i >= 0; i--)
        {
            if (_handCardList[i]._id == id)
            {
                count++;
                Destroy(_handCardList[i]._obj);
                _handCardList.RemoveAt(i);
                if (count == Number)
                {
                    UpdateHandCard();
                    break;
                }
            }
        }

    }
    GameObject FindGameObj(int id)
    {
        GameObject obj = null;
        for (int i = _handCardList.Count - 1; i >= 0; i--)
        {
            HandCardItem item = _handCardList[i];
            if (item._id == id)
                obj = item._obj;
        }
        return obj;
    }
    /// <summary>
    /// 定缺,缺一门，给手牌排序
    /// </summary>
    /// <param name="key">指定的需要定缺的类型</param>
    /// 1：万
    /// 2：条
    /// 3：筒
    /// 0：默认状态
    public void DingQue(RuleManager.DingQueType type)//定缺、需要定缺的id+30、排序ID、定缺过的ID-30、刷新uv
    {
        int low = -1;
        int hight = -1;
        switch(type)
        {
            case RuleManager.DingQueType.WAN:
                low = 0;
                hight = 9;
                break;
            case RuleManager.DingQueType.TIAO:
                low = 10;
                hight = 19;
                break;
            case RuleManager.DingQueType.TONG:
                low = 20;
                hight = 29;
                break;
        }
        if (low != -1 || hight != -1)
        {
            for (int i = idArray.Count - 1; i >= 0; i--)
            {
                int id = idArray[i];
                if (idArray[i] > low && idArray[i] <= hight)
                {
                    idArray[i] += 30;
                }
            }
        }
        SortList();     //排序id
        RevertIdArray();//定缺过的ID-30
        UVOffSet();     //刷新uv
    }
    public bool CheckIsDingQue(int id)
    {
        int low = -1;
        int hight = -1;
        switch (_dingQueType)
        {
            case RuleManager.DingQueType.WAN:
                low = 0;
                hight = 9;
                break;
            case RuleManager.DingQueType.TIAO:
                low = 10;
                hight = 19;
                break;
            case RuleManager.DingQueType.TONG:
                low = 20;
                hight = 29;
                break;
        }
        if (low != -1 || hight != -1)
        {
            if (id > low && id <= hight)
            {
                return true;
            }
        }
        return false;
    }
    void RevertIdArray()
    {
        for(int i =0;i<idArray.Count;i++)
        {
            if(idArray[i]>=31 && idArray[i] <= 59)
            {
                idArray[i] -= 30;
            }
        }
    }
    /// <summary>
    /// 重新刷新手牌
    /// </summary>
    public void UpdateHandCard()
    {
        //重新刷新显示
        AudioManager.Instance.PlayEffectAudio("sort");
        for (int i = 0;i < _handCardList.Count;i++)
        {
            float x =_handCardList.Count / 2.0f -i ;
            GameObject obj = _handCardList[i]._obj;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.Euler(Vector3.zero);//---------------------------
            obj.transform.Translate(offSetX * x, 0, 0);
        }
        _MoHandPos.localPosition = Vector3.zero;
        _MoHandPos.Translate(-(_handCardList.Count/2.0f +0.5f)*offSetX,0,0);
    }
    /// <summary>
    /// 设置摸牌的位置
    /// 1 偏移UV贴图
    /// 2 检测是否是定缺的牌
    /// </summary>
    /// <param name="id"></param>
    /// <param name="go"></param>
    public void SetMoHandCard(int id,GameObject go=null)
    {
        Debug.LogWarning("摸的牌" + id);
        if(go==null)
        {
            go = Instantiate(_handCardPrefab) as GameObject;
            RuleManager.m_instance.UVoffSet(id, go);//UV偏移
        }
        _MoHand = new HandCardItem();
        _MoHand._id = id;
        _MoHand._obj = go;
        _MoHand._obj.layer = m_handCard_layer;
        _MoHand._obj.tag = tagValue;
        _MoHand._obj.transform.SetParent(null);
        _MoHand._obj.transform.rotation = _MoHandPos.rotation;
        _MoHand._obj.transform.position = _MoHandPos.TransformPoint(0.0731f*offSetX,0,0);
        RuleManager.m_instance.DingQue(_dingQueType,_MoHand);//检测是否需要定缺
        GameManager.m_instance.islock = false;
    }
    /// <summary>
    /// 发牌
    /// </summary>
    public void FaPai()
    {
        ClearList();
        for (int i = 0; i < 13; i++)
        {
            GameObject obj = Instantiate(_handCardPrefab);

            obj.layer = m_handCard_layer;
            obj.gameObject.tag = tagValue;
            obj.transform.SetParent(_HandCardPlace);
            obj.transform.Rotate(90, 0, 0);
            HandCardItem item = new HandCardItem();
            item._obj = obj;
            _handCardList.Add(item);
        }
        UpdateHandCard();
        TestUV();
    }
    /// <summary>
    /// 
    /// 问题：如何获取 id 数组
    /// 
    /// </summary>
    public void TestUV()
    {
        //string str = "1,21,3,4,13,21,16,17,18,21,23,24,25";
        //ParseString(str);
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
        //idArray = new int[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            idArray[i] = int.Parse(array[i]);
        }
    }
    public void SetIDArray(List<int> idList)
    {
        idArray = idList;
    }
    /// <summary>
    /// 根据下标返回ID
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public int GetIdFromArrayAtIndex(int index)
    {
        if (idArray.Count != 0 && index < idArray.Count)
        {
            return idArray[index];
        }
        return -1;
    }
    public void TestUVOffSet(List<int> IdArray)
    {
        for (int i = 0; i < IdArray.Count; i++)
        {
            UVoffSet(IdArray[i], _handCardList[i]._obj);
            _handCardList[i]._id = idArray[i];
        }
    }
    void UVOffSet()
    {
        for (int i = 0; i < idArray.Count; i++)
        {
            UVoffSet(idArray[i], _handCardList[i]._obj);
            _handCardList[i]._id = idArray[i];
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
    public void SortList(List<int> array)
    {
        Quick_Sort(array, 0, array.Count - 1);
    }
    /// <summary>
    /// 将ID排序
    /// </summary>
    public void SortList()
    {
        Quick_Sort(idArray, 0, idArray.Count - 1);
    }
    void Quick_Sort(List<int> array, int first, int last)
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
            _handCardList[i]._obj.SetActive(false);
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
        _HandCardPlace.transform.Rotate(90,0,0);
        obj.transform.SetParent(_HandCardPlace);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.rotation = _HandCardPlace.rotation;
        for (int i = 0; i < _handCardList.Count; i++)
        {
            _handCardList[i]._obj.SetActive(true);
            _handCardList[i]._obj.transform.SetParent(obj.transform);
            if ((++count) % 4 == 0)
            {
                yield return StartCoroutine(RotateTo(obj.transform, new Vector3(-90, 0, 0)));
                Transform[] tran = obj.GetComponentsInChildren<Transform>();
                for (int j = 0; j < tran.Length; j++)
                {
                    if (!tran[j].gameObject.Equals(obj.gameObject))
                        tran[j].transform.SetParent(_HandCardPlace);

                }
                yield return new WaitForSeconds(0.5f);
                obj.transform.Rotate(90, 0, 0);
            }
            else if ((i+1) % 13 == 0)
            {

                yield return StartCoroutine(RotateTo(obj.transform, new Vector3(-90, 0, 0)));
                _handCardList[i]._obj.transform.SetParent(_HandCardPlace);
                yield return new WaitForSeconds(0.5f);
            }
            yield return new WaitForFixedUpdate();
        }
        Destroy(obj.gameObject);
        foreach (var item in _handCardList)
        {
            item._obj.transform.Rotate(90, 0, 0);
        }
        yield return StartCoroutine(SortHandCard());
        _HandCardPlace.Rotate(-90, 0, 0);
        if (_currentType.Equals(PlayerType.East))
            MainViewMgr.m_Instance.ActiveDingQueBtn();
        yield break;
    }
    /// <summary>
    /// 旋转到指定的角度
    /// </summary>
    /// <param name="target"></param>
    /// <param name="targetDirection"></param>
    /// <returns></returns>
    IEnumerator RotateTo(Transform target, Vector3 targetDirection)
    {
        Vector3 temp = target.TransformDirection(Vector3.forward).normalized;
        while (true)
        {
            if (Vector3.Dot(temp, target.TransformDirection(Vector3.forward).normalized) <0.01f)//<= Mathf.Cos(Mathf.PI * 90 / 180))
            {
                target.localRotation = Quaternion.Euler(-90,0,0);
                yield break;
            }
            target.Rotate(Vector3.Lerp(Vector3.zero, targetDirection, Time.deltaTime*5));
            yield return new WaitForFixedUpdate();
        }
    }
    /// <summary>
    /// 注册插牌事件，当出牌动画执行完毕自动调用
    /// </summary>
    /// <param name="go"></param>
    public void ChuPaiCallBackEventHandle(GameObject go)
    {
        DHM_HandAnimationCtr handCtr = go.GetComponent<DHM_HandAnimationCtr>();
        handCtr.chaPaiEvent += chapai;
    }
    public void SetDingQueType(RuleManager.DingQueType type)
    {
        _dingQueType = type;
    }

    /// <summary>
    /// 胡牌
    /// </summary>
    public void HuPai(int id)
    {
        Debug.Log("胡牌" + this.name);
        for (int i = 0; i < _handCardList.Count; i++)
        {
            _handCardList[i]._obj.layer = LayerMask.NameToLayer("ZhuoPai");
        }
        _HandCardPlace.transform.Rotate(90, 0, 0);
        if (huPaiSpawn == null)
            huPaiSpawn = this.transform.parent.Find("HuPaiSpwan");


        GameObject effectObj = Instantiate(_huEffect);
        effectObj.SetActive(true);
        effectObj.transform.position = huPaiSpawn.position;
        Destroy(effectObj, 1.5f);

        GameObject huCard = Instantiate(_handCardPrefab);
        RuleManager.m_instance.UVoffSet(id, huCard);
        huCard.transform.rotation = huPaiSpawn.rotation;
        huCard.transform.position = huPaiSpawn.position;
        huCard.transform.SetParent(huPaiSpawn);

        Transform huHandSpawn = this.transform.parent.Find("HandSpawn");
        GameObject huHand = ResourcesMgr.m_Instance.InstantiateGameObjectWithType("HupaiHand", ResourceType.Hand);
        huHand.transform.rotation = huHandSpawn.rotation;
        huHand.transform.position = huHandSpawn.position;
        huHand.GetComponent<DHM_HandAnimationCtr>().huPaiEvent += HuPaiEventHandle;
    }
    /// <summary>
    /// 胡牌事件处理
    /// </summary>
    public void HuPaiEventHandle(GameObject go)
    {

        _HandCardPlace.transform.Translate(0, 0.06f, 0);
        _HandCardPlace.transform.Rotate(-180, 0, 0);

    }
}


