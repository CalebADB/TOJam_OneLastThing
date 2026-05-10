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
    [SerializeField] private int maxDisplayedThoughButtons = 4;

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

        GameObject thoughtButtonObject = Instantiate(thoughtButtonPrefab, this.transform);
        thoughtButtonObject.name = $"ThoughtButton_{thought.text}";
        thoughtButtonObject.transform.localScale = Vector3.one;

        thoughtButtonObject.GetComponent<ThoughtButton>().Initialize(thought);
        
        thoughtButtonObjects.Add(thoughtButtonObject);
    }

    public Thought CaptureSelectedThoughtFromTurn(Turn turn)
    {
        GameObject selectedThoughtButtonObject = null;
        foreach (GameObject thoughtButtonObject in thoughtButtonObjects)
        {
            Thought thought = thoughtButtonObject.GetComponent<ThoughtButton>().GetThought();
            if (thought.tangentName != turn.tangentName)
            {
                continue;
            }
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
        Debug.Log($"CaptureSelectedThoughtFromTurn: turn.tangentName_{turn.tangentName} selectedThoughtButtonObject_{selectedThoughtButtonObject.name}, ");

        return selectedThoughtButtonObject.GetComponent<ThoughtButton>().GetThought();
    }

    public void ClearTangentThoughts(string tangentName)
    {
        Debug.Log($"ClearTangentThoughts: ThoughtVisualizer_{name}, tangentName_{tangentName}");
        List<GameObject> unselectedThoughtButtonObjects = new List<GameObject>();
        foreach (GameObject thoughtButtonObject in thoughtButtonObjects)
        {
            Thought thought = thoughtButtonObject.GetComponent<ThoughtButton>().GetThought();

            if (thought.tangentName == tangentName)
            {
                unselectedThoughtButtonObjects.Add(thoughtButtonObject);
                Debug.Log($"ClearTangentThoughts: Found Thought_{thought.text} from tangent_{thought.tangentName}");
            }
        }

        foreach (GameObject unselectedThoughtButtonObject in unselectedThoughtButtonObjects)
        {
            thoughtButtonObjects.Remove(unselectedThoughtButtonObject);
            Destroy(unselectedThoughtButtonObject);
        }
    }
}
