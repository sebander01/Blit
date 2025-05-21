using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.EnhancedTouch;

public class ControlManager : MonoBehaviour
{
    #region PlayerControls
    [HideInInspector]
    public PlayerControls PlayerControls;
    [HideInInspector]
    public InputAction singleTouch;
    [HideInInspector]
    public InputAction singleClick;

    public float jumpForce;
    public float jumpCooldown;
    #endregion

    private Vector2 firstPosition;
    private Vector2 secondPosition;

    private bool isBeingHeld;
    public enum TouchDirection
    {
        Unset,
        Up,
        Down,
        Left,
        Right
    }

    private TouchDirection[,] currentDirection = { { TouchDirection.Unset, TouchDirection.Unset} };

    #region Instance declaration
    public static ControlManager Instance
    {
        get;
        private set;
    }
    #endregion

    private void Awake()
    {
        #region instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion

        PlayerControls = new PlayerControls();
        singleTouch = PlayerControls.BlitControls.Touch;
        singleClick = PlayerControls.BlitControls.TouchClick;
        isBeingHeld = false;
    }

    private void OnEnable()
    {
        PlayerControls.Enable();
        singleTouch.Enable();
        singleClick.Enable();
    }

    private void OnDisable()
    {
        PlayerControls.Disable();
        singleTouch.Disable();
        singleClick.Disable();
    }

    public void Update()
    {
        //If we are seeing a first touch
        if (singleClick.IsPressed() && !isBeingHeld)
        {
            //Collect the first direction
            firstPosition = TranslateTouchPosition();

            //Set our is being held varaible to true to prevent resetting our first position too soonf
            isBeingHeld = true;
        }

        //When that touch releases
        if (singleClick.WasReleasedThisFrame())
        {
            //Collect the final position
            secondPosition = TranslateTouchPosition();

            //Set our being held variable to false we are resetting this check
            isBeingHeld = false;
        }
    }

    /// <summary>
    /// Using touch input we can detect a swipe and it's direction and return that direction as a 2D array where [0,0] is y and [0,1] is x
    /// </summary>
    public TouchDirection[,] DetectSwipe()
    {
         //If first position y is less then second position y
         if(firstPosition.y < secondPosition.y)
         {
            //The swipe direction is up
            currentDirection[0,0] = TouchDirection.Up;
         }
         //If second position y is less than first position
         else if (secondPosition.y < firstPosition.y)
         {
            //The swipe direction is down
            currentDirection[0,0] = TouchDirection.Down;
         }
         //if y is equal then unassign it
         else if(secondPosition.y == firstPosition.y)
         {
            //Set direction
            currentDirection[0,0] = TouchDirection.Unset;
         }

         //If first position x is less then second position x
         if (firstPosition.x < secondPosition.x)
         {
            //The swipe direction is right
            currentDirection[0,1] = TouchDirection.Right;
         }
         //If second position x is less than first position x
         else if (secondPosition.x < firstPosition.x)
         {
            //The swipe direction is left
            currentDirection[0,1] = TouchDirection.Left;
         }
        //If x is equal then unassign it
        else if(secondPosition.x == firstPosition.x)
        {
            //Set direction
            currentDirection[0,1] = TouchDirection.Unset;
        }

        //Return direction
        return currentDirection;
    }

    /// <summary>
    /// Translates the touch position from singleTouch.read<vector2> to ScreenToWorldPoint(). This method requires no variables as the variables it would need are in the manager the methods in
    /// </summary>
    /// <returns></returns>
    public Vector2 TranslateTouchPosition()
    {
        //Collect the position of the touch
        Vector2 touchPosition = singleTouch.ReadValue<Vector2>();
        //Translate to screen space to world space
        touchPosition = Camera.main.ScreenToWorldPoint(touchPosition);

        return touchPosition;
    }
}
