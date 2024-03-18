using Constructions;
using Zenject;

public class BeesWaxProduceConstructionFactoryBehaviour : ConstructionFactoryBehaviourBase
{
    [Inject] private readonly BeesWaxProduceConstructionSpawnConfig _beesWaxProduceConstructionConfig;

    public override ConstructionType ConstructionType => ConstructionType.BeeWaxProduceConstruction;

    public override TConstruction Create<TConstruction>(ConstructionID constructionID)
    {
        ConstructionSpawnConfiguration<BeesWaxProduceConstruction> configuration = _beesWaxProduceConstructionConfig.GetConfiguration();

        TConstruction construction = DiContainer.InstantiatePrefab(configuration.ConstructionPrefab,
                configuration.ConstructionPrefab.transform.position, configuration.Rotation, null)
            .GetComponent<TConstruction>();
        
        return construction;
    }
}
