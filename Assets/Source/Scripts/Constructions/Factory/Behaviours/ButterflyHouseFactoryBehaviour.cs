using Constructions;
using Zenject;

public class ButterflyHouseFactoryBehaviour : ConstructionFactoryBehaviourBase
{
    [Inject] private readonly ButterflyHouseSpawnConfig _config;
    
    public override ConstructionType ConstructionType => ConstructionType.ButterflyHouse;

    public override TConstruction Create<TConstruction>(ConstructionID constructionID)
    {
        ConstructionSpawnConfiguration<ButterflyHouse> configuration = _config.Configuration;

        TConstruction construction = DiContainer.InstantiatePrefab(configuration.ConstructionPrefab,
                configuration.ConstructionPrefab.transform.position, configuration.Rotation, null)
            .GetComponent<TConstruction>();
        
        return construction;
    }
}
