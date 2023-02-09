using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDetails : MonoBehaviour
{
    public EnumGuestActivity function;

    [HideInInspector] public Queue<GameObject> guestQueue = new Queue<GameObject>();
    [HideInInspector] public List<GameObject> guestsInRoom = new List<GameObject>();
    [HideInInspector] public GameObject corridor;

    [SerializeField] int space;

    private void OnDestroy() {
        if(corridor!=null)
            corridor.GetComponent<CorridorDetails>().isDoor = false; 
    }

    public bool hasSpace(GameObject guest){
        if(guestsInRoom.Count < space)
            if(guestQueue.Peek() == guest){
                guestQueue.Dequeue();
                return true;
            }else{
                return false;
            }
        else
            return false;
    }

    public List<Transform> RoomSquares(){
        List<Transform> roomSquares = new List<Transform>();
        foreach(Transform child in transform){
            if(child.tag == "roomSquare"){
                roomSquares.Add(child); 
            }
        }
        return roomSquares;
    }
}
