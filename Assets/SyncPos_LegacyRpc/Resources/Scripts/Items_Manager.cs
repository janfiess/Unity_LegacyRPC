// script attached to the manager always availlable in scene

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items_Manager : MonoBehaviour {
	// public GameObject cube_orange, cube_green;
	// this dictionary stored all items which are currently in scene- filled in each Item's ThisItem.cs script on start function
	public static Dictionary<string, GameObject> items_dict = new Dictionary<string, GameObject>();


	void Start () {
		
	}
	
	void Update () {
	}
}
