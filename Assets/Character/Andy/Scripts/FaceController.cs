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
    [SerializeField] private CharacterVisualGenerator visualGenerator;
    
    
    private void Start()
    {
        UpdateEyePositions();
        ApplyFace(0);
    }

    private void Update()
    {
        if(visualGenerator == null)
            return;
        if (visualGenerator.GetShouldUpdateFace())
        {
            
            string nextFaceId = visualGenerator.CaptureFaceId();
            ApplyFace(nextFaceId);
        }
    }

    private void LateUpdate()
    {
        UpdateEyePositions();
    }

    private void ApplyFace(int index)
    {
        Face face = faceTable.GetFaceByIndex(0);
        if(face == null) return;
        if (face.faceTexture == null) return;
        propertyBlock ??= new MaterialPropertyBlock();
        faceRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetTexture(FaceTexture_ID, face.faceTexture);
        propertyBlock.SetTexture(EyeWhiteTexture_ID, face.eyeWhiteTexture);
        faceRenderer.SetPropertyBlock(propertyBlock);
    }
    private void ApplyFace(string id)
    {
        Debug.Log($"Applying Face with id {id}");
        Face face = faceTable.GetFaceById(id);
        if(face == null) return;
        if (face.faceTexture == null) return;
        propertyBlock ??= new MaterialPropertyBlock();
        faceRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetTexture(FaceTexture_ID, face.faceTexture);
        propertyBlock.SetTexture(EyeWhiteTexture_ID, face.eyeWhiteTexture);
        faceRenderer.SetPropertyBlock(propertyBlock);
    }
    
    private void UpdateEyePositions()
        {
            if (!faceRenderer || !eyeLeft || !eyeRight || !rootBone)
                return;
    
            propertyBlock ??= new MaterialPropertyBlock();
            faceRenderer.GetPropertyBlock(propertyBlock);
    
            
            Vector3 eyeLeftRootSpace = rootBone.InverseTransformPoint(eyeLeft.position);
            Vector3 eyeRightRootSpace = rootBone.InverseTransformPoint(eyeRight.position);
    
            var rotationAdjust = Quaternion.Euler(0f, 0f, 0f);
            
            Quaternion leftRotRoot = Quaternion.Inverse(rootBone.rotation) * (eyeLeft.rotation * rotationAdjust);
            Quaternion rightRotRoot = Quaternion.Inverse(rootBone.rotation) * (eyeRight.rotation * rotationAdjust);
    
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
    
    
}