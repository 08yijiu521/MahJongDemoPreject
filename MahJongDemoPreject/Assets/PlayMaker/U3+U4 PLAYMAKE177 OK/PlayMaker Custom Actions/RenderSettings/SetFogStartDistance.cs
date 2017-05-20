// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
//--- __ECO__ __ACTION__ ---//

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.RenderSettings)]
	[Tooltip("Set the start distance of the fog")]
	public class SetFogEndDistance : FsmStateAction
	{
		[RequiredField]
		[Tooltip("The start distance")]
		public FsmFloat startDistance;
		
		[Tooltip("Repeat every frame. Useful if the Enable Fog setting is changing.")]
		public bool everyFrame;
		
		
		public override void Reset()
		{
			startDistance =null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			_setValue();
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
			_setValue();
		}
		
		void _setValue()
		{
			RenderSettings.fogStartDistance = startDistance.Value;
		}
	}
}