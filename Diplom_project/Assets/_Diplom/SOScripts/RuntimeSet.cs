using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class RuntimeSet<T> : ScriptableObject, IEnumerable<T> where T : UnityEngine.Object
{
    protected readonly HashSet<T> items = new();

    [SerializeField, RuntimeRO] private int count;

    public event Action OnAdded;
    public event Action OnRemoved;

    public int Count => items.Count;

    public virtual void Add(T item)
    {
        items.Add(item);
        count = items.Count;
        OnAdded?.Invoke();
    }

    public virtual void Remove(T item)
    {
        items.Remove(item);
        count = items.Count;
        OnRemoved?.Invoke();
    }

    public void DestroyAll()
    {
        foreach(var item in items.ToList())
        {
            Remove(item);
            Destroy(item);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneUnloaded += OnSceneChanged;
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnSceneChanged;
    }

    private void OnSceneChanged(Scene scene)
    {
        items.Clear();
    }

    public bool Contains(T item) => items.Contains(item);

    public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
