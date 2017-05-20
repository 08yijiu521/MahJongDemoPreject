// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
//--- __ECO__ __ACTION__ ---//

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.RenderSettings)]
	[Tooltip("Get the density of the fog")]
	public class GetFogDensity : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The fog start distance")]
		public FsmFloat density;
		
		[Tooltip("Repeat every frame. Useful if the Fog setting is changing.")]
		public bool everyFrame;
		
		
		public override void Reset()
		{
			density =null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			_getValue();
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnUpdate()
		{
			_getValue();
		}
		
		void _getValue()
		{
			density.Value = RenderSettings.fogDensity;
		}
	}
}