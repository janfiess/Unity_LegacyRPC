// attached to every interactable object
// check if the object´s position changed and transmit it to the server

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThisItem : MonoBehaviour
{

    [ReadOnly] public bool thisClient_canModify_thisItem = false;
    [ReadOnly] public bool canBeMoved = true, canBeRotated = true;
    public Text text_isMovable;
    Network_Maneger networkManager;
    Vector3 current_position, previous_position;
    Vector3 current_orientation, previous_orientation;
    GameObject thatItem = null;
    void Start()
    {
        networkManager = Reference_Manager.Instance.networkManager;
        Items_Manager.items_dict.Add(gameObject.name, gameObject);
        print("Added to items_dict: " + gameObject.name);
    }

    void Update()
    {

        /********************************************************
		Detect position change
		******************************************************** */


        current_position = transform.position;
        if (current_position != previous_position)
        {
            networkManager.NetworkMove_sender(gameObject.name, current_position);
            previous_position = current_position;
        }

        /********************************************************
		Detect orientation change
		******************************************************** */

        current_orientation = transform.eulerAngles;
        if (current_orientation != previous_orientation)
        {
            networkManager.NetworkTurn_sender(gameObject.name, current_orientation);    // you cannot transfer Quaternions via RPC
            previous_orientation = current_orientation;
        }





        /********************************************************
		Print debug text
		******************************************************** */

        if (canBeMoved == true)
        {
            if (text_isMovable.text != "Item movable")
                text_isMovable.text = "Item movable";
        }

        if (canBeMoved == false)
        {
            if (text_isMovable.text != "Item   NOT   movable")
                text_isMovable.text = "Item   NOT   movable";
        }
    }


    /********************************************************
    Prevent stuttering when this item moves another interactable item
    ******************************************************** */

    void OnCollisionEnter(Collision col)
    {
        // prevent stuttering on network
        if (col.gameObject.tag == "Item" && this.GetComponent<DraggableXZ>().isBeingDragged == true)
        {
            if (col.gameObject.GetComponent<ThisItem>().thisClient_canModify_thisItem != null)
            {
                //              print ("Success: Collision of " + this.gameObject.name + " with " + col.gameObject.name + ". Component Item + attribute canmodify exists");
                thatItem = col.gameObject;
                networkManager.RemoveClientAuthorityForItem_sender(thatItem.name);
                thatItem.GetComponent<ThisItem>().thisClient_canModify_thisItem = true;

                networkManager.SetItemMoveAuthority_sender(thatItem.name, false);
            }
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject == thatItem)
        {
            networkManager.SetItemMoveAuthority_sender(thatItem.name, true); // cange bool thisItem.canBeMoved
            networkManager.SetAuthorityToHostOnly_sender(thatItem.name); // change bool thisItem.thisClient_canModify_thisItem
        }
    }
}
