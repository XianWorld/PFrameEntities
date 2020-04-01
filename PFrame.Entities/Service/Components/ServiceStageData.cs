using PFrame.Entities;
using Unity.Collections;
using Unity.Entities;

namespace PFrame.Entities
{
    public struct ServiceStageData : IBufferElementData, IGameData
    {
        public ushort Id;
        public NativeString32 Name;
        public ushort DataId => Id;
        public byte DataType => (byte)ECommonGameDataType.ServiceStage;
        public string DataName => Name.ToString();

        //public EStageType StageType;

        public Entity Prefab;
    }
}
