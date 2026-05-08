using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThoughtButton : MonoBehaviour
{
    [SerializeField] public GameObject buttonObject = null;
    [SerializeField] public GameObject textObject = null;
    [SerializeField] private bool isSelected = false;
    [SerializeField] private Thought thought = null;
    
    private void Start()
    {
        buttonObject.GetComponent<Button>().onClick.AddListener(OnButtonClicked);
    }

    public void Initialize(Thought thought)
    {
        this.thought = thought;
        textObject.GetComponent<TextMeshProUGUI>().text = thought.text;

    }

    private void OnButtonClicked()
    {
        isSelected = true;
    }

    public bool GetIsSelected()
    {
        return isSelected;
    }

    public Thought GetThought()
    {
        return thought;
    }
}
