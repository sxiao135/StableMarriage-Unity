using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steps : ScriptableObject
{
    public struct Step
    {
        public string Action;
        public string Person;
        public string Partner;
        public int round;
    }
    public int Rounds = 0;
    public int CurrentRound = 0;
    public int CurrentStep = 0;
    public List<Step>[] StepsPerRound;
    // Start is called before the first frame update
    void SetSteps(string action, string person, string partner, int i)
    {
        Step step = new Step();
        step.Action = action;
        step.Person = person;
        step.Partner = partner;
        step.round = i;
        StepsPerRound[i].Add(step);
    }
    public void init(string input){
        // Example:
        // ROUND 1:
        // p: A-3
        // p: B-5
        // p: C-4
        // p: D-1
        // u: B-5
        // p: E-5
        // ROUND 2:
        // u: D-1
        // p: B-1
        // p: D-2
        string[] lines = input.Split('\n');
        // Count how many lines include "ROUND"
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains("ROUND"))
            {
                Rounds++;
            }
        }
        StepsPerRound = new List<Step>[Rounds];
        // Iterate over lines
        int curRound = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains("ROUND"))
            {
                curRound = int.Parse(lines[i].Split(' ')[1]);
                StepsPerRound[curRound] = new List<Step>();
            }
            else
            {
                string[] parts = lines[i].Split(':');
                string[] parts2 = parts[1].Split('-');
                // Debug.Log("Parts: '" + parts2[0] + "' '" + parts2[1] + "' " + parts2[0].Length + " " + parts2[1].Length);
                // Remove last character
                parts2[1] = parts2[1].Substring(0, parts2[1].Length - 1);
                if (parts[0].Contains("p"))
                {
                    SetSteps("Propose", parts2[0], parts2[1], curRound);
                }
                else if (parts[0].Contains("u"))
                {
                    SetSteps("Unmatch", parts2[0], parts2[1], curRound);
                }
                else
                {
                    SetSteps("Chain", parts2[0], parts2[1], curRound);
                }
            }
        }
    }
    public void PrintRounds()
    {
        for (int i = 0; i < Rounds; i++)
        {
            Debug.Log("Round " + i);
            for (int j = 0; j < StepsPerRound[i].Count; j++)
            {
                Debug.Log("Action: " + StepsPerRound[i][j].Action + " Person: " + StepsPerRound[i][j].Person + " Partner: " + StepsPerRound[i][j].Partner);
            }
        }
    }
    public Step GetNextStep(){
        Step step = StepsPerRound[CurrentRound][CurrentStep];
        CurrentStep++;
        if (CurrentStep >= StepsPerRound[CurrentRound].Count)
        {
            CurrentStep = 0;
            CurrentRound++;
        }
        return step;
    }
}
