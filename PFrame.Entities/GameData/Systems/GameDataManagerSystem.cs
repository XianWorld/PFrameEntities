//using System.Collections.Generic;
//using Unity.Collections;
//using Unity.Entities;

//namespace PFrame.Entities
//{
//    [UpdateInGroup(typeof(ServiceUpdateSystemGroup))]
//    [UpdateBefore(typeof(ServiceUpdateSystem))]
//    public class GameDataManagerSystem : ComponentSystem
//    {
//        protected override void OnCreate()
//        {
//            base.OnCreate();

//            GameDataManager = new GameDataManager();
//        }

//        protected override void OnDestroy()
//        {
//            base.OnDestroy();

//            GameDataManager.Dispose();
//        }

//        public bool GetGameDataItem<TItem>(int id, out TItem result)
//            where TItem : struct, IBufferElementData, IGameDataItem
//        {
//            TItem data = default;
//            bool isFind = false;
//            Entities
//                .ForEach((DynamicBuffer<TItem> buffer) =>
//                {
//                    foreach (var item in buffer)
//                    {
//                        if (item.Id == id)
//                        {
//                            data = item;
//                            isFind = true;
//                            return;
//                        }
//                    }
//                });
//            result = data;
//            return isFind;
//        }

//        protected override void OnUpdate()
//        {
//            Entities
//                .WithNone<InitedState>()
//                .ForEach((Entity entity, ref GameData gameData, ref DynamicBuffer<GameData> buffer) =>
//                {
//                    EntityManager.AddComponent<InitedState>(entity);

//                    int id = fontComp.Id;
//                    if (id >= MAX_FONT_NUM)
//                        return;

//                    var font = new UI3DIconFontData(EntityManager, entity);
//                    fonts[id] = font;

//                    //UnityEngine.Debug.Log("UI3DIconFontSystem.OnUpdate: " + font);
//                });
//        }

//    }
//}