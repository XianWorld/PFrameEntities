using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEditor;
using UnityEngine;

namespace PFrame.Entities.Authoring
{
    public abstract class AGameDataSO : ScriptableObject, IGameData
	{
	    public ushort Id;
        public string Name;
	
	    public ushort DataId => Id;
        public string DataName => Name;

        public abstract byte DataType { get; }
    }

    [Serializable]
    public class GameDataLevelDataSO
    {
        public AGameDataSO DataSO;
        public byte Level;
    }

    public interface IGameDataSOListConverter// : IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    {
        bool Init(AGameDataSO[] gameDataSOs);
        bool Init(string path);

        void Convert(GameObject gameObject, Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem);
        void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs);
        void DeclareReferencedObjects(GameObjectConversionSystem conversionSystem);
    }

    public abstract class AGameDataSOListConverter<TData, TDataSO> : IGameDataSOListConverter
        where TData : struct, IBufferElementData
        where TDataSO : AGameDataSO
    {
        public abstract byte TypeId { get; }
        protected TDataSO[] dataSOs;

        public bool Init(TDataSO[] gameDataSOs)
        {
            dataSOs = gameDataSOs;
            return true;
        }

        public bool Init(AGameDataSO[] gameDataSOs)
        {
            int len = gameDataSOs.Length;
            dataSOs = new TDataSO[len];

            for (int j = 0; j < len; j++)
            {
                dataSOs[j] = gameDataSOs[j] as TDataSO;
            }

            return true;
        }

        public bool Init(string path)
        {
            //var typeStr = typeof(TDataSO).Name;
            //var assetGUIDs = AssetDatabase.FindAssets("t:" + typeStr, new[] { path });
            //if (assetGUIDs == null)
            //    return false;

            //var len = assetGUIDs.Length;
            //dataSOs = new TDataSO[len];

            //Debug.LogFormat("{0}.Init: {1}, {2}, {3}", GetType().Name, path, typeStr, dataSOs.Length);

            //for (int j = 0; j < len; j++)
            //{
            //    var assetPath = AssetDatabase.GUIDToAssetPath(assetGUIDs[j]);
            //    var asset = AssetDatabase.LoadAssetAtPath<TDataSO>(assetPath);
            //    dataSOs[j] = asset;
            //}

            dataSOs = EntityAuthoringUtil.LoadAssets<TDataSO>(path);
            //Debug.LogFormat("{0}.Init: {1}, {2}", GetType().Name, path, dataSOs.Length);
            return true;
        }

        public void Convert(GameObject gameObject, Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var gameDataList = new GameDataList
            {
                TypeId = TypeId
            };

            dstManager.AddComponentData<GameDataList>(entity, gameDataList);

            var buffer = dstManager.AddBuffer<TData>(entity);

            //Debug.LogFormat("{0}: {1}", GetType().Name, dataSOs.Length);

            if (dataSOs != null && dataSOs.Length > 0)
            {
                var len = dataSOs.Length;
                var list = new NativeList<TData>(Allocator.Temp);
                for (int i = 0; i < len; i++)
                {
                    var item = dataSOs[i];
                    if (item != null)
                    {
                        var data = ConvertGameData(gameObject, entity, dstManager, conversionSystem, item);
                        list.Add(data);
                    }
                }
                buffer = dstManager.GetBuffer<TData>(entity);
                buffer.AddRange(list);
                list.Dispose();
            }
        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            if (dataSOs != null)
            {
                for (int i = 0; i < dataSOs.Length; i++)
                {
                    var item = dataSOs[i];
                    if(item != null)
                    {
                        DeclareReferencedPrefab(referencedPrefabs, item);
                    }
                }
            }
        }

        public void DeclareReferencedObjects(GameObjectConversionSystem conversionSystem)
        {
            if (dataSOs != null)
            {
                for (int i = 0; i < dataSOs.Length; i++)
                {
                    var item = dataSOs[i];
                    if (item != null)
                    {
                        DeclareReferencedObject(conversionSystem, item);
                    }
                }
            }
        }

        public abstract TData ConvertGameData(GameObject gameObject, Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem, TDataSO dataSO);
        public virtual void DeclareReferencedPrefab(List<GameObject> referencedPrefabs, TDataSO dataSO)
        {

        }

        protected virtual void DeclareReferencedObject(GameObjectConversionSystem conversionSystem, TDataSO dataSO)
        {

        }

        private TDataSO[] LoadGameDataSOAssets(string folderPath)
        {
            var typeStr = typeof(TDataSO).Name;
            var assetPaths = AssetDatabase.FindAssets("t:" + typeStr, new[] { folderPath });
            if (assetPaths == null)
                return null;

            var len = assetPaths.Length;
            var gameDataSOs = new TDataSO[len];

            for (int j = 0; j < len; j++)
            {
                var asset = AssetDatabase.LoadAssetAtPath<TDataSO>(assetPaths[j]);
                gameDataSOs[j] = asset;
            }
            return gameDataSOs;
        }
    }

    public abstract class AGameDataSOListAuthoring<TData, TDataSO, TConverter> : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
        where TData : struct, IBufferElementData, IGameData
        where TDataSO : AGameDataSO
        where TConverter : AGameDataSOListConverter<TData, TDataSO>, new()
    {
        public string Path;
        public TDataSO[] Datas;
        protected TConverter converter = new TConverter();

        private void OnValidate()
        {
            if (Datas != null && Datas.Length > 0)
            {
                converter.Init(Datas);
            }
            else if (!string.IsNullOrEmpty(Path))
            {
                converter.Init(Path);
            }
        }

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            //if (Datas != null && Datas.Length > 0)
            //{
            //    converter.Init(Datas);
            //}
            //else if (!string.IsNullOrEmpty(Path))
            //{
            //    converter.Init(Path);
            //}

            converter.Convert(gameObject, entity, dstManager, conversionSystem);
        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            converter.DeclareReferencedPrefabs(referencedPrefabs);
        }
    }

    //public abstract class AGameDataSOListAuthoring<TData, TDataSO> : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    //    where TData : struct, IBufferElementData, IGameData
    //    where TDataSO : AGameDataSO
    //{
    //    public TDataSO[] Datas;
    //    //private IGameDataSOListConverter convertaer;

    //    protected abstract IGameDataSOListConverter convertaer { get; }

    //    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    //    {
    //        convertaer.Convert(entity, dstManager, conversionSystem);
    //    }

    //    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    //    {
    //        convertaer.DeclareReferencedPrefabs(referencedPrefabs);
    //    }
    //}

    //   public abstract class AGameDataSOListAuthoring<TData, TDataSO> : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
    //       where TData : struct, IBufferElementData, IGameData
    //    where TDataSO : AGameDataSO
    //{
    //    public abstract int TypeId { get; }
    //    public TDataSO[] Datas;

    //    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    //    {
    //        var gameDataList = new GameDataList
    //        {
    //            TypeId = TypeId
    //        };
    //        dstManager.AddComponentData<GameDataList>(entity, gameDataList);

    //        var buffer = dstManager.AddBuffer<TData>(entity);

    //           if(Datas != null)
    //           {
    //               for (int i = 0; i < Datas.Length; i++)
    //               {
    //                   var item = Datas[i];
    //                   if (item != null)
    //                   {
    //                       var data = GetGameData(conversionSystem, item);
    //                       buffer.Add(data);
    //                   }
    //               }
    //           }
    //       }

    //       public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    //       {
    //           if (Datas != null)
    //           {
    //               for (int i = 0; i < Datas.Length; i++)
    //               {
    //                   var item = Datas[i];
    //                   DeclareReferencedPrefab(referencedPrefabs, item);
    //               }
    //           }
    //       }

    //       public abstract TData GetGameData(GameObjectConversionSystem conversionSystem, TDataSO dataSO);
    //       public virtual void DeclareReferencedPrefab(List<GameObject> referencedPrefabs, TDataSO dataSO)
    //       {

    //       }
    //   }
}
