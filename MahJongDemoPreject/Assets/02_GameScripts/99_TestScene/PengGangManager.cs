/************************
 * Title：
 * Function：
 * 	－ 管理碰杠
 * Used By：PengGangArea
 * Author：qwt
 * Date：2017.2.28
 * Version：1.0
 * Description：
 *
************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum OtherSeat
{
    Down, //rightPlayer
    Face, //UpPlayr
    Up,    //leftPlayer
    None
}

[System.Serializable]
public class PengGangCardItem
{
    public int _id;
    public GameObject _obj;
}

public class PengGangManager : MonoBehaviour {

    public List<PengGangCardItem> pengGangCardList = new List<PengGangCardItem>();
    private ArrayList pengCardIdList = new ArrayList();//存储横着的牌的ID
    private ArrayList pengCardPosList = new ArrayList();//存储横着的牌的位置

    public GameObject handCardPrefab;//碰杠牌预设
    public GameObject pengHandPrefab;//碰牌手预设
    public GameObject gangHandPrefab;//杠牌手预设
    public GameObject fx_PengGangPrefab;//碰杠特效预设

    public Transform pengGangArea;
    public Transform pengGangAnimSpawn;
    public Transform EFSpawn;

    private Vector3 startPos = new Vector3(0.034f, 0, 0);
    private Vector3 startPosOffset;//开始坐标偏移

    private Vector3 createHandPos;
    private Vector3 createFx_PengGangPos;

    private GameObject pengCard1;
    private GameObject pengCard2;
    private GameObject pengCard3;

    private GameObject gangCard1;
    private GameObject gangCard2;
    private GameObject gangCard3;
    private GameObject gangCard4;

    private Vector3 pengCard1Pos;
    private Vector3 pengCard2Pos;
    private Vector3 pengCard3Pos;

    private Vector3 gangCard1Pos;
    private Vector3 gangCard2Pos;
    private Vector3 gangCard3Pos;
    private Vector3 gangCard4Pos;

    private Quaternion pengCard1Rot;
    private Quaternion pengCard2Rot;
    private Quaternion pengCard3Rot;

    private Quaternion gangCard1Rot;
    private Quaternion gangCard2Rot;
    private Quaternion gangCard3Rot;
    private Quaternion gangCard4Rot;

    private bool LastIsDown = false;
    private bool LastIsFace = false;
    private bool LastIsUp = false;
    private bool LastIsDarkGang = false;

    private GameObject penghand;
    private GameObject ganghand;
    private GameObject fx_PengGang;

    private void Awake()
    {
        EFSpawn = this.transform.parent.Find("EFSpwan");
        pengGangAnimSpawn = this.transform.parent.Find("PengGangAnimaSpawn");
    }
    public void Peng(OtherSeat nowPlay, int id)
    {
        peng(nowPlay,id);
        CreatePengHand();
    }
    /// <summary>
    /// 生成碰的三张牌
    /// </summary>
    /// <param name="nowPlay"></param>
    /// <param name="id"></param>
    /// <param name="handCard"></param>
     void peng(OtherSeat nowPlay,int id)
    {
        switch (nowPlay)
        {
                //碰的是下家
            case OtherSeat.Down:                
                if (LastIsDown||LastIsFace)
                {
                    startPos+=new Vector3(0.066f, 0, 0);
                    startPosOffset += new Vector3(0.066f, 0, 0);
                    createHandPos= SetPengGangAnimSpawnPos();//更新碰时生成手动画的位置
                    createFx_PengGangPos = SetFx_PengGangPos();//更新碰时特效生成位置
                    LastIsDown = false;
                    LastIsFace = false;
                }
                else if (LastIsUp)
                {
                    startPos +=new Vector3(0.049f, 0, 0.017f);
                    startPosOffset +=new Vector3(0.049f, 0, 0.017f);
                    createHandPos = SetPengGangAnimSpawnPos();
                    createFx_PengGangPos = SetFx_PengGangPos();
                    LastIsUp = false;
                }
                else if (LastIsDarkGang)
                {
                    startPos += new Vector3(0.066f, -0.017f, 0);
                    startPosOffset +=new Vector3(0.066f, -0.017f, 0);
                    createHandPos = SetPengGangAnimSpawnPos();
                    createFx_PengGangPos = SetFx_PengGangPos();
                    LastIsDarkGang = false;
                }
                //生成碰的第一张牌
                pengCard1Pos = startPos+ new Vector3(0, 0, -0.017f);
                pengCard1Rot = Quaternion.Euler(-90, 90, 0);
                pengCard1 = CreatePengGangCard(id,handCardPrefab,pengCard1Pos,pengCard1Rot);
                //生成碰的第二张牌
                pengCard2Pos = startPos+new Vector3(0.017f, 0, 0);
                pengCard2Rot=Quaternion.Euler(-90,0,0);
                pengCard2 = CreatePengGangCard(id,handCardPrefab, pengCard2Pos, pengCard2Rot);
                //生成碰的第三张牌
                pengCard3Pos = startPos+ new Vector3(0.051f, 0, 0);
                pengCard3Rot = Quaternion.Euler(-90, 0, 0);
                pengCard3 = CreatePengGangCard(id,handCardPrefab, pengCard3Pos, pengCard3Rot);

                pengCardIdList.Add(id);
                pengCardPosList.Add(pengCard1Pos);

                startPos = pengCard3Pos;
                LastIsDown = true;
                startPosOffset = pengCard3Pos - pengCard1Pos;
                break;
                //碰的是对家
            case OtherSeat.Face:
                if (LastIsDown|| LastIsFace)
                {
                    startPos+=new Vector3(0.037f, 0, 0);
                    startPosOffset += new Vector3(0.037f, 0, 0);
                    createHandPos = SetPengGangAnimSpawnPos();
                    createFx_PengGangPos = SetFx_PengGangPos();
                    LastIsDown = false;
                    LastIsFace = false;
                }
                else if (LastIsUp)
                {
                    startPos+=new Vector3(0.02f, 0, 0.017f);
                    startPosOffset+= new Vector3(0.02f, 0, 0.017f);
                    createHandPos = SetPengGangAnimSpawnPos();
                    createFx_PengGangPos = SetFx_PengGangPos();
                    LastIsUp = false;
                }
                else if (LastIsDarkGang)
                {
                    startPos += new Vector3(0.037f, -0.017f, 0);
                    startPosOffset+= new Vector3(0.037f, -0.017f, 0);
                    createHandPos = SetPengGangAnimSpawnPos();
                    createFx_PengGangPos = SetFx_PengGangPos();
                    LastIsDarkGang = false;
                }
                pengCard1Pos = startPos;
                pengCard1Rot = Quaternion.Euler(-90, 0, 0);
                pengCard1 = CreatePengGangCard(id,handCardPrefab, pengCard1Pos, pengCard1Rot);

                pengCard2Pos = startPos+ new Vector3(0.063f,0,-0.017f);
                pengCard2Rot = Quaternion.Euler(-90, 90, 0);
                pengCard2 = CreatePengGangCard(id,handCardPrefab, pengCard2Pos, pengCard2Rot);

                pengCard3Pos = startPos+ new Vector3(0.08f, 0, 0);
                pengCard3Rot = Quaternion.Euler(-90, 0, 0);
                pengCard3 = CreatePengGangCard(id,handCardPrefab, pengCard3Pos, pengCard3Rot);

                pengCardIdList.Add(id);
                pengCardPosList.Add(pengCard2Pos);

                startPos = pengCard3Pos;
                LastIsFace = true;
                startPosOffset = pengCard3Pos - pengCard1Pos;
                break;
                //碰的是上家
            case OtherSeat.Up:
                if (LastIsDown||LastIsFace)
                {
                    startPos+=new Vector3(0.037f, 0, 0);
                    startPosOffset +=new Vector3(0.037f, 0, 0);
                    createHandPos= SetPengGangAnimSpawnPos();
                    createFx_PengGangPos = SetFx_PengGangPos();
                    LastIsDown = false;
                    LastIsFace = false;
                }
                else if (LastIsUp)
                {
                    startPos+=new Vector3(0.02f, 0, 0.017f);
                    startPosOffset +=new Vector3(0.02f, 0, 0.017f);
                    createHandPos= SetPengGangAnimSpawnPos();
                    createFx_PengGangPos = SetFx_PengGangPos();
                    LastIsUp = false;
                }
                else if (LastIsDarkGang)
                {
                    startPos += new Vector3(0.037f, -0.017f, 0);
                    startPosOffset +=new Vector3(0.037f, -0.017f, 0);
                    createHandPos= SetPengGangAnimSpawnPos();
                    createFx_PengGangPos = SetFx_PengGangPos();
                    LastIsDarkGang = false;
                }
                pengCard1Pos = startPos;
                pengCard1Rot = Quaternion.Euler(-90, 0, 0);
                pengCard1 = CreatePengGangCard(id,handCardPrefab, pengCard1Pos, pengCard1Rot);

                pengCard2Pos = startPos+ new Vector3(0.034f, 0, 0);
                pengCard2Rot = Quaternion.Euler(-90, 0, 0);
                pengCard2 = CreatePengGangCard(id,handCardPrefab, pengCard2Pos, pengCard2Rot);

                pengCard3Pos = startPos+ new Vector3(0.097f, 0, -0.017f);
                pengCard3Rot = Quaternion.Euler(-90, 90, 0);
                pengCard3 = CreatePengGangCard(id,handCardPrefab, pengCard3Pos, pengCard3Rot);

                pengCardIdList.Add(id);
                pengCardPosList.Add(pengCard3Pos);

                startPos = pengCard3Pos;
                LastIsUp = true;
                startPosOffset = pengCard3Pos - pengCard1Pos;
                break;
            default:
                break;
        }
    }

    public void Gang(OtherSeat nowPlay, int id, bool isDarkGang)
    {
        gang(nowPlay, id, isDarkGang);
        CreateGangHand();
    }
    /// <summary>
    /// 生成杠的四张牌
    /// </summary>
    /// <param name="nowPlay"></param>
    /// <param name="id"></param>
    /// <param name="handCard"></param>
     void gang(OtherSeat nowPlay, int id,bool isDarkGang)
    {
        //如果是暗杠
        if (isDarkGang)
        {
            if (LastIsDown||LastIsFace)
            {
                startPos += new Vector3(0.037f, 0, 0);
                startPosOffset +=new Vector3(0.037f, 0, 0);
                createHandPos = SetPengGangAnimSpawnPos();
                createFx_PengGangPos = SetFx_PengGangPos();
                LastIsDown = false;
                LastIsFace = false;
            }
            else if (LastIsUp)
            {
                startPos += new Vector3(0.02f, 0, 0.017f);
                startPosOffset+=new Vector3(0.02f, 0, 0.017f);
                createHandPos = SetPengGangAnimSpawnPos();
                createFx_PengGangPos = SetFx_PengGangPos();
                LastIsUp = false;
            }
            else if (LastIsDarkGang)
            {
                startPos += new Vector3(0.037f, -0.017f, 0);
                startPosOffset +=new Vector3(0.037f, -0.017f, 0);
                createHandPos = SetPengGangAnimSpawnPos();
                createFx_PengGangPos = SetFx_PengGangPos();
                LastIsDarkGang = false;
            }
            gangCard1Pos = startPos;
            gangCard1Rot = Quaternion.Euler(-90, 0, 0);
            gangCard1 = CreatePengGangCard(id, handCardPrefab, gangCard1Pos, gangCard1Rot);

            gangCard2Pos = startPos + new Vector3(0.034f, 0.017f, 0);
            gangCard2Rot = Quaternion.Euler(90, 180, 0);
            gangCard2 = CreatePengGangCard(id, handCardPrefab, gangCard2Pos, gangCard2Rot);

            gangCard3Pos = startPos + new Vector3(0.068f, 0.017f, 0);
            gangCard3Rot = Quaternion.Euler(90, 180, 0);
            gangCard3 = CreatePengGangCard(id, handCardPrefab, gangCard3Pos, gangCard3Rot);

            gangCard4Pos = startPos + new Vector3(0.102f, 0.017f, 0);
            gangCard4Rot = Quaternion.Euler(90, 180, 0);
            gangCard4 = CreatePengGangCard(id, handCardPrefab, gangCard4Pos, gangCard4Rot);

            startPos = gangCard4Pos;
            LastIsDarkGang = true;
            startPosOffset = gangCard4Pos - gangCard1Pos;
            return;
        }
        switch (nowPlay)
        {
                //如果杠的是下家
            case OtherSeat.Down:
                if (LastIsDown||LastIsFace)
                {
                    startPos +=new Vector3(0.066f, 0, 0);
                    startPosOffset+=new Vector3(0.066f, 0, 0);
                    createHandPos = SetPengGangAnimSpawnPos();
                    createFx_PengGangPos = SetFx_PengGangPos();
                    LastIsDown = false;
                    LastIsFace = false;
                }
                else if (LastIsUp)
                {
                    startPos+=new Vector3(0.049f, 0, 0.017f);
                    startPosOffset+=new Vector3(0.049f, 0, 0.017f);
                    createHandPos = SetPengGangAnimSpawnPos();
                    createFx_PengGangPos = SetFx_PengGangPos();
                    LastIsUp = false;
                }
                else if (LastIsDarkGang)
                {
                    startPos += new Vector3(0.066f, -0.017f, 0);
                    startPosOffset +=new Vector3(0.066f, -0.017f, 0);
                    createHandPos = SetPengGangAnimSpawnPos();
                    createFx_PengGangPos = SetFx_PengGangPos();
                    LastIsDarkGang = false;
                }
                //生成杠的第一张牌
                gangCard1Pos = startPos+new Vector3(0, 0, -0.017f);
                gangCard1Rot = Quaternion.Euler(-90, 90, 0);
                gangCard1 = CreatePengGangCard(id, handCardPrefab, gangCard1Pos, gangCard1Rot);
                //生成杠的第二张牌
                gangCard2Pos = startPos + new Vector3(0.017f, 0, 0);
                gangCard2Rot = Quaternion.Euler(-90, 0, 0);
                gangCard2 = CreatePengGangCard(id, handCardPrefab, gangCard2Pos, gangCard2Rot);
                //生成杠的第三张牌
                gangCard3Pos = startPos + new Vector3(0.051f, 0, 0);
                gangCard3Rot = Quaternion.Euler(-90, 0, 0);
                gangCard3 = CreatePengGangCard(id, handCardPrefab, gangCard3Pos, gangCard3Rot);
                //生成杠的第四张牌
                gangCard4Pos = startPos + new Vector3(0.085f, 0, 0);
                gangCard4Rot = Quaternion.Euler(-90, 0, 0);
                gangCard4 = CreatePengGangCard(id, handCardPrefab, gangCard4Pos, gangCard4Rot);

                startPos = gangCard4Pos;
                LastIsDown = true;
                startPosOffset = gangCard4Pos - gangCard1Pos;
                break;
                //如果杠的是对家
            case OtherSeat.Face:
                if (LastIsDown||LastIsFace)
                {
                    startPos +=new Vector3(0.037f, 0, 0);
                    startPosOffset +=new Vector3(0.037f, 0, 0);
                    createHandPos = SetPengGangAnimSpawnPos();
                    createFx_PengGangPos = SetFx_PengGangPos();
                    LastIsDown = false;
                    LastIsFace = false;
                }
                else if (LastIsUp)
                {
                    startPos += new Vector3(0.02f, 0, 0.017f);
                    startPosOffset+=new Vector3(0.02f, 0, 0.017f);
                    createHandPos = SetPengGangAnimSpawnPos();
                    createFx_PengGangPos = SetFx_PengGangPos();
                    LastIsUp = false;
                }
                else if (LastIsDarkGang)
                {
                    startPos += new Vector3(0.037f, -0.017f, 0);
                    startPosOffset +=new Vector3(0.037f, -0.017f, 0);
                    createHandPos = SetPengGangAnimSpawnPos();
                    createFx_PengGangPos = SetFx_PengGangPos();
                    LastIsDarkGang = false;
                }
                gangCard1Pos = startPos;
                gangCard1Rot = Quaternion.Euler(-90, 0, 0);
                gangCard1 = CreatePengGangCard(id, handCardPrefab, gangCard1Pos, gangCard1Rot);

                gangCard2Pos = startPos + new Vector3(0.034f, 0, 0);
                gangCard2Rot = Quaternion.Euler(-90, 0, 0);
                gangCard2 = CreatePengGangCard(id, handCardPrefab, gangCard2Pos, gangCard2Rot);

                gangCard3Pos = startPos + new Vector3(0.097f, 0, -0.017f);
                gangCard3Rot = Quaternion.Euler(-90, 90, 0);
                gangCard3 = CreatePengGangCard(id, handCardPrefab, gangCard3Pos, gangCard3Rot);

                gangCard4Pos = startPos + new Vector3(0.114f, 0, 0);
                gangCard4Rot = Quaternion.Euler(-90, 0, 0);
                gangCard4 = CreatePengGangCard(id, handCardPrefab, gangCard4Pos, gangCard4Rot);

                startPos = gangCard4Pos;
                LastIsFace = true;
                startPosOffset = gangCard4Pos - gangCard1Pos;
                break;
                //如果杠的是上家
            case OtherSeat.Up:
                if (LastIsDown||LastIsFace)
                {
                    startPos += new Vector3(0.037f, 0, 0);
                    startPosOffset +=new Vector3(0.037f, 0, 0);
                    createHandPos = SetPengGangAnimSpawnPos();
                    createFx_PengGangPos = SetFx_PengGangPos();
                    LastIsDown = false;
                    LastIsFace = false;
                }
                else if (LastIsUp)
                {
                    startPos += new Vector3(0.02f, 0, 0.017f);
                    startPosOffset += new Vector3(0.02f, 0, 0.017f);
                    createHandPos = SetPengGangAnimSpawnPos();
                    createFx_PengGangPos = SetFx_PengGangPos();
                    LastIsUp = false;
                }
                else if (LastIsDarkGang)
                {
                    startPos += new Vector3(0.037f, -0.017f, 0);
                    startPosOffset +=new Vector3(0.037f, -0.017f, 0);
                    createHandPos = SetPengGangAnimSpawnPos();
                    createFx_PengGangPos = SetFx_PengGangPos();
                    LastIsDarkGang = false;
                }
                gangCard1Pos = startPos;
                gangCard1Rot = Quaternion.Euler(-90, 0, 0);
                gangCard1 = CreatePengGangCard(id, handCardPrefab, gangCard1Pos, gangCard1Rot);

                gangCard2Pos = startPos + new Vector3(0.034f, 0, 0);
                gangCard2Rot = Quaternion.Euler(-90, 0, 0);
                gangCard2 = CreatePengGangCard(id, handCardPrefab, gangCard2Pos, gangCard2Rot);

                gangCard3Pos = startPos + new Vector3(0.068f, 0, 0);
                gangCard3Rot = Quaternion.Euler(-90, 0, 0);
                gangCard3 = CreatePengGangCard(id, handCardPrefab, gangCard3Pos, gangCard3Rot);

                gangCard4Pos = startPos + new Vector3(0.131f, 0, -0.017f);
                gangCard4Rot = Quaternion.Euler(-90, 90, 0);
                gangCard4 = CreatePengGangCard(id, handCardPrefab, gangCard4Pos, gangCard4Rot);

                startPos = gangCard4Pos;
                LastIsUp = true;
                startPosOffset = gangCard4Pos - gangCard1Pos;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 创建碰牌及设置位置
    /// </summary>
    /// <param name="id"></param>
    /// <param name="go"></param>
    /// <param name="pos"></param>
    /// <param name="q"></param>
    /// <returns></returns>
    public GameObject CreatePengGangCard(int id,GameObject go,Vector3 pos,Quaternion q)
    {
        GameObject obj = Instantiate(go, new Vector3(0, 0, 0), Quaternion.identity, pengGangArea) as GameObject;
        obj.transform.localPosition = pos;
        obj.transform.localRotation = q;
        RuleManager.m_instance.UVoffSet(id,obj);//设置碰杠牌的uv偏移
        AddToList(id, obj);
        obj.layer = LayerMask.NameToLayer("PengPai");
        return obj;
    }

    /// <summary>
    /// 创建弯杠时的牌
    /// </summary>
    /// <param name="id"></param>
    public void CreateWanGangCard(int id)
    {
        if (pengCardIdList.Contains(id))
        {
            int index = pengCardIdList.IndexOf(id);
            Vector3 wanGangCard = (Vector3)pengCardPosList[index] + new Vector3(0, 0, -0.034f);
            Quaternion wanGangCardRot = Quaternion.Euler(-90, 90, 0);
            CreatePengGangCard(id, handCardPrefab, wanGangCard, wanGangCardRot);
            pengCardIdList.RemoveAt(index);
            pengCardPosList.RemoveAt(index);
        }
    }

    //生成碰牌的手
    public void CreatePengHand()
    {
        penghand = ResourcesMgr.m_Instance.InstantiateGameObjectWithType("PengHand", ResourceType.Hand);
        penghand.transform.position = Vector3.zero;
        penghand.transform.rotation = Quaternion.identity;
        penghand.transform.SetParent(pengGangAnimSpawn);
        //penghand= Instantiate(pengHandPrefab,new Vector3(0,0,0) ,Quaternion.identity,pengGangAnimSpawn)as GameObject;
        penghand.transform.localPosition = createHandPos;
        penghand.transform.localRotation = Quaternion.identity;
        CreateFx(createFx_PengGangPos);
    }

    //生成杠牌的手
    public void CreateGangHand()
    {
        ganghand = ResourcesMgr.m_Instance.InstantiateGameObjectWithType("GangHand", ResourceType.Hand);
        ganghand.transform.position = Vector3.zero;
        ganghand.transform.rotation = Quaternion.identity;
        ganghand.transform.SetParent(pengGangAnimSpawn);
        //ganghand = Instantiate(gangHandPrefab, new Vector3(0,0,0), Quaternion.identity,pengGangAnimSpawn) as GameObject;
        ganghand.transform.localPosition = createHandPos;
        ganghand.transform.localRotation = Quaternion.identity;
        CreateFx(createFx_PengGangPos);
    }

    /// <summary>
    /// 生成碰杠特效
    /// </summary>
    /// <param name="spawnPoint"></param>
    public void CreateFx(Vector3 spawnPoint)
    {
        fx_PengGang = Instantiate(fx_PengGangPrefab, new Vector3(0,0,0), Quaternion.identity,EFSpawn) as GameObject;
        fx_PengGang.transform.localPosition = createFx_PengGangPos;
        fx_PengGang.transform.localRotation = Quaternion.identity;
    }

    /// <summary>
    /// 设置碰杠时手生成的位置
    /// </summary>
    /// <returns></returns>
    public Vector3 SetPengGangAnimSpawnPos()
    {
        createHandPos += new Vector3(startPosOffset.x, 0, 0);
        return createHandPos;
    }

    /// <summary>
    /// 设置碰时特效生成的位置
    /// </summary>
    /// <returns></returns>
    public Vector3 SetFx_PengGangPos()
    {
        createFx_PengGangPos += new Vector3(startPosOffset.x, 0, 0);
        return createFx_PengGangPos;
    }

    /// <summary>
    /// 将碰杠牌加到List里
    /// </summary>
    /// <param name="id"></param>
    /// <param name="obj"></param>
    public void AddToList(int id, GameObject obj)
    {
        PengGangCardItem item = new PengGangCardItem();
        item._id = id;
        item._obj = obj;
        pengGangCardList.Add(item);
    }
    public void ResetPengGangInfo()
    {
        foreach (PengGangCardItem item in pengGangCardList)
        {
            Destroy(item._obj);
        }
        pengGangCardList.Clear();
        pengCardIdList.Clear();
        pengCardPosList.Clear();
        startPos = new Vector3(0.034f, 0, 0);
        startPosOffset = Vector3.zero;
        pengGangAnimSpawn.localPosition = new Vector3(-0.28f, 0.022f, 0.507f);
        EFSpawn.localPosition = new Vector3(-0.351f, 0.01f, 0.35f);
    }

}
