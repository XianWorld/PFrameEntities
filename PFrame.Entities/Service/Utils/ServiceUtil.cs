using Unity.Collections;
using Unity.Entities;

namespace PFrame.Entities
{
    public class ServiceUtil
    {
        //public static void SetStageId(EntityManager entityManager, Entity entity, NativeString32 newStageId)
        //{
        //    var serviceComp = entityManager.GetComponentData<ServiceComponent>(entity);
        //    var stageId = serviceComp.StageId;
        //    var stageEntity = serviceComp.StageEntity;

        //    if (stageId.Equals(newStageId))
        //        return;

        //    if (entityManager.HasComponent<LoadStateComponent>(entity))
        //        return;
        //    if (entityManager.HasComponent<UnloadStateComponent>(entity))
        //        return;

        //    if (stageEntity != Entity.Null)
        //    {
        //        serviceComp.NextStageId = newStageId;

        //        entityManager.AddComponent<UnloadCmdComponent>(stageEntity);
        //        CommonUtil.SetState<UnloadStateComponent, UnloadEventComponent, LoadedStateComponent>(entityManager, entity);
        //    }
        //    else
        //    {
        //        if (newStageId.Equals(CommonUtil.EmptyString32))
        //            return;
        //        //stageEntity = CreateStageEntity(stageId);

        //        //EntityManager.AddComponent<LoadCmdComponent>(stageEntity);
        //        serviceComp.StageId = newStageId;
        //        //serviceComp.StageId.CopyFrom(newStageId);
        //        CommonUtil.SetState<LoadStateComponent, LoadEventComponent, NoneStateComponent>(entityManager, entity);
        //    }

        //    entityManager.SetComponentData<ServiceComponent>(entity, serviceComp);
        //}

    }
}