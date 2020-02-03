﻿//Add by Guanchen Raymond Liu
//Version 1.0, Sprint2
//This script would relate to the cameraControl.cs and help move the camera
//depending on the character's position;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManagement : MonoBehaviour
{

	private    GameObject      player1;
	private    GameObject      player2;
	private    GameObject      mainCamera;
	public     GameObject      doorWay;
	private    int             cameraCondition;
	private    CameraControl   sc;
    private    GameObject      Nathan;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera  = GameObject.Find("MainCamera");
        player1     = GameObject.Find("Ghost_1");
        player2     = GameObject.Find("Ghost_2");
        Nathan      = GameObject.Find("Nathan");
        sc          = mainCamera.GetComponent<CameraControl>();
        //Debug.Log(Nathan.transform.position);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Condition: Game Object collision
    //This function will activate when a gameobject collide with the doorway
    void OnTriggerEnter(Collider other){

        //Condition:other.name
        //If the trigger's object is the human, camera
        //will move to the next room.
    	if(other.gameObject.name == "Nathan"){
            Debug.Log("Trigger");
    		var parentRoom        = doorWay.transform.parent.gameObject;
    		var TargetRoom        = parentRoom.transform.Find("cameraPos").gameObject.transform.position;
            var TargetRotation    = parentRoom.transform.Find("cameraPos").gameObject.transform.rotation;
    		var RespawnPoint      = parentRoom.transform.Find("RespawnPoint").gameObject.transform.position;
    		sc.TargetRoom         = TargetRoom;
            sc.TargetRotation     = TargetRotation;

    		if(!Equal(TargetRoom,sc.transform.position)){
    			sc.RespawnPoint            = parentRoom.transform.position;
    			sc.isMoving                = true;
    			player1.transform.position = RespawnPoint;
      		    player2.transform.position = RespawnPoint;
    		}
    	}
        //Condition:other.name
        //If the trigger's object is the Ghost, send
        //the ghost back to the respawn point.
        else if(other.gameObject.name == "Ghost_2"){

            var parentRoom        = doorWay.transform.parent.gameObject;
            var RespawnPoint      = parentRoom.transform.Find("RespawnPoint").gameObject.transform.position;
            player2.transform.position = RespawnPoint;

            
        }else if(other.gameObject.name == "Ghost_1"){
            var parentRoom        = doorWay.transform.parent.gameObject;
            var RespawnPoint      = parentRoom.transform.Find("RespawnPoint").gameObject.transform.position;
            player1.transform.position = RespawnPoint;
        }
    }


    //Condition:Vector3, Vector3
    //This function will compare 2 vector3 together
    //with rounded them into int
    bool Equal(Vector3 A, Vector3 B){
    	int aX = (int)A.x;
		int aY = (int)A.y;
		int aZ = (int)A.z;
		int bX = (int)B.x;
		int bY = (int)B.y;
		int bZ = (int)B.z;
    	return (aX == bX) && (aY == bY) && (aZ == bZ);
    }
}