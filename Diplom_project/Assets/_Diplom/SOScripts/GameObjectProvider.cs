using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectProvider : MonoBehaviour
{
    [SerializeField] Variable<GameObject> targetVariable;

    private void Awake()
    {
        targetVariable.Value = gameObject;
    }
}
