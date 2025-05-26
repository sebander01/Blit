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
    private int spawnedBalls;
    public bool CanSpawn
    {
        get => canSpawn;
        set { 
            canSpawn = value; 
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CanSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(ControlManager.Instance.singleClick.WasPressedThisFrame())
        {
            SpawnObject();
        }

        if(ControlManager.Instance.singleClick.WasReleasedThisFrame())
        {
            canSpawn = true;
        }
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
                
                //Gets the touch position of the players hand
                Vector2 touchPosition = ControlManager.Instance.TranslateTouchPosition();


                //Transform the object variable position based on the translated screen space
                shapeToSpawn.transform.position = new Vector2(touchPosition.x, touchPosition.y);

                //Validate the screen space
                if (ValidatePlayArea(shapeToSpawn.transform))
                {
                    //Spawn the object
                    GameObject copy = Instantiate(shapeToSpawn);
                    copy.name = shapeToSpawn.name + spawnedBalls;
                    ballCount--;
                    spawnedBalls++;

                    //Add the ball to the camera controller so the camera will match it's position
                    try
                    {
                        GameObject.Find("CameraHolder").GetComponent<CameraController>().balls.Add(copy);
                    }
                    catch
                    {
                        Debug.Log("We couldn't find the camera");
                    }
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
