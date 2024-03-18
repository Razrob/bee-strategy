using Constructions;
using Zenject;

public class AntMineFactoryBehaviour : ConstructionFactoryBehaviourBase
{
    [Inject] private readonly AntMineSpawnConfig _config;
    
    public override ConstructionType ConstructionType => ConstructionType.AntMine;

    public override TConstruction Create<TConstruction>(ConstructionID constructionID)
    {
        ConstructionSpawnConfiguration<AntMine> configuration = _config.Configuration;

        TConstruction construction = DiContainer.InstantiatePrefab(configuration.ConstructionPrefab,
                configuration.ConstructionPrefab.transform.position, configuration.Rotation, null)
            .GetComponent<TConstruction>();
        
        return construction;
    }
}
