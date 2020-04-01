using System;
using Unity.Collections;
using Unity.Entities;

namespace PFrame.Entities
{
    public struct FSMState : IComponentData
    {
        public byte State;
        public ComponentType StateType;
        public ComponentType EnterEventType;
        public ComponentType ExitEventType;
    }

    public struct SetFSMStateCmd : IComponentData
    {
        public byte State;
        public ComponentType StateType;
        public ComponentType EnterEventType;
        public ComponentType ExitEventType;
    }

    public struct FSMStateChangedEvent : IComponentData
    {
        public byte State;
        public byte OldState;
    }

    //public struct SetStateCmd : IComponentData
    //{
    //    public ComponentType NewStateType;
    //    public ComponentType EnterEventType;
    //    public ComponentType OldStateType;
    //}
}