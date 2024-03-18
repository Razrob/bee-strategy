using Constructions.LevelSystemCore;

namespace Constructions
{
    public abstract class StorageBase : ConstructionBase, IEvolveConstruction
    {
        public abstract IConstructionLevelSystem LevelSystem { get; protected set; }
    }
}