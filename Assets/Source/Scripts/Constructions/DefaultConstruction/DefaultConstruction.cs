using System.Collections.Generic;
using UnityEngine;

public class DefaultConstruction : ConstructionBase
{
    public override AffiliationEnum Affiliation => AffiliationEnum.None;
    public override ConstructionID ConstructionID => ConstructionID.TestConstruction;
}
