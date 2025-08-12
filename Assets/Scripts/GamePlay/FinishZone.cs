using UnityEngine;

public class FinsihZone : MonoBehaviour
{
    public int scorePoints;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If finish zone collides with a ball
        if(collision.tag == "Ball")
        {
            //Destroy it
            Destroy(collision.gameObject);
            //Increase the point total
            GameManager.Instance.PointTotal += scorePoints;
            //Update the UI
            UIManager.Instance.UpdateTotalPoints();
            //Remove the object from the balls list to prevent errors
            GameObject.Find("Main Camera").GetComponent<CameraController>().balls.Remove(collision.gameObject);
        }
    }
}
