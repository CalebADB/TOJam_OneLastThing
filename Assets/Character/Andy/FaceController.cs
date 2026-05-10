using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FaceController : MonoBehaviour
{
    [SerializeField] private Transform eyeLeft, eyeRight, rootBone;
    [SerializeField] private Renderer faceRenderer;

    private MaterialPropertyBlock propertyBlock;

    private static readonly int EyeLeftOS_ID = Shader.PropertyToID("_EyeLeftOS");
    private static readonly int EyeRightOS_ID = Shader.PropertyToID("_EyeRightOS");

    private static readonly int EyeLeftInvRotOS_ID = Shader.PropertyToID("_EyeLeftInvRotOS");
    private static readonly int EyeRightInvRotOS_ID = Shader.PropertyToID("_EyeRightInvRotOS");
    
    private static readonly int EyeLeftScale_ID = Shader.PropertyToID("_EyeLeftScale");
    private static readonly int EyeRightScale_ID = Shader.PropertyToID("_EyeRightScale");
    
    private static readonly int FaceTexture_ID = Shader.PropertyToID("_FaceTexture");
    private static readonly int EyeWhiteTexture_ID =  Shader.PropertyToID("_EyeWhiteTexture");

    [SerializeField] private FaceTable faceTable;
    
    private void Start()
    {
        UpdateEyes();
        UpdateFace(1);
    }

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            UpdateFace("Test1");
        }
        else if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            UpdateFace("Test2");
        }
    }

    private void LateUpdate()
    {
        UpdateEyes();
    }

    private void UpdateEyes()
    {
        if (!faceRenderer || !eyeLeft || !eyeRight || !rootBone)
            return;

        propertyBlock ??= new MaterialPropertyBlock();
        faceRenderer.GetPropertyBlock(propertyBlock);

        // Position относительно rootBone.
        // Это лучше, чем вручную eye.position - root.position.
        Vector3 eyeLeftRootSpace = rootBone.InverseTransformPoint(eyeLeft.position);
        Vector3 eyeRightRootSpace = rootBone.InverseTransformPoint(eyeRight.position);

        var rotationAdjust = Quaternion.Euler(0f, 0f, 0f);
        
        // Rotation глаза относительно rootBone.
        Quaternion leftRotRoot = Quaternion.Inverse(rootBone.rotation) * (eyeLeft.rotation * rotationAdjust);
        Quaternion rightRotRoot = Quaternion.Inverse(rootBone.rotation) * (eyeRight.rotation * rotationAdjust);

        // Inverse rotation нужен, если в Shader Graph ты хочешь:
        // root/object delta -> eye local delta
        Quaternion leftInvRotRoot = Quaternion.Inverse(leftRotRoot);
        Quaternion rightInvRotRoot = Quaternion.Inverse(rightRotRoot);

        Vector3 leftEyeScale = rotationAdjust * eyeLeft.lossyScale;
        Vector3 rightEyeScale = rotationAdjust * eyeRight.lossyScale;
        
        // leftEyeScale = Utils.DivideVector(leftEyeScale, faceRenderer.transform.lossyScale);
        // rightEyeScale = Utils.DivideVector(rightEyeScale, faceRenderer.transform.lossyScale);
        leftEyeScale = Utils.DivideVector(leftEyeScale, rootBone.localScale);
        rightEyeScale = Utils.DivideVector(rightEyeScale, rootBone.localScale);

        propertyBlock.SetVector(
            EyeLeftOS_ID,
            new Vector4(eyeLeftRootSpace.x, eyeLeftRootSpace.y, eyeLeftRootSpace.z, 1)
        );

        propertyBlock.SetVector(
            EyeRightOS_ID,
            new Vector4(eyeRightRootSpace.x, eyeRightRootSpace.y, eyeRightRootSpace.z, 1)
        );

        propertyBlock.SetVector(
            EyeLeftInvRotOS_ID,
            new Vector4(leftInvRotRoot.x, leftInvRotRoot.y, leftInvRotRoot.z, leftInvRotRoot.w)
        );

        propertyBlock.SetVector(
            EyeRightInvRotOS_ID,
            new Vector4(rightInvRotRoot.x, rightInvRotRoot.y, rightInvRotRoot.z, rightInvRotRoot.w)
        );
        
        propertyBlock.SetVector(
            EyeLeftScale_ID,
            new Vector4(leftEyeScale.x, leftEyeScale.y, leftEyeScale.z, 1)
        );

        propertyBlock.SetVector(
            EyeRightScale_ID,
            new Vector4(rightEyeScale.x, rightEyeScale.y, rightEyeScale.z, 1)
        );

        faceRenderer.SetPropertyBlock(propertyBlock);
    }
    private void UpdateFace(int index)
    {
        propertyBlock ??= new MaterialPropertyBlock();
        faceRenderer.GetPropertyBlock(propertyBlock);
        Face face = faceTable.GetFaceByIndex(0);
        propertyBlock.SetTexture(FaceTexture_ID, face.faceTexture);
        propertyBlock.SetTexture(EyeWhiteTexture_ID, face.eyeWhiteTexture);
        faceRenderer.SetPropertyBlock(propertyBlock);
    }
    private void UpdateFace(string id)
    {
        propertyBlock ??= new MaterialPropertyBlock();
        faceRenderer.GetPropertyBlock(propertyBlock);
        Face face = faceTable.GetFaceById(id);
        propertyBlock.SetTexture(FaceTexture_ID, face.faceTexture);
        propertyBlock.SetTexture(EyeWhiteTexture_ID, face.eyeWhiteTexture);
        faceRenderer.SetPropertyBlock(propertyBlock);
    }
    
    
    
}