using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public Vector2 moveObject;
        public float roteObject;
        public bool jump;
        public bool shoot;
        public bool stopShoot = false;
        public bool shootCancel;
        public bool sprint;
        public bool menu;
        public bool changeCamera;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            if (cursorInputForLook)
            {
                LookInput(value.Get<Vector2>());
            }
        }
        public void OnMoveObject(InputValue value)
        {
            MoveObjectInput(value.Get<Vector2>());
        }
        public void OnRotateObject(InputValue value)
        {
            RotateObjectInput(value.Get<float>());
        }
        public void OnJump(InputValue value)
        {
            JumpInput(value.isPressed);
        }
        public void OnShoot(InputValue value)
        {
            ShootInput(value.isPressed);
        }
        public void OnStopShoot(InputValue value)
        {
            StopShootInput(value.isPressed);
        }
        public void OnShootCancel(InputValue value)
        {
            ShootCancelInput(value.isPressed);
        }
        public void OnSprint(InputValue value)
        {
            SprintInput(value.isPressed);
        }
        public void OnMenu(InputValue value)
        {
            MenuInput(value.isPressed);
        }
        public void OnChangeCamera(InputValue value)
        {
            ChangeCameraInput(value.isPressed);
        }
#endif


        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }
        public void MoveObjectInput(Vector2 newMoveDirection)
        {
            moveObject = newMoveDirection;
        }
        public void RotateObjectInput(float newRoteGrade)
        {
            roteObject = newRoteGrade;  
        }
        public void JumpInput(bool newJumpState)
        {
            jump = newJumpState;
        }
        public void ShootInput(bool newShootState)
        {
            shoot = newShootState;
        }
        public void StopShootInput(bool newShootState)
        {
            stopShoot = newShootState;
        }
        public void ShootCancelInput(bool newShootState)
        {
            shootCancel = newShootState;
        }

        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }
        public void MenuInput(bool newMenuState)
        {
            menu = newMenuState;
        }
        public void ChangeCameraInput(bool newMenuState)
        {
            changeCamera = newMenuState;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }

}