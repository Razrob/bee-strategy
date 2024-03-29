using System;
using System.Collections.Generic;

namespace UnitsRecruitingSystemCore
{
    public interface IReadOnlyUnitsRecruiter
    {
        public event Action OnChange;
        public event Action OnRecruitUnit;
        public event Action OnAddStack;
        public event Action OnTick;
        public event Action OnCancelRecruit;

        public List<IReadOnlyUnitRecruitingStack> GetRecruitingInformation();
    }
}