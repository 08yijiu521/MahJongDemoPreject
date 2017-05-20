// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
/*--- __ECO__ __ACTION__ ---*/

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Linearly interpolates between 2 floats.")]
	public class FloatLerp : FsmStateAction
	{
		[RequiredField]
		[Tooltip("First Float")]
		public FsmFloat fromFloat;
		
		[RequiredField]
		[Tooltip("Second Float.")]
		public FsmFloat toFloat;
		
		[RequiredField]
		[Tooltip("Interpolate between FromFloat and ToFloat by this amount. Value is clamped to 0-1 range. 0 = FromFloat; 1 = ToFloat; 0.5 = half way in between.")]
		public FsmFloat amount;
		
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the result in this float variable.")]
		public FsmFloat storeResult;

		[Tooltip("Repeat every frame. Useful if any of the values are changing.")]
		public bool everyFrame;

		public override void Reset()
		{
			fromFloat = new FsmFloat { UseVariable = true };
			toFloat = new FsmFloat { UseVariable = true };
			storeResult = null;
			everyFrame = true;
		}

		public override void OnEnter()
		{
			DoFloatLerp();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoFloatLerp();
		}

		void DoFloatLerp()
		{
			storeResult.Value = Mathf.Lerp(fromFloat.Value, toFloat.Value, amount.Value);
		}
	}
}

