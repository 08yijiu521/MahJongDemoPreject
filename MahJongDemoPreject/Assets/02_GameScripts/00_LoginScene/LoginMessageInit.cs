using UnityEngine;
using System.Collections;

public class LoginMessageInit : MonoBehaviour {
    public UILabel label_LoginMessage;

    public void InitBox(string messageText)
    {
        label_LoginMessage.text = messageText;
    }

    public void OnButtonEnterClick()
    {
        Destroy(gameObject);
    }

}
