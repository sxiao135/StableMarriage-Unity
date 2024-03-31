using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using EasyButtons;
using TMPro;

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
    public TextMeshProUGUI text;
    public TextMeshProUGUI roundText;
    private Steps steps;
    private int currentStep = 0;
    private AnimatorController controller;
    public float speed = 5f;
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
                
                }
                else{
                    // Debug.Log("Adding '" + name + "' to right side");
                    Person p = new Person(name, lPref, rSide, currentPos);
                    rPeopleDict.Add(name, p);
                    // rPeopleDict.Add(name, new Person(name, lPref, rSide, currentPos));
                    
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
            // steps.PrintRounds();
        }
        roundText.text = "ROUNDS " + (steps.Rounds-1);
        text.text = "Ready";
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
        if(text != null) text.text = "Propose: " + step.Person + " to " + step.Partner;
        // Get person from dictionary
        Person p = lPeopleDict[step.Person];
        p.SetMoverSpeed(speed);

        // Get partner from dictionary
        Person partner = rPeopleDict[step.Partner];
        p.SetColor(Color.green);
        partner.SetColor(Color.green);
        // Move person to partner
        p.MoveTo(partner.personObject.GetComponent<TargetInfo>().GetTarget());

    }
    public void Unmatch(Steps.Step step){
        Debug.Log("Unmatch: " + step.Person);
        if(text != null) text.text = "Unmatch: " + step.Person + " from " + step.Partner;
        Person p = lPeopleDict[step.Person];
        p.SetMoverSpeed(speed);
        Person oldPartner = rPeopleDict[step.Partner];
        p.SetColor(Color.red);
        p.Return();
    }
    public void Chain(Steps.Step step){
        Debug.Log("Chain: " + step.Person + " to " + step.Partner);
        if(text != null) text.text = "Chain: " + step.Person + " to " + step.Partner;
        // Get person from dictionary
        Person p = lPeopleDict[step.Person];
        p.SetMoverSpeed(speed);
        // Get partner from dictionary
        string[] partners = step.Partner.Split('-');
        p.SetColor(Color.blue);
        TargetInfo[] targets = new TargetInfo[partners.Length];
        for (int i = 0; i < partners.Length; i++)
        {
            Debug.Log("Partner: " + partners[i] + " Length " + partners[i].Length);
            // if(i == partners.Length - 1)
            //     partners[i] = partners[i].Substring(0, partners[i].Length - 1);
            Person partner = rPeopleDict[partners[i]];
            targets[i] = partner.personObject.GetComponent<TargetInfo>();
            // Move person to partner
            // p.MoveTo(partner.personObject);
        }
        // Set person match to last partner
        p.matchedPerson = rPeopleDict[partners[partners.Length - 1]];
        p.StartChainMove(targets, p.matchedPerson.GetGameObject().GetComponent<Mover>());
        
    }
    public void TogglePlay(){
        playAutomatically = !playAutomatically;
    }
    public void SetMoverSpeed(float speed){
        this.speed = speed  * 1000;
    }
    // Update is called once per frame
    void Update()
    {
        if(playAutomatically && !isPlaying){
            roundText.text = "Round " + steps.CurrentRound;
            NextStep();
            if(steps.IsLastStep()){
                playAutomatically = false;
                NextStep();
                text.text = "Done";
            }
        }
    }
}
