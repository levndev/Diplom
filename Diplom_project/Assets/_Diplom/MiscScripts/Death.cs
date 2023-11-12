using UnityEngine;
using UnityEngine.Events;

public class Death : MonoBehaviour
{
    [SerializeField] private bool DestroyOnDeath = true;
    [SerializeField] private bool DeactivateOnDeath;

    public UnityEvent OnDeath;

    public void Die()
    {
        OnDeath?.Invoke();
        if (DeactivateOnDeath)
        {
            gameObject.SetActive(false);
        }
        if (DestroyOnDeath)
        {
            Destroy(gameObject);
        }
    }
}
