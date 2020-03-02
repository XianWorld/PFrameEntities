using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
//using UnityEngine;

namespace PFrame.Entities
{
    public class EntityUtil
    {
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
                    //Debug.Log(entity + ", " + buffer);
                    foreach (Child child in buffer)
                    {
                        SetActive(entityManager, child.Value, isActive, isRecursively);
                    }
                }
            }
        }

        public static void SetParent(EntityManager entityManager, Entity entity, Entity parentEntity)
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
    }
}