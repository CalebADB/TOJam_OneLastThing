using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Pose
{
    public string id;
    public AnimationClip animationClip;
}

[CreateAssetMenu(fileName = "PoseTable", menuName = "Characters/Pose Table", order = 1)]
public class PoseTable : ScriptableObject
{
    [SerializeField] private List<Pose> poses = new();

    public IReadOnlyList<Pose> Poses => poses;

    public Pose GetPoseByIndex(int index)
    {
        if (index < 0 || index >= poses.Count)
        {
            Debug.LogWarning($"Index {index} is out of range for poses.");
            return null;
        }

        return poses[index];
    }

    public Pose GetPoseById(string id)
    {
        foreach (var pose in poses)
        {
            if (pose.id == id)
                return pose;
        }

        Debug.LogWarning($"Pose with id '{id}' was not found.");
        return null;
    }
}