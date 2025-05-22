using System.Collections;
using UnityEngine;

public class BallControls : MonoBehaviour
{
    private Rigidbody2D rb;
    private ControlManager.TouchDirection[,] direction;

    private bool canJump = true;
    private bool touchingBall = false;
    private bool canLunge = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Assign the objects ridged body
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(ControlManager.Instance.singleClick.WasPressedThisFrame())
        {
            if (TouchingBall(ControlManager.Instance.TranslateTouchPosition()))
            {
                touchingBall = true;
            }
        }
        if (ControlManager.Instance.singleClick.WasReleasedThisFrame())
        {
            //Detect the swipe direction
            direction = ControlManager.Instance.DetectSwipe();

            //Check if this ball can be moved
            if(touchingBall)
            {
                touchingBall = false;
                //Jump if possible
                StartCoroutine(BallJump());
                //Ball left or right
                StartCoroutine(BallLeftRight());
            }
        }
    }
    #region ballMovement
    /// <summary>
    /// Allows the player ball to jump
    /// </summary>
    public IEnumerator BallJump()
    {   
        //If swiped up
        if (direction[0, 0] == ControlManager.TouchDirection.Up && canJump)
        {
            canJump = false;

            //If we haven't passed max jump force
            if (ControlManager.Instance.speed[0,0] < ControlManager.Instance.jumpMaxForce)
            {
                //Add an upward force
                rb.AddForce(new Vector2(0, ControlManager.Instance.speed[0, 0]), ForceMode2D.Impulse);
            }
            else
            {
                //Add an upward force using the max speed
                rb.AddForce(new Vector2(0, ControlManager.Instance.jumpMaxForce), ForceMode2D.Impulse);
            }
            
            //Wait for our cool down
            yield return new WaitForSecondsRealtime(ControlManager.Instance.jumpCooldown);
            //Restore jumping ability
            canJump = true;
        }
    }

    /// <summary>
    /// This allows the ball to pull left or right
    /// </summary>
    /// <returns></returns>
    public IEnumerator BallLeftRight()
    {
        //If swipped right
        if (direction[0,1] == ControlManager.TouchDirection.Right && canLunge)
        {
            //Stop lunging so we can cool down
            canLunge = false;

            //If we haven't passed the max force
            if (ControlManager.Instance.speed[0, 1] < ControlManager.Instance.pushMaxForce)
            {
                rb.AddForce(new Vector2(ControlManager.Instance.speed[0, 1], 0), ForceMode2D.Impulse);
            }
            else
            {
                //Add a force right
                rb.AddForce(new Vector2(ControlManager.Instance.pushMaxForce, 0), ForceMode2D.Impulse);
            }

            //wait for cool down
            yield return new WaitForSecondsRealtime(ControlManager.Instance.pushCooldown);
            //Restore push force
            canLunge = true;
        }

        //If swipped left
        if (direction[0,1] == ControlManager.TouchDirection.Left && canLunge)
        {
            //Stop lunging so we can cool down
            canLunge = false;

            //If we haven't passed the max force
            if (ControlManager.Instance.speed[0, 1] < ControlManager.Instance.pushMaxForce)
            {
                rb.AddForce(new Vector2(-ControlManager.Instance.speed[0, 1], 0), ForceMode2D.Impulse);
            }
            else
            {
                //Add a force right
                rb.AddForce(new Vector2(-ControlManager.Instance.pushMaxForce, 0), ForceMode2D.Impulse);
            }

            //Wait for cool down
            yield return new WaitForSecondsRealtime(ControlManager.Instance.pushCooldown);
            //Restore push force
            canLunge = true;
        }
    }
    #endregion
    /// <summary>
    /// Checks if this ball is being pressed
    /// </summary>
    private bool TouchingBall(Vector2 target)
    {
        RaycastHit2D hit = Physics2D.Raycast(target, this.transform.position);
        if (hit.collider.gameObject.name == this.gameObject.name)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}
