using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Face
{
    public string id;
    public Texture2D faceTexture;
    public Texture2D eyeWhiteTexture;
}

[CreateAssetMenu(fileName = "FaceTable", menuName = "Characters/Face Table", order = 0)]
public class FaceTable : ScriptableObject
{
    [SerializeField] private List<Face> faces = new();

    public IReadOnlyList<Face> Faces => faces;

    public Face GetFaceByIndex(int index)
    {
        if (index < 0 || index >= faces.Count)
        {
            Debug.LogWarning($"Index {index} is out of range for face textures.");
            return null;
        }

        return faces[index];
    }

    public Face GetFaceById(string id)
    {
        foreach (var entry in faces)
        {
            if (entry.id == id)
                return entry;
        }

        Debug.LogWarning($"Face texture with tag '{id}' was not found.");
        return null;
    }
}