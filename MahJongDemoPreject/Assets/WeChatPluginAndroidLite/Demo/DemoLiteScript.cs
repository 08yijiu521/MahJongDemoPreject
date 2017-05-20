using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DemoLiteScript : MonoBehaviour {

    [SerializeField]
    WeChatPluginLiteScript m_pluginObject;
    [SerializeField]
    Toggle m_momentsToggle;
    [SerializeField]
    Texture2D[] m_Textures;

    public void ShareImage()
    {
        m_pluginObject.Prepare();
        m_pluginObject.m_isMoments = m_momentsToggle.isOn;
        m_pluginObject.m_ShareText = "Test Text Image";
        m_pluginObject.m_Textures = new Texture2D[1] { m_Textures[0] };
        m_pluginObject.Share();
    }

    public void ShareMultpleImages()
    {
        m_pluginObject.Prepare();
        m_pluginObject.m_isMoments = m_momentsToggle.isOn;
        m_pluginObject.m_ShareText = "Test Text MultiImage";
        m_pluginObject.m_Textures = m_Textures;
        m_pluginObject.Share();
    }
}
