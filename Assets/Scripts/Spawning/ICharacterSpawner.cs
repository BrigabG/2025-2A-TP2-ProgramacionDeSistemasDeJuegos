public interface ICharacterSpawner : ISetup<CharacterSpawnerModel>
{
    void Spawn(CharacterConfig config);
}
