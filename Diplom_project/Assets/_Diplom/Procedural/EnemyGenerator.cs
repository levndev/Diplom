using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    public void Generate(List<Vector3Int> localMaximums, NavGraph navGraph)
    {
        localMaximums = localMaximums.ToList();

        Vector3Int takeRandomPoint()
        {
            int index = Random.Range(0, localMaximums.Count);
            Vector3Int pt = localMaximums[index];
            localMaximums.RemoveAt(index);
            return pt;
        }

        if (enemyPrefab != null)
        {
            var enemy = Instantiate(enemyPrefab);
            enemy.transform.position = takeRandomPoint();
            var pt1 = takeRandomPoint();
            var pt2 = takeRandomPoint();
            enemy.GetComponent<NavAgent>().SetNavGraph(navGraph);
            enemy.GetComponent<BasicAgentBehaviour>().SetPatrol(new() { pt1, pt2 });
        }
    }

}
