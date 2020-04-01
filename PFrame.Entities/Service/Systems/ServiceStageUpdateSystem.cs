using Unity.Collections;
using Unity.Entities;

namespace PFrame.Entities
{
    [UpdateInGroup(typeof(ServiceUpdateSystemGroup))]
    [UpdateBefore(typeof(ServiceUpdateSystem))]
    public class ServiceStageUpdateSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((Entity entity, ref ServiceStage stageComp, ref LoadCmd loadCmdComp) =>
            {
                //LogUtil.Log($"{GetType().Name}: Load {stageComp.Id}");
                //CommonUtil.SetState<LoadState, LoadEvent, NoneState>(World, entity);
                EntityUtil.SetState<LoadState, EnterLoadStateEvent, NoneState, ExitNoneStateEvent>(EntityManager, entity);
                EntityManager.RemoveComponent<LoadCmd>(entity);
            });

            Entities.ForEach((Entity entity, ref ServiceStage stageComp, ref UnloadCmd unloadCmdComp) =>
            {
                //LogUtil.Log($"{GetType().Name}: Unload {stageComp.Id}");
                //CommonUtil.SetState<UnloadState, UnloadEvent, LoadedState>(World, entity);
                EntityUtil.SetState<UnloadState, EnterUnloadStateEvent, LoadedState, ExitLoadedStateEvent>(EntityManager, entity);
                EntityManager.RemoveComponent<UnloadCmd>(entity);
            });
        }
    }
}