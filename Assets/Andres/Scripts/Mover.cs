using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;
using UnityEditor.Experimental.GraphView;

public class Mover : MonoBehaviour
{
    public GameObject target;
    private bool isMoving = false;
    public float speed = 2.28f;
    public Vector3 tpos;
    // Start is called before the first frame update
    [Button]
    public void SetMove(bool setTpos = true){
        isMoving = !isMoving;
        if (setTpos) tpos = target.transform.position;
    }
    void Start()
    {   
        // tpos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving){
            this.transform.position = Vector3.MoveTowards(this.transform.position, tpos, speed * Time.deltaTime);
            if(this.transform.position == tpos){
                Debug.Log("ARRIGVED");
                transform.SetPositionAndRotation(tpos, new Quaternion(0, 0, 0, 0));
                isMoving = false;
            }
        }
    }
}
