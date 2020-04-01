using System;
using Unity.Collections;
using Unity.Entities;

namespace PFrame.Entities
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class GameDataManagerSystem : ComponentSystem
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireSingletonForUpdate<GameDataManager>();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        public bool TryGetGameDataArray<T>(out NativeArray<T> result)
            where T : struct, IBufferElementData, IGameData
        {
            var list = new NativeList<T>(Allocator.Temp);
            Entities.ForEach((DynamicBuffer<T> buffer) =>
            {
                list.AddRange(buffer.AsNativeArray());
            });
            bool isFind = list.Length > 0;
            result = default;
            if(isFind)
                result = list.ToArray(Allocator.Temp);
            list.Dispose();
            return isFind;
        }

        public bool GetGameData<T>(ushort id, out T result)
            where T : struct, IBufferElementData, IGameData
        {
            T data = default;
            bool isFind = false;
            Entities.ForEach((DynamicBuffer<T> buffer) =>
                {
                    foreach (var item in buffer)
                    {
                        if (item.DataId == id)
                        {
                            data = item;
                            isFind = true;
                            return;
                        }
                    }
                });
            result = data;
            return isFind;
        }

        public bool GetGameData<T>(Func<T, bool> func, out T result)
            where T : struct, IBufferElementData, IGameData
        {
            T data = default;
            bool isFind = false;
            Entities.ForEach((DynamicBuffer<T> buffer) =>
            {
                foreach (var item in buffer)
                {
                    if (func(item))
                    {
                        data = item;
                        isFind = true;
                        return;
                    }
                }
            });
            result = data;
            return isFind;
        }

        protected override void OnUpdate()
        {
            //Entities
            //    .WithNone<InitedState>()
            //    .ForEach((Entity entity, ref GameData gameData, ref DynamicBuffer<GameData> buffer) =>
            //    {
            //        EntityManager.AddComponent<InitedState>(entity);

            //        int id = fontComp.Id;
            //        if (id >= MAX_FONT_NUM)
            //            return;

            //        var font = new UI3DIconFontData(EntityManager, entity);
            //        fonts[id] = font;

            //        //UnityEngine.Debug.Log("UI3DIconFontSystem.OnUpdate: " + font);
            //    });
        }
    }
}