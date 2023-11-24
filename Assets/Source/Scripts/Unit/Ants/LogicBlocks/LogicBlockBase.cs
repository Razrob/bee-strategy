public abstract class LogicBlockBase
{
    protected readonly UnitVisibleZone VisibleZone;
    protected readonly float ReactionDistance;
    
    protected IUnitTarget Target;
    
    protected LogicBlockBase(UnitVisibleZone unitVisibleZone, float reactionDistance)
    {
        VisibleZone = unitVisibleZone;
        ReactionDistance = reactionDistance;

        VisibleZone.EnterEvent += OnEnterTargetInVisibleZone;
        VisibleZone.ExitEvent += OnExitTargetInVisibleZone;
    }
    
    protected LogicBlockBase(UnitVisibleZone unitVisibleZone, float reactionDistance, IUnitTarget target)
    {
        VisibleZone = unitVisibleZone;
        ReactionDistance = reactionDistance;
        Target = target;
        
        VisibleZone.EnterEvent += OnEnterTargetInVisibleZone;
        VisibleZone.ExitEvent += OnExitTargetInVisibleZone;
    }
    
    protected abstract void OnEnterTargetInVisibleZone(IUnitTarget target);
    protected abstract void OnExitTargetInVisibleZone(IUnitTarget target);
    
    /// <summary>
    /// Check targets in visible zone
    /// </summary>
    public abstract void HandleUpdate( );
    
    /// <summary>
    /// Set target for unit
    /// </summary>
    /// <param name="newTarget"></param>
    /// <returns> return true, if distance for a target less then distance reaction, else return false </returns>
    public abstract bool GiveOrder(IUnitTarget newTarget);
}