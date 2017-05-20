﻿// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
/*--- __ECO__ __ACTION__
EcoMetaStart
{
"script dependancies":[
						"Assets/PlayMaker Custom Actions/__Internal/PlayMakerActionsUtils.cs"
					]
}
EcoMetaEnd
---*/

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Vector3)]
	[Tooltip("Adds a value to Vector3 Variable.\n Advanced features allows selection of update type.")]
	public class Vector3AddAdvanced : FsmStateAction
	{
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmVector3 vector3Variable;
		[RequiredField]
		public FsmVector3 addVector;
		public bool everyFrame;
		public PlayMakerActionsUtils.EveryFrameUpdateSelector updateType;
		public bool perSecond;
		
		public override void Reset()
		{
			vector3Variable = null;
			addVector = new FsmVector3 { UseVariable = true };
			everyFrame = false;
			updateType = PlayMakerActionsUtils.EveryFrameUpdateSelector.OnUpdate;
			perSecond = false;
		}

		public override void Awake()
		{
			if (updateType == PlayMakerActionsUtils.EveryFrameUpdateSelector.OnFixedUpdate)
			{
				Fsm.HandleFixedUpdate = true;
			}
		}

		public override void OnUpdate()
		{
			if (updateType == PlayMakerActionsUtils.EveryFrameUpdateSelector.OnUpdate)
			{
				DoVector3Add();
			}
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnLateUpdate()
		{
			if (updateType == PlayMakerActionsUtils.EveryFrameUpdateSelector.OnLateUpdate)
			{
				DoVector3Add();
			}
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		
		public override void OnFixedUpdate()
		{
			if (updateType == PlayMakerActionsUtils.EveryFrameUpdateSelector.OnFixedUpdate)
			{
				DoVector3Add();
			}
			
			if (!everyFrame)
			{
				Finish();
			}
		}
		
		void DoVector3Add()
		{
			if(perSecond)
				vector3Variable.Value = vector3Variable.Value + addVector.Value * Time.deltaTime;
			else
				vector3Variable.Value = vector3Variable.Value + addVector.Value;
			
		}
	}
}

