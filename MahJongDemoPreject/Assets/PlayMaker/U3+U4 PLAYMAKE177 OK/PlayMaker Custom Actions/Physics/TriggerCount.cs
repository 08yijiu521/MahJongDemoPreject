// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
/*--- __ECO__ __ACTION__ ---*/

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Detect collisions with objects that have RigidBody components. \nNOTE: The system events, TRIGGER ENTER, TRIGGER STAY, and TRIGGER EXIT are sent when any object collides with the trigger. Use this action to filter collisions by Tag.")]
	public class TriggerCount : FsmStateAction
	{
//		public TriggerType trigger;
		[UIHint(UIHint.Tag)]
		public FsmString collideTag;
//		public FsmEvent sendEvent;
		public FsmInt collideCount;
//		public FsmInt maxCollide;
		public FsmBool collidedBool;
		public bool everyFrame;

		public override void Reset()
		{
//			trigger = TriggerType.OnTriggerEnter;
			collideTag = "Untagged";
//			sendEvent = null;
			collideCount = null;
			collidedBool = null;
			everyFrame = false;
		}
		
		public override void Awake()
		{

			Fsm.HandleTriggerEnter = true;
					
			Fsm.HandleTriggerExit = true;
		
		}
		
		public override void OnEnter()
		{
			if (!everyFrame)
				Finish();
		}

		public override void DoTriggerEnter(Collider other)
			{
				if (other.gameObject.tag == collideTag.Value)
				{
					collideCount.Value = collideCount.Value + 1;
				}
			}

		public override void DoTriggerExit(Collider other)
			{
				if (other.gameObject.tag == collideTag.Value)
				{
					collideCount.Value = collideCount.Value - 1;
				}
			}
		
		public override void OnUpdate()
		{
			if (collideCount.Value == 0 )
			{
				collidedBool.Value = false;
			}
			else
			{
				collidedBool.Value = true;
			}
		}

//		public override string ErrorCheck()
//		{
//			return ActionHelpers.CheckOwnerPhysicsSetup(Owner);
//		}


	}
}