// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
/*--- __ECO__ __ACTION__ ---*/

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Application)]
	[Tooltip("Unloads assets that are not used. An asset is deemed to be unused if it isn't reached after walking the whole game object hierarchy, including script components. Static variables are also examined.")]
	public class UnloadUnusedAssets : FsmStateAction
	{
		
		[Tooltip("Event to send when the operatoin has finished.")]
		public FsmEvent UnloadDoneEvent;	
		
		private AsyncOperation asyncOperation;
		
		public override void Reset()
		{
			UnloadDoneEvent = null;
		}
		
		public override void OnEnter()
		{
			asyncOperation = Resources.UnloadUnusedAssets();
			
			if (UnloadDoneEvent!=null)
			{
				return; // Don't Finish()
			}
			
			Finish();
		}
		
		public override void OnUpdate()
		{
			if (asyncOperation.isDone)
			{
				Fsm.Event(UnloadDoneEvent);
				Finish();
			}
		}
		
	}
}