using Constructions.LevelSystemCore;

namespace Constructions
{
    public abstract class AntWorkshopBase : ConstructionBase, IEvolveConstruction
    {
        public abstract IConstructionLevelSystem LevelSystem { get; protected set; }
    }
}