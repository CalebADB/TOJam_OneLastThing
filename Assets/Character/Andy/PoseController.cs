using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PoseController : MonoBehaviour
{
    [SerializeField] private PoseTable poseTable;
    [SerializeField] private Animator animator;

    [Header("Animator Override")]
    [SerializeField] private AnimationClip placeholderPoseClip;

    [SerializeField] private CharacterVisualGenerator visualGenerator;
    
    private AnimatorOverrideController overrideController;

    private void Awake()
    {
        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;
    }

    private void Start()
    {
        ApplyPose(0);
    }

    private void Update()
    {
        if(visualGenerator == null)
            return;
        
        if (visualGenerator.GetShouldUpdatePose())
        {
            string nextPoseId = visualGenerator.CapturePoseId();
            ApplyPose(nextPoseId);
            Debug.Log($"Changing Pose to {nextPoseId}");
        }
    }

    public void ApplyPose(string poseId)
    {
        Pose pose = poseTable.GetPoseById(poseId);
        if (pose == null ) return;
        if (pose.animationClip == null) return;
        Debug.Log(pose.animationClip.name);

        overrideController[placeholderPoseClip] = pose.animationClip;

        // animator.CrossFade("PoseSlot", 0.1f);
    }
    public void ApplyPose(int index)
    {
        Pose pose = poseTable.GetPoseByIndex(index);
        if (pose == null ) return;
        if (pose.animationClip == null) return;
        Debug.Log(pose.animationClip.name);

        overrideController[placeholderPoseClip] = pose.animationClip;

        // animator.CrossFade("PoseSlot", 0.1f);
    }
}