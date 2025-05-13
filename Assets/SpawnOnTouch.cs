using UnityEditor.DeviceSimulation;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.EnhancedTouch;
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

    private void SpawnObject()
    {
            if (CanSpawn && ballCount > 0)
            {
                CanSpawn = false;
                Vector2 touchPosition = singleTouch.ReadValue<Vector2>();
                touchPosition = Camera.main.ScreenToWorldPoint(touchPosition);
                shapeToSpawn.transform.position = new Vector2(touchPosition.x, touchPosition.y);
                Instantiate(shapeToSpawn);
                ballCount--;
            }
    }
}
