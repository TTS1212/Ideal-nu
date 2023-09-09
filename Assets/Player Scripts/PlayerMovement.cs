using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.InputSystem.Users;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
{
    
    // Physics
    
    [SerializeField] float _speed;
    private Rigidbody2D _rigidbody2D;
    
    // Input
    
    private Vector2 _movementInput;
    private MainInput _input;
    
    // Events (Broadcasters)
    
    public static event Action _confirmed ;
    public static event Action _canceledconfirmed;
    
    // Visuals

    private Animator _animator;
    
    // Params

    public static string device;
    
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _input = new MainInput();

    }

    private void Start()
    {
        _animator = transform.GetChild(0).GetComponent<Animator>();
        device = "Keyboard";
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.Player.Fire.performed += ConfirmedFunction;
        _input.Player.Fire.canceled += CanceledConfirmedFunction;
    }

    private void OnDisable()
    {
        _input.Disable();
        _input.Player.Fire.performed -= ConfirmedFunction;
        _input.Player.Fire.canceled -= CanceledConfirmedFunction;
    }

    private void Update()
    {
        InputUser.onChange += (user, change, device_) =>
        {
            if (change == InputUserChange.ControlSchemeChanged)
            {
                Debug.Log("User switched control scheme to " + user.controlScheme);

                if (user.controlScheme.ToString() == "Gamepad(<Gamepad>)")
                {
                    Debug.Log("to Gamepad");
                    device = "Gamepad";
                }
                if (user.controlScheme.ToString() == "Keyboard&Mouse(<Keyboard>,<Mouse>)")
                {
                    Debug.Log("to Keyboard");
                    device = "Keyboard";
                }
            }
        };
        if (DialogueManager.GetInstance()._dialogueIsPlaying || PauseManager.paused)
        {
            _animator.SetFloat("Speed", 0.00f);
            return;
        }
        
        _animator.SetFloat("Horizontal", _movementInput.x);
        _animator.SetFloat("Vertical", _movementInput.y);
        _animator.SetFloat("Speed", _movementInput.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        if (DialogueManager.GetInstance()._dialogueIsPlaying || PauseManager.paused)
        {
            _rigidbody2D.velocity = new Vector2(0, 0);
            return;
        }
        
        _rigidbody2D.velocity = _movementInput * _speed;
    }

    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }

    private void ConfirmedFunction(InputAction.CallbackContext value)
    {
        _confirmed?.Invoke();
    }
    
    private void CanceledConfirmedFunction(InputAction.CallbackContext value)
    {
        _canceledconfirmed?.Invoke();
    }
}
