using Unity.Collections;
using Unity.Entities;

namespace PFrame.Entities
{
    public class CommonUtil
    {
        public static NativeString32 EmptyString32 = new NativeString32("");

        #region Common State
        //public static bool IsInState<TStateComp>(EntityManager entityManager, Entity entity)
        //    where TStateComp : IComponentData
        //{
        //    return entityManager.HasComponent<TStateComp>(entity);
        //}

        //public static void SetState<TStateComp, TEventComp, TOldStateComp>(EntityManager entityManager, Entity entity)
        //    where TStateComp : struct, IComponentData
        //    where TEventComp : struct, IComponentData
        //    where TOldStateComp : struct, IComponentData
        //{
        //    if (entityManager.HasComponent<TOldStateComp>(entity))
        //    {
        //        entityManager.RemoveComponent<TOldStateComp>(entity);
        //    }

        //    if (!entityManager.HasComponent<TStateComp>(entity))
        //    {
        //        if (typeof(TStateComp) != typeof(NoneState))
        //        {
        //            entityManager.AddComponent<TStateComp>(entity);
        //        }
        //        entityManager.AddComponent<TEventComp>(entity);
        //    }
        //}

        //public static void SetState<TStateComp, TEventComp, TOldStateComp>(EntityManager entityManager, EntityCommandBuffer commandBuffer, Entity entity)
        //    where TStateComp : struct, IComponentData
        //    where TEventComp : struct, IComponentData
        //    where TOldStateComp : struct, IComponentData
        //{
        //    if (entityManager.HasComponent<TOldStateComp>(entity))
        //    {
        //        commandBuffer.RemoveComponent<TOldStateComp>(entity);
        //    }

        //    if (!entityManager.HasComponent<TStateComp>(entity))
        //    {
        //        if (typeof(TStateComp) != typeof(NoneState))
        //        {
        //            commandBuffer.AddComponent<TStateComp>(entity);
        //        }
        //        commandBuffer.AddComponent<TEventComp>(entity);
        //    }
        //}

        //public static void SetState<TStateComp, TEventComp, TOldStateComp>(World world, Entity entity)
        //    where TStateComp : struct, IComponentData
        //    where TEventComp : struct, IComponentData
        //    where TOldStateComp : struct, IComponentData
        //{
        //    var entityManager = world.EntityManager;
        //    var ecbSystem = world.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        //    var commandBuffer = ecbSystem.CreateCommandBuffer();

        //    if (entityManager.HasComponent<TOldStateComp>(entity))
        //    {
        //        commandBuffer.RemoveComponent<TOldStateComp>(entity);
        //    }

        //    if (!entityManager.HasComponent<TStateComp>(entity))
        //    {
        //        if (typeof(TStateComp) != typeof(NoneState))
        //        {
        //            commandBuffer.AddComponent<TStateComp>(entity);
        //        }
        //        commandBuffer.AddComponent<TEventComp>(entity);
        //    }
        //}

        //public static void SetFSMStateCmd<TState, TEnterEvent, TOldState>(EntityManager entityManager, Entity entity)
        //    where TState : struct, IComponentData
        //    where TEnterEvent : struct, IComponentData
        //    where TOldState : struct, IComponentData
        //{
        //    var cmd = new SetStateCmd
        //    {
        //        NewStateType = typeof(TState),
        //        EnterEventType = typeof(TEnterEvent),
        //        OldStateType = typeof(TOldState),
        //    };

        //    entityManager.SetOrAddComponentData<SetStateCmd>(entity, cmd);
        //}

        public static bool SetFSMStateCmd<TState, TEnterEvent, TExitEvent>(EntityManager entityManager, Entity entity, byte state)
            where TState : struct, IComponentData
            where TEnterEvent : struct, IComponentData
            where TExitEvent : struct, IComponentData
        {
            if (!entityManager.HasComponent<FSMState>(entity))
                return false;

            var fsmState = entityManager.GetComponentData<FSMState>(entity);
            if (fsmState.State == state)
                return false;

            var cmd = new SetFSMStateCmd
            {
                State = state,
                StateType = typeof(TState),
                EnterEventType = typeof(TEnterEvent),
                ExitEventType = typeof(TExitEvent),
            };
            entityManager.SetOrAddComponentData(entity, cmd);
            return true;
        }

        #endregion
    }
}