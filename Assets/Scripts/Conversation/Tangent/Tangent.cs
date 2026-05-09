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
    [SerializeField] public string tangentName;
    [SerializeField] private TextAsset inkStoryAsset;
    [SerializeField] private bool shouldOutputSituationRequisites = false;

    [Header("State Data")]
    [SerializeField] private Story inkStory = null;
    [SerializeField] private TangentRequirementData tangentRequirementData;
    [SerializeField] private TangentSituationalData tangentSituationalData;
    [SerializeField] private bool isTangentDone = false;
    [SerializeField] private GameObject conversationSituationObject = null;
    [SerializeField] private List<Thought> unclaimedThoughts;
    [SerializeField] private string lastArticulationText = "";
    [SerializeField] private string lastPersonalityTagValue = "";




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
                Debug.Log($"Error: ReceiveCompletedTurn: tangentName_{tangentName} completedTurn.chosenThought is null with completedTurn.thoughts.Count_{completedTurn.thoughts.Count}");
                completedTurn.chosenThought = completedTurn.thoughts[0];
            }

            inkStory.ChooseChoiceIndex(completedTurn.chosenThought.tangentChoiceIdx);
            inkStory.Continue();
        }

        Debug.Log($"ReceiveCompletedTurn: tangentName_{tangentName} shouldCaptureNextTurn set to true");
        shouldCaptureNextTurn = true;
    }

    public Turn CaptureNextTurn()
    {
        //Debug.Log($"CaptureNextTurn: Tangent_{tangentName}");
        if (!shouldCaptureNextTurn)
        {
            return null;
        }

        if (isTangentDone)
        {
            //Debug.Log($"Error: CaptureNextTurn: tangent_{tangentName} is done");
            return null;
        }

        Turn nextTurn = new Turn();
        if (unclaimedThoughts.Count > 0)
        {
            nextTurn.tangentName = tangentName;
            nextTurn.personalityName = unclaimedThoughts[0].personalityName;
            nextTurn.isThinkingTurn = true;

            foreach (Thought unclaimedThought in unclaimedThoughts)
            {
                if (unclaimedThought.personalityName == nextTurn.personalityName)
                {
                    nextTurn.thoughts.Add(unclaimedThought);
                }
            }

            foreach (Thought thought in nextTurn.thoughts)
            {
                unclaimedThoughts.Remove(thought);
            }
            if (unclaimedThoughts.Count == 0)
            {
                shouldCaptureNextTurn = false;
            }            
            
            return nextTurn;
        }

        string personalityTagValue = GetPersonalityTagValue(inkStory.currentTags);
        if ("null" == personalityTagValue)
        {
            Debug.Log($"Error: CaptureNextTurn: inkStory does not have a personality tag. inkStory.currentTags_{inkStory.currentTags}, inkStory.currentText_{inkStory.currentText} "); 
            return null;
        }

        string articulationText = "";
        nextTurn.tangentName = tangentName;
        nextTurn.personalityName = personalityTagValue;
        nextTurn.isThinkingTurn = false;
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
                    nextTurn.isThinkingTurn = true;
                    nextTurn.thoughts = BuildThoughts(inkStory.currentChoices, nextTurn.personalityName);
                    foreach (Thought thought in nextTurn.thoughts)
                    {
                        nextTurn.thoughtText += $"Tangent_{tangentName}, ThoughtIdx_{thought.tangentChoiceIdx}: {thought.text}\n";
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
        //Debug.Log($"CaptureNextTurn: nextTurn tangentName_{tangentName}, personalityName_{nextTurn.personalityName}\narticulationText:\n{nextTurn.articulationText}thoughtText:\n{nextTurn.thoughtText}");

        if (unclaimedThoughts.Count == 0)
        {
            shouldCaptureNextTurn = false;
        }
        lastArticulationText = articulationText;
        lastPersonalityTagValue = personalityTagValue;
        return nextTurn;
    }

    private Articulation BuildArticulation(string text, List<string> tags)
    {
        Articulation articulation = new Articulation();
        articulation.text = text;
        articulation.length = text.Length;
        articulation.speedFactor = GetSpeedFactorTagValue(tags);
        articulation.bufferTime = GetBufferTimeTagValue(tags);
        string startTangentTagValue = GetStartTangentTagValue(tags);
        if (startTangentTagValue != "null")
        {
            Debug.Log("WWWWWEEEEEEEEEEEEEEEEEEEEEEEEEEEEE1");
            articulation.newTangentName = startTangentTagValue;
            articulation.shouldStartNewTangent = true;
        }

        Debug.Log($"BuildArticulation: articulation: length_{articulation.length}\nspeedFactor_{articulation.speedFactor}\nbufferTime_{articulation.bufferTime}\ntext:\n{articulation.text}");

        return articulation;
    }
    private List<Thought> BuildThoughts(List<Choice> choices, string personalityName)
    {
        List<Thought> thoughts = new List<Thought>();

        foreach (Choice choice in choices)
        {
            string personalityTagValue = GetPersonalityTagValue(choice.tags);
            if (personalityTagValue == "null" ||
                personalityTagValue == personalityName)
            {
                Thought thought = BuildThought(choice);
                thought.personalityName = personalityName;
                thoughts.Add(thought);
            }
            else
            {
                Thought thought = BuildThought(choice);
                thought.personalityName = personalityTagValue;
                unclaimedThoughts.Add(thought);
                Debug.Log($"BuildThoughts: thought: idx_{thought.tangentChoiceIdx} text_{thought.text} has a seperate personalityTagValue_{personalityTagValue}");
            }
        }

        return thoughts;
    }
    private Thought BuildThought(Choice choice)
    {
        Thought thought = new Thought();
        thought.tangentName = this.tangentName;
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
    private string GetStartTangentTagValue(List<string> tags)
    {
        string personalityTagKey = "StartTangent";
        string personalityTagValue = GetTagValueString(personalityTagKey, tags);

        return personalityTagValue;
    }
}
