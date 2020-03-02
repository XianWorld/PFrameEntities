using Unity.Collections;
using Unity.Entities;

namespace PFrame.Entities
{
    public class CommonUtil
    {
        public static NativeString32 EmptyString32 = new NativeString32("");

        #region Common State
        public static bool IsInState<TStateComp>(EntityManager entityManager, Entity entity)
            where TStateComp : IComponentData
        {
            return entityManager.HasComponent<TStateComp>(entity);
        }

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

        public static void SetState<TStateComp, TEventComp, TOldStateComp>(World world, Entity entity)
            where TStateComp : struct, IComponentData
            where TEventComp : struct, IComponentData
            where TOldStateComp : struct, IComponentData
        {
            var entityManager = world.EntityManager;
            var ecbSystem = world.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
            var commandBuffer = ecbSystem.CreateCommandBuffer();

            if (entityManager.HasComponent<TOldStateComp>(entity))
            {
                commandBuffer.RemoveComponent<TOldStateComp>(entity);
            }

            if (!entityManager.HasComponent<TStateComp>(entity))
            {
                if (typeof(TStateComp) != typeof(NoneState))
                {
                    commandBuffer.AddComponent<TStateComp>(entity);
                }
                commandBuffer.AddComponent<TEventComp>(entity);
            }
        }
        #endregion
    }
}