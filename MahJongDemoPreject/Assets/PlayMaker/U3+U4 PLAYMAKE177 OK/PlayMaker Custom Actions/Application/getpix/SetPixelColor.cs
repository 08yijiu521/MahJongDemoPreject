using UnityEngine;
using System.Collections;


namespace HutongGames.PlayMaker.Actions
{
   [ActionCategory("Texture")]
   [Tooltip("Update the graph once. If you want to upgrade every frame, use dynamic obstacles")]
   public class SetPixelColor   : FsmStateAction 
   {
      [RequiredField]
      [Tooltip("The texture needs to have the Is readable flag set to true in the import settings")]
      public FsmTexture texture;    
	  
	 [RequiredField]
      public FsmInt x;
   
	 [RequiredField]
      public FsmInt y;
	  
	  [RequiredField]
	  public FsmColor color;
	  
		public override void OnEnter() {
			var texture1 = texture.Value as Texture2D;
			texture1.SetPixel(x.Value,y.Value,color.Value);
			Finish();
		} 
	}
}