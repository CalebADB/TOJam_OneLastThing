using System;
using UnityEngine;

public class FaceController : MonoBehaviour
{
    [SerializeField]
    Transform eyeLeft, eyeRight;
    [SerializeField]
    Renderer faceRenderer;
    
    MaterialPropertyBlock propertyBlock;
    private static readonly int EyeLeftOS_ID = Shader.PropertyToID("_EyeLeftOS");
    private static readonly int EyeRightOS_ID = Shader.PropertyToID("_EyeRightOS");
    private static readonly int EyeLeftRotOS_ID = Shader.PropertyToID("_EyeLeftRotOS");
    private static readonly int EyeRightRotOS_ID = Shader.PropertyToID("_EyeRightRotOS");

    private void OnEnable()
    {
        UpdateEyesPositions();
    }

    private void LateUpdate()
    {
        UpdateEyesPositions();
    }

    private void UpdateEyesPositions()
    {
        if (!faceRenderer || !eyeLeft || !eyeRight)
            return;

        propertyBlock ??= new MaterialPropertyBlock();
        faceRenderer.GetPropertyBlock(propertyBlock);

        
        Vector3 eyeLeftOS = faceRenderer.transform.InverseTransformPoint(eyeLeft.position);
        Vector3 eyeRightOS = faceRenderer.transform.InverseTransformPoint(eyeRight.position);
        
        Debug.Log($"Eye left world: {eyeLeft.position}, renderer pos: {faceRenderer.transform.position}, eyeLeftOS: {eyeLeftOS}");

        propertyBlock.SetVector(EyeLeftOS_ID, new Vector4(eyeLeftOS.x, eyeLeftOS.z, eyeLeftOS.y, 1));
        propertyBlock.SetVector(EyeRightOS_ID, new Vector4(eyeRightOS.x, eyeRightOS.z, eyeRightOS.y, 1));

        faceRenderer.SetPropertyBlock(propertyBlock);
    }
}
