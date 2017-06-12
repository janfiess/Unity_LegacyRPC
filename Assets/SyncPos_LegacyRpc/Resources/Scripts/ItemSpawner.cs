// attach this script to the Manager

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    //group all interactable items to one common GameObject
    public GameObject interactableItems_container;
	public Network_Maneger networkManager;
    void Start()
    {
		networkManager = GetComponent<Network_Maneger>();
    }

    void Update()
    {

    }

	/**********************************************************************************************
	Spawn item pipeline: 
	1. (If the item is spawned using a button:) SpawnItem_button() in ItemSpawner.cs calls SpawnItemTrigger in ItemSpawner.cs
	2. SpawnItem_trigger() in ItemSpawner.cs calls SpawnItem_overNetwork_sender() in Network_Manager.cs
	3. SpawnItem_overNetwork_sender() in Network_Manager.cs calls SpawnItem_overNetwork() in Network_Manager.cs for every client using an RPC call
	4. SpawnItem_overNetwork() in Network_Manager.cs of each client calls SpawnItem() in ItemSpawner.cs
	********************************************************************************************** */
    public void SpawnItem_button(GameObject itemToSpawn_prefab)
    {
        SpawnItem_trigger(itemToSpawn_prefab, GetRandomSpawnPosition());
    }

	 public void SpawnItem_trigger(GameObject itemToSpawn_prefab, Vector3 spawnPosition)
    {
		networkManager.SpawnItem_overNetwork_sender(itemToSpawn_prefab, spawnPosition);
	}

    public void SpawnItem(GameObject itemToSpawn_prefab, Vector3 spawnPosition)
    {
        GameObject newItem = Instantiate(itemToSpawn_prefab, spawnPosition, Quaternion.identity);
        // parent all items to a central container, later it can be deleted more easily
        newItem.transform.parent = interactableItems_container.transform;
    }

    // for debug purposes: If items are spawned using a key, their position is set randomly
    Vector3 GetRandomSpawnPosition()
    {
        return new Vector3(
            Random.Range(-1.2f, 1.2f),
            0.5f,
            Random.Range(-1.2f, 1.2f)
        );
    }

}
