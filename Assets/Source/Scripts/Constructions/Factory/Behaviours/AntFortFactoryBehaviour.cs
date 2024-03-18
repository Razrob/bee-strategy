using Constructions;
using Zenject;

public class AntFortFactoryBehaviour : ConstructionFactoryBehaviourBase
{
    [Inject] private readonly AntFortSpawnConfig _config;
    
    public override ConstructionType ConstructionType => ConstructionType.AntFort;

    public override TConstruction Create<TConstruction>(ConstructionID constructionID)
    {
        ConstructionSpawnConfiguration<AntFort> configuration = _config.Configuration;

        TConstruction construction = DiContainer.InstantiatePrefab(configuration.ConstructionPrefab,
                configuration.ConstructionPrefab.transform.position, configuration.Rotation, null)
            .GetComponent<TConstruction>();
        
        return construction;
    }
}
