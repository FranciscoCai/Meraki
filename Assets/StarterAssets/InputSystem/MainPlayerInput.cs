using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainPlayerInput : MonoBehaviour
{
    public delegate void ShotEvent();
    public ShotEvent OnBeginNormalShootEfect;
    public ShotEvent OnEndNormalShootEfect;
    public ShotEvent OnBeginCheckMoveObjectEfect;
    public ShotEvent OnBeginCancelMoveObjectEfect;
    public delegate void TurnEvent();
    public TurnEvent OnPassTurnEfect;
    [Header("Character Input Values")]
    public Vector2 Move;
    public Vector2 Look;

    public Vector2 MoveObject;
    public float RoteObject;

    public bool Jump;

    public bool Shoot;
    public bool StopShoot = false;
    public bool ShootCancel;

    public bool Sprint;
    public bool Menu;
    public bool ChangeCamera;

    [Header("Movement Settings")]
    public bool AnalogMovement;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;


    private IA_Game m_Actions;

    private void Awake()
    {
        m_Actions = new IA_Game();
    }
    void Start()
    {
        SetCursorState(cursorLocked);
    }
    //private void OnApplicationFocus(bool hasFocus)
    //{
    //    SetCursorState(cursorLocked);
    //}
    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }

    private void OnEnable()
    {
        m_Actions.Player.Enable();
        m_Actions.Player.Jump.started += OnJumping;
        m_Actions.Player.Shoot.started += OnBeginShot;
        m_Actions.Player.Shoot.canceled += OnEndShot;
        m_Actions.Player.PassTurn.started += ToPassTurn;

        m_Actions.Player.CheckShoot.started += OnBeginCheckShoot;
        m_Actions.Player.ShootCancel.started += OnBeginCancelShoot;


        m_Actions.Player.Sprint.started += OnBeginSprint;
        m_Actions.Player.Sprint.canceled += OnEndSprint;
        m_Actions.Player.Menu.started += OnBeginMenu;
        m_Actions.Player.Menu.canceled += OnEndMenu;
        m_Actions.Player.Menu.canceled += OnBeginChangeCamera;
    }



    private void OnDisable()
    {
        m_Actions.Player.Disable();
        m_Actions.Player.Jump.started -= OnJumping;
        m_Actions.Player.Shoot.started -= OnBeginShot;
        m_Actions.Player.Shoot.canceled -= OnEndShot;
        m_Actions.Player.PassTurn.started -= ToPassTurn;

        m_Actions.Player.CheckShoot.started -= OnBeginCheckShoot;
        m_Actions.Player.ShootCancel.started -= OnBeginCancelShoot;

        m_Actions.Player.Sprint.started -= OnBeginSprint;
        m_Actions.Player.Sprint.canceled -= OnEndSprint;
        m_Actions.Player.Menu.started -= OnBeginMenu;
        m_Actions.Player.Menu.canceled -= OnEndMenu;
        m_Actions.Player.Menu.canceled -= OnBeginChangeCamera;
    }


    private void Update()
    {
        Move = m_Actions.Player.Move.ReadValue<Vector2>();

        if (cursorInputForLook)
        {
            Look = m_Actions.Player.Look.ReadValue<Vector2>();
        }
        MoveObject = m_Actions.Player.MoveObject.ReadValue<Vector2>();
        RoteObject = m_Actions.Player.RotateObject.ReadValue<float>();
    }

    private void OnJumping(InputAction.CallbackContext context)
    {
        Jump = true;
    }
    private void ToPassTurn(InputAction.CallbackContext context)
    {
        OnPassTurnEfect?.Invoke();
    }

    private void OnBeginShot(InputAction.CallbackContext context)
    {
        Shoot = true;
        OnBeginNormalShootEfect?.Invoke();
    }

    private void OnEndShot(InputAction.CallbackContext context)
    {
        Shoot = false;
        OnEndNormalShootEfect?.Invoke();
    }

    private void OnBeginCheckShoot(InputAction.CallbackContext context)
    {
        OnBeginCheckMoveObjectEfect?.Invoke();
    }
    private void OnBeginCancelShoot(InputAction.CallbackContext context)
    {
        OnBeginCancelMoveObjectEfect?.Invoke();
    }

    private void OnBeginSprint(InputAction.CallbackContext context)
    {
        Sprint = true;
    }
    private void OnEndSprint(InputAction.CallbackContext context)
    {
        Sprint = false;
    }
    private void OnBeginMenu(InputAction.CallbackContext context)
    {
        Menu = true;
    }
    private void OnEndMenu(InputAction.CallbackContext context)
    {
        Menu = false;
    }
    private void OnBeginChangeCamera(InputAction.CallbackContext context)
    {
        ChangeCamera = true;
    }
}
