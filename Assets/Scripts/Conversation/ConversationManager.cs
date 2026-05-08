using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConversationManager : MonoBehaviour
{
    private static ConversationManager instance;
    [SerializeField] private GameObject globalSituationObject = null;

    [Header("UI")]
    [SerializeField] public GameObject conversationVisualizerPrefab = null;

    [Header("Conversations")]
    [SerializeField] private List<GameObject> activeConversationObjects;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Multiple instances of ConversationManager");
        }

        instance = this;
    }

    public static ConversationManager GetInstance()
    {
        return instance;
    }

    private void Update()
    {
        HandleConversations();


    }

    private void HandleConversations()
    {
        foreach (GameObject activeConversationObject in activeConversationObjects)
        {
            //Debug.Log($"HandleConversations: activeConversationObject_{activeConversationObject.name}");

            activeConversationObject.GetComponent<Conversation>().Handle();
        }
    }

    public void CreateConversation(List<GameObject> personalityObjects, GameObject tangentOpenerPrefab)
    {
        Debug.Log($"AddConversation: tangentOpenerPrefab_{tangentOpenerPrefab.name}");

        GameObject conversationObject = new GameObject("Conversation");
        conversationObject.transform.SetParent(this.transform);
        conversationObject.AddComponent<Conversation>().Initialize(personalityObjects, tangentOpenerPrefab);
        activeConversationObjects.Add(conversationObject);
        //GameObject conversationVisualizerObject = Instantiate(conversationVisualizerPrefab);
        //conversationVisualizerObject.transform.SetParent(this.transform);

    }

    public GameObject GetGlobalSituationObject()
    {
        return globalSituationObject;
    }
}
