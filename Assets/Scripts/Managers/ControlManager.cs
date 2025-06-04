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

    [Header("Ball attributes")]
    public float jumpMaxForce;
    public float jumpCooldown;
    public float pushMaxForce;
    public float pushCooldown;
    [Header("Swipe attributes")]
    [Tooltip("This variable offsets the speed and arrows for a more juiced experiance. The higher the number the weaker your have to swipe to see the ball move")]
    public float speedOffSet;
    #endregion

    private Vector2 firstPosition;
    private Vector2 secondPosition;

    private bool isBeingHeld;

    /// <summary>
    /// 0,0 = y 0,1 = x variable that holds last swipe speed
    /// </summary>
    public float[,] speed = { { 0, 0 } };
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

            //Get swipe force
            SwipeForce();
        }
    }

    #region swipe calculations
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
    /// A method to find the force of a swipe and returns a array reading y, x
    /// </summary>
    /// <returns></returns>
    public float[,] SwipeForce()
    {

        #region x
        //If first position x is bigger
        if (firstPosition.x > secondPosition.x)
        {
            //Find the difference
            speed[0, 1] = firstPosition.x - secondPosition.x;
        }
        //If second position x is bigger
        else if (secondPosition.x > firstPosition.x)
        {
            //Find the difference
            speed[0, 1] = secondPosition.x - firstPosition.x;
        }
        //If it's equal
        else
        {
            //if it's equal give it 0
            speed[0, 1] = 0;
        }
        #endregion

        #region y
        //If first position y is bigger
        if (firstPosition.y > secondPosition.y)
        {
            //Find the difference
            speed[0, 0] = firstPosition.y - secondPosition.y;
        }
        //If second position y is bigger
        else if (secondPosition.y > firstPosition.y)
        {
            //Find the difference
            speed[0, 0] = secondPosition.y - firstPosition.y;
        }
        //If it's equal
        else
        {
            //Give it 0
            speed[0, 0] = 0;
        }
        #endregion
        //This little conversion juices the speed value very nicely so that a user can see a result with a light swipe
        speed[0, 0] = speed[0, 0] * speedOffSet;
        speed[0, 1] = speed[0, 1] * speedOffSet;
        //Return the speed value
        return speed;
    }
    #endregion
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
