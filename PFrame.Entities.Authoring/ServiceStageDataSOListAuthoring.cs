using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace PFrame.Entities.Authoring
{
    [GameDataAuthoring(Name = "ServiceStageData")]
    public class ServiceStageDataSOListConverter : AGameDataSOListConverter<ServiceStageData, ServiceStageDataSO>
    {
        public override byte TypeId => (byte)ECommonGameDataType.ServiceStage;

        public override ServiceStageData ConvertGameData(GameObject gameObject, Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem, ServiceStageDataSO dataSO)
        {
            var data = new ServiceStageData();
            data.Id = dataSO.Id;
            data.Name = dataSO.Name;

            //data.StageType = dataSO.StageType;
            data.Prefab = conversionSystem.GetPrimaryEntity(dataSO.Prefab);

            //Debug.LogFormat("ServiceStageDataSOListConverter: {0}", dataSO.Name);

            return data;
        }

        public override void DeclareReferencedPrefab(List<GameObject> referencedPrefabs, ServiceStageDataSO dataSO)
        {
            referencedPrefabs.Add(dataSO.Prefab);
        }
    }

    public class ServiceStageDataSOListAuthoring : AGameDataSOListAuthoring<ServiceStageData, ServiceStageDataSO, ServiceStageDataSOListConverter>
    {
    }
}
