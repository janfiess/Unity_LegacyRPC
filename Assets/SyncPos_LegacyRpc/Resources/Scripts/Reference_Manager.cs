using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reference_Manager : MonoBehaviour {

	private static Reference_Manager refManagerScript;

	public Network_Maneger networkManager;
	public Items_Manager itemsManager;
	void Awake () {
		refManagerScript = this;	
	}

	public static Reference_Manager Instance{
		get{
			return refManagerScript;
		}
	}
}
