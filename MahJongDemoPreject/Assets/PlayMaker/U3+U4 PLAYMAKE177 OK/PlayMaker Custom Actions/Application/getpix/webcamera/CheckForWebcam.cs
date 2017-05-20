// Based on code (c) copyright Hutong Games, LLC 2010-2012. All rights reserved.
// by Brzozowsky Adam (brzozowsky@gmail.com)

using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("WebCam")]
	[Tooltip("Checks system for a webcam.")]
	public class CheckForWebcam : FsmStateAction
	{
        [Tooltip("Event to send if a webcam was found.")]
		public FsmEvent webcamFound;

        [Tooltip("Event to send if a webcam was not found.")]
		public FsmEvent webcamNotFound;

		public override void Reset()
		{
			webcamFound = null;
			webcamNotFound = null;
		}

		public override void OnEnter()
		{
			int numOfCams = WebCamTexture.devices.Length;
			if (numOfCams > 0)
			{
				Fsm.Event(webcamFound);
			}
			else
			{
				Fsm.Event(webcamNotFound);
			}
		}
	}
}