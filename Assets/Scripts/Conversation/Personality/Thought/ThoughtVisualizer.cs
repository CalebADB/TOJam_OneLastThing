using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThoughtVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject thoughtButtonPrefab = null;
    [SerializeField] private List<GameObject> thoughtButtonObjects = new List<GameObject>();
    [SerializeField] private int maxDisplayedThoughButtons = 5;

    private void Update()
    {
        OrganizeThoughtButtons();
    }

    private void OrganizeThoughtButtons()
    {
        float anchorVerticalBuffer = 5.0f;
        Vector3 anchorPosition = Vector3.zero;
        for (int idx = 0; idx < thoughtButtonObjects.Count; idx++)
        {
            if (idx >= maxDisplayedThoughButtons)
            {
                break;
            }

            RectTransform rectTransform = thoughtButtonObjects[idx].GetComponent<RectTransform>();
            anchorPosition.y -= anchorVerticalBuffer;
            rectTransform.anchoredPosition = anchorPosition;
            anchorPosition.y -= rectTransform.rect.height;
            anchorPosition.y -= anchorVerticalBuffer;
        }
    }

    public void AddThoughtButton(Thought thought)
    {
        if (thoughtButtonPrefab == null)
        {
            Debug.Log($"Error: AddThought: ThoughtVisualizer_{this.name} thoughtButtonPrefab is null");
            return;
        }

        GameObject thoughtButtonObject = Instantiate(thoughtButtonPrefab);
        thoughtButtonObject.transform.parent = this.transform;

        thoughtButtonObject.GetComponent<ThoughtButton>().Initialize(thought);
        
        thoughtButtonObjects.Add(thoughtButtonObject);
    }

    public Thought CaptureSelectedThought(Turn turn)
    {
        GameObject selectedThoughtButtonObject = null;
        foreach (GameObject thoughtButtonObject in thoughtButtonObjects)
        {
            if (!thoughtButtonObject.GetComponent<ThoughtButton>().GetIsSelected())
            {
                continue;
            }
            selectedThoughtButtonObject = thoughtButtonObject;
            break;
        }

        if (selectedThoughtButtonObject == null)
        {
            return null;
        }

        thoughtButtonObjects.Remove(selectedThoughtButtonObject);

        Thought selectedThought = selectedThoughtButtonObject.GetComponent<ThoughtButton>().GetThought();

        List<GameObject> unselectedThoughtButtonObjects = new List<GameObject>();
        foreach (GameObject thoughtButtonObject in thoughtButtonObjects)
        {
            Thought thought = thoughtButtonObject.GetComponent<ThoughtButton>().GetThought();

            if(thought.tangentName == selectedThought.tangentName)
            {
                unselectedThoughtButtonObjects.Add(thoughtButtonObject);
            }
        }

        Destroy(selectedThoughtButtonObject);
        foreach (GameObject unselectedThoughtButtonObject in unselectedThoughtButtonObjects)
        {
            thoughtButtonObjects.Remove(unselectedThoughtButtonObject);
            Destroy(unselectedThoughtButtonObject);
        }

        return selectedThought;
    }
}
