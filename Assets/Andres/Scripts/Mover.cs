using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

public class Mover : MonoBehaviour
{
    private Vector3 tpos;
    public GameObject target;
    private bool isMoving = false;
    public float speed = 2.28f;
    // Start is called before the first frame update
    [Button]
    public void SetMove(){
        isMoving = !isMoving;
    }
    void Start()
    {   
        tpos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving){
            this.transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, speed * Time.deltaTime);
        }
        else{
            this.transform.position = Vector3.MoveTowards(this.transform.position, tpos, speed*Time.deltaTime);
        }
    }
}
