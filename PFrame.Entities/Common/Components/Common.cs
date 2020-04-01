using System;
using Unity.Collections;
using Unity.Entities;

namespace PFrame.Entities
{
    //public class EventComponentAttribute : Attribute
    //{
    //    public string Name;
    //}

    public struct NoneState : IComponentData { }
    public struct EnterNoneStateEvent : IComponentData { }
    public struct ExitNoneStateEvent : IComponentData { }

    #region Init/Release
    public struct InitCmd : IComponentData { }
    //[EventComponent]
    public struct InitState : IComponentData { }
    public struct EnterInitStateEvent : IComponentData { }
    public struct ExitInitStateEvent : IComponentData { }

    public struct InitedCmd : IComponentData { }
    //[EventComponent]
    public struct InitedState : IComponentData { }
    public struct EnterInitedStateEvent : IComponentData { }
    public struct ExitInitedStateEvent : IComponentData { }

    public struct ReleaseCmd : IComponentData { }
    //[EventComponent]
    public struct ReleaseState : IComponentData { }
    public struct EnterReleaseStateEvent : IComponentData { }
    public struct ExitReleaseStateEvent : IComponentData { }

    public struct ReleasedCmd : IComponentData { }
    //[EventComponent]
    public struct ReleasedState : IComponentData { }
    public struct EnterReleasedStateEvent : IComponentData { }
    public struct ExitReleasedStateEvent : IComponentData { }
    #endregion

    #region Load/Unload
    public struct LoadCmd : IComponentData { }
    //[EventComponent]
    public struct LoadState : IComponentData { }
    public struct EnterLoadStateEvent : IComponentData { }
    public struct ExitLoadStateEvent : IComponentData { }

    public struct LoadedCmd : IComponentData { }
    //[EventComponent]
    public struct LoadedState : IComponentData { }
    public struct EnterLoadedStateEvent : IComponentData { }
    public struct ExitLoadedStateEvent : IComponentData { }

    public struct UnloadCmd : IComponentData { }
    //[EventComponent]
    public struct UnloadState : IComponentData { }
    public struct EnterUnloadStateEvent : IComponentData { }
    public struct ExitUnloadStateEvent : IComponentData { }

    public struct UnloadedCmd : IComponentData { }
    //[EventComponent]
    public struct UnloadedState : IComponentData { }
    public struct EnterUnloadedStateEvent : IComponentData { }
    public struct ExitUnloadedStateEvent : IComponentData { }

    public struct UnloadAllCmd : IComponentData { }
    #endregion

    #region Error
    public struct ErrorState : IComponentData
    {
        public NativeString128 ErrorMessage; 
    }
    //[EventComponent]
    public struct EnterErrorStateEvent : IComponentData
    {
        public NativeString128 ErrorMessage;
    }
    public struct ExitErrorStateEvent : IComponentData { }
    #endregion

    public struct SelectedState : IComponentData { }
    public struct EnterSelectedStateEvent : IComponentData { }
    public struct ExitSelectedStateEvent : IComponentData { }

    public struct UpdatedState : IComponentData { }

    //[EventComponent]
    public struct ValueChangedEvent : IComponentData { }

    public enum EHAlignType : byte
    {
        Left,
        Middle,
        Right
    }
    public enum EVAlignType : byte
    {
        Bottom,
        Middle,
        Top
    }
}