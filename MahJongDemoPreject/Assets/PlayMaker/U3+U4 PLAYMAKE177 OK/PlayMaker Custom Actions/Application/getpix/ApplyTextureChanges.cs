using UnityEngine;
using System.Collections;


namespace HutongGames.PlayMaker.Actions
{
   [ActionCategory("Texture")]
   [Tooltip("This is a potentially expensive operation, so you'll want to change as many pixels as possible between Apply calls. ")]
   public class ApplyTextureStranges   : FsmStateAction 
   {
      [RequiredField]
      [Tooltip("The texture needs to have the Is readable flag set to true in the import settings")]
      public FsmTexture texture;    
	  
	  
		public override void OnEnter() {
			var texture1 = texture.Value as Texture2D;
			texture1.Apply();
			Finish();
		} 
	}
}