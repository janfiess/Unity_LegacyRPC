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

    void Awake()
    {
        networkView = GetComponent<NetworkView>();
    }
    void Start()
    {
        if (ipAddress_server == "") ipAddress_server = "127.0.0.1";
        // StartServer();
        // StartClient();
    }

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
        Destroy(startServer_btn);
        Destroy(startClient_btn);
    }

    void StartClient()
    {
        print("[Network] Start Client!");
        Network.Connect(ipAddress_server, 3000);
        Destroy(startServer_btn);
        Destroy(startClient_btn);
    }


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


}
