// this script is attached to the Manager gameobject which is always available in scene

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRemover : MonoBehaviour
{
    Network_Maneger networkManager;

    void Start()
    {
        networkManager = GetComponent<Network_Maneger>();
    }

	/**********************************************************************************************
	Remove item pipeline: 
	1. (If the item is removed using right mouse click:) In the update function of ItemRemover.cs RemoveItem_trigger() in ItemSpawner.cs is called
	2. RemoveItem_trigger() in ItemRemover.cs calls RemoveItem_overNetwork_sender() in Network_Manager.cs
	3. RemoveItem_overNetwork_sender() in Network_Manager.cs calls RemoveItem_overNetwork() in Network_Manager.cs for every client using an RPC call
	4. RemoveItem_overNetwork() in Network_Manager.cs of each client calls RemoveItem() in ItemSpawner.cs
	********************************************************************************************** */

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
			print("mouse right");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitGameObject = hit.collider.gameObject;
                if (hitGameObject.tag == "Item")
                {
					print("Remove " + hitGameObject.name);
                    RemoveItem_trigger(hit.collider.gameObject);
                }

            }
        }
    }

    public void RemoveItem_trigger(GameObject hitGameObject)
    {
        networkManager.RemoveItem_overNetwork_sender(hitGameObject);
    }

    // called from Network_Manager.RemoveItem_overNetwork()
    public void RemoveItem(GameObject itemToRemove)
    {
        // remove from items dictionary
        Items_Manager.items_dict.Remove(itemToRemove.name);
        // remove item from scene
        Destroy(itemToRemove);
    }
}
