using Unity.Entities;

namespace PFrame.Entities
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public class RemoveCommonEventSystem : ComponentSystem
    {
        protected override void OnUpdate()
        {
            #region Init/Rlease events
            Entities.ForEach((Entity entity, ref InitEvent eventComp) =>
            {
                PostUpdateCommands.RemoveComponent<InitEvent>(entity);
            });
            Entities.ForEach((Entity entity, ref InitedEvent eventComp) =>
            {
                PostUpdateCommands.RemoveComponent<InitedEvent>(entity);
            });
            Entities.ForEach((Entity entity, ref ReleaseEvent eventComp) =>
            {
                PostUpdateCommands.RemoveComponent<ReleaseEvent>(entity);
            });
            Entities.ForEach((Entity entity, ref ReleasedEvent eventComp) =>
            {
                PostUpdateCommands.RemoveComponent<ReleasedEvent>(entity);
            });
            #endregion

            #region Load/Unload events
            Entities.ForEach((Entity entity, ref LoadEvent eventComp) =>
            {
                PostUpdateCommands.RemoveComponent<LoadEvent>(entity);
            });
            Entities.ForEach((Entity entity, ref LoadedEvent eventComp) =>
            {
                PostUpdateCommands.RemoveComponent<LoadedEvent>(entity);
            });
            Entities.ForEach((Entity entity, ref UnloadEvent eventComp) =>
            {
                PostUpdateCommands.RemoveComponent<UnloadEvent>(entity);
            });
            Entities.ForEach((Entity entity, ref UnloadedEvent eventComp) =>
            {
                PostUpdateCommands.RemoveComponent<UnloadedEvent>(entity);
            });
            #endregion
        }
    }
}