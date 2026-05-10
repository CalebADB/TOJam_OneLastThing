using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    [Header("Volume")]
    [Range(0, 1)]
    public float masterVolume = 1;
    [Range(0, 1)]
    public float musicVolume = 1;
    [Range(0, 1)]
    public float ambienceVolume = 1;
    [Range(0, 1)]
    public float SFXVolume = 1;

    private Bus masterBus;
    private Bus musicBus;
    private Bus ambienceBus;
    private Bus sfxBus;

    private List<EventInstance> eventInstances;
    private List<StudioEventEmitter> eventEmitters;

    private EventInstance ambienceEventInstance;
    private EventInstance musicEventInstance;

    [SerializeField] private AudioSituationGenerator audioSituationGenerator = null;

    [Header("Debug")]
    [SerializeField] public bool shouldOutputValues = false;


    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene.");
        }
        instance = this;

        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
    }

    private void Start()
    {
        InitializeMusic(FMODEvents.instance.conversationMusic);
    }

    private void Update()
    {
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        ambienceBus.setVolume(ambienceVolume);
        sfxBus.setVolume(SFXVolume);

        HandleSituationVibes();

        if (shouldOutputValues)
        {
            musicEventInstance.getParameterByName(audioSituationGenerator.exhaustedSoundVibe.vibeName, out float exhaustedValue);
            musicEventInstance.getParameterByName(audioSituationGenerator.unheardSoundVibe.vibeName, out float unheardValue);
            musicEventInstance.getParameterByName(audioSituationGenerator.indifferenceSoundVibe.vibeName, out float indifferenceValue);
            musicEventInstance.getParameterByName(audioSituationGenerator.wantingSoundVibe.vibeName, out float wantingValue);
            musicEventInstance.getParameterByName(audioSituationGenerator.rejectingSoundVibe.vibeName, out float rejectingValue);

            Debug.Log($"exhaustedValue: {exhaustedValue}, unheardValue: {unheardValue}, indifferenceValue: {indifferenceValue}, wantingValue: {wantingValue}, rejectingValue: {rejectingValue} \nexhaustedSoundVibe.value: {audioSituationGenerator.exhaustedSoundVibe.value}, unheardSoundVibe.value: {audioSituationGenerator.unheardSoundVibe.value}, indifferenceSoundVibe.value: {audioSituationGenerator.indifferenceSoundVibe.value}, wantingSoundVibe.value: {audioSituationGenerator.wantingSoundVibe.value}, rejectingSoundVibe.value: {audioSituationGenerator.rejectingSoundVibe.value}");
        }

    }

    private void InitializeAmbience(EventReference ambienceEventReference)
    {
        ambienceEventInstance = CreateInstance(ambienceEventReference);
        ambienceEventInstance.start();
    }

    private void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateInstance(musicEventReference);
        musicEventInstance.start();
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
    {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);
        return emitter;
    }

    private void CleanUp()
    {
        // stop and release any created instances
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
        // stop all of the event emitters, because if we don't they may hang around in other scenes
        foreach (StudioEventEmitter emitter in eventEmitters)
        {
            emitter.Stop();
        }
    }

    private void HandleSituationVibes()
    {
        if (audioSituationGenerator == null)
        {
            Debug.Log("AudioManager.HandleSituationVibes: audioSituationGenerator is null");
            return;
        }

        musicEventInstance.setParameterByName(audioSituationGenerator.exhaustedSoundVibe.vibeName, audioSituationGenerator.exhaustedSoundVibe.value);
        musicEventInstance.setParameterByName(audioSituationGenerator.unheardSoundVibe.vibeName, audioSituationGenerator.unheardSoundVibe.value);
        musicEventInstance.setParameterByName(audioSituationGenerator.indifferenceSoundVibe.vibeName, audioSituationGenerator.indifferenceSoundVibe.value);
        musicEventInstance.setParameterByName(audioSituationGenerator.wantingSoundVibe.vibeName, audioSituationGenerator.wantingSoundVibe.value);
        musicEventInstance.setParameterByName(audioSituationGenerator.rejectingSoundVibe.vibeName, audioSituationGenerator.rejectingSoundVibe.value);
    }

    private void OnDestroy()
    {
        CleanUp();
    }
}