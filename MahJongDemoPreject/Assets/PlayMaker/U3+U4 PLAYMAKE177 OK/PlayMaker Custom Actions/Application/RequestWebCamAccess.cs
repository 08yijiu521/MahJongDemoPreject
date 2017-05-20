// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
/*--- __ECO_ACTION__ ---*/

using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Application)]
	[Tooltip("Requests access to webcam and microphone")]
	public class RequestWebCamAndMicAccess : FsmStateAction
	{
		[RequiredField]
		[Tooltip("Event sent when access is granted")]
		public FsmEvent AccessGrantedEvent;
		
		[RequiredField]
		[Tooltip("Event sent when access is denied")]
		public FsmEvent AccessDeniedEvent;
		
		static private RequestWebCamAccessHelper _helper;
		
		public override void Reset()
		{
			AccessDeniedEvent = null;
			
			AccessGrantedEvent = null;
		}

		public override void OnEnter()
		{
			_helper = ( new GameObject("RequestWebCamAccessHelper") ).AddComponent< RequestWebCamAccessHelper >();
			_helper.RequestAuthorization(this);
		
		}
		
		public void AccessGranted(bool state)
		{
			if (state) {
				Fsm.Event(AccessGrantedEvent);
       		 } else {
				Fsm.Event(AccessDeniedEvent);
       	 	}
			
		_helper = null;
		}

	}
	
	//This component bridge this action to perform a coroutine.
	public sealed class RequestWebCamAccessHelper : MonoBehaviour {
	   				
		RequestWebCamAndMicAccess _action;		
		
		public void RequestAuthorization(RequestWebCamAndMicAccess action)
		{
			_action= action;
			
			StartCoroutine("check");

		}
		
		IEnumerator check() {
			
       		yield return Application.RequestUserAuthorization(UserAuthorization.WebCam | UserAuthorization.Microphone);
			
			_action.AccessGranted(Application.HasUserAuthorization(UserAuthorization.WebCam | UserAuthorization.Microphone));
			
			Destroy(this.gameObject);
		}
		
	}
	
}



