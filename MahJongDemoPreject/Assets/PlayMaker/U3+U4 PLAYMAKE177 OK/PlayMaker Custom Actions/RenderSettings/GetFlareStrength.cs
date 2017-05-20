// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
//--- __ECO__ __ACTION__ ---//

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.RenderSettings)]
	[Tooltip("Get the intensity of all flares in the scene.")]
	public class GetFlareStrength : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("The intensity of all flares in the scene.")]
		public FsmFloat flareStrength;
		
		[Tooltip("Repeat every frame. Useful if the value is changing.")]
		public bool everyFrame;
		
		
		public override void Reset()
		{
			flareStrength =null;
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
			flareStrength.Value = RenderSettings.flareStrength;
		}
	}
}