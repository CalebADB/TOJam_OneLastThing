using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Conversation : MonoBehaviour
{
    [SerializeField] public List<GameObject> personalityObjects = new List<GameObject>();
    [SerializeField] private GameObject situationObject = null;
    [SerializeField] public List<GameObject> activeTangentObjects = new List<GameObject>();

    [SerializeField] public ConversationVisualizer conversationVisualizer = null;

    public void Handle()
    {
        HandleActiveTurns();
        
        HandleNextTurns();
    }
    private void Awake()
    {
        situationObject = new GameObject();
        situationObject.AddComponent<Situation>();
        situationObject.transform.SetParent(this.transform);
    }

    public void Initialize(List<GameObject> personalityObjects, GameObject tangentOpenerPrefab)
    {
        this.personalityObjects = personalityObjects;
        
        AddTangent(tangentOpenerPrefab);
    }


    private void HandleActiveTurns()
    {
        foreach (GameObject personalityObject in personalityObjects)
        {
            Personality personality = personalityObject.GetComponent<Personality>();
            if (personality == null)
            {
                Debug.Log($"Error: HandleActiveTurns: personalityObject_{personalityObject.name} does not have a personality");
                continue;
            }

            List<Turn> activeTurns = personalityObject.GetComponent<Personality>().GetActiveTurns();
            List<Turn> completedTurns = new List<Turn>();
            foreach (Turn activeTurn in activeTurns)
            {
                if (activeTurn.isTurnComplete)
                {
                    foreach (GameObject activeTangentObject in activeTangentObjects)
                    {
                        if (activeTangentObject.GetComponent<Tangent>().name == activeTurn.tangentName)
                        {
                            activeTangentObject.GetComponent<Tangent>().ReceiveCompletedTurn(activeTurn);
                            completedTurns.Add(activeTurn);
                        }
                    }
                }
            }
            foreach (Turn completedTurn in completedTurns)
            {
                personalityObject.GetComponent<Personality>().RemoveActiveTurn(completedTurn);
            }
        }
    }

    private void HandleNextTurns()
    {
        foreach (GameObject activeTangentObject in activeTangentObjects)
        {
            Turn nextTurn = activeTangentObject.GetComponent<Tangent>().CaptureNextTurn();
            if (nextTurn == null)
            {
                // Debug.Log($"HandleNextTurns: nextTurn is null");
                continue;
            }

            bool isNextTurnHandled = false;
            foreach (GameObject personalityObject in personalityObjects)
            {
                if (personalityObject.GetComponent<Personality>().GetPersonalityName() == nextTurn.personalityName)
                {
                    personalityObject.GetComponent<Personality>().AddActiveTurn(nextTurn);
                    isNextTurnHandled = true;
                    continue;
                }
            }
            if (!isNextTurnHandled)
            {
                Debug.Log($"Error: HandleNextTurns: Next turn was not handled");
            }
        }
    }

    private void AddTangent(GameObject tangentPrefab)
    {
        GameObject tangentObject = Instantiate(tangentPrefab);
        tangentObject.transform.SetParent(this.transform);

        Tangent tangent = tangentObject.GetComponent<Tangent>();
        tangent.Initialize(situationObject);

        Situation globalSituation = ConversationManager.GetInstance().GetGlobalSituationObject().GetComponent<Situation>();
        Situation situation = situationObject.GetComponent<Situation>();
        situation.AddRelevantVibes(globalSituation.GetRelevantVibes(tangent.GetTangentSituationalData().vibeNames));
        situation.AddRelevantPresents(globalSituation.GetRelevantPresents(tangent.GetTangentSituationalData().vibeNames));

        activeTangentObjects.Add(tangentObject);
    }
}
