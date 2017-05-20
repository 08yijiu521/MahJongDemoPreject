// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
/*--- __ECO__ __ACTION__ ---*/

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Each time this action is called it iterate to the next value from Start to End. This lets you safely loop and process anything on each iteratation.")]
	public class Iterate : FsmStateAction
	{

		[RequiredField]
		[Tooltip("Start value")]
		public FsmInt startIndex;

		[Tooltip("End value")]
		public FsmInt endIndex;
		
		[Tooltip("increment value at each iteration, absolute value only, it will itself find if it needs to substract or add")]
		public FsmInt increment;
		
		[Tooltip("Event to send to get the next child.")]
		public FsmEvent loopEvent;
		
		[Tooltip("Event to send when we reached the end.")]
		public FsmEvent finishedEvent;
		
		[ActionSection("Result")]
		[Tooltip("The current value of the iteration process")]
		[UIHint(UIHint.Variable)]
		public FsmInt currentIndex;
		
		private bool started = false;
		
		private bool _up = true;
		public override void Reset()
		{
			startIndex = 0;
			endIndex = 10;
			currentIndex = null;
			loopEvent = null;
			finishedEvent = null;
			increment = 1;
		}
		

		
		public override void OnEnter()
		{
			DoGetNext();

			Finish();
		}
		
		void DoGetNext()
		{

			// reset?
			if (!started)
			{
				_up = startIndex.Value<endIndex.Value;
				currentIndex.Value = startIndex.Value;
				started = true;
				
				if (loopEvent != null)
				{
					Fsm.Event(loopEvent);
				}
				return;
			}
			
			if (_up)
			{
				if (currentIndex.Value >= endIndex.Value)
				{
					started = false;
			
					Fsm.Event(finishedEvent);
				
					return;
				}
				// iterate
				currentIndex.Value =  Mathf.Max(startIndex.Value,currentIndex.Value+Mathf.Abs(increment.Value));
			}else{
				if (currentIndex.Value <= endIndex.Value)
				{
					started = false;
	
					Fsm.Event(finishedEvent);
				
					return;
				}
				// iterate
				currentIndex.Value = Mathf.Max(endIndex.Value,currentIndex.Value -Mathf.Abs(increment.Value));
			}

		
			if (loopEvent != null)
			{
				Fsm.Event(loopEvent);
			}
		}
	}
}