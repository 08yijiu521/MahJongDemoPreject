using UnityEngine;
using System.Collections;

public class ResultScoreWindowCtr : MonoBehaviour {

    public class PlayerScore
    {
        public string name;
        public string icon;
        public int score;
    }
    public PlayerScoreDownCtr player_Down;//一搬为本家
    public PlayerScoreCtr player_Right;
    public PlayerScoreCtr player_Up;
    public PlayerScoreCtr player_Left;
    private void Start()
    {
        UIButton[] btns = GetComponentsInChildren<UIButton>();
        foreach(var btn in btns)
        {
            UIEventListener.Get(btn.gameObject).onClick = (go) => {
                ButtonClickCallBack(go);
            };
        }
    }
    public void ButtonClickCallBack(GameObject go)
    {
        string name = go.name;
        switch(name)
        {
            case "Button_Close":
                Debug.Log("Button_Close");
                ResourcesMgr.m_Instance.RemoveGameObject(this.gameObject);
                break;
        }
    }
    public void InitResultScoreWindow(RoomInfoMgr roomInfo,int score)
    {
        //RoomInfoMgr roomInfo = RoomInfoMgr.m_Instance;
        //int score = 0;
        switch(RoomInfoMgr.m_Instance.m_MySeat)
        {
            case MainSceneMger.PlayerSeat.PlayerEast:
                //
                player_Down.SetPlayerScore(roomInfo.m_PlayerEastName, InitPlayerIcon(roomInfo.m_PlayerEastIconID), score);
                player_Down.SetDetalScore();
                player_Right.SetPlayerScore(roomInfo.m_PlayerSouthName,InitPlayerIcon(roomInfo.m_PlayerSouthIconID), score);
                player_Up.SetPlayerScore(roomInfo.m_PlayerWestName, InitPlayerIcon(roomInfo.m_PlayerWestIconID), score);
                player_Left.SetPlayerScore(roomInfo.m_PlayerNorthName, InitPlayerIcon(roomInfo.m_PlayerNorthIconID), score);
                
                break;
            case MainSceneMger.PlayerSeat.PlayerSouth:
                //
                player_Down.SetPlayerScore(roomInfo.m_PlayerSouthName, InitPlayerIcon(roomInfo.m_PlayerSouthIconID), score);
                player_Down.SetDetalScore();
                player_Right.SetPlayerScore(roomInfo.m_PlayerWestName, InitPlayerIcon(roomInfo.m_PlayerWestIconID), score);
                player_Up.SetPlayerScore(roomInfo.m_PlayerNorthName, InitPlayerIcon(roomInfo.m_PlayerNorthIconID), score);
                player_Left.SetPlayerScore(roomInfo.m_PlayerEastName, InitPlayerIcon(roomInfo.m_PlayerEastIconID), score);
                break;
            case MainSceneMger.PlayerSeat.PlayerWest:
                //
                player_Down.SetPlayerScore(roomInfo.m_PlayerWestName, InitPlayerIcon(roomInfo.m_PlayerWestIconID), score);
                player_Down.SetDetalScore();
                player_Right.SetPlayerScore(roomInfo.m_PlayerNorthName, InitPlayerIcon(roomInfo.m_PlayerNorthIconID), score);
                player_Up.SetPlayerScore(roomInfo.m_PlayerEastName, InitPlayerIcon(roomInfo.m_PlayerEastIconID), score);
                player_Left.SetPlayerScore(roomInfo.m_PlayerSouthName, InitPlayerIcon(roomInfo.m_PlayerSouthIconID), score);
                break;
            case MainSceneMger.PlayerSeat.PlayerNorth:
                //
                player_Down.SetPlayerScore(roomInfo.m_PlayerNorthName, InitPlayerIcon(roomInfo.m_PlayerNorthIconID), score);
                player_Down.SetDetalScore();
                player_Right.SetPlayerScore(roomInfo.m_PlayerEastName, InitPlayerIcon(roomInfo.m_PlayerEastIconID), score);
                player_Up.SetPlayerScore(roomInfo.m_PlayerSouthName, InitPlayerIcon(roomInfo.m_PlayerSouthIconID), score);
                player_Left.SetPlayerScore(roomInfo.m_PlayerWestName, InitPlayerIcon(roomInfo.m_PlayerWestIconID), score);
                break;
        }
    }

    /// <summary>
    /// 初始化玩家头像
    /// </summary>
    /// <param name="IconNum"></param>
    /// <returns></returns>
    public string InitPlayerIcon(int IconNum)
    {
        switch (IconNum)
        {
            case 1:
                return "Img_tx1";
            case 2:
                return "Img_tx2";
            case 3:
                return "Img_tx3";
            case 4:
                return "Img_tx4";
            default:
                Debug.Log("错误玩家头像ID！");
                return null;
        }
    }
   
}
