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
                Debug.Log("Line: " + lines[i]);
                // Check if current line contains a separator
                if (lines[i].Contains("-"))
                {
                    isLeft = false;
                    Debug.Log("Separator found, original x position: " + initialPosition.position.x + " z position: " + currentPos.position.z);
                    // Rotate 180
                    currentPos.Rotate(0, 180, 0);
                    currentPos.position = new Vector3(initialPosition.position.x, initialPosition.position.y, initialPosition.position.z + horizontalOffset);                    
                    continue;
                }
                string[] line = lines[i].Split(':');
                string name = line[0];
                string[] preferences = line[1].Split(',');
                Debug.Log("Name: " + name);
                List<string> lPref = new List<string>();
                for (int j = 0; j < preferences.Length; j++)
                {
                    lPref.Add(preferences[j]);
                    Debug.Log("Preference: " + preferences[j]);
                }
                if(isLeft){
                    // insert person in dictionary by name
                    Person p = new Person(name, lPref, lSide, currentPos);
                    lPeopleDict.Add(name, p);
                    if (lPeopleDict[name] == null){
                        Debug.Log("Person is null");
                        lPeopleDict[name] = p;
                    }
                    else
                        lPeopleDict[name].SetPerson(name, lPref, lSide, currentPos);
                    // if (lPeopleDict[name] == null)
                    //     lPeopleDict[name] = new Person(name, lPref, lSide, currentPos);
                    // else
                    //     lPeopleDict[name].SetPerson(name, lPref, lSide, currentPos);
                }
                else{
                    Debug.Log("Adding '" + name + "' to right side");
                    rPeopleDict.Add(name, new Person(name, lPref, rSide, currentPos));
                    if (rPeopleDict[name] == null)
                        rPeopleDict[name] = new Person(name, lPref, rSide, currentPos);
                    else
                        rPeopleDict[name].SetPerson(name, lPref, rSide, currentPos);
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
            Debug.Log("Text file: " + textFile.text);
            steps.init(textFile.text);
            steps.PrintRounds();
        }
    }
    [Button]
    public void NextStep(){
        // Get next step
        Steps.Step step = steps.GetNextStep();
        if (step.Action == "Propose")
            Propose(step);
        // else if (step.Action == "Unmatch")
        //     Unmatch(step);
        // else if (step.Action == "Chain")
        //     Chain(step);
        
    }
    public void Propose(Steps.Step step){
        Debug.Log("Propose: " + step.Person + " to " + step.Partner);
        // Print dictionary keys:
        foreach (KeyValuePair<string, Person> kvp in lPeopleDict)
        {
            Debug.Log("Key = " + kvp.Key + ", Value = " + kvp.Value.name);
        }
        foreach (KeyValuePair<string, Person> kvp in rPeopleDict)
        {
            Debug.Log("Key = " + kvp.Key + ", Value = " + kvp.Value.name);
        }

        // Get person from dictionary
        Person p = lPeopleDict[step.Person];
        // Get partner from dictionary
        Person partner = rPeopleDict[step.Partner];
        // Move person to partner
        p.MoveTo(partner.personObject);
        // Move partner to person
        // partner.MoveTo(p.personObject.transform.position);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
