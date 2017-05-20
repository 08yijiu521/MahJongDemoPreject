//	(c) Jean Fabre, 2011-2013 All rights reserved.
//	http://www.fabrejean.net
//  contact: http://www.fabrejean.net/contact.htm
//
// Version Alpha 0.92

// INSTRUCTIONS
// Drop a PlayMakerArrayList script onto a GameObject, and define a unique name for reference if several PlayMakerArrayList coexists on that GameObject.
// In this Action interface, link that GameObject in "arrayListObject" and input the reference name if defined. 
// Note: You can directly reference that GameObject or store it in an Fsm variable or global Fsm variable

using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Easy Save 2")]
	[Tooltip("Saves a PlayMaker Array List Proxy component")]
	public class ArrayListEasySave : ArrayListActions
	{
		
		[ActionSection("Set up")]
		
		[RequiredField]
		[Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
		[CheckForComponent(typeof(PlayMakerArrayListProxy))]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component (necessary if several component coexists on the same GameObject)")]
		[UIHint(UIHint.FsmString)]
		public FsmString reference;
		
		[ActionSection("Easy Save Set Up")]
		
		[Tooltip("A unique tag for this save. For example, the object's name if no other objects use the same name.")]
		public FsmString uniqueTag = "";
		
		[RequiredField]
		[Tooltip("The name of the file that we'll create to store our data.")]
		public FsmString saveFile = "defaultES2File.txt";
		
		public override void Reset()
		{
			gameObject = null;
			reference = null;
			
			uniqueTag = new FsmString(){UseVariable=true};
			saveFile = "defaultES2File.txt";
		}
		
		public override void OnEnter()
		{

			if ( SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject),reference.Value) )
				SaveArrayList();
			
			Finish();
		}
		
		public void SaveArrayList()
		{
			if (! isProxyValid() ) 
				return;
			
			string _tag = uniqueTag.Value;
			if (string.IsNullOrEmpty(_tag))
			{
				_tag = Fsm.GameObjectName+"/"+Fsm.Name+"/arraylist/"+reference;
			}
			
			List<string> _list = new List<string>();

			foreach(object item in proxy.arrayList)
			{
				_list.Add(PlayMakerUtils.ParseValueToString(item));
			}
			
			ES2.Save<string>(_list, saveFile.Value+"?tag="+uniqueTag);
			
			Log("Saved to "+saveFile.Value+"?tag="+uniqueTag);
			Finish();
		}
	
	}
}