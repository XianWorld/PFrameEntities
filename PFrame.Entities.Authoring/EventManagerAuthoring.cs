//using System;
//using System.Collections.Generic;
//using System.Reflection;
//using Unity.Collections;
//using Unity.Entities;
//using UnityEngine;

//namespace PFrame.Entities.Authoring
//{
//    public class EventManagerAuthoring : MonoBehaviour, IConvertGameObjectToEntity
//    {
//        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
//        {
//            var eventManager = new EventManager();

//            var types = GetEventComponentTypes();

//            //eventManager.TypesAssetRef = EntityAuthoringUtil.CreateArrayAssetRef(types);


//            var builder = new BlobBuilder(Allocator.Temp);
//            ref var root = ref builder.ConstructRoot<EventTypesAsset>();

//            var len = types.Length;
//            var array = builder.Allocate(ref root.Array, len);

//            root.Count = (ushort)len;
//            for (int i = 0; i < len; i++)
//            {
//                array[i] = types[i];
//            }

//            eventManager.TypesAssetRef = builder.CreateBlobAssetReference<EventTypesAsset>(Allocator.Persistent);
//            builder.Dispose();

//            dstManager.AddComponentData<EventManager>(entity, eventManager);
//        }

//        private ComponentType[] GetEventComponentTypes()
//        {
//            var typeList = new List<ComponentType>();

//            var compType = typeof(IComponentData);

//            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
//            foreach (var assembly in assemblies)
//            {
//                if (assembly.GetName().Name.EndsWith(".Authoring"))
//                    continue;

//                var assemblyTypes = assembly.GetTypes();

//                foreach (var type in assemblyTypes)
//                {
//                    var attr = type.GetCustomAttribute<EventComponentAttribute>();
//                    if (attr == null)
//                        continue;

//                    if (!compType.IsAssignableFrom(type))
//                        continue;

//                    if (type.ContainsGenericParameters)
//                        continue;

//                    if (type.IsAbstract)
//                        continue;

//                    //Debug.LogFormat("EventConversionSystem: {0}", type.Name);

//                    typeList.Add(type);
//                }
//            }
//            return typeList.ToArray();
//        }
//    }

//    //[UpdateInGroup(typeof(GameObjectAfterConversionGroup))]
//    //internal class EventConversionSystem : GameObjectConversionSystem
//    //{
//    //    private bool isConverted = false;

//    //    public override bool ShouldRunConversionSystem()
//    //    {
//    //        return base.ShouldRunConversionSystem();
//    //    }

//    //    protected override void OnUpdate()
//    //    {
//    //        if (isConverted)
//    //            return;
//    //        isConverted = true;

//    //        var compType = typeof(IComponentData);

//    //        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
//    //        foreach (var assembly in assemblies)
//    //        {
//    //            if (assembly.GetName().Name.EndsWith(".Authoring"))
//    //                continue;

//    //            var assemblyTypes = assembly.GetTypes();

//    //            foreach (var type in assemblyTypes)
//    //            {
//    //                var attr = type.GetCustomAttribute<EventComponentAttribute>();
//    //                if (attr == null)
//    //                    continue;

//    //                if (!compType.IsAssignableFrom(type))
//    //                    continue;

//    //                if (type.ContainsGenericParameters)
//    //                    continue;

//    //                if (type.IsAbstract)
//    //                    continue;

//    //                Debug.LogFormat("EventConversionSystem: {0}", type.Name);
//    //            }
//    //        }
//    //    }
//    //}
//}
