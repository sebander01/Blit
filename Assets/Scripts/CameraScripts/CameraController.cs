using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{

    public List<GameObject> balls;
    public Transform thisCam;
    private Vector2 lastPosition;
    public float camSpeedx;
    public float camSpeedy;
    public float offSetx;
    public float offSety;
    private Vector3 velocity = Vector3.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        CameraMove();
    }

    private void FixedUpdate()
    {
       
    }

    /// <summary>
    /// This is a method that allows the camera to move if there is only 1 spawned ball it focuses the camera on just that ball but if we spawn more it averages a camera position
    /// </summary>
    public void CameraMove()
    {
        #region SetAverageCameraPosition
        //If there are multiple balls in the scene start trying to average it
        if (balls.Count != 0 || balls.Count != 1)
        {

        }
        //Otherwise if there is just 1 ball in the scene we need to focus on it
        if (balls.Count == 1)
        {
            #region Move camera along Y
            //Moving along the Y up
            //We make sure that the camera will factor for an offset
            if (lastPosition.y > thisCam.transform.position.y + offSety)
            {
                //Make the velocity go up by the speed of the y linear velocity
                velocity = new Vector3(0, balls[0].GetComponent<Rigidbody2D>().linearVelocityY + Time.deltaTime, 0);
                //SmoothDamp allows us to smoothly transition between directions. We give it a target that factors for the new position with camera speed and ball velocity so it can always keep up. Then for the speed with use a similar calculation again so it keeps up.
                thisCam.position = Vector3.SmoothDamp(thisCam.position, new Vector3(thisCam.transform.position.x, thisCam.position.y + camSpeedy + balls[0].GetComponent<Rigidbody2D>().linearVelocityY * Time.deltaTime, thisCam.transform.position.z), ref velocity, balls[0].GetComponent<Rigidbody2D>().linearVelocityY + camSpeedy * Time.deltaTime);
            }
            //Move along the Y down
            //We make sure the camera will factor for an offset
            else if (lastPosition.y < thisCam.transform.position.y - offSety)
            {
                velocity = new Vector3(0, balls[0].GetComponent<Rigidbody2D>().linearVelocityY - Time.deltaTime, 0);
                thisCam.position = Vector3.SmoothDamp(thisCam.position, new Vector3(thisCam.transform.position.x, thisCam.position.y - camSpeedy - balls[0].GetComponent<Rigidbody2D>().linearVelocityY * Time.deltaTime, thisCam.transform.position.z), ref velocity, -balls[0].GetComponent<Rigidbody2D>().linearVelocityY + camSpeedy * Time.deltaTime);
            }
            #endregion
            #region Move camera along x
            //Moving along the x right
            //We make sure that the new position will factor for an offset
            if (lastPosition.x > thisCam.transform.position.x + offSetx)
            {
                //This line fixes an issue where camera momentum sometimes carried over when going left specificially making it impossible to go left if you had already started moving right unless in a dead stop.
                //We make sure we are moving a + velocity
                //If we are moving a negative velocity we do nothing
                if (balls[0].GetComponent<Rigidbody2D>().linearVelocityX > 0)
                {
                    //Move right in a method for easy adjustments in the future since it's reused later
                    moveRight();
                }
            }
            //Moving along the x left
            //We make sure that the new position will factor for an offset
            else if (lastPosition.x < thisCam.transform.position.x - offSetx)
            {
                //This line fixes an issue where camera momentum sometimes carried over when going left specificially making it impossible to go left if you had already started moving right unless in a dead stop.
                //We make sure that we are moving a - velocity
                if (balls[0].GetComponent<Rigidbody2D>().linearVelocityX <= 0)
                {
                    velocity = new Vector3(balls[0].GetComponent<Rigidbody2D>().linearVelocityX - Time.deltaTime, 0, 0);
                    thisCam.position = Vector3.SmoothDamp(thisCam.position, new Vector3(thisCam.position.x - camSpeedx - balls[0].GetComponent<Rigidbody2D>().linearVelocityX * Time.deltaTime, thisCam.position.y, thisCam.position.z), ref velocity, -balls[0].GetComponent<Rigidbody2D>().linearVelocityX + camSpeedx * Time.deltaTime);
                }
                //If we are still moving a + velocity we keep moving the camera the right direction because it's not time to switch yet
                else
                {
                    //Move right in a method for easy adjustments as it was used in the past aswell
                    moveRight();
                }
            }
            #endregion
            //Save the last position after a new ones been calcualted
            lastPosition = new Vector2(balls[0].transform.position.x, balls[0].transform.position.y);
        }
        #endregion
    }

    /// <summary>
    /// A method to move the camera right smoothly with smooth damp
    /// </summary>
    private void moveRight()
    {
        //SmoothDamp allows us to smoothly transition between directions. We give it a target that factors for the new position with camera speed and ball velocity so it can always keep up. Then for the speed with use a similar calculation again so it keeps up.
        velocity = new Vector3(balls[0].GetComponent<Rigidbody2D>().linearVelocityX + Time.deltaTime, 0, 0);
        thisCam.position = Vector3.SmoothDamp(thisCam.position, new Vector3(thisCam.position.x + camSpeedx + balls[0].GetComponent<Rigidbody2D>().linearVelocityX * Time.deltaTime, thisCam.position.y, thisCam.position.z), ref velocity, balls[0].GetComponent<Rigidbody2D>().linearVelocityX + camSpeedx * Time.deltaTime);

    }
}
