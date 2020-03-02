using System.Collections.Generic;
using UnityEngine;

namespace PFrame.Entities.Authoring
{
    public class PrefabDataSOListAuthoring : AGameDataSOListAuthoring<PrefabData, PrefabDataSO>
    {
        public override void DeclareReferencedPrefab(List<GameObject> referencedPrefabs, PrefabDataSO item)
        {
            referencedPrefabs.Add(item.Prefab);
        }

        public override PrefabData GetGameData(GameObjectConversionSystem conversionSystem, PrefabDataSO item)
        {
            var data = new PrefabData();
            data.Id = item.Id;
            data.Name = item.Name;
            data.Prefab = conversionSystem.GetPrimaryEntity(item.Prefab);

            return data;
        }
    }
}
