using UnityEngine;

public class Monster : MonoBehaviour
{
    [HideInInspector]
    public float speed;

    private Rigidbody2D myRb2D;

    public void Awake()
    {
        myRb2D = GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {
        //Also using addForce() to the rb;
        myRb2D.linearVelocity = new Vector2(speed, myRb2D.linearVelocityY);
    }

} // class
