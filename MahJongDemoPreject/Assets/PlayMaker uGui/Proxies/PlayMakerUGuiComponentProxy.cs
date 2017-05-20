﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using System.Collections;
using HutongGames.PlayMaker;

public class PlayMakerUGuiComponentProxy : MonoBehaviour {


	#region internal variables helpers
	public bool debug = false;
	public enum ActionType {SendFsmEvent,SetFsmVariable};
	public enum PlayMakerProxyEventTarget {Owner,GameObject,BroadCastAll,FsmComponent};
	public enum PlayMakerProxyVariableTarget {Owner,GameObject,GlobalVariable,FsmComponent};

	[System.Serializable]
	public struct FsmVariableSetup
	{
		public PlayMakerProxyVariableTarget target;
		public GameObject gameObject;
		public PlayMakerFSM fsmComponent;

		public int fsmIndex;
		public int variableIndex;

		public VariableType variableType;
		public string variableName;
	}

	[System.Serializable]
	public struct FsmEventSetup
	{
		public PlayMakerProxyEventTarget target;
		public GameObject gameObject;
		public PlayMakerFSM fsmComponent;
		public string customEventName;
		public string builtInEventName;
	}

	string error;

	#endregion

	#region set up variables

	public OwnerDefaultOption UiTargetOption;
	public GameObject UiTarget;

	public ActionType action;

	// Variable target
	public FsmVariableSetup fsmVariableSetup;
	FsmFloat fsmFloatTarget;
	FsmBool fsmBoolTarget;
	FsmVector2 fsmVector2Target;
	FsmString fsmStringTarget;

	// event target
	public FsmEventSetup fsmEventSetup;
	FsmEventTarget fsmEventTarget;	

	bool WatchInputField;
	InputField inputField;
	string lastInputFieldValue;

	#endregion

	#region MonoBehavior
	void Start()
	{
		if (action == ActionType.SetFsmVariable)
		{
			SetupVariableTarget();
		}else{
			SetupEventTarget();
		}

		SetupUiListeners();


	}

	void Update()
	{
		if (WatchInputField && inputField!=null)
		{
			if ( !inputField.text.Equals(lastInputFieldValue))
			{
				lastInputFieldValue = inputField.text;
				SetFsmVariable(lastInputFieldValue);
			}
		}
	}

	#endregion

	#region Initial Setup

	void SetupEventTarget()
	{
		if (fsmEventTarget==null)
		{
			fsmEventTarget = new FsmEventTarget();
		}

		// BROADCAST
		if (fsmEventSetup.target == PlayMakerProxyEventTarget.BroadCastAll)
		{
			fsmEventTarget.target = FsmEventTarget.EventTarget.BroadcastAll;
			fsmEventTarget.excludeSelf = false;
		}

		// FSM COMPONENT
		else if ( fsmEventSetup.target == PlayMakerProxyEventTarget.FsmComponent)
		{
			fsmEventTarget.target = FsmEventTarget.EventTarget.FSMComponent;
			fsmEventTarget.fsmComponent = fsmEventSetup.fsmComponent;
		}

		// GAMEOBJECT
		else if(fsmEventSetup.target == PlayMakerProxyEventTarget.GameObject)
		{
			fsmEventTarget.target = FsmEventTarget.EventTarget.GameObject;
			fsmEventTarget.gameObject = new FsmOwnerDefault();
			fsmEventTarget.gameObject.OwnerOption = OwnerDefaultOption.SpecifyGameObject;
			fsmEventTarget.gameObject.GameObject.Value = fsmEventSetup.gameObject;
		}

		// OWNER
		else if ( fsmEventSetup.target == PlayMakerProxyEventTarget.Owner)
		{
			fsmEventTarget.ResetParameters();
			fsmEventTarget.target = FsmEventTarget.EventTarget.GameObject;
			fsmEventTarget.gameObject = new FsmOwnerDefault();
			fsmEventTarget.gameObject.OwnerOption = OwnerDefaultOption.SpecifyGameObject;
			fsmEventTarget.gameObject.GameObject.Value = this.gameObject;

		}


	}

	void SetupVariableTarget()
	{
			// GLOBAL VARIABLE
			if (fsmVariableSetup.target == PlayMakerUGuiComponentProxy.PlayMakerProxyVariableTarget.GlobalVariable)
			{
				if (fsmVariableSetup.variableType == VariableType.Bool)
				{
					fsmBoolTarget = FsmVariables.GlobalVariables.FindFsmBool(fsmVariableSetup.variableName);
					
				}else if (fsmVariableSetup.variableType == VariableType.Float)
				{
					fsmFloatTarget = FsmVariables.GlobalVariables.FindFsmFloat(fsmVariableSetup.variableName);
				}else if (fsmVariableSetup.variableType == VariableType.Vector2)
				{
					fsmVector2Target = FsmVariables.GlobalVariables.FindFsmVector2(fsmVariableSetup.variableName);

				}else if (fsmVariableSetup.variableType == VariableType.String)
				{
					fsmStringTarget = FsmVariables.GlobalVariables.FindFsmString(fsmVariableSetup.variableName);
				}

			}
			
			// FSM COMPONENT
			else if (fsmVariableSetup.target == PlayMakerUGuiComponentProxy.PlayMakerProxyVariableTarget.FsmComponent)
			{
				if (fsmVariableSetup.fsmComponent!=null)
				{
					if (fsmVariableSetup.variableType == VariableType.Bool)
					{
						fsmBoolTarget = fsmVariableSetup.fsmComponent.FsmVariables.FindFsmBool(fsmVariableSetup.variableName);
						
					}else if (fsmVariableSetup.variableType == VariableType.Float)
					{
						fsmFloatTarget = fsmVariableSetup.fsmComponent.FsmVariables.FindFsmFloat(fsmVariableSetup.variableName);

					}else if (fsmVariableSetup.variableType == VariableType.Vector2)
					{
						fsmVector2Target = fsmVariableSetup.fsmComponent.FsmVariables.FindFsmVector2(fsmVariableSetup.variableName);

					}else if (fsmVariableSetup.variableType == VariableType.String)
					{
						fsmStringTarget =fsmVariableSetup.fsmComponent.FsmVariables.FindFsmString(fsmVariableSetup.variableName);
					}
				}else{
					Debug.LogError("set to target a FsmComponent but fsmEventTarget.target is null");
				}
			}
			
			// OWNER
			else if (fsmVariableSetup.target == PlayMakerUGuiComponentProxy.PlayMakerProxyVariableTarget.Owner)
			{
				if (fsmVariableSetup.gameObject!=null)
				{
					if (fsmVariableSetup.fsmComponent!=null)
					{
						if (fsmVariableSetup.variableType == VariableType.Bool)
						{
							fsmBoolTarget = fsmVariableSetup.fsmComponent.FsmVariables.FindFsmBool(fsmVariableSetup.variableName);
							
						}else if (fsmVariableSetup.variableType == VariableType.Float)
						{
							fsmFloatTarget = fsmVariableSetup.fsmComponent.FsmVariables.FindFsmFloat(fsmVariableSetup.variableName);

						}else if (fsmVariableSetup.variableType == VariableType.Vector2)
						{
							fsmVector2Target = fsmVariableSetup.fsmComponent.FsmVariables.FindFsmVector2(fsmVariableSetup.variableName);

						}else if (fsmVariableSetup.variableType == VariableType.String)
						{
							fsmStringTarget =fsmVariableSetup.fsmComponent.FsmVariables.FindFsmString(fsmVariableSetup.variableName);
						}
					}
					
				}else{
					Debug.LogError("set to target Owbner but fsmEventTarget.target is null");
				}
			}

			// GAMEOBJECT
			else if (fsmVariableSetup.target == PlayMakerUGuiComponentProxy.PlayMakerProxyVariableTarget.GameObject)
			{
				if (fsmVariableSetup.gameObject!=null)
				{
					if (fsmVariableSetup.fsmComponent!=null)
					{
						if (fsmVariableSetup.variableType == VariableType.Bool)
						{
							fsmBoolTarget = fsmVariableSetup.fsmComponent.FsmVariables.FindFsmBool(fsmVariableSetup.variableName);
							
						}else if (fsmVariableSetup.variableType == VariableType.Float)
						{
							fsmFloatTarget = fsmVariableSetup.fsmComponent.FsmVariables.FindFsmFloat(fsmVariableSetup.variableName);

						}else if (fsmVariableSetup.variableType == VariableType.Vector2)
						{
							fsmVector2Target = fsmVariableSetup.fsmComponent.FsmVariables.FindFsmVector2(fsmVariableSetup.variableName);
						}else if (fsmVariableSetup.variableType == VariableType.String)
						{
							fsmStringTarget =fsmVariableSetup.fsmComponent.FsmVariables.FindFsmString(fsmVariableSetup.variableName);
						}
					}
					
				}else{
					Debug.LogError("set to target a Gameobject but fsmEventTarget.target is null");
				}
		}
	}

	void SetupUiListeners()
	{
		if (UiTarget.GetComponent<Button>()!=null)
		{
			UiTarget.GetComponent<Button>().onClick.AddListener(OnClick);
		}

		if (UiTarget.GetComponent<Toggle>()!=null)
		{
			UiTarget.GetComponent<Toggle>().onValueChanged.AddListener(OnValueChanged);
			// force the value because it's not fired when starting ( Unity said they may implement it)
			if (action== ActionType.SetFsmVariable)
			{
				SetFsmVariable(UiTarget.GetComponent<Toggle>().isOn);
			}
		}
		if (UiTarget.GetComponent<Slider>()!=null)
		{
			UiTarget.GetComponent<Slider>().onValueChanged.AddListener(OnValueChanged);
			
			// force the value because it's not fired when starting ( Unity said they may implement it)
			if (action== ActionType.SetFsmVariable)
			{
				SetFsmVariable(UiTarget.GetComponent<Slider>().value);
			}
		}
		if (UiTarget.GetComponent<Scrollbar>()!=null)
		{
			UiTarget.GetComponent<Scrollbar>().onValueChanged.AddListener(OnValueChanged);
			// force the value because it's not fired when starting ( Unity said they may implement it)
			if (action== ActionType.SetFsmVariable)
			{
				SetFsmVariable(UiTarget.GetComponent<Scrollbar>().value);
			}
		}
		if (UiTarget.GetComponent<ScrollRect>()!=null)
		{
			UiTarget.GetComponent<ScrollRect>().onValueChanged.AddListener(OnValueChanged);
			// force the value because it's not fired when starting ( Unity said they may implement it)
			if (action== ActionType.SetFsmVariable)
			{
				SetFsmVariable(UiTarget.GetComponent<ScrollRect>().normalizedPosition);
			}
		}
		if (UiTarget.GetComponent<InputField>()!=null)
		{
			UiTarget.GetComponent<InputField>().onEndEdit.AddListener(onEndEdit);
			if (action== ActionType.SetFsmVariable)
			{
				WatchInputField = true;
				inputField = UiTarget.GetComponent<InputField>();
				lastInputFieldValue = "";
			}
		}
		

	}
	#endregion

	#region UI Listeners


	protected void OnClick()
	{
		if (debug) Debug.Log("OnClick");

		FsmEventData _eventData = new FsmEventData();
		this.FirePlayMakerEvent(_eventData);
	}

	protected void OnValueChanged(bool value)
	{
		if (debug) Debug.Log("OnValueChanged(bool): "+value);

		if (action== ActionType.SendFsmEvent)
		{
			FsmEventData _eventData = new FsmEventData();
			_eventData.BoolData = value;
			FirePlayMakerEvent(_eventData);
		}else
		{
			SetFsmVariable(value);
		}
	}

	protected void OnValueChanged(float value)
	{
		if (debug) Debug.Log("OnValueChanged(float): "+value);

		if (action== ActionType.SendFsmEvent)
		{
			FsmEventData _eventData = new FsmEventData();
			_eventData.FloatData = value;
			FirePlayMakerEvent(_eventData);
		}else
		{
			SetFsmVariable(value);
		}
	}

	protected void OnValueChanged(Vector2 value)
	{
		if (debug) Debug.Log("OnValueChanged(vector2): "+value);
		
		if (action== ActionType.SendFsmEvent)
		{
			FsmEventData _eventData = new FsmEventData();
			_eventData.Vector2Data = value;
			FirePlayMakerEvent(_eventData);
		}else
		{
			SetFsmVariable(value);
		}
	}

	protected void onEndEdit(string value)
	{
		if (debug) Debug.Log("onEndEdit(string): "+value);

		if (action== ActionType.SendFsmEvent)
		{
			FsmEventData _eventData = new FsmEventData();
			_eventData.StringData = value;
			FirePlayMakerEvent(_eventData);
		}else
		{
			SetFsmVariable(value);
		}

	}

	#endregion

	#region tools
	/*
	void SetFsmVariable(object value)
	{
		if (debug) Debug.Log("SetFsmVariable: "+fsmVariableTarget.NamedVar.Name+" "+fsmVariableTarget.DebugString()+" = "+value);
		
		fsmVariableTarget.SetValue(value);
		
	}
*/
	void SetFsmVariable(Vector2 value)
	{
		if (fsmVector2Target!=null)
		{
			if (debug) Debug.Log("PlayMakerUGuiComponentProxy on "+this.name+": Fsm Vector2 set to "+value);
			fsmVector2Target.Value = value;
		}else{
			Debug.LogError("PlayMakerUGuiComponentProxy on "+this.name+": Fsm Vector2 MISSING !!",this.gameObject);
		}
		
	}
	void SetFsmVariable(bool value)
	{
		if (fsmBoolTarget!=null)
		{
			if (debug) Debug.Log("PlayMakerUGuiComponentProxy on "+this.name+": Fsm Bool set to "+value);
			fsmBoolTarget.Value = value;
		}else{
			Debug.LogError("PlayMakerUGuiComponentProxy on "+this.name+": Fsm Bool MISSING !!",this.gameObject);
		}

	}
	void SetFsmVariable(float value)
	{

		if (fsmFloatTarget!=null)
		{
			if (debug) Debug.Log("PlayMakerUGuiComponentProxy on "+this.name+": Fsm Float set to "+value);

			fsmFloatTarget.Value = value;
		}else{
			Debug.LogError("PlayMakerUGuiComponentProxy on "+this.name+": Fsm Float MISSING !!",this.gameObject);
		}
		
	}

	void SetFsmVariable(string value)
	{
		
		if (fsmStringTarget!=null)
		{
			if (debug) Debug.Log("PlayMakerUGuiComponentProxy on "+this.name+": Fsm String set to "+value);
			
			fsmStringTarget.Value = value;
		}else{
			Debug.LogError("PlayMakerUGuiComponentProxy on "+this.name+": Fsm String MISSING !!",this.gameObject);
		}
		
	}

	void FirePlayMakerEvent(FsmEventData eventData)
	{

		if (eventData!=null)
		{
			HutongGames.PlayMaker.Fsm.EventData = eventData;
		}

		fsmEventTarget.excludeSelf = false;

	//	_eventTarget.sendToChildren = false;

		if (PlayMakerUGuiSceneProxy.fsm == null)
		{
			Debug.LogError("Missing 'PlayMaker UGui' prefab in scene");
			return;
		}
		Fsm _fsm = 	PlayMakerUGuiSceneProxy.fsm.Fsm;

		if (debug) Debug.Log("Fire event: "+GetEventString());
		_fsm.Event(fsmEventTarget,GetEventString());

	}


	public bool DoesTargetImplementsEvent()
	{
	
		string eventName = GetEventString();

		if (fsmEventSetup.target == PlayMakerProxyEventTarget.BroadCastAll)
		{
			return FsmEvent.IsEventGlobal(eventName);
		}
		
		if (fsmEventSetup.target == PlayMakerProxyEventTarget.FsmComponent)
		{
			return PlayMakerUtils.DoesFsmImplementsEvent(fsmEventSetup.fsmComponent,eventName);
		}

		if (fsmEventSetup.target == PlayMakerProxyEventTarget.GameObject)
		{
			return PlayMakerUtils.DoesGameObjectImplementsEvent(fsmEventSetup.gameObject,eventName);
		}
		
		if (fsmEventSetup.target == PlayMakerProxyEventTarget.Owner)
		{
			return PlayMakerUtils.DoesGameObjectImplementsEvent(this.gameObject,eventName);
		}

		return false;
	}

	string GetEventString()
	{
		return string.IsNullOrEmpty(fsmEventSetup.customEventName)?fsmEventSetup.builtInEventName:fsmEventSetup.customEventName;
	}


	#endregion


}
