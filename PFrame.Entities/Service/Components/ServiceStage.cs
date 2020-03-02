using Unity.Collections;
using Unity.Entities;

namespace PFrame.Entities
{
    public struct ServiceStage : IComponentData
    {
        //public NativeString32 Id;
        public int Id;
        public Entity ServiceEntity;
    }
}