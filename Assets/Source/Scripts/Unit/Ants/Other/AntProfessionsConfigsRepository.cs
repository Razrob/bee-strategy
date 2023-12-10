using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Unit.Ants
{
    [CreateAssetMenu(fileName = "AntProfessionsConfigsRepository", menuName = "Configs/Units/Ants/Ant Professions Configs Repository")]
    public class AntProfessionsConfigsRepository : ScriptableObject, ISingleConfig
    {
        [SerializeField] private SerializableDictionary
            <AntProfessionType, SerializableDictionary<AntProfessionRang, AntProfessionConfigBase>> data;

        public IReadOnlyDictionary<AntProfessionType, IReadOnlyDictionary<AntProfessionRang, AntProfessionConfigBase>>
            Configs => data.ToDictionary(pair => pair.Key,
            pair => pair.Value as IReadOnlyDictionary<AntProfessionRang, AntProfessionConfigBase>);
    }
}