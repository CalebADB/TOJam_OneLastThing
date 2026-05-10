using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Music")]
    [field: SerializeField] public EventReference conversationMusic { get; private set; }
    
    //[field: SerializeField] public EventReference I1 { get; private set; }
    //[field: SerializeField] public EventReference I1E { get; private set; }
    //[field: SerializeField] public EventReference I1U { get; private set; }
    //[field: SerializeField] public EventReference I2 { get; private set; }
    //[field: SerializeField] public EventReference I2E { get; private set; }
    //[field: SerializeField] public EventReference I2U { get; private set; }
    //[field: SerializeField] public EventReference I3 { get; private set; }
    //[field: SerializeField] public EventReference I3E { get; private set; }
    //[field: SerializeField] public EventReference I3U { get; private set; }
    //[field: SerializeField] public EventReference I4 { get; private set; }
    //[field: SerializeField] public EventReference I4E { get; private set; }
    //[field: SerializeField] public EventReference I4U { get; private set; }
    //[field: SerializeField] public EventReference I5 { get; private set; }
    //[field: SerializeField] public EventReference I5E { get; private set; }
    //[field: SerializeField] public EventReference I5U { get; private set; }
    //[field: SerializeField] public EventReference R1 { get; private set; }
    //[field: SerializeField] public EventReference R1E { get; private set; }
    //[field: SerializeField] public EventReference R1U { get; private set; }
    //[field: SerializeField] public EventReference R2 { get; private set; }
    //[field: SerializeField] public EventReference R2E { get; private set; }
    //[field: SerializeField] public EventReference R2U { get; private set; }
    //[field: SerializeField] public EventReference R3 { get; private set; }
    //[field: SerializeField] public EventReference R3E { get; private set; }
    //[field: SerializeField] public EventReference R3U { get; private set; }
    //[field: SerializeField] public EventReference R4 { get; private set; }
    //[field: SerializeField] public EventReference R4E { get; private set; }
    //[field: SerializeField] public EventReference R4U { get; private set; }
    //[field: SerializeField] public EventReference R5 { get; private set; }
    //[field: SerializeField] public EventReference R5E { get; private set; }
    //[field: SerializeField] public EventReference R5U { get; private set; }
    //[field: SerializeField] public EventReference W1 { get; private set; }
    //[field: SerializeField] public EventReference W1E { get; private set; }
    //[field: SerializeField] public EventReference W1U { get; private set; }
    //[field: SerializeField] public EventReference W2 { get; private set; }
    //[field: SerializeField] public EventReference W2E { get; private set; }
    //[field: SerializeField] public EventReference W2U { get; private set; }
    //[field: SerializeField] public EventReference W3 { get; private set; }
    //[field: SerializeField] public EventReference W3E { get; private set; }
    //[field: SerializeField] public EventReference W3U { get; private set; }
    //[field: SerializeField] public EventReference W4 { get; private set; }
    //[field: SerializeField] public EventReference W4E { get; private set; }
    //[field: SerializeField] public EventReference W4U { get; private set; }
    //[field: SerializeField] public EventReference W5 { get; private set; }
    //[field: SerializeField] public EventReference W5E { get; private set; }
    //[field: SerializeField] public EventReference W5U { get; private set; }
    [field: Header("Dishes SFX")]
    [field: SerializeField] public EventReference dishesDebug { get; private set; }

    [field: Header("UI SFX")]
    [field: SerializeField] public EventReference UIDebug { get; private set; }

    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the scene.");
        }
        instance = this;
    }
}