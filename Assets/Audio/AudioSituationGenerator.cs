using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum SoundVibeType
{
    Indifference,
    Wanting,
    Rejecting
}

[Serializable]
public class SoundVibe
{
    [SerializeField] public string vibeName = "";
    [SerializeField] public float value = 0;
}

public class AudioSituationGenerator : MonoBehaviour
{
    private enum VibeType
    {
        Unheard,
        Exhausted,
        Anger,
        Fear
    }
    [Header("Debug")]
    [SerializeField] bool shouldIgnoreSituation = false;

    [Header("Situation")]
    [SerializeField] GameObject conversationSituationObject = null;

    [Header("Sound Vibe")]
    [SerializeField] public SoundVibe exhaustedSoundVibe = new SoundVibe();
    [SerializeField] public SoundVibe unheardSoundVibe = new SoundVibe();
    [SerializeField] public SoundVibe indifferenceSoundVibe = new SoundVibe();
    [SerializeField] public SoundVibe wantingSoundVibe = new SoundVibe();
    [SerializeField] public SoundVibe rejectingSoundVibe = new SoundVibe();

    [SerializeField] float exhaustedSoundVibeValue = 0;
    [SerializeField] float unheardSoundVibeValue = 0;
    [SerializeField] float indifferenceSoundVibeValue = 0;
    [SerializeField] float wantingSoundVibeValue = 0;
    [SerializeField] float rejectingSoundVibeValue = 0;

    [SerializeField] bool isTransitioningVibe = false;
    [SerializeField] float changingVibeTransitionTime = 5.0f;
    [SerializeField] float changingVibeTransitionTimeRemaining = 0.0f;
    [SerializeField] SoundVibeType activeSoundVibeType = SoundVibeType.Indifference;
    [SerializeField] SoundVibeType nextSoundVibeType = SoundVibeType.Indifference;


    public void Initialize(GameObject conversationSituationObject)
    {
        this.conversationSituationObject = conversationSituationObject;
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateVibeValues();
    }

    private void UpdateVibeValues()
    {
        if (shouldIgnoreSituation)
        {
            return;
        }

        if (conversationSituationObject == null)
        { return; }
        Situation situation = conversationSituationObject.GetComponent<Situation>();
        if (situation == null)
        {
            Debug.LogError("Error: AudioSituationGenerator: UpdateVibeValues: situation is null");
            return;
        }

        CalculateExhaustedSoundVibeValue();
        CalculateUnheardSoundVibeValue();
        CalculateIndifferenceSoundVibeValue();
        CalculateWantingSoundVibeValue();
        CalculateRejectingSoundVibeValue();

        HandleSoundVibes();
    }

    private void CalculateExhaustedSoundVibeValue()
    {
        exhaustedSoundVibeValue = conversationSituationObject.GetComponent<Situation>().GetVibeValue("AndyExhausted");
    }
    private void CalculateUnheardSoundVibeValue()
    {
        unheardSoundVibeValue = conversationSituationObject.GetComponent<Situation>().GetVibeValue("AndyUnheard");
    }
    private void CalculateIndifferenceSoundVibeValue()
    {
        indifferenceSoundVibeValue = 0;// 5 - conversationSituationObject.GetComponent<Situation>().GetVibeValue("AndyAnger");
    }
    private void CalculateWantingSoundVibeValue()
    {
        wantingSoundVibeValue = conversationSituationObject.GetComponent<Situation>().GetVibeValue("AndyFear");
    }
    private void CalculateRejectingSoundVibeValue()
    {
        float andyAngerValue = conversationSituationObject.GetComponent<Situation>().GetVibeValue("AndyAnger");
        //Debug.Log($"CalculateRejectingSoundVibe: floatAndyAnger_{andyAngerValue}");
        rejectingSoundVibeValue = andyAngerValue;
    }

    private void HandleSoundVibes()
    {
        if (isTransitioningVibe)
        {
            changingVibeTransitionTimeRemaining -= Time.deltaTime;

            float activeSoundVibeWeight = (1 - ((changingVibeTransitionTime - changingVibeTransitionTimeRemaining) / changingVibeTransitionTime));
            float nextSoundVibeTrackWeight = (((changingVibeTransitionTime - changingVibeTransitionTimeRemaining) / changingVibeTransitionTime));

            switch (activeSoundVibeType)
            {
                case SoundVibeType.Indifference:
                    {
                        indifferenceSoundVibe.value = activeSoundVibeWeight * indifferenceSoundVibeValue;
                        break;
                    }
                case SoundVibeType.Wanting:
                    {
                        wantingSoundVibe.value = activeSoundVibeWeight * wantingSoundVibeValue;

                        break;
                    }

                case SoundVibeType.Rejecting:
                    {
                        rejectingSoundVibe.value = activeSoundVibeWeight * rejectingSoundVibeValue;

                        break;
                    }

            }


            switch (nextSoundVibeType)
            {
                case SoundVibeType.Indifference:
                    {
                        indifferenceSoundVibe.value = nextSoundVibeTrackWeight * indifferenceSoundVibeValue;
                        break;
                    }
                case SoundVibeType.Wanting:
                    {
                        Debug.Log($"nextSoundVibeType: wantingSoundVibeValue_{wantingSoundVibeValue}, nextSoundVibeTrackWeight_{nextSoundVibeTrackWeight}");
                        wantingSoundVibe.value = nextSoundVibeTrackWeight * wantingSoundVibeValue;

                        break;
                    }

                case SoundVibeType.Rejecting:
                    {
                        rejectingSoundVibe.value = nextSoundVibeTrackWeight * rejectingSoundVibeValue;

                        break;
                    }

            }

            if (changingVibeTransitionTimeRemaining <= 0)
            {
                isTransitioningVibe = false;
                activeSoundVibeType = nextSoundVibeType;
            }

            return;
        }


        SoundVibeType soundVibeType = SoundVibeType.Indifference;

        if (rejectingSoundVibeValue > indifferenceSoundVibeValue)
        {
            soundVibeType = SoundVibeType.Rejecting;
            if (wantingSoundVibeValue > rejectingSoundVibeValue)
            {
                soundVibeType = SoundVibeType.Wanting;
            }

        }
        else if (wantingSoundVibeValue > indifferenceSoundVibeValue)
        {
            soundVibeType = SoundVibeType.Wanting;
        }


        if (soundVibeType != activeSoundVibeType)
        {
            nextSoundVibeType = soundVibeType;

            changingVibeTransitionTimeRemaining = changingVibeTransitionTime;
            isTransitioningVibe = true;
        }


    }

}
