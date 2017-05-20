using UnityEngine;
using System.Collections;
using HighlightingSystem;

public class HighlighterOccluder : MonoBehaviour
{
	public bool seeThrough = false;
	private bool _seeThrough = false;

	private Highlighter h;

	// 
	void Awake()
	{
		h = GetComponent<Highlighter>();
		if (h == null) { h = gameObject.AddComponent<Highlighter>(); }
	}

	// 
	void OnEnable()
	{
		h.OccluderOn();
		if (seeThrough) { h.SeeThroughOn(); }
		else { h.SeeThroughOff(); }
	}

	// 
	void Update()
	{
		if (_seeThrough != seeThrough)
		{
			_seeThrough = seeThrough;
			if (_seeThrough) { h.SeeThroughOn(); }
			else { h.SeeThroughOff(); }
		}
	}
}
