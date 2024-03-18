using System;
using System.Collections.Generic;

namespace Poison
{
    public class PoisonFogsRepository
    {
        private readonly List<PoisonFog> _fogs = new List<PoisonFog>(8);

        public IReadOnlyList<PoisonFog> Fogs => _fogs;

        public void Add(PoisonFog fog)
        {
            fog.ElementReturnEvent += Remove;

            if (_fogs.Contains(fog))
                throw new Exception("Duplicate");
            
            _fogs.Add(fog);
        }

        public void Remove(PoisonFog fog)
            => _fogs.Remove(fog);
        
        public PoisonFog TryGet(Predicate<PoisonFog> predicate = null, bool remove = false) 
        {
            int index = _fogs.IndexOf(fog => fog.CastPossible<PoisonFog>() && (predicate is null || predicate(fog.Cast<PoisonFog>())));

            if (index is -1)
                return default;

            PoisonFog poisonFog = _fogs[index].Cast<PoisonFog>();

            if (remove)
                _fogs.RemoveAt(index);

            return poisonFog;
        }
    }
}