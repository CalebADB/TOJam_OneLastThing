
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ConversationStarter : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private GameObject icon;
    [SerializeField] private Reader Input;
    [SerializeField] private PersonalityInteractionHandler personalityInteractionHandler;


    [Header("Conversation")]
    [SerializeField] private bool shouldStartConversation = true;
    [SerializeField] public GameObject[] availableTangentPrefabs = new GameObject[] { };


    private void Start()
    {
        foreach (GameObject tangentPrefab in availableTangentPrefabs)
        {
            Tangent tangentComponent = tangentPrefab.GetComponent<Tangent>();
            if (tangentComponent == null)
            {
                Debug.Log($"ERROR: ConversationStarter.Start: gameObject_{tangentPrefab.GetComponent<Tangent>().tangentName} does not have a Tangent component");
            }
        }
        
    }

    private void Update()
    {

        GameObject validTangentObject = GetValidTangentPrefab();
        if (shouldStartConversation &&
            validTangentObject != null)
        {
            //Debug.Log($"Tanget.Update: validTangentObject_{validTangentObject.name}");
            icon.SetActive(true);
            if (Input.isInteractionPressed)
            {
                bool isConversationStarted = AttemptStartConversation();
                shouldStartConversation = !isConversationStarted;
            }
        }
        else 
        {
            //Debug.Log($"Tanget.Update: no validTangentObject");
            icon.SetActive(false);
        }
    }

    private bool AttemptStartConversation()
    {
        GameObject tangentPrefab = GetValidTangentPrefab();
        if (tangentPrefab != null)
        {
            Debug.Log($"AttemptStartConversation: Conversation started with tangentName_{tangentPrefab.GetComponent<Tangent>().tangentName}");

            List<GameObject> conversationPersonalityObjects = new List<GameObject>();

            Debug.Log($"AttemptStartConversation: Adding personalityObject_{this.gameObject.name}");
            conversationPersonalityObjects.Add(this.gameObject);

            foreach (string requiredPersonalityName in tangentPrefab.GetComponent<Tangent>().GetTangentRequirementData().requiredPersonalityNames)
            {
                foreach (GameObject localPersonalityObject in personalityInteractionHandler.GetLocalPersonalityObjects())
                {
                    if (requiredPersonalityName == localPersonalityObject.GetComponent<Personality>().GetPersonalityName())
                    {
                        Debug.Log($"AttemptStartConversation: Adding localPersonalityObject_{localPersonalityObject.name}");

                        conversationPersonalityObjects.Add(localPersonalityObject);
                    }
                }
            }
            ConversationManager.GetInstance().CreateConversation(conversationPersonalityObjects, tangentPrefab);
            return true;
        }
        Debug.Log("AttemptStartConversation: Conversation did not start");

        return false;
    }

    private GameObject GetValidTangentPrefab()
    {
        foreach (GameObject availableTangentPrefab in availableTangentPrefabs)
        {
            //Debug.Log($"GetValidTangentObject: availableTangentObject_{availableTangentPrefab.name}");
            Tangent availableTangent = availableTangentPrefab.GetComponent<Tangent>();
            if (availableTangent == null)
            {
                Debug.Log($"Error: GetValidTangentObject: availableTangent is null");
                continue;
            }

            bool isValidTangent = availableTangent.GetIsTangentValid();
            if (!isValidTangent)
            {
                //Debug.Log($"GetValidTangentObject: availableTangentObject_{availableTangentPrefab.name} is not a valid tangent");
                continue;
            }

            TangentRequirementData tangentRequirementData = availableTangent.GetTangentRequirementData();

            int presentPersonalityNameCount = 0;

            bool isLocalPersonalityRequired = false;
            Personality localPersonality = this.transform.GetComponent<Personality>();
            if (localPersonality != null)
            {
                foreach (string requiredPersonalityName in tangentRequirementData.requiredPersonalityNames)
                {
                    if (requiredPersonalityName == localPersonality.GetPersonalityName())
                    {
                        //Debug.Log($"GetValidTangentObject: Name Check1 requiredPersonalityName_{requiredPersonalityName} localPersonality.GetPersonalityName_{localPersonality.GetPersonalityName()}");
                        isLocalPersonalityRequired = true;
                        break;
                    }
                }

                if (isLocalPersonalityRequired)
                {
                    presentPersonalityNameCount++;
                }
                else
                {
                    Debug.Log($"GetValidTangentObject: Local Personality is not required for Tangent_{availableTangent.tangentName}");
                }
            }
            else
            {
                Debug.Log("Error: GetValidTangentObject: ConversationStarter does not have personality in gameobject");
            }


            foreach (string requiredPersonalityName in tangentRequirementData.requiredPersonalityNames)
            {
                foreach (GameObject localPersonalityObject in personalityInteractionHandler.GetLocalPersonalityObjects())
                {
                    if (requiredPersonalityName == localPersonalityObject.GetComponent<Personality>().GetPersonalityName())
                    {
                        presentPersonalityNameCount++;
                    }
                }
            }

            if (presentPersonalityNameCount < tangentRequirementData.requiredPersonalityNames.Length)
            {
                //Debug.Log($"GetValidTangentObject: presentPersonalityNameCount_{presentPersonalityNameCount} is LESS than requiredPersonalityNamesLength_{tangentRequirementData.requiredPersonalityNames.Length} for Tanget_{availableTangent.name}");

                continue;
            }
            else if (presentPersonalityNameCount > tangentRequirementData.requiredPersonalityNames.Length)
            {
                //Debug.Log($"Error: GetValidTangentObject: presentPersonalityNameCount_{presentPersonalityNameCount} is LARGER than requiredPersonalityNamesLength_{tangentRequirementData.requiredPersonalityNames.Length}");
            }

            //Debug.Log($"GetValidTangentObject: availableTangentObject_{availableTangentObject.name} was returned");

            return availableTangentPrefab;            
        }

        return null;
    }
}
