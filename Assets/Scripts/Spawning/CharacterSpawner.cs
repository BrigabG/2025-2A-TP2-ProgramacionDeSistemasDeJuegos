using UnityEngine;

public class CharacterSpawner : MonoBehaviour, ICharacterSpawner, ISetup<CharacterSpawnerModel>
{
    [SerializeField] CharacterSpawnerModel startupModel;

    CharacterSpawnerModel model;
    int sequentialIndex;
    bool isInactive;

    void Awake()
    {
        if (!ServiceLocator.TryGet<IGameServices>(out var services))
        {
            isInactive = true;
            enabled = false;
            return;
        }

        if (services.CharacterSpawner != null && services.CharacterSpawner != this)
        {
            isInactive = true;
            enabled = false;
            return;
        }

        services.CharacterSpawner = this;
        if (startupModel)
            Setup(startupModel);
    }

    void OnDestroy()
    {
        if (isInactive)
            return;

        if (ServiceLocator.TryGet<IGameServices>(out var services) && services.CharacterSpawner == this)
            services.CharacterSpawner = null;
    }

    public void Setup(CharacterSpawnerModel value)
    {
        model = value;
        sequentialIndex = 0;
    }

    public void Spawn(CharacterConfig config)
    {
        if (isInactive || config == null)
            return;

        var pose = DeterminePose();
        var factory = ServiceLocator.Get<ICharacterAbstractFactory>();
        factory.Create(config, pose);
    }

    Pose DeterminePose()
    {
        if (!model || model.SpawnPoints == null || model.SpawnPoints.Length == 0)
            return new Pose(transform.position, transform.rotation);

        var point = SelectSpawnPoint();
        var position = transform.TransformPoint(point.localPosition);
        var rotation = transform.rotation * Quaternion.Euler(point.localEulerRotation);

        if (model.RandomizeYaw)
        {
            var yaw = Random.Range(model.RandomYawRange.x, model.RandomYawRange.y);
            rotation *= Quaternion.Euler(0f, 0f, yaw);
        }

        return new Pose(position, rotation);
    }

    CharacterSpawnerModel.SpawnPoint SelectSpawnPoint()
    {
        var points = model.SpawnPoints;
        if (model.UseRandomOrder)
        {
            var index = Random.Range(0, points.Length);
            return points[index];
        }

        var point = points[sequentialIndex];
        sequentialIndex = (sequentialIndex + 1) % points.Length;
        return point;
    }

}
