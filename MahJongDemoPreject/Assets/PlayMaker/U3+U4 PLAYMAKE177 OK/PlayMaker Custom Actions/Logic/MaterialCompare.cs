// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.
/*--- __ECO__ __ACTION__ ---*/

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Logic)]
	[Tooltip("Compare 2 Material Variables and send events based on the result.")]
	public class MaterialCompare : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmMaterial materialVariable;
		
		[RequiredField]
		public FsmMaterial compareTo;

		[Tooltip("Event to send if the 2 object values are equal.")]
		public FsmEvent equalEvent;
		
		[Tooltip("Event to send if the 2 object values are not equal.")]
		public FsmEvent notEqualEvent;

		[UIHint(UIHint.Variable)]
		[Tooltip("Store the result in a variable.")]
		public FsmBool storeResult;

		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		public override void Reset()
		{
			materialVariable = null;
			compareTo = null;
			storeResult = null;
			equalEvent = null;
			notEqualEvent = null;
			everyFrame = false;
		}

		public override void OnEnter()
		{
			DoCompare();

			if (!everyFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			DoCompare();
		}

		void DoCompare()
		{
			var result = materialVariable.Value == compareTo.Value;

			storeResult.Value = result;

			Fsm.Event(result ? equalEvent : notEqualEvent);
		}
	}
}