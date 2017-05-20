//	(c) Jean Fabre, 2011-2013 All rights reserved.
//	http://www.fabrejean.net
//  contact: http://www.fabrejean.net/contact.htm
//
// Version Alpha 0.92

// INSTRUCTIONS
// Drop a PlayMakerArrayList script onto a GameObject, and define a unique name for reference if several PlayMakerArrayList coexists on that GameObject.
// In this Action interface, link that GameObject in "arrayListObject" and input the reference name if defined. 
// Note: You can directly reference that GameObject or store it in an Fsm variable or global Fsm variable

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Easy Save 2")]
	[Tooltip("Saves a PlayMaker HashTable Proxy component")]
	public class HashTableEasySave : HashTableActions
	{
		
		[ActionSection("Set up")]
		
		[RequiredField]
		[Tooltip("The Game Object to add the Hashtable Component to.")]
		public FsmOwnerDefault gameObject;
		
		[Tooltip("Author defined Reference of the PlayMaker arrayList proxy component ( necessary if several component coexists on the same GameObject")]
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

			if ( SetUpHashTableProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject),reference.Value) )
				SaveHashTable();
			
			Finish();
		}
		
		
		public void SaveHashTable()
		{
			if (! isProxyValid() ) 
				return;
			
			string _tag = uniqueTag.Value;
			if (string.IsNullOrEmpty(_tag))
			{
				_tag = Fsm.GameObjectName+"/"+Fsm.Name+"/hashTable/"+reference;
			}

						
			Dictionary<string,string> _dict =	new Dictionary<string,string>();
			
			
			foreach(object key in proxy.hashTable.Keys)
			{		
				_dict[(string)key] = PlayMakerUtils.ParseValueToString(proxy.hashTable[key]);
			}
			

			ES2.Save(_dict, saveFile.Value+"?tag="+_tag);
			
			
			Log("Saved to "+saveFile.Value+"?tag="+_tag);
			
			Finish();
		}
		
		
	}
}