// (c) Copyright HutongGames, LLC 2010-2014. All rights reserved.
/*--- __ECO__ __ACTION__ ---*/

#if !( UNITY_FLASH || UNITY_PS3)

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Web")]
	[Tooltip("Gets data from a url and store it in variables, Accept Post variables. See Unity WWW docs for more details.")]
	public class WWWPOSTMobile : FsmStateAction
	{
		[RequiredField]
		[Tooltip("Url to download data from.")]
		public FsmString url;
		
		[ActionSection("POST Data")]
		
		[CompoundArray("POST", "Key", "Value")]
		public FsmString[] postKeys;
		public FsmVar[] postValues;
		
		[ActionSection("Results")]

		[UIHint(UIHint.Variable)]
		[Tooltip("Gets text from the url.")]
		public FsmString storeText;
		
		[UIHint(UIHint.Variable)]
		[Tooltip("Gets a Texture from the url.")]
		public FsmTexture storeTexture;

		[UIHint(UIHint.Variable)]
		[Tooltip("Error message if there was an error during the download.")]
		public FsmString errorString;

		[UIHint(UIHint.Variable)] 
		[Tooltip("How far the download progressed (0-1).")]
		public FsmFloat progress;

		[ActionSection("Events")] 
		
		[Tooltip("Event to send when the data has finished loading (progress = 1).")]
		public FsmEvent isDone;
		
		[Tooltip("Event to send if there was an error.")]
		public FsmEvent isError;

		private WWW wwwObject;

		public override void Reset()
		{
			url = null;
			
			postKeys = new FsmString[0];
			postValues = new FsmVar[0];
			
			storeText = null;
			storeTexture = null;
			errorString = null;
			progress = null;
			isDone = null;
		}

		public override void OnEnter()
		{
			if (string.IsNullOrEmpty(url.Value))
			{
				Finish();
				return;
			}
			
			if (postKeys.Length>0)
			{
				WWWForm _wwwForm = new WWWForm();
				int i = 0;
				
				foreach(FsmString _Fsmkey in postKeys)
				{
					string _key = _Fsmkey.Value;
					
					switch (postValues[i].Type)
					{
						case VariableType.Material:
						case VariableType.Unknown:
						case VariableType.Object:
							//not supported;
						break;
					case VariableType.Texture:
						
						Texture2D rt = (Texture2D)postValues[i].textureValue;
						
						_wwwForm.AddBinaryData(_key,rt.EncodeToPNG());
						break;
						default:
						_wwwForm.AddField(_key,postValues[i].ToString());
						break;
					}
					
					
					i++;
				}
				
				wwwObject = new WWW(url.Value,_wwwForm);
				
			}else{
				wwwObject = new WWW(url.Value);
			}
		}


		public override void OnUpdate()
		{
			if (wwwObject == null)
			{
				errorString.Value = "WWW Object is Null!";
				Finish();
				Fsm.Event(isError);
				return;
			}

			errorString.Value = wwwObject.error;

			if (!string.IsNullOrEmpty(wwwObject.error))
			{
				Finish();
				Fsm.Event(isError);
				return;
			}

			progress.Value = wwwObject.progress;

			if (progress.Value.Equals(1f))
			{
				storeText.Value = wwwObject.text;
				storeTexture.Value = wwwObject.texture;

				errorString.Value = wwwObject.error;

				Fsm.Event(string.IsNullOrEmpty(errorString.Value) ? isDone : isError);

				Finish();
			}
		}
		
		public override string ErrorCheck ()
		{
			foreach(FsmVar _Fsmvar in postValues)
			{
				switch (_Fsmvar.Type)
				{
					case VariableType.Material:
					case VariableType.Unknown:
					case VariableType.Object:
					return _Fsmvar.Type+" not supported";
				}
			}
			return "";
		}

		
	}
}

#endif