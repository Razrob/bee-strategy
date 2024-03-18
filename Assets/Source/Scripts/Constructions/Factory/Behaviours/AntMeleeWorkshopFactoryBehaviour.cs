using Constructions;
using Zenject;

public class AntMeleeWorkshopFactoryBehaviour : ConstructionFactoryBehaviourBase
{
    [Inject] private readonly AntMeleeWorkshopSpawnConfig _config;
    
    public override ConstructionType ConstructionType => ConstructionType.AntMeleeWorkshop;

    public override TConstruction Create<TConstruction>(ConstructionID constructionID)
    {
        ConstructionSpawnConfiguration<AntMeleeWorkshop> configuration = _config.Configuration;

        TConstruction construction = DiContainer.InstantiatePrefab(configuration.ConstructionPrefab,
                configuration.ConstructionPrefab.transform.position, configuration.Rotation, null)
            .GetComponent<TConstruction>();
        
        return construction;
    }
}
