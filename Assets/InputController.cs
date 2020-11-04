using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
[Serializable]
public class  MoveInputEvent : UnityEvent<float, float> { } // created a public class that is a unity event. need this for events that take inputs(up to 4)

public class InputController : MonoBehaviour
{
    Controls controls; // 

    public MoveInputEvent MoveInputEvent; //declare an instance of the class
    
    
    
    
    private void Awake() //use this for anything the InputController class needs to do, itself.
    {
        controls = new Controls();
    }


    private void OnEnable() 
    {
        controls.Player.Enable();//enable action map
        controls.Player.Move.performed += OnMovePerformed;    
        controls.Player.Move.canceled += OnMovePerformed;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 moveInput= context.ReadValue<Vector2>();   //when OnMovePerformed is called it will pull this function
        MoveInputEvent.Invoke(moveInput.x, moveInput.y); 
        Debug.Log($"move input: { moveInput}");
    }
}
