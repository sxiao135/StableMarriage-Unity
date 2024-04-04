using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;
using UnityEditor.Experimental.GraphView;
using System.Threading;

public class Mover : MonoBehaviour
{
    public GameObject target;
    private bool isMoving = false;
    private bool isChainMoving = false;
    public Renderer rend;
    private Color originalColor;
    public float speed = 3.28f;
    public Vector3 tpos;
    private AnimationManager animationManager;
    public TargetInfo[] targetInfo;
    private bool playChain;
    private Mover matchedMover;
    private int curChainIndex = 0;
    public int time = 100;
    public Animator animator;
    public void WalkAnimation(bool isWalking){
        // Set animation controller parameter to isWalking
        animator.SetBool("isWalking", isWalking);
    }
    public void SetMatchedMover(Mover mover){
        matchedMover = mover;
    } 
    // Start is called before the first frame update
    [Button]
    public void SetMove(bool setTpos = true){
        // isMoving = !isMoving;
        isMoving = true;
        WalkAnimation(true);
        if (setTpos) tpos = target.transform.position;
    }
    public void ResetMove(){
        // isMoving = !isMoving;
        isMoving = false;
        WalkAnimation(false);
        animationManager.isPlaying = false;
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
        animator = GetComponentInChildren<Animator>();
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
    public void SetChainMove(bool setTpos = true){
        // isMoving = !isMoving;
        curChainIndex = 0;
        isChainMoving = true;
        playChain = true;
        isMoving = true;
        WalkAnimation(true);
        // if (setTpos) tpos = target.transform.position;
    }
    
    // Update is called once per frame
    void Update()
    {
        if(!playChain && isMoving){ //Regular walk and unpaired mover
            this.transform.position = Vector3.MoveTowards(this.transform.position, tpos, speed * Time.deltaTime);
            if(this.transform.position == tpos){
                Debug.Log("ARRIGVED");
                transform.SetPositionAndRotation(tpos, new Quaternion(0, 0, 0, 0));
                isMoving = false;
                animationManager.isPlaying = false;
                WalkAnimation(false);
            }
        }
        if (playChain && !isChainMoving && time > 0){
            // Debug.Log("PAUSED");
            time--;
            if (time == 0){
                isChainMoving = true;
                curChainIndex++;
                time = 100;
                if(curChainIndex >= targetInfo.Length){
                    playChain = false;
                    animationManager.isPlaying = false;
                    curChainIndex = 0;
                    isMoving = false;
                    SetColor(Color.green);
                    if (matchedMover != null){
                        matchedMover.SetColor(Color.green);
                    }
                    else{
                        Debug.Log("NO MATCHED MOVER");
                    }
                    Debug.Log("CHAIN_ARRIGVED2");
                    WalkAnimation(false);
                }
            }
        }
        else if(isChainMoving && playChain){
            // Debug.Log("CHAIN MOVING");
            this.transform.position = Vector3.MoveTowards(this.transform.position, targetInfo[curChainIndex].GetTargetPosition(), speed * Time.deltaTime);
            if(this.transform.position == targetInfo[curChainIndex].GetTargetPosition()){
                Debug.Log("CHAIN_ARRIGVED");
                transform.SetPositionAndRotation(targetInfo[curChainIndex].GetTargetPosition(), new Quaternion(0, 0, 0, 0));
                isChainMoving = false;
                if(curChainIndex < targetInfo.Length){
                    time = 100;
                }
                else{
                    playChain = false;
                    animationManager.isPlaying = false;
                    curChainIndex = 0;
                    Debug.Log("CHAIN_ARRIGVED3");
                }
            }
        }
    }
    public bool getIsChainMoving(){
        return isChainMoving;
    }
}
