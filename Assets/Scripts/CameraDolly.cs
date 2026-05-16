using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using Dishes;

public class CameraDolly : MonoBehaviour
{
    public bool LookingAtNPC { get; private set; }

    public Vector3 LookAtNPCAngles = new Vector3(0, 0, 0);
    public Vector3 LookAtDishesAngles = new Vector3(75, 0, 0);
    public float LookChangeDuration = 1;

    public TMPro.TextMeshProUGUI _swapViewButtonText;
    void Start()
    {
        LookingAtNPC = true;
        transform.rotation = Quaternion.Euler(LookAtNPCAngles);
        ResolveLook();
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            ToggleView();
    }

    /// <summary>
    /// Also on UI button
    /// </summary>
    public void ToggleView()
    {
        LookingAtNPC = !LookingAtNPC;
        ResolveLook();
    }

    private void ResolveLook()
    {
        if (LookingAtNPC)
        {
            _swapViewButtonText.text = "Look Down";
            transform.DORotate(LookAtNPCAngles, LookChangeDuration);
        }
        else
        {
            _swapViewButtonText.text = "Look Up";
            transform.DORotate(LookAtDishesAngles, LookChangeDuration);
        }

        _swapViewButtonText.ForceMeshUpdate(forceTextReparsing: true);
        DishWasher.Instance.SetDishCanvases(!LookingAtNPC);
    }
}
