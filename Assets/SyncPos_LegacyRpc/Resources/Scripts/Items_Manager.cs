// script attached to the manager always availlable in scene

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items_Manager : MonoBehaviour {
	// public GameObject cube_orange, cube_green;
	// this dictionary stores all items which are currently in scene- filled in each Item's ThisItem.cs script on start function
	public static Dictionary<string, GameObject> items_dict = new Dictionary<string, GameObject>();

	// this dictionary stores all item prefabs that can be instantiated
	public static Dictionary<string, GameObject> itemPrefabs_dict = new Dictionary<string, GameObject>();
	
	// Prefabs to be added to itemPrefabs_dict
	public GameObject orange_prefab, green_prefab; 

	// every instance of newly instantiated items gets an incrementing number
	[HideInInspector] public int itemNumber = 0; 

	void Start () {
		// store the prefabs in the dictionary itemPrefabs_dict
		itemPrefabs_dict.Add(orange_prefab.name,orange_prefab);
		itemPrefabs_dict.Add(green_prefab.name,green_prefab);
	}
}
