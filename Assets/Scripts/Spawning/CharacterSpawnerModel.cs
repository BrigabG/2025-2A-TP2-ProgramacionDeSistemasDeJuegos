using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Spawning/Character Spawner Model")]
public class CharacterSpawnerModel : ScriptableObject
{
    [Serializable]
    public struct SpawnPoint
    {
        public Vector3 localPosition;
        public Vector3 localEulerRotation;
    }

    [SerializeField] SpawnPoint[] spawnPoints =
    {
        new SpawnPoint { localPosition = Vector3.zero, localEulerRotation = Vector3.zero }
    };

    [SerializeField] bool useRandomOrder;
    [SerializeField] bool randomizeYaw;
    [SerializeField] Vector2 randomYawRange = new Vector2(0f, 360f);

    public SpawnPoint[] SpawnPoints => spawnPoints;
    public bool UseRandomOrder => useRandomOrder;
    public bool RandomizeYaw => randomizeYaw;
    public Vector2 RandomYawRange => randomYawRange;
}