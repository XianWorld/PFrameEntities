using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

namespace PFrame.Entities
{
    [UpdateBefore(typeof(ServiceUpdateSystemGroup))]
    //[UpdateInGroup(typeof(SimulationSystemGroup))]
    public class SetFSMStateCmdSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((Entity entity, ref FSMState fsmState, ref SetFSMStateCmd setStateCmd) =>
            {
                var state = fsmState.State;
                var newState = setStateCmd.State;
                if (state == newState)
                    return;

                var newStateType = setStateCmd.StateType;
                var newEnterEventType = setStateCmd.EnterEventType;
                var newExitEventType = setStateCmd.ExitEventType;

                var oldStateType = fsmState.StateType;
                var oldEnterEventType = fsmState.EnterEventType;
                var oldExitEventType = fsmState.ExitEventType;

                //remove old state comp
                if(oldStateType.TypeIndex != 0)
                {
                    if (EntityManager.HasComponent(entity, oldStateType))
                        EntityManager.RemoveComponent(entity, oldStateType);
                    EntityManager.AddComponent(entity, oldExitEventType, false, true);
                }

                //add new sate comp
                if(newStateType.TypeIndex != 0)
                {
                    if (!EntityManager.HasComponent(entity, newStateType))
                    {
                        EntityManager.AddComponent(entity, newStateType);
                        EntityManager.AddComponent(entity, newEnterEventType, false, true);
                    }
                }

                fsmState.State = newState;
                fsmState.StateType = newStateType;
                fsmState.EnterEventType = newEnterEventType;
                fsmState.ExitEventType = newExitEventType;

                //add change event
                var changedEvent = new FSMStateChangedEvent
                {
                    State = newState,
                    OldState = state
                };
                EntityManager.AddComponentData(entity, changedEvent, false, true);

                //remove cmd
                EntityManager.RemoveComponent<SetFSMStateCmd>(entity);
            });
        }
    }
}