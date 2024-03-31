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
    private Renderer rend;
    private Color originalColor;
    public float speed = 300.28f;
    public Vector3 tpos;
    private AnimationManager animationManager;
    public TargetInfo[] targetInfo;
    private bool playChain;
    private int curChainIndex = 0;
    public int time = 100;
    // Start is called before the first frame update
    [Button]
    public void SetMove(bool setTpos = true){
        // isMoving = !isMoving;
        isMoving = true;
        if (setTpos) tpos = target.transform.position;
    }
    public void ResetMove(){
        // isMoving = !isMoving;
        isMoving = false;
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
        // if (setTpos) tpos = target.transform.position;
    }
    
    // Update is called once per frame
    void Update()
    {
        if(!playChain && isMoving){
            this.transform.position = Vector3.MoveTowards(this.transform.position, tpos, speed * Time.deltaTime);
            if(this.transform.position == tpos){
                Debug.Log("ARRIGVED");
                transform.SetPositionAndRotation(tpos, new Quaternion(0, 0, 0, 0));
                isMoving = false;
                animationManager.isPlaying = false;
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
                }
            }
        }
    }
    public bool getIsChainMoving(){
        return isChainMoving;
    }
}
