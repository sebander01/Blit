using System.Collections;
using UnityEngine;

public class BallControls : MonoBehaviour
{
    private Rigidbody2D rb;
    private ControlManager.TouchDirection[,] direction;

    private bool canJump = true;
    private bool touchingBall = false;

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
                StartCoroutine(LaunchBall());
            }
        }
    }

    /// <summary>
    /// Allows the player ball to jump
    /// </summary>
    public IEnumerator LaunchBall()
    {   
        //If swiped up
        if (direction[0, 0] == ControlManager.TouchDirection.Up && canJump)
        {
            canJump = false;
            //Add an upward force
            rb.AddForce(new Vector2(0, ControlManager.Instance.jumpForce), ForceMode2D.Impulse);
            //Wait for our cool down
            yield return new WaitForSecondsRealtime(ControlManager.Instance.jumpCooldown);
            //Restore jumping ability
            canJump = true;
        }
    }

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
