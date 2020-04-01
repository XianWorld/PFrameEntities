using PFrame.Entities.Authoring;
using UnityEngine;

namespace PFrame.Entities.Authoring
{
    [CreateAssetMenu(fileName = "ServiceStageData_", menuName = "PFrame/GameData/ServiceStageData")]
    public class ServiceStageDataSO : AGameDataSO
    {
        public override byte DataType => (byte)ECommonGameDataType.ServiceStage;

        //public EStageType StageType;

        public GameObject Prefab;
    }
}
