﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabs : Chores
{
    [SerializeField]
    protected GameObject targetPosition;

    [SerializeField]
    protected Vector3 correctRotation;
    
    void Start()
    {
        targetPosition.SetActive(false);
    }

    public override void placed()
    {
        Vector2 item = new Vector2(transform.position.x, transform.position.z);
        Vector2 target = new Vector2(targetPosition.transform.position.x, targetPosition.transform.position.z);
        if(checkItemInPos(target, item, 1)){
            transform.position = new Vector3(target.x, transform.position.y, target.y);
            StartCoroutine(ExampleCoroutine(target));
            targetPosition.gameObject.SetActive(false);
            transform.rotation = Quaternion.Euler(correctRotation);
            this.tag = "PositionedItem";
            _complete = true;
        }
    }
    IEnumerator ExampleCoroutine(Vector2 target){
        var _rigid = GetComponent<Rigidbody>();
        yield return new WaitForSeconds(2);
        transform.position = new Vector3(target.x, transform.position.y, target.y);
        _rigid.isKinematic = true;
    }
    

    bool checkItemInPos(Vector2 target, Vector2 item, float radius)
    {
        return Vector2.Distance(target, item) < radius;
    }

    public override void finishClean()
    {
        Debug.LogError("Object type is not Clean");
        return;
    }

    public override void activeChore(int ghostid)
    {
        base.activeChore(ghostid);
        _active = true;
    }

    public override void deactiveChore()
    {
        base.deactiveChore();
        targetPosition.SetActive(false);
        _active = false;
    }

    public override GameObject getTargetPosition()
    {
        return targetPosition;
    }
}
