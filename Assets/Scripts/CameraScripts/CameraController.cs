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
        Move();
    }

    private void FixedUpdate()
    {
       
    }

    public void Move()
    {
        #region SetAverageCameraPosition
        if (balls.Count != 0 || balls.Count != 1)
        {
            ////Temp variables to hold our x and y sums
            //float sumOfPositionsx = 0;
            //float sumOfPositionsy = 0;

            ////For each ball in our list of balls
            //foreach (GameObject ball in balls)
            //{
            //    //Sum the x and y positions seperately
            //    sumOfPositionsx = sumOfPositionsx + ball.transform.position.x;
            //    sumOfPositionsy = sumOfPositionsy + ball.transform.position.y;
            //}

            ////Then are able to move the camera position depending on if it's x is an increase (right) or a decrease (Left)
            //if (sumOfPositionsx / balls.Count > lastPosition.x)
            //{
            //    thisCam.transform.position = new Vector3(thisCam.transform.position.x + camSpeed, thisCam.transform.position.y, thisCam.transform.position.z);
            //}
            //else if (sumOfPositionsx / balls.Count < lastPosition.x)
            //{
            //    thisCam.transform.position = new Vector3(thisCam.transform.position.x - camSpeed, thisCam.transform.position.y, thisCam.transform.position.z);
            //}

            ////Then we move the camera on the Y depending on if Y is an increase (Up) or a decrease (Down)
            //if (sumOfPositionsy / balls.Count > lastPosition.y)
            //{
            //    thisCam.transform.position = new Vector3(thisCam.transform.position.x, thisCam.transform.position.y + camSpeed, thisCam.transform.position.z);
            //}
            //else if (sumOfPositionsy / balls.Count < lastPosition.y)
            //{
            //    thisCam.transform.position = new Vector3(thisCam.transform.position.x, thisCam.transform.position.y - camSpeed, thisCam.transform.position.z);
            //}

            ////We use the sum of x and sum of y in a new vector2 (Sum x / ball.count, Sumy / ball count)
            //lastPosition = new Vector2(sumOfPositionsx / balls.Count, sumOfPositionsy / balls.Count);
        }
        if (balls.Count == 1)
        {
            ////Moving along the Y
            //if (lastPosition.y > thisCam.transform.position.y + offSety)
            //{
            //    //thisCam.position = Vector3.Slerp(thisCam.position, new Vector3(thisCam.transform.position.x, thisCam.transform.position.y + camSpeedy * Time.fixedUnscaledDeltaTime, thisCam.transform.position.z), ControlManager.Instance.speed[0, 0] * camSpeedy * Time.deltaTime);
            //    thisCam.position = Vector3.SmoothDamp(thisCam.position, new Vector3(thisCam.transform.position.x, thisCam.transform.position.y + camSpeedy * Time.fixedUnscaledDeltaTime, thisCam.transform.position.z), ref velocity, ControlManager.Instance.speed[0, 0] * camSpeedy * Time.deltaTime);
            //}
            //else if (lastPosition.y < thisCam.transform.position.y - offSety)
            //{
            //    //thisCam.position = Vector3.Slerp(thisCam.position, new Vector3(thisCam.transform.position.x, thisCam.transform.position.y - camSpeedy * Time.fixedUnscaledDeltaTime, thisCam.transform.position.z), ControlManager.Instance.speed[0, 0] * camSpeedy * Time.deltaTime);
            //    thisCam.position = Vector3.SmoothDamp(thisCam.position, new Vector3(thisCam.transform.position.x, thisCam.transform.position.y - camSpeedy * Time.fixedUnscaledDeltaTime, thisCam.transform.position.z), ref velocity, ControlManager.Instance.speed[0, 0] * camSpeedy * Time.deltaTime);
            //}

            //Then are able to move the camera position depending on if it's x is an increase (right) or a decrease (Left)
            if (lastPosition.x > thisCam.transform.position.x + offSetx)
            {
                //thisCam.position = Vector3.Slerp(thisCam.position, new Vector3(thisCam.position.x + camSpeedx * Time.fixedUnscaledDeltaTime, thisCam.position.y, thisCam.position.z), ControlManager.Instance.speed[0, 1] * camSpeedx * Time.deltaTime);
                velocity = new Vector3(balls[0].GetComponent<Rigidbody2D>().linearVelocityX + Time.deltaTime, 0, 0);
                thisCam.position = Vector3.SmoothDamp(thisCam.position, new Vector3(thisCam.position.x + camSpeedx * balls[0].GetComponent<Rigidbody2D>().linearVelocityX * Time.deltaTime, thisCam.position.y, thisCam.position.z), ref velocity, balls[0].GetComponent<Rigidbody2D>().linearVelocityX + camSpeedx * Time.deltaTime);
            }
            else if (lastPosition.x < thisCam.transform.position.x - offSetx)
            {
                //thisCam.position = Vector3.Slerp(thisCam.position, new Vector3(thisCam.position.x - camSpeedx * Time.fixedUnscaledDeltaTime, thisCam.position.y, thisCam.position.z), ControlManager.Instance.speed[0, 1] * camSpeedx * Time.deltaTime);
                //thisCam.position = Vector3.SmoothDamp(thisCam.position, new Vector3(thisCam.position.x - camSpeedx * Time.fixedUnscaledDeltaTime, thisCam.position.y, thisCam.position.z), ref velocity, ControlManager.Instance.speed[0, 1] * camSpeedx * Time.deltaTime);
                velocity = new Vector3(balls[0].GetComponent<Rigidbody2D>().linearVelocityX + Time.deltaTime, 0, 0);
                thisCam.position = Vector3.SmoothDamp(thisCam.position, new Vector3(thisCam.position.x - camSpeedx * balls[0].GetComponent<Rigidbody2D>().linearVelocityX * Time.deltaTime, thisCam.position.y, thisCam.position.z), ref velocity, balls[0].GetComponent<Rigidbody2D>().linearVelocityX - camSpeedx * Time.deltaTime);
            }

            lastPosition = new Vector2(balls[0].transform.position.x, balls[0].transform.position.y);
        }
        #endregion
    }
}
