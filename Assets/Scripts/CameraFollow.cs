using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;
    private Vector3 tempPos;
    [SerializeField]
    private float minX, maxX; // +- 35;

    [SerializeField]
    private Sprite playerRadarSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("No GameObject with tag 'Player' found in the scene!");
        }
    }


    // run after all the updates are run;
    void LateUpdate()
    {
        if (!player) return;

        tempPos = transform.position;
        tempPos.x = player.position.x;

        if (tempPos.x < minX) tempPos.x = minX;
        if (tempPos.x > maxX) tempPos.x = maxX;

        transform.position = tempPos;
    }
}
