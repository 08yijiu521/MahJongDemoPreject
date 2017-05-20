// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
/*--- __ECO__ __ACTION__ ---*/

using UnityEngine;
using System.Collections;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Physics)]
	[Tooltip("Order a game object to look at a target by using Torque on its axi")]
	public class TorqueLookRotation : FsmStateAction
	{
		[RequiredField]
		[CheckForComponent(typeof(Rigidbody))]
	 	[Tooltip("The object to effect.")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Variable)]
		[Tooltip("The target it should be looking at")]
		public FsmGameObject targetObject;

		[Tooltip("The multiplier of torque to apply.")]
		public FsmFloat forceMultiplier;

		[Tooltip("The multiplier of torque to apply.")]
		public FsmFloat storeAngleTo;

		public FsmFloat maxSpeed;
		
		public override void Reset()
		{
			gameObject = null;
			targetObject = null;
			forceMultiplier = new FsmFloat{ UseVariable = true};
			maxSpeed = 100f;
			storeAngleTo = new FsmFloat{ UseVariable = true};
		}
		
		public override void OnLateUpdate()
		{
			DoLookAt();
		}
		
		void DoLookAt()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
 			var target = targetObject.Value;

			Vector3 targetDelta = target.transform.position - go.transform.position;
 			float angleDiff = Vector3.Angle(go.transform.forward, targetDelta);
			storeAngleTo.Value = angleDiff;
			Vector3 cross = Vector3.Cross(go.transform.forward, targetDelta);
			cross = Vector3.ClampMagnitude(cross, maxSpeed.Value);
			go.GetComponent<Rigidbody>().AddTorque(cross * angleDiff * forceMultiplier.Value);
			
		}
	}
}