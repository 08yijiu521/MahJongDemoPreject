using UnityEngine;
using System.Collections;
using GameProtocol.Model;
using System.Collections.Generic;
public class DuiJuContainerWindowCtr : MonoBehaviour {
    public class PlayerScoreItem
    {
        public string m_name;
        public int m_score;
    }
    public class PlayerJFScoreItem
    {
        public List<PlayerScoreItem> m_PlayerItemList = new List<PlayerScoreItem>();
        public int m_Count;
    }

    string[] names;
    public UILabel m_Count;
    public GameObject PlayerTJ;//玩家统计
    public GameObject PLayerJF;//局分记录
    private UIGrid PlayerJFGrid;
    void Start () {
        UIButton[] btns = GetComponentsInChildren<UIButton>();
        foreach(var btn in btns)
        {
            UIEventListener.Get(btn.gameObject).onClick = (go) => {
                BtnClick(go);
            };
        }
        if (PlayerJFGrid == null)
            PlayerJFGrid = GetComponentInChildren<UIGrid>();
        InitNames();
	}
    void InitNames()
    {
        names = new string[] {
            RoomInfoMgr.m_Instance.m_PlayerEastName,
            RoomInfoMgr.m_Instance.m_PlayerSouthName,
            RoomInfoMgr.m_Instance.m_PlayerWestName,
            RoomInfoMgr.m_Instance.m_PlayerNorthName
        };
        //names = new string[] {"12","df","dd","zzz" };
    }

    public void InitScoreResult(ScoreModel scoreModel)
    {
        //初始化局分记录
        if (names == null || names.Length == 0)
            InitNames();
        Debug.Log("scoreModel.Round:" + scoreModel.Round);
        for(int i = 0; i < 4; i++)
        {
            Debug.Log( scoreModel.RoundOne[i + 1]);
        }
        for (int i = 1; i <=scoreModel.Round ; i++)
        {
            Dictionary<int, int> dic = null;
            switch (i)
            {
                case 1:
                    dic = scoreModel.RoundOne;
                    break;
                case 2:
                    dic = scoreModel.RoundTwo;
                    break;
                case 3:
                    dic = scoreModel.RoundThree;
                    break;
                case 4:
                    dic = scoreModel.RoundFour;
                    break;
            }
            PlayerJFScoreItem JFItem = new PlayerJFScoreItem();
            for (int j = 1; j <= 4; j++)
            {
                PlayerScoreItem item = new PlayerScoreItem();
                item.m_name = names[j-1];
                if (dic != null)
                    item.m_score = dic[j];
                JFItem.m_PlayerItemList.Add(item);
            }
            JFItem.m_Count = i;
            Sort(JFItem.m_PlayerItemList);
            GameObject JF_GameObj = ResourcesMgr.m_Instance.GetGameObject("Prefab/MainScene/ResultScore/PlayerJF_Item");
            if(PlayerJFGrid==null)
                PlayerJFGrid = GetComponentInChildren<UIGrid>();
            JF_GameObj.GetComponent<PlayerJF_Item>().SetScore(JFItem);
            JF_GameObj.transform.SetParent(PLayerJF.transform);
            PlayerJFGrid.AddChild(JF_GameObj.transform);
            JF_GameObj.transform.localPosition = Vector3.zero;
            JF_GameObj.transform.localScale = Vector3.one;
        }
        PlayerJFGrid.Reposition();
        PLayerJF.GetComponent<UIScrollView>().ResetPosition();
        //玩家统计
        PlayerJFScoreItem scoreItem = new PlayerJFScoreItem();
        for(int i = 1; i <= 4; i++)
        {
            PlayerScoreItem item = new PlayerScoreItem();
            item.m_name = names[i-1];
            item.m_score = scoreModel.GetTotal(i);
            scoreItem.m_PlayerItemList.Add(item);
        }
        Sort(scoreItem.m_PlayerItemList);
        PlayerTJ.GetComponent<PlayerJF_Item>().SetTotalScore(scoreItem);

        PLayerJF.SetActive(false);
        PlayerTJ.SetActive(true);

    }
    void InitRoomInfo(ScoreModel model)
    {
        RoomInfoMgr.m_Instance.m_PlayerEastCoin = model.DongScore;
        RoomInfoMgr.m_Instance.m_PlayerSouthCoin = model.NanScore;
        RoomInfoMgr.m_Instance.m_PlayerWestCoin = model.XiScore;
        RoomInfoMgr.m_Instance.m_PlayerNorthCoin = model.BeiScore;
    }
    int[] Sort(int[] array)
    {
        for(int i = 0;i < array.Length-1;i++ )
        {
            int index = i;
            int key = array[i];
            for(int j = i+1;j < array.Length;j++)
            {
                if(array[index]<array[j])
                {
                    index = j;
                }
            }
            if(index!=i)
            {
                array[i] = array[index];
                array[index] = key;
            }
        }
        return array;
    }
    List<PlayerScoreItem> Sort(List<PlayerScoreItem> list)
    {
        for (int i = 0; i < list.Count - 1; i++)
        {
            int index = i;
            PlayerScoreItem key = list[i];
            for (int j = i + 1; j < list.Count; j++)
            {
                if (list[index].m_score < list[j].m_score)
                {
                    index = j;
                }
            }
            if (index != i)
            {
                list[i] = list[index];
                list[index] = key;
            }
        }
        return list;
    }
    void BtnClick(GameObject go)
    {
        string name = go.name;
        switch(name)
        {
            case "Btn_TJ"://玩家统计
                PLayerJF.SetActive(false);
                PlayerTJ.SetActive(true);
                break;
            case "Btn_JF"://统计记录
                PlayerTJ.SetActive(false);
                PLayerJF.SetActive(true);
                break;
            case "Btn_Close"://关闭按钮
                PlayerJF_Item[] fjs = PlayerJFGrid.GetComponentsInChildren<PlayerJF_Item>();
                foreach(var jf in fjs)
                {
                    ResourcesMgr.m_Instance.RemoveGameObject(jf.gameObject);
                }
                ResourcesMgr.m_Instance.RemoveGameObject(this.gameObject);
                break;
        }
    }
	
}
