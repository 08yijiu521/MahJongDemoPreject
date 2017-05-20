// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
/*--- __ECO__ __ACTION__ ---*/

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Math)]
	[Tooltip("Pick a random weighted Int picked from an array of Ints.")]
	public class RandomWeightedInt : FsmStateAction
	{
		[CompoundArray("Ints", "Int", "Weight")]
		public FsmInt[] ints;
		[HasFloatSlider(0, 1)]
		public FsmFloat[] weights;
		
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmInt result;
		
		
		public override void Reset()
		{
			ints = new FsmInt[3];
			ints[0] = 0;
			ints[1] = 2;
			ints[2] = 3;
			weights = new FsmFloat[] {1,1,1};
			result = null;
			
		}

		public override void OnEnter()
		{
			if (ints.Length > 0)
			{
				result.Value = ActionHelpers.GetRandomWeightedIndex(weights);
			}						
			
			Finish();
		}
		
	}
}