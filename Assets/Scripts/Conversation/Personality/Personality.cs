using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[Serializable]
public class Thought
{
    public string text = "";

    public string tangentName = "null";
    public string personalityName = "null";
    public int tangentChoiceIdx = -1;

    public bool isConceived = false;
}
[Serializable]
public class Articulation   
{
    public string text = "";

    public bool isCommenced = false;

    public int length = 0;
    public float speedFactor = 1.0f;
    public float bufferTime = 0.0f;
    public string forcedAnimId = "null";
    public bool shouldStartNewTangent = false;
    public string newTangentName = "";
}
[Serializable]
public class Turn
{
    public bool isTurnComplete = false;

    public string tangentName = "";
    public string personalityName = "";

    public bool isArticulationComplete = false;
    public string articulationText = "";
    public int articulationIndex = 0;
    public float articulationTimeRemaining = 1.0f;
    public List<Articulation> articulations = new List<Articulation>();

    public bool isThinkingTurn = false;
    public bool isThinkingComplete = false;
    public string thoughtText = "";
    public List<Thought> thoughts = new List<Thought>();
    public Thought chosenThought = null;

    public bool shouldStartNewTangent = false;
    public string newTangentName = "";
}

public class Personality : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] public PersonalityVisualizer personalityVisualizer = null;

    [Header("Data")]
    [SerializeField] public string personalityName = "";
    [SerializeField] private List<Turn> activeTurns = new List<Turn>();
    [SerializeField] private float personalityTalkingSpeed = 20.0f; // char/sec

    public void Initialize(GameObject conversationSituationObject)
    {
        CharacterVisualGenerator characterVisualGenerator = GetComponent<CharacterVisualGenerator>();

        if(characterVisualGenerator != null)
        {
            characterVisualGenerator.Initialize(conversationSituationObject);
        }

    }
    private void Update()
    {
        HandleTurns();
    }

    public List<Turn> GetActiveTurns()
    {
        return activeTurns;
    }

    public void RemoveTangentTurns(string tangentTurnName)
    {
        personalityVisualizer.thoughtVisualizer.ClearTangentThoughts(tangentTurnName);

        List<Turn> completeTangentTurns = new List<Turn>();
        foreach (Turn activeTurn in activeTurns)
        {
            if (tangentTurnName == activeTurn.tangentName)
            {
                completeTangentTurns.Add(activeTurn);
            }
        }

        foreach (Turn completeTangentTurn in completeTangentTurns)
        {
            activeTurns.Remove(completeTangentTurn);
        }

        
    }

    public string GetPersonalityName()
    {
        return personalityName;
    }

    public void AddActiveTurn(Turn nextTurn)
    {
        Debug.Log($"AddActiveTurn: personalityName_{personalityName}, nextTurn: tangentName_{nextTurn.tangentName} articulationCount_{nextTurn.articulations.Count}, articulationCount_{nextTurn.thoughts.Count} \narticulationText:\n{nextTurn.articulationText}thoughtText:\n{nextTurn.thoughtText}");
        activeTurns.Add(nextTurn);
    }

    private void HandleTurns()
    {
        bool isArticulatingATurn = false;
        foreach (Turn activeTurn in activeTurns)
        {
            if (activeTurn.isTurnComplete)
            {
                Debug.Log($"Error: HandleTurns: activeTurn_{activeTurn.articulationText} from tangent_{activeTurn.tangentName} is Complete");
                continue;
            }

            // cycle through articulation
            if (!isArticulatingATurn &&
                !activeTurn.isArticulationComplete)
            {
                Articulate(activeTurn);
                isArticulatingATurn = true;
            }

            Think(activeTurn);

            if (activeTurn.isArticulationComplete &&
                activeTurn.isThinkingComplete)
            {
                activeTurn.isTurnComplete = true;
            }
        }
    }

    private void Articulate(Turn turn)
    {
        if (turn.isArticulationComplete)
        {
            //Debug.Log($"Articulate: activeTurn_{turn.articulationText} from tangent_{turn.tangentName} is Complete");
            return;
        }
        if (turn.articulations.Count == 0)
        {
            turn.isArticulationComplete = true;
            return;
        }

        if (turn.articulationIndex >= turn.articulations.Count)
        {
            Debug.Log($"Error: Articulate: articulationIndex_{turn.articulationIndex} >= articulations.Count_{turn.articulations.Count}");
            return;
        }

        if (!turn.articulations[turn.articulationIndex].isCommenced)
        {
            Debug.Log($"Articulate: Commencing articulation_{turn.articulations[turn.articulationIndex].text}");
            if (turn.articulations[turn.articulationIndex].shouldStartNewTangent)
            {
                turn.newTangentName = turn.articulations[turn.articulationIndex].newTangentName;
                turn.shouldStartNewTangent = true;
            }
            
            if ("NULL\n" == turn.articulations[turn.articulationIndex].text)
            {
                Debug.Log($"Articulate: We Capture a NULL line");
                personalityVisualizer.articulationVisualizer.textMeshProUGUI.text = "";
                turn.articulationTimeRemaining = 0.0f;
            }
            else
            {
                personalityVisualizer.articulationVisualizer.textMeshProUGUI.text = turn.articulations[turn.articulationIndex].text;
                turn.articulationTimeRemaining = CalculateArticulationTime(turn.articulations[turn.articulationIndex]);
            }

            turn.articulations[turn.articulationIndex].isCommenced = true;
        }

        turn.articulationTimeRemaining -= Time.deltaTime;

        if (turn.articulationTimeRemaining <= 0.0f)
        {
            turn.articulationIndex++;
            if (turn.articulationIndex >= turn.articulations.Count)
            {
                turn.isArticulationComplete = true;
            }
        }
    }

    private void Think(Turn turn)
    {
        if (turn.isThinkingComplete)
        { 
            return; 
        }

        if (turn.thoughts.Count == 0 &&
            !turn.isThinkingTurn)
        {
            turn.isThinkingComplete = true;
            return;
        }

        //Debug.Log($"Think: turn.thoughtText:\n{turn.thoughtText}");
        foreach (Thought thought in turn.thoughts)
        {
            if (!thought.isConceived)
            {
                personalityVisualizer.thoughtVisualizer.AddThoughtButton(thought);
                thought.isConceived = true;
            }
        }

        Thought capturedThought = personalityVisualizer.thoughtVisualizer.CaptureSelectedThoughtFromTurn(turn); 
        if (capturedThought != null)
        {
            foreach (Turn activeTurn in activeTurns)
            {
                if (activeTurn.tangentName == capturedThought.tangentName)
                {
                    Debug.Log($"Think: capturedThought.text_{capturedThought.text} with capturedThought.tangentName_{capturedThought.tangentName}");
                    turn.chosenThought = capturedThought;
                    turn.isThinkingComplete = true;
                }
            }
        }
    }
    private float CalculateArticulationTime(Articulation articulation)
    {
        return (articulation.text.Length * articulation.speedFactor / personalityTalkingSpeed) + articulation.bufferTime;
    }

}
