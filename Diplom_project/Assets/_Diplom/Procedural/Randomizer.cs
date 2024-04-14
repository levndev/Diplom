using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Randomizer : MonoBehaviour
{
    [SerializeField] private List<GameObject> possibleLocations;
    [SerializeField] private GameObject prefab;
    [SerializeField] private int spawnCount;
    [SerializeField] private bool parentObjects;
    // Start is called before the first frame update
    void Start()
    {
        var _possibleLocations = possibleLocations.ToList();
        if (_possibleLocations.Count == 0)
            return;
        if (spawnCount == 0)
            spawnCount = 1;
        for (int i = 0; i < spawnCount; i++)
        {
            int random = Random.Range(0, _possibleLocations.Count);
            if (random >= 0 && random < _possibleLocations.Count)
            {
                if (parentObjects)
                {
                    var newObject = Instantiate(prefab, _possibleLocations[random].transform);
                    newObject.transform.localPosition = Vector3.zero;
                    newObject.transform.localRotation = Quaternion.identity;
                }
                else
                {
                    var newObject = Instantiate(prefab);
                    newObject.transform.position = _possibleLocations[random].transform.position;
                    newObject.transform.rotation = _possibleLocations[random].transform.rotation;
                }
                _possibleLocations.RemoveAt(random);
            }
        }
    }
}
