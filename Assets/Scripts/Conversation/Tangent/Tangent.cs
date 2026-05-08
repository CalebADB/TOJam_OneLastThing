using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

[Serializable]
public struct TangentRequirementData
{
    public string[] requiredPersonalityNames;

    public static TangentRequirementData CreateDefault()
    {
        return new TangentRequirementData
        {
            requiredPersonalityNames = Array.Empty<string>()
        };
    }
}

[Serializable]
public class TangentSituationalData
{
    public List<string> vibeNames = new List<string>();
    public List<string> presentFactNames = new List<string>();
}

public class Tangent : MonoBehaviour
{
    [Header("Ink")]
    [SerializeField] private TextAsset inkStoryAsset;
    [SerializeField] private bool shouldOutputSituationRequisites = false;

    [Header("State Data")]
    [SerializeField] private Story inkStory = null;
    [SerializeField] private TangentRequirementData tangentRequirementData;
    [SerializeField] private TangentSituationalData tangentSituationalData;
    [SerializeField] private bool isTangentDone = false;
    [SerializeField] private GameObject conversationSituationObject = null;



    [Header("Tangent Debug Requirements")]
    [SerializeField] public bool isOpener = false;
    [SerializeField] public bool shouldCaptureNextTurn = true;

    public void Initialize(GameObject conversationSituationObject)
    {
        this.conversationSituationObject = conversationSituationObject;

        inkStory = new Story(inkStoryAsset.text);
        inkStory.Continue();

        BuildTangentSituationalData();
    }


    public void Handle()
    {
        // 
        if (inkStory.canContinue)
        {
            // if ink should continue
            // move forward (incorporate player choice)
            // collect next line
            // pass thoughts or speech
        }
        else
        {
            //end the tangent
        }
    }

    public bool GetIsTangentValid()
    {
        if (inkStoryAsset == null)
        {
            Debug.Log($"ERROR: GetIsTangentValid: inkStoryAsset is null");
            return false;
        }

        Story inkStory = new Story(inkStoryAsset.text);

        if (!inkStory.canContinue)
        {
            Debug.Log($"ERROR: GetIsTangentValid: inkStory_{inkStoryAsset.name} is empty");
            return false;
        }

        if (isOpener)
        {
            //Debug.Log($"GetIsTangentValid: Tangent_{name} isOpener_True");
            return true;
        }

        //Debug.Log($"GetIsTangentValid: Tangent_{name} is not valid");
        return false;
    }

    public TangentRequirementData GetTangentRequirementData()
    {
        return tangentRequirementData;
    }

    private void BuildTangentSituationalData()
    {
        tangentSituationalData = new TangentSituationalData();
        tangentSituationalData.vibeNames = new List<string>();
        tangentSituationalData.presentFactNames = new List<string>();
    }
    public TangentSituationalData GetTangentSituationalData()
    {
        return tangentSituationalData;
    }

    public void ReceiveCompletedTurn(Turn completedTurn)
    {
        if(completedTurn.thoughts.Count > 0)
        {
            if (null == completedTurn.chosenThought)
            {
                Debug.Log($"Error: ReceiveCompletedTurn: Tangent_{this.gameObject.name} completedTurn.chosenThought is null with completedTurn.thoughts.Count_{completedTurn.thoughts.Count}");
                completedTurn.chosenThought = completedTurn.thoughts[0];
            }

            inkStory.ChooseChoiceIndex(completedTurn.chosenThought.tangentChoiceIdx);
            inkStory.Continue();
        }

        Debug.Log($"ReceiveCompletedTurn: Tangent_{this.gameObject.name} shouldCaptureNextTurn set to true");
        shouldCaptureNextTurn = true;
    }

    public Turn CaptureNextTurn()
    {
        //Debug.Log($"CaptureNextTurn: Tangent_{this.gameObject.name}");
        if (!shouldCaptureNextTurn)
        {
            return null;
        }
        if (isTangentDone)
        {
            //Debug.Log($"Error: CaptureNextTurn: tangent_{this.name} is done");
            return null;
        }

        string personalityTagValue = GetPersonalityTagValue(inkStory.currentTags);
        if ("null" == personalityTagValue)
        {
            Debug.Log($"Error: CaptureNextTurn: inkStory.currentText_{inkStory.currentText} does not have a personality tag");
            return null;
        }

        Turn nextTurn = new Turn();
        string articulationText = "";
        nextTurn.tangentName = this.gameObject.name;
        nextTurn.personalityName = personalityTagValue;

        nextTurn.articulations.Add(BuildArticulation(inkStory.currentText, inkStory.currentTags));
        articulationText += inkStory.currentText;

        bool isTurnCaptured = false;
        while (!isTurnCaptured)
        {
            if (inkStory.canContinue)
            {
                inkStory.Continue();
                if("null" == GetPersonalityTagValue(inkStory.currentTags) ||
                    personalityTagValue == GetPersonalityTagValue(inkStory.currentTags))
                {
                    if (personalityTagValue == GetPersonalityTagValue(inkStory.currentTags))
                    {
                        Debug.Log($"Warning: CaptureNextTurn: inkStory.currentText_{inkStory.currentText} has a repeat personalityTagValue_{personalityTagValue}");
                    }

                    nextTurn.articulations.Add(BuildArticulation(inkStory.currentText, inkStory.currentTags));
                    articulationText += inkStory.currentText;
                }
                else
                {
                    Debug.Log($"CaptureNextTurn: should not include inkStory.currentText_{inkStory.currentText} because personalityTagValue_{GetPersonalityTagValue(inkStory.currentTags)}");
                    isTurnCaptured = true;
                }

            }
            else 
            {
                if(inkStory.currentChoices.Count > 0) // choices 
                {
                    nextTurn.thoughts = BuildThoughts(inkStory.currentChoices);
                    foreach (Thought thought in nextTurn.thoughts)
                    {
                        nextTurn.thoughtText += $"Tangent_{name}, ThoughtIdx_{thought.tangentChoiceIdx}: {thought.text}\n";
                    }
                }
                else // done
                {
                    isTangentDone = true;
                }
                // choices/thoughts
                // tangent done
                isTurnCaptured = true;
            }
        }

        nextTurn.articulationText = articulationText;
        Debug.Log($"CaptureNextTurn: nextTurn tangentName_{this.gameObject.name}, personalityName_{nextTurn.personalityName}\narticulationText:\n{nextTurn.articulationText}thoughtText:\n{nextTurn.thoughtText}");

        shouldCaptureNextTurn = false;

        return nextTurn;
    }

    private Articulation BuildArticulation(string text, List<string> tags)
    {
        Articulation articulation = new Articulation();
        articulation.text = text;
        articulation.length = text.Length;
        articulation.speedFactor = GetSpeedFactorTagValue(tags);
        articulation.bufferTime = GetBufferTimeTagValue(tags);

        Debug.Log($"BuildArticulation: articulation: length_{articulation.length}\nspeedFactor_{articulation.speedFactor}\nbufferTime_{articulation.bufferTime}\ntext:\n{articulation.text}");

        return articulation;
    }
    private List<Thought> BuildThoughts(List<Choice> choices)
    {
        List<Thought> thoughts = new List<Thought>();

        foreach (Choice choice in choices)
        {
            thoughts.Add(BuildThought(choice));
        }

        return thoughts;
    }
    private Thought BuildThought(Choice choice)
    {
        Thought thought = new Thought();
        thought.tangentName = this.name;
        thought.tangentChoiceIdx = choice.index;
        thought.text = choice.text;

        return thought;
    }
    private string GetTagValueString(string Key, List<string> tags)
    {
        foreach (string tag in tags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.Log($"Error: GetTagValue: Found tag_{tag}, with splitTag.Length_{splitTag.Length}");
                return "null";
            }

            if (splitTag[0].Trim() == Key)
            {
                return splitTag[1].Trim();
            }
        }

        return "null";
    }
    private string GetPersonalityTagValue(List<string> tags)
    {
        string personalityTagKey = "Pers";
        string personalityTagValue = GetTagValueString(personalityTagKey, tags);       

        return personalityTagValue;
    }
    private float GetSpeedFactorTagValue(List<string> tags)
    {
        string speedFactorTagKey = "SpeedFactor";
        float defaultSpeedFactorValue = 1.0f;

        string speedFactorTagValueString = GetTagValueString(speedFactorTagKey, tags);

        if ("null" == speedFactorTagValueString)
        {
            return defaultSpeedFactorValue;
        }
        if (!float.TryParse(speedFactorTagValueString, out float speedFactorTagValue))
        {
            Debug.Log($"Error: GetBufferTimeTagValue: speedFactorTagValueString_{speedFactorTagValueString} is an invalid float");
            return defaultSpeedFactorValue;
        }

        return speedFactorTagValue;
    }
    private float GetBufferTimeTagValue(List<string> tags)
    {
        string bufferTimeTagKey = "BufferTime";
        float defaultBufferTimeValue = 0.0f;

        string bufferTimeTagValueString = GetTagValueString(bufferTimeTagKey, tags);

        if ("null" == bufferTimeTagValueString)
        {
            return defaultBufferTimeValue;
        }
        if (!float.TryParse(bufferTimeTagValueString, out float bufferTimeTagValue))
        {
            Debug.Log($"Error: GetBufferTimeTagValue: bufferTimeTagValueString_{bufferTimeTagValueString} is an invalid float");
            return defaultBufferTimeValue;
        }

        return bufferTimeTagValue;
    }
}
