using Unity.Collections;
using Unity.Entities;

namespace PFrame.Entities
{
    public struct Service : IComponentData
    {
        //public NativeString32 Id;
        public int Id;

        //public NativeString32 StageId;
        public int StageId;
        public Entity StageEntity;

        //public NativeString32 NextStageId;
        public int NextStageId;
    }

    public struct LoadStageCmd : IComponentData
    {
        //public NativeString32 StageId;
        public int StageId;
    }

    public struct UnloadStageCmd : IComponentData
    {
    }
}