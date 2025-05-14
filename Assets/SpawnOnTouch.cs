using UnityEditor.DeviceSimulation;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.iOS;
using UnityEngine.Rendering.Universal;

public class SpawnOnTouch : MonoBehaviour
{
    public GameObject shapeToSpawn;
    public int ballCount;
    private bool canSpawn;
    public bool CanSpawn
    {
        get => canSpawn;
        set { 
            canSpawn = value; 
        }
    }
    #region PlayerControls
    private PlayerControls PlayerControls;
    private InputAction singleTouch;
    private InputAction singleClick;
    #endregion 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CanSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(singleClick.WasPressedThisFrame())
        {
            SpawnObject();
        }

        if(singleClick.WasReleasedThisFrame())
        {
            canSpawn = true;
        }
    }

    private void Awake()
    {
        PlayerControls = new PlayerControls();
        singleTouch = PlayerControls.BlitControls.Touch;
        singleClick = PlayerControls.BlitControls.TouchClick;
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

    /// <summary>
    /// Spawns our player object balls
    /// </summary>
    private void SpawnObject()
    {
            //If we are allowed to spawn and we have and inventory of balls left
            if (CanSpawn && ballCount > 0)
            {
                //We can't spawn
                CanSpawn = false;
                //Collect the position of the touch
                Vector2 touchPosition = singleTouch.ReadValue<Vector2>();
                //Translate to screen space to world space
                touchPosition = Camera.main.ScreenToWorldPoint(touchPosition);
                
                
                //Transform the object variable position based on the translated screen space
                shapeToSpawn.transform.position = new Vector2(touchPosition.x, touchPosition.y);
                //Validate the screen space
                if (ValidatePlayArea(shapeToSpawn.transform))
                {
                    //Spawn the object
                    Instantiate(shapeToSpawn);
                    ballCount--;
                }
                else
                {
                    Debug.Log("Not in play area");
                }
            }
    }

    /// <summary>
    /// Checks if an object is being spawned somewhere that is valid. The target is the game object that would be summoned if allowed
    /// </summary>
    private bool ValidatePlayArea(Transform target)
    {
        RaycastHit2D hit = Physics2D.Raycast(target.position, this.transform.position);
        if (hit.collider.tag == "PlayZone")
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }
}
