using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;
using UnityEditor.Experimental.GraphView;

public class Mover : MonoBehaviour
{
    public GameObject target;
    private bool isMoving = false;
    private Renderer rend;
    private Color originalColor;
    public float speed = 2.28f;
    public Vector3 tpos;
    private AnimationManager animationManager;
    // Start is called before the first frame update
    [Button]
    public void SetMove(bool setTpos = true){
        // isMoving = !isMoving;
        isMoving = true;
        if (setTpos) tpos = target.transform.position;
    }
    void Start()
    {   
        // tpos = this.transform.position;
        // find animation manager in scene
        animationManager = GameObject.FindObjectOfType<AnimationManager>();
        rend = GetComponent<Renderer>();
        // if rend is null, find in children
        if(rend == null){
            rend = GetComponentInChildren<Renderer>();
        }
        originalColor = rend.material.color;
    }
    public void SetColor(Color color){
        rend.material.color = color;
    }
    public void ResetColor(){
        rend.material.color = originalColor;
    }
    public bool GetIsMoving(){
        return isMoving;
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
                animationManager.isPlaying = false;
            }
        }
    }
}
