using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Vibe
{
    [SerializeField] public string vibeName = "";
    [SerializeField] public float value = 0.0f;
    [SerializeField] public float maxValue = 0.0f;
}

[Serializable]
public class VibeModifier
{
    [SerializeField] public string vibeName = "";
    [SerializeField] public float value = 0.0f;
}
[Serializable]
public class VibeRequirement
{
    [SerializeField] public string vibeName = "";
    [SerializeField] public float value = 0.0f;
}

[Serializable]
public class Present
{
    string presentName = "";
    int databaseId = -1; // this should be a hash
    GameObject databaseObject = null;
}

public class Situation : MonoBehaviour
{
    // vibe
    //   float good/bad_vibe = between 1 and 0;
    [SerializeField] List<Vibe> vibes = new List<Vibe>();
    // present
    [SerializeField] List<Present> presents = new List<Present>();
    //   bool knowsYouCheated
    //   enum currentWeather
    // statuses
    // phases

    [SerializeField] bool hasBeenUpdated = false;

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Vibe> GetRelevantVibes(List<string> relevantVibeNames)
    {
        List<Vibe> relevantVibes = new List<Vibe>();

        relevantVibes = vibes;

        return relevantVibes;
    }

    public List<Present> GetRelevantPresents(List<string> relevantPresentFactNames)
    {
        List<Present> relevantPresents = new List<Present>();

        relevantPresents = presents;

        return relevantPresents;
    }

    public void AddRelevantVibes(List<Vibe> relevantVibes)
    {
        foreach (Vibe vibe in relevantVibes)
        {
            if (!vibes.Contains(vibe))
            {
                vibes.Add(vibe);
            }
        }
    }

    public void AddRelevantPresents(List<Present> relevantPresents)
    {
        foreach (Present present in relevantPresents)
        {
            if (!presents.Contains(present))
            {
                presents.Add(present);
            }
        }
    }

    public void SetVibeValue(string vibeName, float vibeValue)
    {
        bool isVibeFound = false;
        foreach (Vibe vibe in vibes)
        {
            if (vibe.vibeName == vibeName)
            {
                vibe.value = vibeValue;
                vibe.value = Mathf.Clamp(vibe.value, 0.0f, vibe.maxValue);
            }
        }

        if (!isVibeFound)
        {
            Debug.Log($"Error: SetVibe: vibeName_{vibeName} was not found");
        }            
    }

    public void ModifyVibeValue(string vibeName, float vibeDifference)
    {
        bool isVibeFound = false;
        foreach (Vibe vibe in vibes)
        {
            if (vibe.vibeName == vibeName)
            {
                vibe.value += vibeDifference;
                vibe.value = Mathf.Clamp(vibe.value, 0.0f, vibe.maxValue);
            }
        }

        if (!isVibeFound)
        {
            Debug.Log($"Error: ModifyVibe: vibeName_{vibeName} was not found");
        }
    }

    public float GetVibeValue(string vibeName)
    {
        bool isVibeFound = false;
        foreach (Vibe vibe in vibes)
        {
            if (vibe.vibeName == vibeName)
            {
                return vibe.value;
            }
        }

        if (!isVibeFound)
        {
            Debug.Log($"Error: SetVibe: vibeName_{vibeName} was not found");
        }
        return 0.0f;
    }
}
