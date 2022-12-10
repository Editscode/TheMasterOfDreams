using Assets.Scripts;
using UnityEngine;

namespace StarterAssets
{
    public class UICanvasControllerInput : MonoBehaviour
    {
        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            InputManager.Instance.MoveInput(virtualMoveDirection);
        }

        public void VirtualLookInput(Vector2 virtualLookDirection)
        {
            InputManager.Instance.LookInput(virtualLookDirection);
        }
    }
}
