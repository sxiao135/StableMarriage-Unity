using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Person : MonoBehaviour
{
    public string pName;
    public bool isEngaged;
    public Person matchedPerson;
    public List<string> preferences;
    public int currentPreferenceIndex;
    public GameObject personObject;

    private bool isMoving = false;
    private GameObject target;
    private Mover mover;
    private Vector3 tpos;
    private Quaternion tRot;
    private TargetInfo[] targets;
    private int curChainIndex = 0;
    private bool isChainMoving = false;
    private bool playChain = false;
    
// Constructor
    public Person(string name, List<string> preferences)
    {
        this.pName = name;
        this.preferences = preferences;
        this.isEngaged = false;
        this.currentPreferenceIndex = 0;
    }
    // Constructor with game object
    public Person(string name, List<string> preferences, GameObject pObject, Transform tPosition)
    {
        // Set gameObject name
        this.pName = name;
        this.preferences = preferences;
        this.isEngaged = false;
        this.currentPreferenceIndex = 0;
        tpos = new Vector3(tPosition.position.x, tPosition.position.y, tPosition.position.z);
        tRot = new Quaternion(tPosition.rotation.x, tPosition.rotation.y, tPosition.rotation.z, tPosition.rotation.w);
        personObject = Instantiate(pObject, tPosition.position, tPosition.rotation);
        personObject.name = name;
        mover = personObject.AddComponent<Mover>();

    }
    // Setter for name, preferences and person object
    public void SetPerson(string name, List<string> preferences, GameObject pobject, Transform tPosition)
    {
        // this.name = name;
        this.pName = name;
        this.preferences = preferences;
        tpos = new Vector3(tPosition.position.x, tPosition.position.y, tPosition.position.z);
        tRot = new Quaternion(tPosition.rotation.x, tPosition.rotation.y, tPosition.rotation.z, tPosition.rotation.w);
        this.isEngaged = false;
        this.currentPreferenceIndex = 0;
        personObject = Instantiate(personObject, tPosition.position, tPosition.rotation);
        mover = personObject.AddComponent<Mover>();
        
    }

    // Start is called before the first frame update
    public void SetMoverSpeed(float mSpeed)
    {
        mover.speed = mSpeed;
    }
    public void MoveTo(GameObject target)
    {
        // Look at
        this.target = target;
        personObject.transform.LookAt(target.transform);
        mover.target = target;
        mover.SetMove();
        // personObject.transform.position = Vector3.MoveTowards(personObject.transform.position, target.transform.position, step);
    }
    public void MoveToChain(TargetInfo target)
    {
        // Look at
        // Debug.Log("Moving to chain + " + target.GetTarget().name + " from " + personObject.name);
        // personObject.transform.LookAt(target.GetTarget().transform.position);
        // mover.tpos = target.GetTarget().transform.position;
        // mover.targetInfo = target;
        mover.SetChainMove();
        // personObject.transform.position = Vector3.MoveTowards(personObject.transform.position, target.transform.position, step);
    }
    // MoveTo implemented as coroutine with a wait for object to stop moving. Stops waiting when isMoving is false again. Can use a yield?
    public IEnumerator MoveToCoroutine(GameObject target)
    {
        // Look at
        this.target = target;
        personObject.transform.LookAt(target.transform);
        mover.target = target;
        mover.SetMove();
        // personObject.transform.position = Vector3.MoveTowards(personObject.transform.position, target.transform.position, step);
        while(mover.GetIsMoving()){
            yield return null;
        }
    }
    public void StartChainMove(TargetInfo[] targets, Mover partner){
        // Debug.Log("Starting chain move " + targets.Length);
        // this.targets = targets;
        // isChainMoving = false;
        // playChain = true;
        // curChainIndex = 0;
        mover.targetInfo = targets;
        mover.SetMatchedMover(partner);
        mover.SetChainMove();
        // MoveToChain(targets[curChainIndex++]);

    }
    public void SetColor(Color color, bool reset = false){
        if(reset){
            mover.ResetColor();
        }
        else mover.SetColor(color);

    }
    public void Return(){
        mover.tpos = tpos;
        GameObject g = new GameObject();
        g.transform.SetPositionAndRotation(tpos, tRot);
        personObject.transform.LookAt(g.transform.position);
        mover.SetMove(false);
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log("Person update");
        if (playChain && !isChainMoving)
        {
            Debug.Log("Playing chain");
            if (curChainIndex < targets.Length)
            {
                MoveToChain(targets[curChainIndex++]);
            }
            else
            {
                curChainIndex = 0;
                playChain = false;
                mover.ResetMove();
            }
            isChainMoving = mover.getIsChainMoving();
            Debug.Log("Chain moving: " + isChainMoving);
        }

        if(mover != null){
            this.isMoving = mover.GetIsMoving();
        }
    }
    public bool IsMoving(){
        return isMoving;
    }
    public GameObject GetGameObject(){
        return personObject;
    }
}
