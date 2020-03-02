using PFrame.Entities;
using Unity.Collections;
using Unity.Entities;

namespace PFrame.Entities
{
    public struct PrefabData : IBufferElementData, IGameData
    {
        public int Id;
        public NativeString64 Name;
        public Entity Prefab;

        public int DataId => Id;
        public string DataName => Name.ToString();
    }
}