using System.Collections;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] monsterReference;

    private GameObject spawnedMonster;

    [SerializeField]
    private Transform leftPos, rightPos;

    private int randIndx, randSide;

    void Start()
    {
        StartCoroutine(SpawnMonsters());
    }

    public IEnumerator SpawnMonsters()
    {
        while (true)
        {            
            yield return new WaitForSeconds(Random.Range(4, 7));  //randomize the for a certain range sec

            randSide = Random.Range(0, 2);
            randIndx = Random.Range(0, monsterReference.Length);

            spawnedMonster = Instantiate(monsterReference[randIndx]);  //create a copy of a game object

            // create radar icon
            RadarImage radar = Object.FindFirstObjectByType<RadarImage>();
            radar.CreateRadarIcon(spawnedMonster.transform, randIndx, leftPos, rightPos);

            if (randSide == 0)
            {
                // left side
                spawnedMonster.transform.position = leftPos.position;
                spawnedMonster.GetComponent<Monster>().speed = Random.Range(4, 10);
            }
            else
            {
                // right side
                spawnedMonster.transform.position = rightPos.position;
                spawnedMonster.GetComponent<Monster>().speed = -Random.Range(4, 10);
                spawnedMonster.transform.localScale = new Vector3(-1f, 1f, 1f);
            }
        }
    }
    
}  // class