using System.Collections;
using System.Collections.Generic;
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
        personObject = Instantiate(pObject, tPosition.position, tPosition.rotation);
        mover = personObject.AddComponent<Mover>();
        // personObject.AddComponent<Person>();
        // personObject.GetComponent<Person>().pName = name;
        // personObject.GetComponent<Person>().preferences = preferences;
        // personObject.GetComponent<Person>().isEngaged = false;
        // personObject.GetComponent<Person>().currentPreferenceIndex = 0;
        // personObject.GetComponent<Person>().personObject = personObject;

    }
    // Setter for name, preferences and person object
    public void SetPerson(string name, List<string> preferences, GameObject pobject, Transform tPosition)
    {
        // this.name = name;
        this.pName = name;
        this.preferences = preferences;
        this.isEngaged = false;
        this.currentPreferenceIndex = 0;
        personObject = Instantiate(personObject, tPosition.position, tPosition.rotation);
        mover = personObject.AddComponent<Mover>();
        // personObject.AddComponent<Person>();
        // personObject.GetComponent<Person>().pName = name;
        // personObject.GetComponent<Person>().preferences = preferences;
        // personObject.GetComponent<Person>().isEngaged = false;
        // personObject.GetComponent<Person>().personObject = personObject;
        // personObject.GetComponent<Person>().currentPreferenceIndex = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
    // Update is called once per frame
    void Update()
    {
     
    }
}
