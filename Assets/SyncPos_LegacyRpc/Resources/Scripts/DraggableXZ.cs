// Attached to the interactable object
// Change the position on mouse drag

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DraggableXZ : MonoBehaviour{

	GameObject hitGameObject;
	public GameObject networkManager;
	[ReadOnly] public bool isDragging = false;

	void Start(){
	}


	void FixedUpdate(){
		if (Input.GetMouseButtonDown (0)) {
			Vector2 mPosScreen = Input.mousePosition;
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (mPosScreen); //


			if (Physics.Raycast (ray, out hit)) { 
				if (hit.collider.gameObject == this.gameObject) {

					// set canModify == false on every client except the active one 
					// networkManager.GetComponent<NetworkView>().RPC ("CanNotModify", RPCMode.Others, this.gameObject.name); // 20170215

					hitGameObject = hit.collider.gameObject;
					isDragging = true;

					GetComponent<ThisItem> ().thisClient_canModify_thisItem = true;
				}			
			}
		}

		if (Input.GetMouseButton(0)){
			if (isDragging == true) {
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				if (Physics.Raycast (ray, out hit) ) { 

					
					Vector3 newPos = new Vector3 (hit.point.x, this.transform.position.y, hit.point.z);
					hitGameObject.transform.position = newPos;
					
				}
			}
		}
		if (Input.GetMouseButtonUp (0)) {
			if (isDragging == true) {
				isDragging = false;
				// networkManager.GetComponent<NetworkView>().RPC ("OnlyHostCanModify", RPCMode.Others, this.gameObject.name); // 20170215
			}
		}
	}
}

