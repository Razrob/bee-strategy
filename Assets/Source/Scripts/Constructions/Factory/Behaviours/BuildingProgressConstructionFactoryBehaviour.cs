using Zenject;

public class BuildingProgressConstructionFactoryBehaviour : ConstructionFactoryBehaviourBase
{
    [Inject] private readonly BuildingProgressConstructionConfig _constructionConfig;

    public override ConstructionType ConstructionType => ConstructionType.BuildingProgressConstruction;

    public override TConstruction Create<TConstruction>(ConstructionID constructionID)
    {
        ConstructionSpawnConfiguration<BuildingProgressConstruction> configuration = _constructionConfig.GetConfiguration();

        TConstruction construction = DiContainer.InstantiatePrefab(configuration.ConstructionPrefab,
                configuration.ConstructionPrefab.transform.position, configuration.Rotation, null)
            .GetComponent<TConstruction>();
        
        return construction;
    }
}
 