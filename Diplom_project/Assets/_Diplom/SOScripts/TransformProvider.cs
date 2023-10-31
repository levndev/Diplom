using UnityEngine;

public class TransformProvider : MonoBehaviour
{
    [SerializeField] private Variable<Transform> transformVariable;

    private void Awake()
    {
        transformVariable.Set(transform);
    }
}
