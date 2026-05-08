using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Darius/InputReader")]
public class Reader : ScriptableObject, GameInput.IGameActions
{
    public GameInput InputCore;
    public bool isJumpPressed;
    public bool isInteractionPressed;
    public Vector2 move;
     
    private void OnEnable()
    {
        InputCore = new GameInput();
        InputCore.Game.SetCallbacks(this);
        InputCore.Game.Enable();
    }

    private void OnDisable()
    {
        InputCore.Game.Disable();
        InputCore.Game.RemoveCallbacks(this);
        InputCore = null;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        isInteractionPressed = context.ReadValueAsButton();
    }
}
