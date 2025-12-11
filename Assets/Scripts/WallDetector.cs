using UnityEngine;

public class WallDetector : MonoBehaviour
{
    public bool onWall = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            onWall = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            onWall = false;
        }
    }
}