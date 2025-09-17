using UnityEngine;

public class Collector : MonoBehaviour
{
    public void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
    }
}
