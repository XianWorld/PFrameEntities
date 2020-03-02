using Unity.Collections;
using Unity.Entities;

namespace PFrame.Entities
{
    public struct NoneState : IComponentData { }

    #region Init/Release
    public struct InitCmd : IComponentData { }
    public struct InitEvent : IComponentData { }
    public struct InitState : IComponentData { }

    public struct InitedCmd : IComponentData { }
    public struct InitedEvent : IComponentData { }
    public struct InitedState : IComponentData { }

    public struct ReleaseCmd : IComponentData { }
    public struct ReleaseEvent : IComponentData { }
    public struct ReleaseState : IComponentData { }

    public struct ReleasedCmd : IComponentData { }
    public struct ReleasedEvent : IComponentData { }
    #endregion

    #region Load/Unload
    public struct LoadCmd : IComponentData { }
    public struct LoadEvent : IComponentData { }
    public struct LoadState : IComponentData { }

    public struct LoadedCmd : IComponentData { }
    public struct LoadedEvent : IComponentData { }
    public struct LoadedState : IComponentData { }

    public struct UnloadCmd : IComponentData { }
    public struct UnloadEvent : IComponentData { }
    public struct UnloadState : IComponentData { }

    public struct UnloadedCmd : IComponentData { }
    public struct UnloadedEvent : IComponentData { }

    public struct UnloadAllCmd : IComponentData { }
    #endregion

    #region Error
    public struct ErrorState : IComponentData
    {
        public NativeString128 ErrorMessage; 
    }
    public struct ErrorEvent : IComponentData
    {
        public NativeString128 ErrorMessage;
    }
    #endregion

    public struct UpdatedState : IComponentData { }

    public struct ValueChangedEvent : IComponentData { }
}