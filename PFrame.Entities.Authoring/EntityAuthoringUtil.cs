using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using UnityEditor;

namespace PFrame.Entities.Authoring
{
    public class EntityAuthoringUtil
    {
        public static BlobAssetReference<UshortArrayAsset> CreateArrayAssetRef<T>(T[] datas)
            where T : IGameData
        {
            ushort[] dataIds = datas.Select((d) => d == null ? (ushort)0 : d.DataId).ToArray();
            return EntityUtil.CreateArrayAssetRef(dataIds);
        }

        //public static BlobAssetReference<UshortArrayAsset> CreateArrayAssetRef<T>(T[] datas)
        //    where T : IGameData
        //{
        //    var builder = new BlobBuilder(Allocator.Temp);
        //    ref var root = ref builder.ConstructRoot<UshortArrayAsset>();

        //    var len = datas.Length;
        //    var array = builder.Allocate(ref root.Array, len);

        //    root.Count = (ushort)len;
        //    for (int i = 0; i < len; i++)
        //    {
        //        if (datas[i] != null)
        //            array[i] = datas[i].DataId;
        //    }

        //    var assetRef = builder.CreateBlobAssetReference<UshortArrayAsset>(Allocator.Persistent);
        //    builder.Dispose();

        //    return assetRef;
        //}


        //public static BlobAssetReference<ArrayAsset<T>> CreateArrayAssetRef<T>(T[] datas)
        //    where T : struct
        //{
        //    var builder = new BlobBuilder(Allocator.Temp);
        //    ref var root = ref builder.ConstructRoot<ArrayAsset<T>>();

        //    var len = datas.Length;
        //    var array = builder.Allocate(ref root.Array, len);

        //    root.Count = (ushort)len;
        //    for (int i = 0; i < len; i++)
        //    {
        //        array[i] = datas[i];
        //    }

        //    var assetRef = builder.CreateBlobAssetReference<ArrayAsset<T>>(Allocator.Persistent);
        //    builder.Dispose();

        //    return assetRef;
        //}

        public static FileInfo[] GetFileInfos(string fullPath, string searchPattern = "*", SearchOption searchOption = SearchOption.AllDirectories)
        {
            List<FileInfo> fileInfoList = new List<FileInfo>();
            if (!Directory.Exists(fullPath))
                return fileInfoList.ToArray();

            DirectoryInfo direction = new DirectoryInfo(fullPath);
            FileInfo[] files = direction.GetFiles(searchPattern, searchOption);
            for (int i = 0; i < files.Length; i++)
            {

                if (files[i].Name.EndsWith(".meta"))
                    continue;

                fileInfoList.Add(files[i]);
            }

            return fileInfoList.ToArray();
        }

        public static T[] LoadAssets<T>(string fullPath)
            where T : UnityEngine.Object
        {
            List<T> assetList = new List<T>();

            foreach (string subFile in Directory.GetFiles(fullPath))
            {
                if (subFile.EndsWith(".meta"))
                    continue;

                int index = subFile.IndexOf("Assets");
                if (index < 0)
                    continue;

                var assetPath = subFile.Substring(index);
                var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                    assetList.Add(asset);
            }

            return assetList.ToArray();
        }

        public static string[] GetAssetPaths(string fullPath, string ext = "")
        {
            List<string> pathList = new List<string>();

            //Path.GetFullPath
            foreach (string subFile in Directory.GetFiles(fullPath))
            {
                if (subFile.EndsWith(".meta"))
                    continue;
                if (string.IsNullOrEmpty(ext) && !subFile.EndsWith(ext))
                    continue;

                int index = subFile.IndexOf("Assets");
                if (index < 0)
                    continue;

                pathList.Add(subFile.Substring(index));
            }

            return pathList.ToArray();
        }
    }

}
