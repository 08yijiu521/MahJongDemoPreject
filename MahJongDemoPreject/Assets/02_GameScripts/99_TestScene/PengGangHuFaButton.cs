using UnityEngine;
using System.Collections;

public class PengGangHuFaButton : MonoBehaviour {

	public PlayMakerFSM fsm;


	void OnClick(){
		UILabel label = this.gameObject.GetComponentInChildren<UILabel> ();
		switch (label.text) {
		case "碰":
			fsm.SendEvent ("Peng");
			break;
		case "杠":
			fsm.SendEvent ("Gang");
			break;
		case "胡":
			fsm.SendEvent ("Hu");
			break;
		case "发牌":
                DHM_HandCardManager[] hands = FindObjectsOfType<DHM_HandCardManager>();
                foreach (var item in hands)
                {
                    item.FaPai();
                }
                fsm.SendEvent ("Fapai");
			break;
		default:
			break;
		}
	}
}
