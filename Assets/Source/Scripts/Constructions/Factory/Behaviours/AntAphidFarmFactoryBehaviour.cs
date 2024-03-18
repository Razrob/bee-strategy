using Constructions;
using Zenject;

public class AntAphidFarmFactoryBehaviour : ConstructionFactoryBehaviourBase
{
    [Inject] private readonly AntAphidFarmSpawnConfig _config;
    
    public override ConstructionType ConstructionType => ConstructionType.AntAphidFarm;

    public override TConstruction Create<TConstruction>(ConstructionID constructionID)
    {
        ConstructionSpawnConfiguration<AntAphidFarm> configuration = _config.Configuration;

        TConstruction construction = DiContainer.InstantiatePrefab(configuration.ConstructionPrefab,
                configuration.ConstructionPrefab.transform.position, configuration.Rotation, null)
            .GetComponent<TConstruction>();
        
        return construction;
    }
}
