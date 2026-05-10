using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PoseController : MonoBehaviour
{
    [SerializeField] private PoseTable poseTable;
    [SerializeField] private Animator animator;

    [Header("Animator Override")]
    [SerializeField] private AnimationClip placeholderPoseClip;

    private AnimatorOverrideController overrideController;

    private void Awake()
    {
        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;
    }

    private void Start()
    {
        ApplyPoseById("Test2");
    }

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            ApplyPoseById("Test1");
        }
        else if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            ApplyPoseById("Test2");
        }
    }

    public void ApplyPoseById(string poseId)
    {
        Pose pose = poseTable.GetPoseById(poseId);
        Debug.Log(pose.animationClip.name);
        if (pose == null || pose.animationClip == null)
            return;

        overrideController[placeholderPoseClip] = pose.animationClip;

        animator.CrossFade("PoseSlot", 0.1f);
    }
}