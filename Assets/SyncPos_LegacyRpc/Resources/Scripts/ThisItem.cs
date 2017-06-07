// attached to every interactable object
// check if the object´s position changed and transmit it to the server

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThisItem : MonoBehaviour {

	[ReadOnly] public bool thisClient_canModify_thisItem = false;
	void Start () {
		
	}
	
	void Update () {
		
	}
}
