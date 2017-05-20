/************************
 * Title：
 * Function：
 * 	－ 在玩家摸牌之前，从牌垛上删除一张牌
 * Used By：paiDuoManager
 * Author：qwt
 * Date：2017.2.27
 * Version：1.0
 * Description：
 *
************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeletePai : MonoBehaviour {

    public List<GameObject> deletePaiList = new List<GameObject>();

    private int i = 0;
    private int j = 0;

    public static DeletePai m_instance = null;
    private void Awake()
    {
        m_instance = this;
    }
    /// <summary>
    /// 将剩余的牌垛加到List里
    /// </summary>
    /// <param name="obj"></param>
    public void GetDeletePaiList(GameObject obj)
    {        
        deletePaiList.Add(obj);
    }

  /// <summary>
  /// 将得到的list排序
  /// </summary>
  /// <param name="isGang"></param>
    public void ReverseDeletePaiList()
    {
        for (int i = 0; i < deletePaiList.Count-2; i = i + 2)
        {
            GameObject temp = deletePaiList[i];
            deletePaiList[i] = deletePaiList[i + 1];
            deletePaiList[i + 1] = temp;
        } 
    }

    /// <summary>
    /// 当上家出完牌后，从牌垛上删除一张牌
    /// </summary>
    public void Deletepai()
    {
        if (deletePaiList.Count>0)
        {
            ReverseDeletePaiList();
            Destroy(deletePaiList[i]);
            ShowRemainCard.instance.ShowCardCountAfterDelete();
            i++;
            ReverseDeletePaiList();
        }
    }

    /// <summary>
    /// 当玩家杠时，从牌垛的尾部删除一张牌
    /// </summary>
    public void Deletepai_Gang()
    {
        if (deletePaiList.Count>0)
        {
            Destroy(deletePaiList[deletePaiList.Count - 1 - j]);
            ShowRemainCard.instance.ShowCardCountAfterDelete();
            j++;
        }
    }
    public void ClearList()
    {
        i = 0;
        j = 0;
        deletePaiList.Clear();
    }
}
