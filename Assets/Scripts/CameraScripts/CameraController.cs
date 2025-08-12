using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{

    public List<GameObject> balls;
    public Transform thisCam;
    public float camSpeedx;
    public float camSpeedy;
    public float offSetx;
    public float offSety;
    public float minimumCameraZoom;
    public float maximumCameraZoom;
    private Vector3 velocity = Vector3.zero;
    private Vector3 averageSum;
    public float cameraZoomSpeed;
    public float zoomBuffer;
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
        //A catch to prevent an error when the list of balls is empty
        if(balls.Count != 0 || balls == null)
        {
            //If there are multiple balls in the scene start trying to average it
            #region camera position
            //For each ball in the scene
            foreach (GameObject ball in balls)
            {
                //Add the ball positions togeather
                averageSum += ball.transform.position;
                //Add velocity togeather we could care less about z it will be unchanged
                velocity.x += ball.GetComponent<Rigidbody2D>().linearVelocityX;
                velocity.y += ball.GetComponent<Rigidbody2D>().linearVelocityY;
            }
            //Average is sum / total count of the set
            averageSum = averageSum / balls.Count;
            velocity = velocity / balls.Count;

            //Change the camera position in smooth damp to have a smooth transition with camera position as the current, the average of x and y positions as the target, velocity holding linear velocity of x and y, smooth over time is x
            thisCam.position = Vector3.SmoothDamp(thisCam.position, new Vector3(averageSum.x, averageSum.y, thisCam.position.z), ref velocity, camSpeedx);

            //Reset average sum and velocity so we aren't constantly adding to a grander and grander sum
            averageSum = new Vector3(0, 0, 0);
            velocity = new Vector3(0, 0, 0);
            #endregion

            #region Camera Zoom Out and In

            //We need the edges of the screen
            Camera cam = thisCam.GetComponent<Camera>();
            //leftSide
            Vector3 leftCameraSide = cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, 0));
            //rightSide
            Vector3 rightCameraSide = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, 0));

            //For each and every ball
            foreach (GameObject ball in balls)
            {
                #region Zoom Out
                //If the ball goes off the left side of the camera
                if (ball.transform.position.x - zoomBuffer < leftCameraSide.x)
                {
                    ZoomOut();
                }

                //If the ball goes off the right side of the camera
                else if (ball.transform.position.x + zoomBuffer > rightCameraSide.x)
                {
                    ZoomOut();
                }
                #endregion

                #region Zoom In
                //If not on the right or left size if we can zoom in then zoom in
                else if (thisCam.GetComponent<Camera>().orthographicSize > minimumCameraZoom)
                {
                    //Use Mathf.Lerp to smoothly zoom the camera in by the orthographic size - cameraZoomSpeed at a speed of half of cameraZoomSpeed
                    thisCam.GetComponent<Camera>().orthographicSize = Mathf.Lerp(thisCam.GetComponent<Camera>().orthographicSize, thisCam.GetComponent<Camera>().orthographicSize - cameraZoomSpeed, cameraZoomSpeed / 2);
                }
                #endregion
            }
            #endregion
            #endregion
        }

    }

    /// <summary>
    /// A method to zoom the camera in checking to make sure we haven't passed the maximum camera zoom
    /// </summary>
    private void ZoomOut()
    {
        //If the orthographicSize is less then the maximum
        if(thisCam.GetComponent<Camera>().orthographicSize < maximumCameraZoom)
        {
            //Use Mathf.Lerp to smoothly zoom the camera out by the orthographic size + cameraZoomSpeed at a speed of half of cameraZoomSpeed
            thisCam.GetComponent<Camera>().orthographicSize = Mathf.Lerp(thisCam.GetComponent<Camera>().orthographicSize, thisCam.GetComponent<Camera>().orthographicSize + cameraZoomSpeed, cameraZoomSpeed / 2);
        }
    }
}
