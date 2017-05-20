// Based on code (c) copyright Hutong Games, LLC 2010-2012. All rights reserved.
// by Brzozowsky Adam (brzozowsky@gmail.com)

using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("WebCam")]
    [Tooltip("Find webcam and store name as string")]
    public class GetWebcamName : FsmStateAction
    {
        public FsmString[] storeWebcamName;
		public FsmInt storeWebcamCount;
		private FsmString[] camName;
		private WebCamTexture webCamTexture;

        public override void Reset()
        {
			storeWebcamName = null;
			storeWebcamCount = 0;
        }

        public override void OnEnter()
		{
			int numOfCams = WebCamTexture.devices.Length;
			this.camName = new FsmString[numOfCams];
			storeWebcamCount.Value = numOfCams;
			
			for(int i = 0; i < numOfCams; i++)  
			{  
				this.camName[i] = WebCamTexture.devices[i].name;    
				storeWebcamName[i].Value = camName[i].Value;
			} 
		Finish();
		}
	}
}