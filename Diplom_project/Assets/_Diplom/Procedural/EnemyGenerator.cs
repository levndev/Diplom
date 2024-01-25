using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private int enemyCount;
    [SerializeField] private int patrolLength;

    public List<GameObject> Generate(List<Vector3Int> localMaximums, NavGraph navGraph)
    {
        var result = new List<GameObject>();
        localMaximums = localMaximums.ToList();

        Debug.Log(string.Format("{0} local maximums", localMaximums.Count));

        Vector3Int takeRandomPoint()
        {
            int index = Random.Range(0, localMaximums.Count);
            Vector3Int pt = localMaximums[index];
            localMaximums.RemoveAt(index);
            return pt;
        }

        if (enemyPrefab != null)
        {
            for (int i = 0; i < enemyCount; i++)
            {
                List<Vector3Int> patrol = new();
                if (localMaximums.Count > patrolLength)
                {
                    for (int j = 0; j < patrolLength; j++)
                    {
                        patrol.Add(takeRandomPoint());
                    }
                }
                else if (localMaximums.Count > 1)
                {
                    int count = localMaximums.Count;
                    for (int j = 0; j < count; j++)
                    {
                        patrol.Add(takeRandomPoint());
                    }
                }
                else
                {
                    break;
                }
                var enemy = Instantiate(enemyPrefab);
                enemy.transform.position = patrol[0];
                enemy.GetComponent<NavAgent>().SetNavGraph(navGraph);
                enemy.GetComponent<BasicAgentBehaviour>().SetPatrol(patrol);
                result.Add(enemy);
            }
        }


        return result;
    }

}
