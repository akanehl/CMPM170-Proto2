﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GramophoneScript : MonoBehaviour
{
    float distance1;
    float distance2;
    public bool locked1;
    public bool locked2;
    private bool ghostCondition;
    GameObject[] ghosts;
    GameObject[] gramophones;
    public GameObject closest1;
    public GameObject closest2;
    // Start is called before the first frame update
    void Start()
    {
        locked1 = true;
        locked2 = true;
        distance1 = 2;
        distance2 = 2;
    }

    // Update is called once per frame
    void Update()
    {
        ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        gramophones = GameObject.FindGameObjectsWithTag("Gramophone");

        findClosest(gramophones);
        GhostCondition(ghosts);
        //Edited BY Guanchen
        if(ghostCondition){
            distance1 = Vector3.Distance(closest1.transform.position, ghosts[0].transform.position);
            distance2 = Vector3.Distance(closest2.transform.position, ghosts[1].transform.position);
    
            if (distance1 < 1)
            {
                unlockSwitch(0);
            }
            else
            {
                lockSwitch(0);
            }
    
            if (distance2 < 1)
            {
                unlockSwitch(1);
            }
            else
            {
                lockSwitch(1);
            }
        }
    }

    void unlockSwitch(int num)
    {
        if (num == 0)
        {
            locked1 = false;
        }
        if (num == 1)
        {
            locked2 = false;
        }
            ghosts[num].transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(true);
    }

    void lockSwitch(int num)
    {
        if (num == 0)
        {
            locked1 = true;
        }
        if (num == 1)
        {
            locked2 = true;
        }
            ghosts[num].transform.GetChild(0).transform.GetChild(1).gameObject.SetActive(false);
    }

    //Edited BY Guanchen
    void GhostCondition(GameObject[] ghosts){
        if(ghosts.Length != 2){
            ghostCondition = false;
        }else{
            ghostCondition = true;
        }
    }

    //Determine closest gramophone
    void findClosest(GameObject[] objects)
    {
        closest1 = objects[0];
        closest2 = objects[0];
        for(int i =0; i<objects.Length; i++)
        {
            //for ghost 1
            if(Vector3.Distance(objects[i].transform.position, ghosts[0].transform.position) < Vector3.Distance(closest1.transform.position, ghosts[0].transform.position))
            {
                //Object in question is closer than the current closest
                closest1 = objects[i];
            }

            //for ghost 2
            if (Vector3.Distance(objects[i].transform.position, ghosts[1].transform.position) < Vector3.Distance(closest2.transform.position, ghosts[1].transform.position))
            {
                //Object in question is closer than the current closest
                closest2 = objects[i];
            }
        }
    }
}
