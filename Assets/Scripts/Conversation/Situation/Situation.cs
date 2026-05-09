using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Vibe
{
    string vibeName = "";
    float value = 0.0f;
    float maxValue = 0.0f;
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
    List<Vibe> vibes = new List<Vibe>();
    // present
    List<Present> presents = new List<Present>();
    //   bool knowsYouCheated
    //   enum currentWeather
    // statuses
    // phases

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
}
