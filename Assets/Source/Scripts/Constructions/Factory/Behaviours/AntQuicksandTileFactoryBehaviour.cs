using Constructions;
using Zenject;

public class AntQuicksandTileFactoryBehaviour : ConstructionFactoryBehaviourBase
{
    [Inject] private readonly AntQuicksandTileSpawnConfig _config;

    public override ConstructionType ConstructionType => ConstructionType.AntQuicksandTile;

    public override TConstruction Create<TConstruction>(ConstructionID constructionID)
    {
        ConstructionSpawnConfiguration<AntQuicksandTile> configuration = _config.Configuration;

        TConstruction construction = DiContainer.InstantiatePrefab(configuration.ConstructionPrefab,
                configuration.ConstructionPrefab.transform.position, configuration.Rotation, null)
            .GetComponent<TConstruction>();
        
        return construction;
    }
}
