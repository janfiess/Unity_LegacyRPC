// script attached to the Manager gameobject which is always available in scene
// Network_Manager.cs establishes the connection either as client or as host.
// and receives / sends away all the RPC commands

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Network_Maneger : MonoBehaviour
{
    NetworkView networkView;
    public string ipAddress_server = "127.0.0.1";
    public GameObject startServer_btn, startClient_btn;
    Button testConnection_btn;
    public Text text_console;
    [ReadOnly] public bool isServer;

    public Dictionary<string, GameObject> currentItems = new Dictionary<string, GameObject>();
    // public int itemNumber = 0;
    // public GameObject syncedObject;
    ItemSpawner itemSpawner;

    void Awake()
    {
        networkView = GetComponent<NetworkView>();
    }
    void Start()
    {
        if (ipAddress_server == "") ipAddress_server = "127.0.0.1";
        // StartServer();
        // StartClient();

        itemSpawner = GetComponent<ItemSpawner>();
    }

    /********************************************************
     Initialize connection
     ******************************************************** */


    // executed when startServer_btn is pressed
    public void Btn_StartServer()
    {
        StartServer();
    }

    // executed when startClient_btn is pressed
    public void Btn_StartClient()
    {
        StartClient();
    }



    void StartServer()
    {
        print("[Network] Start Server!");
        Network.InitializeServer(32, 3000, false);
        isServer = true;
        if (startServer_btn != null) Destroy(startServer_btn);
        if (startClient_btn != null) Destroy(startClient_btn);
    }

    void StartClient()
    {
        print("[Network] Start Client!");
        Network.Connect(ipAddress_server, 3000);
        isServer = false;
        if (startServer_btn != null) Destroy(startServer_btn);
        if (startClient_btn != null) Destroy(startClient_btn);
    }

    /********************************************************
     Test connection
     ******************************************************** */


    // executed when testConnection_btn is pressed
    public void Btn_TestConnection()
    {
        networkView.RPC("TestConnection", RPCMode.All, null);
    }


    // Called on every client and on the host client
    [RPC]
    public void TestConnection()
    {
        print("[Network] TestConnection");
        if (text_console.text == "Works") text_console.text = "Connected";
        else if (text_console.text == "Connected") text_console.text = "Works";
        else text_console.text = "Connected";
    }


    /********************************************************
     Remove authority to transform an object over network for all connected clients (including host)
     ******************************************************** */

    // called when mouse down: First the operating client removes the authority over the object for all
    // clients, then (not here) only the operating client gets the authority.
    public void RemoveClientAuthorityForItem_sender(string itemName)
    {
        networkView.RPC("RemoveClientAuthorityForItem", RPCMode.Others, itemName);
    }

    [RPC]
    public void RemoveClientAuthorityForItem(string movingItem)
    {
        GameObject selectedItem = Items_Manager.items_dict[movingItem];
        selectedItem.GetComponent<ThisItem>().thisClient_canModify_thisItem = false;
    }

    /********************************************************
     Set authority to host 
     ******************************************************** */

    // called when release mouse: Give the authority back to the host
    public void SetAuthorityToHostOnly_sender(string itemName)
    {
        networkView.RPC("SetAuthorityToHostOnly", RPCMode.All, itemName);
    }

    [RPC]
    public void SetAuthorityToHostOnly(string movingItem)
    {
        GameObject selectedItem = Items_Manager.items_dict[movingItem];
        if (isServer == false)
        {
            selectedItem.GetComponent<ThisItem>().thisClient_canModify_thisItem = false;
        }
        if (isServer == true)
        {
            selectedItem.GetComponent<ThisItem>().thisClient_canModify_thisItem = true;
        }
    }


    /********************************************************
     Sync position over network
     ******************************************************** */

    public void NetworkMove_sender(string movingItem, Vector3 current_position)
    {
        networkView.RPC("NetworkMove", RPCMode.Others, movingItem, current_position);
    }

    [RPC]
    public void NetworkMove(string movingItem, Vector3 current_position)
    {
        GameObject selectedItem = Items_Manager.items_dict[movingItem];
        if (selectedItem != null)
        {
            // you needn´t update the position of the local player if thisClient_canModify_thisItem == true, because that´s the initiator of the movement
            if (selectedItem.GetComponent<ThisItem>().thisClient_canModify_thisItem == true)
            {
                return;
            }
            selectedItem.transform.position = current_position;
        }
    }

    /********************************************************
     Sync rotation over network
     ******************************************************** */


    public void NetworkTurn_sender(string movingItem, Vector3 current_orientation)
    {
        


        networkView.RPC("NetworkTurn", RPCMode.Others, movingItem, current_orientation);
    }

    [RPC]
    public void NetworkTurn(string movingItem, Vector3 current_orientation)
    {
        GameObject selectedItem = Items_Manager.items_dict[movingItem];
        
        if (selectedItem != null)
        {
            // you needn´t update the position of the local player if thisClient_canModify_thisItem == true, because that´s the initiator of the movement
            if (selectedItem.GetComponent<ThisItem>().thisClient_canModify_thisItem == true)
            {
                return;
            }
            selectedItem.transform.eulerAngles = current_orientation;
        }   
    }


    /********************************************************
     Set authority over an item to be moved by any client to true or false
     ******************************************************** */


    public void SetItemMoveAuthority_sender(string itemName, bool bool_authority)
    {
        networkView.RPC("SetItemMoveAuthority", RPCMode.Others, itemName, bool_authority);

    }

    [RPC]
    public void SetItemMoveAuthority(string movingItem, bool bool_authority)
    {
        GameObject selectedItem = Items_Manager.items_dict[movingItem];
        if (selectedItem != null)
        {
             selectedItem.GetComponent<ThisItem>().canBeMoved = bool_authority;
        }
    }

     /********************************************************
     Spawn items over network
     ******************************************************** */
     public void SpawnItem_overNetwork_sender(GameObject itemToSpawn_prefab, Vector3 position){
         networkView.RPC("SpawnItem_overNetwork", RPCMode.All, itemToSpawn_prefab.name, position);

     }

     [RPC]
     public void SpawnItem_overNetwork(string itemToSpawn_prefab_name, Vector3 position){
         GameObject itemToSpawn_prefab = Items_Manager.itemPrefabs_dict[itemToSpawn_prefab_name];
         itemSpawner.SpawnItem(itemToSpawn_prefab, position);
     }

}
