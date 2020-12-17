using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    public GameObject enemies;
    public int xPos;
    public int yPos;
    public int enemyCount;

    void Start()
    {
        StartCoroutine(Spawning());
    }

    IEnumerator Spawning()
    {
        while (enemyCount < 3)
        {
            xPos = Random.Range(-10, 3);
            yPos = Random.Range(2, 1);
            Instantiate(enemies, new Vector3(xPos, yPos, 0), Quaternion.identity);
            yield return new WaitForSeconds(2.0f);
            enemyCount += 1;
        }
    }
   
}
