using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Conversation : MonoBehaviour
{
    [SerializeField] public List<GameObject> personalityObjects = new List<GameObject>();
    [SerializeField] private GameObject situationObject = null;
    [SerializeField] public List<GameObject> activeTangentObjects = new List<GameObject>();
    [SerializeField] public List<GameObject> tangentPrefabs = new List<GameObject>();
    

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

    public void Initialize(List<GameObject> personalityObjects, GameObject tangentOpenerPrefab, List<GameObject> tangentPrefabs)
    {
        this.personalityObjects = personalityObjects;

        this.tangentPrefabs = tangentPrefabs;

        AddTangent(tangentOpenerPrefab);
    }

    private void HandleActiveTurns()
    {
        List<Turn> completedTurns = new List<Turn>();

        foreach (GameObject personalityObject in personalityObjects)
        {
            Personality personality = personalityObject.GetComponent<Personality>();
            if (personality == null)
            {
                Debug.Log($"Error: HandleActiveTurns: personalityName_{personality.personalityName} does not have a personality");
                continue;
            }

            List<Turn> activeTurns = personalityObject.GetComponent<Personality>().GetActiveTurns();
            foreach (Turn activeTurn in activeTurns)
            {
                if (activeTurn.shouldStartNewTangent)
                {
                    Debug.Log("WWWWWEEEEEEEEEEEEEEEEEEEEEEEEEEEEE4");

                    foreach (GameObject tangentPrefab in tangentPrefabs)
                    {
                        Debug.Log($"WWWWWEEEEEEEEEEEEEEEEEEEEEEEEEEEEE5 tangentPrefab.name_{tangentPrefab.GetComponent<Tangent>().tangentName} activeTurn.newTangentName_{activeTurn.newTangentName}");
                        if (tangentPrefab.GetComponent<Tangent>().tangentName == activeTurn.newTangentName)
                        {
                            Debug.Log("WWWWWEEEEEEEEEEEEEEEEEEEEEEEEEEEEE6");
                            AddTangent(tangentPrefab);
                            break;
                        }
                    }

                    activeTurn.shouldStartNewTangent = false;
                }

                if (activeTurn.isTurnComplete)
                {
                    Debug.Log($"HandleActiveTurns: activeTurn.tangentName_{activeTurn.tangentName}");

                    foreach (GameObject activeTangentObject in activeTangentObjects)
                    {
                        if (activeTangentObject.GetComponent<Tangent>().tangentName == activeTurn.tangentName)
                        {
                            activeTangentObject.GetComponent<Tangent>().ReceiveCompletedTurn(activeTurn);
                            completedTurns.Add(activeTurn);
                        }
                    }
                }
            }
        }

        foreach (GameObject personalityObject in personalityObjects)
        {
            foreach (Turn completedTurn in completedTurns)
            {
                personalityObject.GetComponent<Personality>().RemoveTangentTurns(completedTurn.tangentName);
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
        foreach (GameObject activeTangentObject in activeTangentObjects)
        {
            if(activeTangentObject.GetComponent<Tangent>().tangentName == tangentPrefab.GetComponent<Tangent>().tangentName)
            {
                Debug.Log("THROWING A TANTRUM");
                return;
            }    
        }

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
