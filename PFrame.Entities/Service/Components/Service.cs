using Unity.Collections;
using Unity.Entities;

namespace PFrame.Entities
{
    public struct Service : IComponentData
    {
        //public NativeString32 Id;
        public byte Id;

        //public NativeString32 StageId;
        public byte StageId;
        public Entity StageEntity;

        //public NativeString32 NextStageId;
        public byte NextStageId;
    }

    public struct LoadStageCmd : IComponentData
    {
        //public NativeString32 StageId;
        public byte StageId;
    }

    public struct UnloadStageCmd : IComponentData
    {
    }
}