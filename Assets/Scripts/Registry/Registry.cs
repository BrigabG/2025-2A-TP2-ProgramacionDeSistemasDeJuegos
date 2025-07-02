using System.Collections.Generic;
using UnityEngine;

public abstract class Registry<T> : MonoBehaviour where T : Registry<T>
{
    static readonly List<T> _instances = new List<T>();
    public static IReadOnlyList<T> Instances => _instances;

    protected virtual void OnEnable()  => _instances.Add((T)this);
    protected virtual void OnDisable() => _instances.Remove((T)this);
}