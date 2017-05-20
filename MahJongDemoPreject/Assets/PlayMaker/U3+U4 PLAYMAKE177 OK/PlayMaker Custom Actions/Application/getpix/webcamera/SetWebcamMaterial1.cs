// Based on code (c) copyright Hutong Games, LLC 2010-2012. All rights reserved.
// by Brzozowsky Adam (brzozowsky@gmail.com)

using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("WebCam")]
    [Tooltip("Stream a webcam video to selected material.")]
    public class SetWebcamMaterial2: FsmStateAction
    {
        public FsmString webcamName;
	    public FsmInt requestedHeight1;
		public FsmInt requestedWidth1;
		public FsmInt requestedFPS1;
		public FsmMaterial webcamMaterial;
		//private WebCamTexture webCamTexture;
		public WebCamTexture webCamTexture1;
		

        public override void Reset()
        {
			webcamName = null;
			requestedHeight1 = null;
			requestedWidth1 = null;
			requestedFPS1 = null;
			webcamMaterial = null;
			webCamTexture1 = null;
        }

        public override void OnEnter()
		{
			webCamTexture1 = new WebCamTexture();
			webCamTexture1.deviceName = webcamName.Value;
			webcamMaterial.Value.SetTexture("_MainTex", webCamTexture1);
			//webCamTexture.requestedHeight(640);
			//webCamTexture.requestedWidth(480);
			//webCamTexture.texelSize
		    webCamTexture1.requestedHeight = requestedHeight1.Value;
			webCamTexture1.requestedWidth = requestedWidth1.Value;
			webCamTexture1.requestedFPS = requestedFPS1.Value;
			//webCamTexture1.Play();
			webCamTexture1.Pause();
			Finish();
		}
	}
}