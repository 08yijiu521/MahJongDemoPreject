// Based on code (c) copyright Hutong Games, LLC 2010-2012. All rights reserved.
// by Brzozowsky Adam (brzozowsky@gmail.com)

using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("WebCam")]
    [Tooltip("Stream a webcam video to selected material.")]
    public class SetWebcamMaterial : FsmStateAction
    {
        public FsmString webcamName;
	    public FsmInt requestedHeight1;
		public FsmInt requestedWidth1;
		public FsmInt requestedFPS1;
		public FsmMaterial webcamMaterial;
		//private WebCamTexture webCamTexture;
		public WebCamTexture webCamTexture;
		

        public override void Reset()
        {
			webcamName = null;
			requestedHeight1 = null;
			requestedWidth1 = null;
			requestedFPS1 = null;
			webcamMaterial = null;
			webCamTexture = null;
        }

        public override void OnEnter()
		{
			webCamTexture = new WebCamTexture();
			webCamTexture.deviceName = webcamName.Value;
			webcamMaterial.Value.SetTexture("_MainTex", webCamTexture);
			//webCamTexture.requestedHeight(640);
			//webCamTexture.requestedWidth(480);
			//webCamTexture.texelSize
		    webCamTexture.requestedHeight = requestedHeight1.Value;
			webCamTexture.requestedWidth = requestedWidth1.Value;
			webCamTexture.requestedFPS = requestedFPS1.Value;
			webCamTexture.Play();
			Finish();
		}
	}
}