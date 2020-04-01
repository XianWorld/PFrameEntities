using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;

namespace PFrame.Entities
{
    //[UpdateInGroup(typeof(LateSimulationSystemGroup))]
    //public class RemoveCommonEventSystem : ComponentSystem
    //{
    //    protected override void OnUpdate()
    //    {
    //        #region Init/Rlease events
    //        Entities.ForEach((Entity entity, ref InitEvent eventComp) =>
    //        {
    //            PostUpdateCommands.RemoveComponent<InitEvent>(entity);
    //        });
    //        Entities.ForEach((Entity entity, ref InitedEvent eventComp) =>
    //        {
    //            PostUpdateCommands.RemoveComponent<InitedEvent>(entity);
    //        });
    //        Entities.ForEach((Entity entity, ref ReleaseEvent eventComp) =>
    //        {
    //            PostUpdateCommands.RemoveComponent<ReleaseEvent>(entity);
    //        });
    //        Entities.ForEach((Entity entity, ref ReleasedEvent eventComp) =>
    //        {
    //            PostUpdateCommands.RemoveComponent<ReleasedEvent>(entity);
    //        });
    //        #endregion

    //        #region Load/Unload events
    //        Entities.ForEach((Entity entity, ref LoadEvent eventComp) =>
    //        {
    //            PostUpdateCommands.RemoveComponent<LoadEvent>(entity);
    //        });
    //        Entities.ForEach((Entity entity, ref LoadedEvent eventComp) =>
    //        {
    //            PostUpdateCommands.RemoveComponent<LoadedEvent>(entity);
    //        });
    //        Entities.ForEach((Entity entity, ref UnloadEvent eventComp) =>
    //        {
    //            PostUpdateCommands.RemoveComponent<UnloadEvent>(entity);
    //        });
    //        Entities.ForEach((Entity entity, ref UnloadedEvent eventComp) =>
    //        {
    //            PostUpdateCommands.RemoveComponent<UnloadedEvent>(entity);
    //        });
    //        #endregion
    //    }
    //}

    //[UpdateInGroup(typeof(LateSimulationSystemGroup))]
    //public class EventCleanUpSystem : ComponentSystem
    //{
    //    public List<EntityQuery> queryList;

    //    protected override void OnCreate()
    //    {
    //        base.OnCreate();

    //        RequireSingletonForUpdate<EventManager>();
    //    }

    //    protected override void OnStartRunning()
    //    {
    //        base.OnStartRunning();

    //        queryList = new List<EntityQuery>();

    //        var eventManager = GetSingleton<EventManager>();
    //        ref var types = ref eventManager.TypesAssetRef.Value.Array;
    //        var len = types.Length;
    //        for (int i = 0; i < len; i++)
    //        {
    //            var eventType = types[i];
    //            var query = GetEntityQuery(eventType);
    //            queryList.Add(query);

    //            LogUtil.LogFormat("EventCleanUpSystem.OnStartRunning: {0}, {1}\r\n", eventType.TypeIndex, i);
    //        }
    //    }

    //    protected override void OnStopRunning()
    //    {
    //        base.OnStopRunning();

    //        queryList.Clear();
    //        queryList = null;
    //    }

    //    protected override void OnUpdate()
    //    {
    //        var eventManager = GetSingleton<EventManager>();
    //        ref var types = ref eventManager.TypesAssetRef.Value.Array;
    //        var len = types.Length;
    //        for (int i = 0; i < len; i++)
    //        {
    //            var eventType = types[i];

    //            var query = queryList[i];
    //            LogUtil.LogFormat("EventCleanUpSystem.OnUpdate: {0}, {1}, {2}\r\n", eventType.TypeIndex, i, query.CalculateEntityCount());
    //            if (query.CalculateEntityCount() == 0)
    //                continue;

    //            var entities = query.ToEntityArray(Allocator.TempJob);
    //            for (int j = 0; j < entities.Length; j++)
    //            {
    //                PostUpdateCommands.RemoveComponent(entities[j], eventType);
    //            }

    //            entities.Dispose();
    //        }

    //        //Entities.ForEach((ref DynamicBuffer<EventElement> buffer) =>
    //        //{
    //        //    foreach (var eventElement in buffer)
    //        //    {
    //        //        var eventType = eventElement.EventType;
    //        //        var query = GetEntityQuery(eventType);
    //        //        if (query.CalculateEntityCount() == 0)
    //        //            continue;

    //        //        var entities = query.ToEntityArray(Allocator.Temp);
    //        //        PostUpdateCommands.RemoveComponent(entities[0], eventType);

    //        //        entities.Dispose();
    //        //    }

    //        //    PostUpdateCommands.RemoveComponent<LoadEvent>(entity);
    //        //});
    //    }
    //}

    public struct RemoveComponentData
    {
        public ComponentType Type;
        public Entity Entity;
    }

    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public class RemoveEntityComponentSystem : ComponentSystem
    {
        private NativeList<RemoveComponentData> RemoveList;

        protected override void OnCreate()
        {
            base.OnCreate();
            RemoveList = new NativeList<RemoveComponentData>(Allocator.Persistent);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            RemoveList.Dispose();
        }

        public void AddRemoveComponentData<T>(Entity entity)
        {
            RemoveComponentData data = new RemoveComponentData
            {
                Entity = entity,
                Type = typeof(T)
            };
            RemoveList.Add(data);
        }

        public void AddRemoveComponentData(Entity entity, ComponentType componentType)
        {
            RemoveComponentData data = new RemoveComponentData
            {
                Entity = entity,
                Type = componentType
            };
            RemoveList.Add(data);
        }

        protected override void OnUpdate()
        {
            var len = RemoveList.Length;
            if (len == 0)
                return;

            for (int i = len - 1; i >= 0; i--)
            {
                var entity = RemoveList[i].Entity;
                var type = RemoveList[i].Type;

                var isRemove = true;
                if (EntityManager.Exists(entity))
                {
                    if (EntityManager.HasComponent(entity, type))
                        EntityManager.RemoveComponent(entity, type);
                    else
                        isRemove = false;
                }

                if (isRemove)
                    RemoveList.RemoveAtSwapBack(i);
            }
        }
    }
}