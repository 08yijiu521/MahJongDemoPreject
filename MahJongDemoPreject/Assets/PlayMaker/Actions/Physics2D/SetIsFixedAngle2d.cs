﻿// (c) Copyright HutongGames, LLC 2010-2016. All rights reserved.

using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	#pragma warning disable 618
	[Obsolete("This action is obsolete; use Constraints instead.")]
	[ActionCategory(ActionCategory.Physics2D)]
	[Tooltip("Controls whether the rigidbody 2D should be prevented from rotating")]
    public class SetIsFixedAngle2d : ComponentAction<Rigidbody2D>
	{
		[RequiredField]
		[CheckForComponent(typeof(Rigidbody2D))]
		[Tooltip("The GameObject with the Rigidbody2D attached")]
		public FsmOwnerDefault gameObject;

		[RequiredField]
		[Tooltip("The flag value")]
		public FsmBool isFixedAngle;
		
		public override void Reset()
		{
			gameObject = null;
			isFixedAngle = false;
		}
		
		public override void OnEnter()
		{
			DoSetIsFixedAngle();
			Finish();
		}
		
		void DoSetIsFixedAngle()
		{
			var go = Fsm.GetOwnerDefaultTarget(gameObject);
            if (!UpdateCache(go))
            {
                return;
            }
			
			rigidbody2d.fixedAngle = isFixedAngle.Value;
		}
	}
}

