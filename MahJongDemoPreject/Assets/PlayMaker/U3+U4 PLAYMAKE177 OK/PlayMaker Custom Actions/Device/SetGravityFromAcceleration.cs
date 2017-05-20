// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
/*--- __ECO__ __ACTION__ ---*/

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Device)]
	[Tooltip("Adjust the gravity direction based on the device acceleration. Typically to control rolling balls")]
	public class SetGravityfromAcceleration : FsmStateAction
	{
		

		public FsmFloat multiplier;
	
		
		public override void Reset()
		{
			multiplier = 10f;
		}
		
		public override void OnEnter()
		{

			SetGravity();
		}
		

		public override void OnUpdate()
		{
			SetGravity();
		}
		
		
		void SetGravity()
		{
			float _mult = multiplier.Value;
			
			if (Screen.orientation == ScreenOrientation.LandscapeLeft) {;

                     Physics.gravity = new Vector3 (-Input.acceleration.y * _mult , Input.acceleration.x * _mult , -Input.acceleration.z * _mult);

              }  else {

                     Physics.gravity = new Vector3 (Input.acceleration.y * _mult , -Input.acceleration.x * _mult , -Input.acceleration.z * _mult);

              }
			
		}
		
	}
}