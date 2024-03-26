using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using EasyButtons;

public class AnimationManager : MonoBehaviour
{
    public string inFile;
    public string stepsFile;
    public Transform initialPosition;
    public float horizontalOffset;
    public float verticalOffset;
    // public Person[] lPeople;
    public Dictionary<string, Person> lPeopleDict = new Dictionary<string, Person>();
    public Dictionary<string, Person> rPeopleDict = new Dictionary<string, Person>();
    // public Person[] rPeople;
    public GameObject lSide;
    public GameObject rSide;
    public bool playAutomatically = false;
    public bool isPlaying = false;
    private Steps steps;
    private AnimatorController controller;
    // Start is called before the first frame update
    void Awake()
    {
        if (inFile != null)
        {
            TextAsset textFile = Resources.Load<TextAsset>(inFile);
            if (textFile == null) return;
            // Get line count
            string[] lines = textFile.text.Split('\n');
            // lPeople = new Person[lines.Length];
            // rPeople = new Person[lines.Length];
            bool isLeft = true;
            Transform currentPos = new GameObject().transform;
            currentPos.SetPositionAndRotation(initialPosition.position, initialPosition.rotation);
            // Line format "A: 3,2,4,1,5" where A is the person and 3,2,4,1,5 are the preferences
            for (int i = 0; i < lines.Length; i++)
            {
                // Check if current line contains a separator
                if (lines[i].Contains("-"))
                {
                    isLeft = false;
                    // Rotate 180
                    currentPos.Rotate(0, 180, 0);
                    currentPos.position = new Vector3(initialPosition.position.x, initialPosition.position.y, initialPosition.position.z + horizontalOffset);                    
                    continue;
                }
                string[] line = lines[i].Split(':');
                string name = line[0];
                string[] preferences = line[1].Split(',');
                List<string> lPref = new List<string>();
                for (int j = 0; j < preferences.Length; j++)
                {
                    lPref.Add(preferences[j]);
                }
                if(isLeft){
                    // insert person in dictionary by name
                    Person p = new Person(name, lPref, lSide, currentPos);
                    lPeopleDict.Add(name, p);
                    // if (lPeopleDict[name] == null){
                    //     Debug.Log("Person is null");
                    //     lPeopleDict[name] = p;
                    // }
                    // else
                    //     lPeopleDict[name].SetPerson(name, lPref, lSide, currentPos);
                    // if (lPeopleDict[name] == null)
                    //     lPeopleDict[name] = new Person(name, lPref, lSide, currentPos);
                    // else
                    //     lPeopleDict[name].SetPerson(name, lPref, lSide, currentPos);
                }
                else{
                    Debug.Log("Adding '" + name + "' to right side");
                    rPeopleDict.Add(name, new Person(name, lPref, rSide, currentPos));
                    // if (rPeopleDict[name] == null)
                    //     rPeopleDict[name] = new Person(name, lPref, rSide, currentPos);
                    // else
                    //     rPeopleDict[name].SetPerson(name, lPref, rSide, currentPos);
                    // if (rPeopleDict[name] == null)
                    //     rPeopleDict[name] = new Person(name, lPref, rSide, currentPos);
                    // else
                    //     rPeopleDict[name].SetPerson(name, lPref, rSide, currentPos);
                }
                currentPos.position = new Vector3(currentPos.position.x + verticalOffset, currentPos.position.y, currentPos.position.z);
            }
        }
        if (stepsFile != null){
            TextAsset textFile = Resources.Load<TextAsset>(stepsFile);
            if (textFile == null) return;
            if(steps == null)
                steps = ScriptableObject.CreateInstance<Steps>();
            steps.init(textFile.text);
            steps.PrintRounds();
        }
    }
    [Button]
    public void NextStep(){
        // Get next step
        isPlaying = true;
        Steps.Step step = steps.GetNextStep();
        if (step.Action == "Propose")
            Propose(step);
        else if (step.Action == "Unmatch")
            Unmatch(step);
        else if (step.Action == "Chain")
            Chain(step);
    }
    public void Propose(Steps.Step step){
        Debug.Log("Propose: " + step.Person + " to " + step.Partner);
        // Get person from dictionary
        Person p = lPeopleDict[step.Person];
        // Get partner from dictionary
        Person partner = rPeopleDict[step.Partner];
        p.SetColor(Color.green);
        partner.SetColor(Color.green);
        // Move person to partner
        p.MoveTo(partner.personObject);

    }
    public void Unmatch(Steps.Step step){
        Debug.Log("Unmatch: " + step.Person);
        Person p = lPeopleDict[step.Person];
        Person oldPartner = rPeopleDict[step.Partner];
        p.SetColor(Color.red, true);
        p.SetColor(Color.red, true);
        p.Return();
    }
    public void Chain(Steps.Step step){
        Debug.Log("Chain: " + step.Person + " to " + step.Partner);
        // Get person from dictionary
        Person p = lPeopleDict[step.Person];
        // Get partner from dictionary
        string[] partners = step.Partner.Split('-');
        for (int i = 0; i < partners.Length; i++)
        {
            Debug.Log("Partner: " + partners[i] + " Length " + partners[i].Length);
            if(i == partners.Length - 1)
                partners[i] = partners[i].Substring(0, partners[i].Length - 1);
            Person partner = rPeopleDict[partners[i]];
            // Move person to partner
            // p.MoveTo(partner.personObject);
            StartCoroutine(p.MoveToCoroutine(partner.personObject));
            while(p.IsMoving()){
                // continue;
                Debug.Log("Moving");
            }
        }
        // foreach (string curPartner in )
        // {
        //     // trim last character
        //     Person curPartnerObj = rPeopleDict[curPartner]; 
        // // Calls coroutine to move person to partner
        //     StartCoroutine(p.MoveToCoroutine(curPartnerObj.personObject));
        // }
        
    }
    // Update is called once per frame
    void Update()
    {
        if(playAutomatically && !isPlaying){
            NextStep();
        }
    }
}
