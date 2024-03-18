using Constructions;
using Zenject;

public class BeesTownHallFactoryBehaviour : ConstructionFactoryBehaviourBase
{
    [Inject] private readonly BeeTownHallSpawnConfig _config;
    
    public override ConstructionType ConstructionType => ConstructionType.BeeTownHall;

    public override TConstruction Create<TConstruction>(ConstructionID constructionID)
    {
        ConstructionSpawnConfiguration<BeeTownHall> configuration = _config.GetConfiguration();

        TConstruction construction = DiContainer.InstantiatePrefab(configuration.ConstructionPrefab,
                configuration.ConstructionPrefab.transform.position, configuration.Rotation, null)
            .GetComponent<TConstruction>();
        
        return construction;
    }
}
