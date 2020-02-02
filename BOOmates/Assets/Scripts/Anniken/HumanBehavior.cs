﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class HumanBehavior : MonoBehaviour
{
    public enum HumanState {NORMAL, CONTROL, SCARE}

    public enum selectedItem {GrabObject, CleanObject, None}

    private selectedItem currentItem;
    private HumanState currentState;

    [SerializeField]
    private float speed;
    
    GameObject grabObj;

    public Transform[] moveSpots; 
    private int randomSpot;

    private float waitTime;

    [SerializeField]
    private float startWaitTime;

    MyHumanController myControls;

    Vector2 myMove;

    private Transform _selection;

    public UnityEngine.AI.NavMeshAgent agent;

    public float ScarePoint = 0.0f;

    [SerializeField]
    private float freezeTime = 3.0f;

    void Awake()
    {
        myControls = new MyHumanController();
       // myControls.GamePlay.TakeBody.performed += context => TakeBody();
        //myControls.GamePlay.LeaveBody.performed += context => LeaveBody();

        myControls.GamePlay.Grab.performed += context => GrabObject();
        myControls.GamePlay.Grab.canceled += context => ReleaseObject();

        myControls.GamePlay.Clean.performed += context => CleanObject();

        myControls.GamePlay.MyMovement.performed += context => myMove = context.ReadValue<Vector2>();
        myControls.GamePlay.MyMovement.canceled += context => myMove = Vector2.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        randomSpot = Random.Range(0, moveSpots.Length);
        currentState = HumanState.NORMAL;
        waitTime = startWaitTime;
        currentItem = selectedItem.None;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SacreMagement();
        transform.GetChild(0).rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        if(currentState == HumanState.NORMAL)
        {
            agent.isStopped = false;
            normalBehavior();
        }
        else if (currentState == HumanState.SCARE)
        {
            if(freezeTime > 0)
            {
                freezeTime -= Time.deltaTime;
            }
            else 
            {
                //Reach to scare MAX
                //Sound: Scare voice
                freezeTime = 3;
                currentState = HumanState.NORMAL;
            }
        }
        else
        {
            if(currentItem != selectedItem.CleanObject)
            {
                agent.isStopped = true;
                movement();
            }
            if(currentItem == selectedItem.GrabObject)
            {
                if(_selection != null)
                {
                    //Sound: Grab Item Sound
                    _selection.transform.position = transform.position + transform.forward * 25 + new Vector3(0.0f, _selection.transform.localScale.y/2, 0.0f);
                }
            }
            else if(currentItem == selectedItem.CleanObject)
            {
                if(_selection != null)
                {
                    //Sound: Clean Sound around 5 seconds
                    Debug.Log("Start Clean");
                    transform.GetChild(0).gameObject.SetActive(true);
                    Transform timeBar = transform.GetChild(0).GetChild(0);
                    if(timeBar.localScale.x < 1)
                    {
                        Debug.Log("is adding " + timeBar.name);
                        timeBar.localScale += new Vector3(Time.deltaTime * 0.2f, 0.0f, 0.0f);
                    }
                    else
                    {
                        _selection.gameObject.SetActive(false);
                        Vector3 lTemp = timeBar.localScale;
                        lTemp.x = 0.0f;
                        timeBar.localScale = lTemp;
                        transform.GetChild(0).gameObject.SetActive(false);
                        _selection = null;
                        currentItem = selectedItem.None;
                    }
                }
            }
        }

    }

    //Human AI, move around when human isn't control by ghost(player)
    void normalBehavior()
    {
        // transform.position = Vector3.MoveTowards(transform.position, moveSpots[randomSpot].position, 
        //                                             speed * Time.deltaTime);

        //Sound: walking sound
        agent.SetDestination(moveSpots[randomSpot].position);
        transform.rotation = Quaternion.LookRotation(transform.forward);
        if(Vector3.Distance(transform.position, moveSpots[randomSpot].position) < 0.5f)
        {
            if(waitTime <= 0) 
            {
                int prevRandomSpot = randomSpot;
                while (prevRandomSpot == randomSpot)
                {
                    randomSpot = Random.Range(0, moveSpots.Length);
                    Debug.Log("change random spot to" + randomSpot);
                }
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
    void movement()
    {
        //Walking Sound
        Vector3 moveVec = new Vector3(myMove.x, 0.0f ,myMove.y);
        transform.Translate(moveVec * speed * Time.deltaTime, Space.World);
        if(moveVec != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(myMove.x, 0 ,myMove.y));
        }
    }

    void OnEnable()
    {
        myControls.GamePlay.Enable();
    }

    void OnDisable()
    {
        myControls.GamePlay.Disable();
    }

    //take the body/take control by player
    void TakeBody()
    {
        Debug.Log("Control");
        currentState = HumanState.CONTROL;
        
    }

    //leave the body/back to AI
    void LeaveBody()
    {
        if(currentState != HumanState.SCARE)
        {
            ScarePoint += Random.Range(10, 16);
        }
    }

    void GrabObject()
    {      
        currentItem = selectedItem.GrabObject;
        _selection = getItem("GrabObject");
    }

    void ReleaseObject()
    {
        // if(_selection != null){
        //     Sound: put down object Sound
        // }
        Debug.Log("Release Object");
        currentItem = selectedItem.None;
        _selection = null;
    }

    void CleanObject()
    {
        _selection = getItem("CleanObject");
        if(_selection != null) {
            currentItem = selectedItem.CleanObject;
            Debug.Log("Clean item is" + _selection.gameObject.name);
            Debug.Log("Clean found");
        }
    }

    Transform getItem(string tag)
    {
        Ray ray = new Ray(transform.position, transform.forward); 
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 50.0f))
        {
            Transform selection = hit.transform;
            if(selection.transform.CompareTag(tag)){
                return selection;
            }
        }
        return null;
    }

    void SacreMagement()
    {
        if(ScarePoint >= 100.0f)
        {
            currentState = HumanState.SCARE;
            ScarePoint = 0.0f;
        }
        else if(ScarePoint > 0.0f)
        {
            ScarePoint -= Time.deltaTime * 5.0f;
        }
        else
        {
            ScarePoint = 0.0f;
        }
    }
}
