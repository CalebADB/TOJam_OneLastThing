using UnityEngine;


public class CharacterVisualGenerator : MonoBehaviour
{    
    private enum VibeType
    {
        Unheard,
        Exhausted,
        Anger,
        Fear
    }


    [Header("Situation")]
    [SerializeField] Situation conversationSituation = null;

    [SerializeField] string exhaustedVibeName = "";
    [SerializeField] float exhaustedVibeValue = 0.0f;
    [SerializeField] string unheardVibeName = "";
    [SerializeField] float unheardVibeValue = 0.0f;
    [SerializeField] string angerVibeName = "";
    [SerializeField] float angerVibeValue = 0.0f;
    [SerializeField] string fearVibeName = "";
    [SerializeField] float fearVibeValue = 0.0f;

    [Header("Character ID Output")]
    [SerializeField] bool shouldUpdateFace = false;
    [SerializeField] string faceID = "";

    [SerializeField] bool shouldUpdatePose = false;
    [SerializeField] string poseID = "";

    public void Initialize(Situation conversationSituation)
    {
        this.conversationSituation = conversationSituation;
    }

    // Update is called once per frame
    private void Update()
    {
        //UpdateVibeValues();
        CalculateFaceId();
        CalculatePoseId();
    }

    public bool GetShouldUpdateFace()
    {
        return shouldUpdateFace;
    }

    public bool GetShouldUpdatePose()
    { 
        return shouldUpdatePose; 
    }

    private void UpdateVibeValues()
    {
        exhaustedVibeValue = conversationSituation.GetVibeValue(exhaustedVibeName);
        unheardVibeValue = conversationSituation.GetVibeValue(unheardVibeName);
        angerVibeValue = conversationSituation.GetVibeValue(angerVibeName);
        fearVibeValue = conversationSituation.GetVibeValue(fearVibeName);
    }

    private void CalculateFaceId()
    {
        string newFaceId = "default";

        VibeType largestVibeType = VibeType.Exhausted;
        VibeType secondLargestVibeType = VibeType.Exhausted;

        float largestVibeValue = 0.0f;
        float secondLargestVibeValue = 0.0f;

        if (exhaustedVibeValue >= secondLargestVibeValue)
        {
            if (exhaustedVibeValue >= largestVibeValue)
            {
                secondLargestVibeValue = largestVibeValue;
                secondLargestVibeType = largestVibeType;
                largestVibeValue = exhaustedVibeValue;
                largestVibeType = VibeType.Exhausted;
            }
            else
            {
                secondLargestVibeValue = exhaustedVibeValue;
                secondLargestVibeType = VibeType.Exhausted;
            }
        }
        if (unheardVibeValue >= secondLargestVibeValue)
        {
            if (unheardVibeValue >= largestVibeValue)
            {
                secondLargestVibeValue = largestVibeValue;
                secondLargestVibeType = largestVibeType;
                largestVibeValue = unheardVibeValue;
                largestVibeType = VibeType.Unheard;
            }
            else
            {
                secondLargestVibeValue = unheardVibeValue;
                secondLargestVibeType = VibeType.Unheard;
            }
        }
        if (angerVibeValue >= secondLargestVibeValue)
        {
            if (angerVibeValue >= largestVibeValue)
            {
                secondLargestVibeValue = largestVibeValue;
                secondLargestVibeType = largestVibeType;
                largestVibeValue = angerVibeValue;
                largestVibeType = VibeType.Anger;
            }
            else
            {
                secondLargestVibeValue = angerVibeValue;
                secondLargestVibeType = VibeType.Anger;
            }
        }
        if (fearVibeValue >= secondLargestVibeValue)
        {
            if (fearVibeValue >= largestVibeValue)
            {
                secondLargestVibeValue = largestVibeValue;
                secondLargestVibeType = largestVibeType;
                largestVibeValue = fearVibeValue;
                largestVibeType = VibeType.Fear;
            }
            else
            {
                secondLargestVibeValue = fearVibeValue;
                secondLargestVibeType = VibeType.Fear;
            }
        }
        Debug.Log($"largestVibeValue_{largestVibeValue}, secondLargestVibeValue_{secondLargestVibeValue}");

        if (largestVibeValue < 0.8f) // default
        {
            newFaceId = "default";
        }
        else if (largestVibeValue < 2.9f) // first is 1
        {
            int firstVibeWeight = 1;
            switch (largestVibeType)
            {
                case VibeType.Exhausted: newFaceId = "E" + firstVibeWeight; break;
                case VibeType.Unheard: newFaceId = "U" + firstVibeWeight; break;
                case VibeType.Anger: newFaceId = "A" + firstVibeWeight; break;
                case VibeType.Fear: newFaceId = "F" + firstVibeWeight; break;
            }
        }
        else if (largestVibeValue < 4.5f) // first is 3
        {
            int firstVibeWeight = 3;
            switch (largestVibeType)
            {
                case VibeType.Exhausted: newFaceId = "E" + firstVibeWeight; break;
                case VibeType.Unheard: newFaceId = "U" + firstVibeWeight; break;
                case VibeType.Anger: newFaceId = "A" + firstVibeWeight; break;
                case VibeType.Fear: newFaceId = "F" + firstVibeWeight; break;
            }

            if (secondLargestVibeValue > 0.9f)
            {
                int secondVibeWeight = 1;
                switch (secondLargestVibeType)
                {
                    case VibeType.Exhausted: newFaceId += "E" + secondVibeWeight; break;
                    case VibeType.Unheard: newFaceId += "U" + secondVibeWeight; break;
                    case VibeType.Anger: newFaceId += "A" + secondVibeWeight; break;
                    case VibeType.Fear: newFaceId += "F" + secondVibeWeight; break;
                }
            }
        }
        else // first is 5
        {
            int firstVibeWeight = 5;
            switch (largestVibeType)
            {
                case VibeType.Exhausted: newFaceId = "E" + firstVibeWeight; break;
                case VibeType.Unheard: newFaceId = "U" + firstVibeWeight; break;
                case VibeType.Anger: newFaceId = "A" + firstVibeWeight; break;
                case VibeType.Fear: newFaceId = "F" + firstVibeWeight; break;
            }

            if (secondLargestVibeValue > 2.4f)
            {
                int secondVibeWeight = 3;
                switch (secondLargestVibeType)
                {
                    case VibeType.Exhausted: newFaceId += "E" + secondVibeWeight; break;
                    case VibeType.Unheard: newFaceId += "U" + secondVibeWeight; break;
                    case VibeType.Anger: newFaceId += "A" + secondVibeWeight; break;
                    case VibeType.Fear: newFaceId += "F" + secondVibeWeight; break;
                }
            }
        }

        if (faceID != newFaceId)
        {
            faceID = newFaceId;
            shouldUpdateFace = true;
        }
    }

    private void CalculatePoseId()
    {
        string newPoseId = "default";

        VibeType largestVibeType = VibeType.Exhausted;
        VibeType secondLargestVibeType = VibeType.Exhausted;

        float largestVibeValue = 0.0f;
        float secondLargestVibeValue = 0.0f;

        if (exhaustedVibeValue >= secondLargestVibeValue)
        {
            if (exhaustedVibeValue >= largestVibeValue)
            {
                secondLargestVibeValue = largestVibeValue;
                secondLargestVibeType = largestVibeType;
                largestVibeValue = exhaustedVibeValue;
                largestVibeType = VibeType.Exhausted;
            }
            else
            {
                secondLargestVibeValue = exhaustedVibeValue;
                secondLargestVibeType = VibeType.Exhausted;
            }
        }
        if (unheardVibeValue >= secondLargestVibeValue)
        {
            if (unheardVibeValue >= largestVibeValue)
            {
                secondLargestVibeValue = largestVibeValue;
                secondLargestVibeType = largestVibeType;
                largestVibeValue = unheardVibeValue;
                largestVibeType = VibeType.Unheard;
            }
            else
            {
                secondLargestVibeValue = unheardVibeValue;
                secondLargestVibeType = VibeType.Unheard;
            }
        }
        if (angerVibeValue >= secondLargestVibeValue)
        {
            if (angerVibeValue >= largestVibeValue)
            {
                secondLargestVibeValue = largestVibeValue;
                secondLargestVibeType = largestVibeType;
                largestVibeValue = angerVibeValue;
                largestVibeType = VibeType.Anger;
            }
            else
            {
                secondLargestVibeValue = angerVibeValue;
                secondLargestVibeType = VibeType.Anger;
            }
        }
        if (fearVibeValue >= secondLargestVibeValue)
        {
            if (fearVibeValue >= largestVibeValue)
            {
                secondLargestVibeValue = largestVibeValue;
                secondLargestVibeType = largestVibeType;
                largestVibeValue = fearVibeValue;
                largestVibeType = VibeType.Fear;
            }
            else
            {
                secondLargestVibeValue = fearVibeValue;
                secondLargestVibeType = VibeType.Fear;
            }
        }
        Debug.Log($"largestVibeValue_{largestVibeValue}, secondLargestVibeValue_{secondLargestVibeValue}");

        if (largestVibeValue < 1.2f) // default
        {
            newPoseId = "default";
        }
        else if (largestVibeValue < 3.6f) // first is 2
        {
            int firstVibeWeight = 2;
            switch (largestVibeType)
            {
                case VibeType.Exhausted: newPoseId = "E" + firstVibeWeight; break;
                case VibeType.Unheard: newPoseId = "U" + firstVibeWeight; break;
                case VibeType.Anger: newPoseId = "A" + firstVibeWeight; break;
                case VibeType.Fear: newPoseId = "F" + firstVibeWeight; break;
            }
        }
        else // first is 4
        {
            int firstVibeWeight = 4;
            switch (largestVibeType)
            {
                case VibeType.Exhausted: newPoseId = "E" + firstVibeWeight; break;
                case VibeType.Unheard: newPoseId = "U" + firstVibeWeight; break;
                case VibeType.Anger: newPoseId = "A" + firstVibeWeight; break;
                case VibeType.Fear: newPoseId = "F" + firstVibeWeight; break;
            }

            if (secondLargestVibeValue > 1.8f)
            {
                int secondVibeWeight = 2;
                switch (secondLargestVibeType)
                {
                    case VibeType.Exhausted: newPoseId += "E" + secondVibeWeight; break;
                    case VibeType.Unheard: newPoseId += "U" + secondVibeWeight; break;
                    case VibeType.Anger: newPoseId += "A" + secondVibeWeight; break;
                    case VibeType.Fear: newPoseId += "F" + secondVibeWeight; break;
                }
            }
        }

        if (poseID != newPoseId)
        {
            poseID = newPoseId;
            shouldUpdatePose = true;
        }
    }

}
