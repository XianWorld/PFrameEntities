using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
//using UnityEngine;

namespace PFrame.Entities
{
    public class EntityUtil
    {
        //public static TypeManager.FieldInfo FieldInfo_Scale = new TypeManager.FieldInfo
        //{
        //    componentTypeIndex = TypeManager.GetTypeIndex<NonUniformScale>(),
        //    byteOffsetInComponent = 0,
        //    primitiveType = PrimitiveFieldTypes.Float3
        //};

        public static bool CopyComponentData<T>(EntityManager entityManager, Entity fromEntity, Entity toEntity)
            where T : struct, IComponentData
        {
            if (!entityManager.HasComponent<T>(fromEntity))
                return false;
            var data = entityManager.GetComponentData<T>(fromEntity);

            if (entityManager.HasComponent<T>(toEntity))
                entityManager.SetComponentData(toEntity, data);
            else
                entityManager.AddComponentData(toEntity, data);
            return true;
        }

        public static bool CopySharedComponentData<T>(EntityManager entityManager, Entity fromEntity, Entity toEntity)
            where T : struct, ISharedComponentData
        {
            if (!entityManager.HasComponent<T>(fromEntity))
                return false;
            var data = entityManager.GetSharedComponentData<T>(fromEntity);

            if (entityManager.HasComponent<T>(toEntity))
                entityManager.SetSharedComponentData(toEntity, data);
            else
                entityManager.AddSharedComponentData(toEntity, data);
            return true;
        }

        public static void SetActive(EntityManager entityManager, Entity entity, bool isActive, bool isRecursively = true)
        {
            if(isActive)
            {
                if (entityManager.HasComponent<Disabled>(entity))
                    entityManager.RemoveComponent<Disabled>(entity);
            }
            else
            {
                if (!entityManager.HasComponent<Disabled>(entity))
                    entityManager.AddComponent<Disabled>(entity);
            }

            if (isRecursively)
            {
                if (entityManager.HasComponent<Child>(entity))
                {
                    var buffer = entityManager.GetBuffer<Child>(entity);
                    var array = buffer.ToNativeArray(Allocator.Temp);
                    for (int i = 0; i < array.Length; i++)
                    {
                        SetActive(entityManager, array[i].Value, isActive, isRecursively);
                    }
                    array.Dispose();
                    //if (buffer.Length > 0)
                    //{
                    //    foreach (Child child in buffer)
                    //    {
                    //        SetActive(entityManager, child.Value, isActive, isRecursively);
                    //    }
                    //}
                }
                else if (entityManager.HasComponent<LinkedEntityGroup>(entity))
                {
                    var buffer = entityManager.GetBuffer<LinkedEntityGroup>(entity);
                    var array = buffer.ToNativeArray(Allocator.Temp);
                    for (int i = 0; i < array.Length; i++)
                    {
                        var childEntity = array[i].Value;
                        if (childEntity == entity)
                            continue;
                        if (!entityManager.HasComponent<Parent>(childEntity))
                            continue;
                        //if (entityManager.GetComponentData<Parent>(childEntity).Value != entity)
                        //    continue;

                        SetActive(entityManager, childEntity, isActive, isRecursively);
                    }
                    array.Dispose();
                }
            }
        }

        public static void SetParent(EntityManager entityManager, Entity entity, Entity parentEntity)
        {
            if (parentEntity == Entity.Null)
            {
                entityManager.RemoveComponentSafe<Parent>(entity);
                entityManager.RemoveComponentSafe<LocalToParent>(entity);
            }
            else
            {
                var parent = new Parent { Value = parentEntity };
                if (entityManager.HasComponent<Parent>(entity))
                    entityManager.SetComponentData(entity, parent);
                else
                {
                    entityManager.AddComponentData(entity, parent);
                    entityManager.AddComponentData(entity, new LocalToParent());
                }
            }
        }

        #region BlobArray
        public static BlobAssetReference<UshortArrayAsset> CreateArrayAssetRef(ushort[] datas)
        {
            var builder = new BlobBuilder(Allocator.Temp);
            ref var root = ref builder.ConstructRoot<UshortArrayAsset>();
            var len = datas.Length;
            var array = builder.Allocate(ref root.Array, len);
            root.Count = (ushort)len;
            for (int i = 0; i < len; i++)
                array[i] = datas[i];
            var assetRef = builder.CreateBlobAssetReference<UshortArrayAsset>(Allocator.Persistent);
            builder.Dispose();
            return assetRef;
        }

        public static BlobAssetReference<ShortArrayAsset> CreateArrayAssetRef(short[] datas)
        {
            var builder = new BlobBuilder(Allocator.Temp);
            ref var root = ref builder.ConstructRoot<ShortArrayAsset>();
            var len = datas.Length;
            var array = builder.Allocate(ref root.Array, len);
            root.Count = (ushort)len;
            for (int i = 0; i < len; i++)
                array[i] = datas[i];
            var assetRef = builder.CreateBlobAssetReference<ShortArrayAsset>(Allocator.Persistent);
            builder.Dispose();
            return assetRef;
        }

        public static BlobAssetReference<LevelDataArrayAsset> CreateArrayAssetRef(LevelData[] datas)
        {
            var builder = new BlobBuilder(Allocator.Temp);
            ref var root = ref builder.ConstructRoot<LevelDataArrayAsset>();
            var len = datas.Length;
            var array = builder.Allocate(ref root.Array, len);
            root.Count = (ushort)len;
            for (int i = 0; i < len; i++)
                array[i] = datas[i];
            var assetRef = builder.CreateBlobAssetReference<LevelDataArrayAsset>(Allocator.Persistent);
            builder.Dispose();
            return assetRef;
        }

        public static BlobAssetReference<Float3ArrayAsset> CreateArrayAssetRef(float3[] datas)
        {
            var builder = new BlobBuilder(Allocator.Temp);
            ref var root = ref builder.ConstructRoot<Float3ArrayAsset>();
            var len = datas.Length;
            var array = builder.Allocate(ref root.Array, len);
            root.Count = (ushort)len;
            for (int i = 0; i < len; i++)
                array[i] = datas[i];
            var assetRef = builder.CreateBlobAssetReference<Float3ArrayAsset>(Allocator.Persistent);
            builder.Dispose();
            return assetRef;
        }

        #endregion

        #region State
        public static void SetState<TNewState, TEnterEvent, TOldState, TExitEvent>(EntityManager entityManager, Entity entity, bool isImmediate = false)
            where TNewState : struct, IComponentData
            where TEnterEvent : struct, IComponentData
            where TOldState : struct, IComponentData
            where TExitEvent : struct, IComponentData
        {
            AddState<TNewState, TEnterEvent>(entityManager, entity, isImmediate);
            RemoveState<TOldState, TExitEvent>(entityManager, entity, isImmediate);
        }

        public static void SetStateData<TNewState, TEnterEvent, TOldState, TExitEvent>(EntityManager entityManager, Entity entity, TNewState data, bool isImmediate = false)
            where TNewState : struct, IComponentData
            where TEnterEvent : struct, IComponentData
            where TOldState : struct, IComponentData
            where TExitEvent : struct, IComponentData
        {
            AddStateData<TNewState, TEnterEvent>(entityManager, entity, data, isImmediate);
            RemoveState<TOldState, TExitEvent>(entityManager, entity, isImmediate);
        }

        public static void AddState<TState, TEvent>(EntityManager entityManager, Entity entity, bool isImmediate = false)
            where TState : struct, IComponentData
            where TEvent : struct, IComponentData
        {
            if (entityManager.HasComponent<TState>(entity))
                return;

            if (isImmediate)
            {
                entityManager.AddComponent<TState>(entity);
                //commandBuffer.AddComponent<TEventComp>(entity);
                entityManager.AddComponent<TEvent>(entity, false, true);
            }
            else
            {
                var world = entityManager.World;
                var ecbSystem = world.GetExistingSystem<BeginSimulationEntityCommandBufferSystem>();
                var commandBuffer = ecbSystem.CreateCommandBuffer();

                commandBuffer.AddComponent<TState>(entity);
                //commandBuffer.AddComponent<TEventComp>(entity);
                entityManager.AddComponent<TEvent>(entity, true, true);
            }
        }

        public static void AddStateData<TState, TEvent>(EntityManager entityManager, Entity entity, TState data, bool isImmediate = false)
            where TState : struct, IComponentData
            where TEvent : struct, IComponentData
        {
            if (entityManager.HasComponent<TState>(entity))
                return;

            if (isImmediate)
            {
                entityManager.AddComponentData<TState>(entity, data);
                //commandBuffer.AddComponent<TEventComp>(entity);
                entityManager.AddComponent<TEvent>(entity, false, true);
            }
            else
            {
                var world = entityManager.World;
                var ecbSystem = world.GetExistingSystem<BeginSimulationEntityCommandBufferSystem>();
                var commandBuffer = ecbSystem.CreateCommandBuffer();

                commandBuffer.AddComponent<TState>(entity, data);
                //commandBuffer.AddComponent<TEventComp>(entity);
                entityManager.AddComponent<TEvent>(entity, true, true);
            }
        }

        public static void RemoveState<TState, TEvent>(EntityManager entityManager, Entity entity, bool isImmediate = false)
            where TState : struct, IComponentData
            where TEvent : struct, IComponentData
        {
            if (!entityManager.HasComponent<TState>(entity))
                return;

            if (isImmediate)
            {
                entityManager.RemoveComponent<TState>(entity);
                //commandBuffer.AddComponent<TEventComp>(entity);
                entityManager.AddComponent<TEvent>(entity, false, true);
            }
            else
            {
                var world = entityManager.World;
                var ecbSystem = world.GetExistingSystem<BeginSimulationEntityCommandBufferSystem>();
                var commandBuffer = ecbSystem.CreateCommandBuffer();

                commandBuffer.RemoveComponent<TState>(entity);
                //commandBuffer.AddComponent<TEventComp>(entity);
                entityManager.AddComponent<TEvent>(entity, true, true);
            }
        }

        public static bool IsInState<TState>(EntityManager entityManager, Entity entity)
            where TState : IComponentData
        {
            return entityManager.HasComponent<TState>(entity);
        }
        #endregion

        public static bool Contains<T>(DynamicBuffer<T> buffer, T data)
            where T : struct, IEquatable<T>
        {
            foreach(var element in buffer)
            {
                if (element.Equals(data))
                    return true;
            }
            return false;
        }

        public static int IndexOf<T>(DynamicBuffer<T> buffer, T data)
            where T : struct, IEquatable<T>
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                var element = buffer[i];
                if (element.Equals(data))
                    return i;
            }
            return -1;
        }
    }

    public static class EntitiesExtensions
    {
        public static void SetOrAddComponentData<T>(this EntityManager entityManager, Entity entity, T data)
            where T : struct, IComponentData
        {
            if (entityManager.HasComponent<T>(entity))
                entityManager.SetComponentData(entity, data);
            else
                entityManager.AddComponentData(entity, data);
        }

        public static void RemoveComponentSafe<T>(this EntityManager entityManager, Entity entity)
            where T : struct, IComponentData
        {
            if (entityManager.HasComponent<T>(entity))
                entityManager.RemoveComponent<T>(entity);
        }

        public static void AddComponent<T>(this EntityManager entityManager, Entity entity, bool isAddInTheBeginning = false, bool isRemoveInTheEnd = false)
            where T : struct, IComponentData
        {
            var world = entityManager.World;

            if (isAddInTheBeginning)
            {
                var ecbSystem = world.GetExistingSystem<BeginSimulationEntityCommandBufferSystem>();
                var commandBuffer = ecbSystem.CreateCommandBuffer();
                commandBuffer.AddComponent<T>(entity);
            }
            else
            {
                entityManager.AddComponent<T>(entity);
            }

            if (isRemoveInTheEnd)
            {
                var ecbSystem = world.GetExistingSystem<RemoveEntityComponentSystem>();
                ecbSystem.AddRemoveComponentData<T>(entity);
                //var ecbSystem = world.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
                //var commandBuffer = ecbSystem.CreateCommandBuffer();
                //commandBuffer.RemoveComponent<T>(entity);
            }
        }

        public static void AddComponent(this EntityManager entityManager, Entity entity, ComponentType componentType, bool isAddInTheBeginning = false, bool isRemoveInTheEnd = false)
        {
            var world = entityManager.World;

            if (isAddInTheBeginning)
            {
                var ecbSystem = world.GetExistingSystem<BeginSimulationEntityCommandBufferSystem>();
                var commandBuffer = ecbSystem.CreateCommandBuffer();
                commandBuffer.AddComponent(entity, componentType);
            }
            else
            {
                entityManager.AddComponent(entity, componentType);
            }

            if (isRemoveInTheEnd)
            {
                var ecbSystem = world.GetExistingSystem<RemoveEntityComponentSystem>();
                ecbSystem.AddRemoveComponentData(entity, componentType);
            }
        }

        public static void AddComponentData<T>(this EntityManager entityManager, Entity entity, T data, bool isAddInTheBeginning = false, bool isRemoveInTheEnd = false)
            where T : struct, IComponentData
        {
            var world = entityManager.World;

            if(isAddInTheBeginning)
            {
                var ecbSystem = world.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
                var commandBuffer = ecbSystem.CreateCommandBuffer();
                commandBuffer.AddComponent<T>(entity, data);
            }
            else
            {
                entityManager.AddComponentData<T>(entity, data);
            }

            if (isRemoveInTheEnd)
            {
                var ecbSystem = world.GetExistingSystem<RemoveEntityComponentSystem>();
                ecbSystem.AddRemoveComponentData<T>(entity);
            }
        }
    }
}