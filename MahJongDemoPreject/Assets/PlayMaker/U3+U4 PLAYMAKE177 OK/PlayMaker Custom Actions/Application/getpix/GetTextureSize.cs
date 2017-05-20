// (c) Copyright HutongGames, LLC 2010-2011. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.Input)]
	[Tooltip("Gets the height and width in pixels.")]
	public class GetTextureSize : FsmStateAction
	{
 
		public FsmTexture tex;
		
		[Tooltip("get the normalized UV space of the point. Only possible if it the ray collides with a mesh collider. Otherwise it will return 0,0 without error or warning")]
		public FsmVector2 scale;
		
		public FsmInt width;
		public FsmInt height;
		
		public override void OnEnter()
		{
		Vector2 go;
		go.x = tex.Value.width;
		go.y = tex.Value.height;
		width.Value = tex.Value.width;
		height.Value = tex.Value.height;
		scale.Value = go;
		Finish();
		}
	}
}