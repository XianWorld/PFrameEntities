using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Unity.Entities;
using UnityEditor;
using UnityEngine;

namespace PFrame.Entities.Authoring
{
    public class GameDataAuthoringAttribute : Attribute
    {
        public string Name;

    }


    public class GameDataSOManagerAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
	{
        public string DataPath = "Assets/GameDatas";
        //public string EnumTypeName;

        private Dictionary<string, IGameDataSOListConverter> authoringDict = new Dictionary<string, IGameDataSOListConverter>();

        public Dictionary<string, IGameDataSOListConverter> AuthoringDict => authoringDict;

        private void OnValidate()
        {
            BuildGameDataAuthoringDict();

            var folders = AssetDatabase.GetSubFolders(DataPath);
            if (folders != null)
            {
                for (int i = 0; i < folders.Length; i++)
                {
                    var folderPath = folders[i];
                    //Debug.LogFormat("OnValidate: {0}: {1}", i, folderPath);

                    var typeName = GetGameDataTypeNameFromPath(folderPath);
                    if (authoringDict.TryGetValue(typeName, out var authoring))
                    {
                        authoring.Init(folderPath);
                    }
                }
            }
        }

        private void BuildGameDataAuthoringDict()
        {
            var authoringType = typeof(IGameDataSOListConverter);

            authoringDict.Clear();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                if (!assembly.GetName().Name.EndsWith(".Authoring"))
                    continue;

                var assemblyTypes = assembly.GetTypes();

                foreach (var type in assemblyTypes)
                {
                    var attr = type.GetCustomAttribute<GameDataAuthoringAttribute>();
                    if (attr == null)
                        continue;

                    if (!authoringType.IsAssignableFrom(type))
                        continue;

                    if (type.ContainsGenericParameters)
                        continue;

                    if (type.IsAbstract)
                        continue;

                    //Debug.LogFormat("BuildGameDataAuthoringDict: {0}", type.Name);

                    IGameDataSOListConverter inst = Activator.CreateInstance(type) as IGameDataSOListConverter;

                    authoringDict.Add(attr.Name, inst);
                }
            }
        }

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
	    {
            //var rootPath = EditorApplication.applicationContentsPath;
            //var path = Path.Combine(rootPath, DataPath);

            dstManager.AddComponent<GameDataManager>(entity);

            var folders = AssetDatabase.GetSubFolders(DataPath);
            if(folders != null)
            {
                for(int i = 0; i < folders.Length; i++)
                {
                    var folderPath = folders[i];

                    var typeName = GetGameDataTypeNameFromPath(folderPath);
                    if(authoringDict.TryGetValue(typeName, out var authoring))
                    {
                        //Debug.LogFormat("Convert: {0}: {1}, {2}", i, folderPath, authoring.GetType().Name);

                        var listEntity = conversionSystem.CreateAdditionalEntity(this);
                        authoring.Convert(gameObject, listEntity, dstManager, conversionSystem);
                    }
                }
            }
        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            var folders = AssetDatabase.GetSubFolders(DataPath);
            if (folders != null)
            {
                for (int i = 0; i < folders.Length; i++)
                {
                    var folderPath = folders[i];
                    //Debug.LogFormat("DeclareReferencedPrefabs: {0}: {1}", i, folderPath);

                    var typeName = GetGameDataTypeNameFromPath(folderPath);
                    if (authoringDict.TryGetValue(typeName, out var authoring))
                    {
                        authoring.DeclareReferencedPrefabs(referencedPrefabs);
                    }
                }
            }
        }

        public void DeclareReferencedObjects(GameObjectConversionSystem conversionSystem)
        {
            var folders = AssetDatabase.GetSubFolders(DataPath);
            if (folders != null)
            {
                for (int i = 0; i < folders.Length; i++)
                {
                    var folderPath = folders[i];
                    //Debug.LogFormat("GameDataSOManagerAuthoring.DeclareReferencedObjects: {0}: {1}", i, folderPath);

                    var typeName = GetGameDataTypeNameFromPath(folderPath);
                    if (authoringDict.TryGetValue(typeName, out var authoring))
                    {
                        authoring.DeclareReferencedObjects(conversionSystem);
                    }
                }
            }
        }

        //private AGameDataSO[] LoadGameDataSOAssets(string folderPath)
        //{
        //    var assetPaths = AssetDatabase.FindAssets("t:AGameDataSO", new[] { folderPath });
        //    if (assetPaths == null)
        //        return null;

        //    var len = assetPaths.Length;
        //    var gameDataSOs = new AGameDataSO[len];

        //    for (int j = 0; j < len; j++)
        //    {
        //        var asset = AssetDatabase.LoadAssetAtPath<AGameDataSO>(assetPaths[j]);
        //        gameDataSOs[j] = asset;
        //    }
        //    return gameDataSOs;
        //}

        private string GetGameDataTypeNameFromPath(string path)
        {
            var index = path.LastIndexOf('/');
            if (index < 0)
                return "";

            return path.Substring(index + 1);
        }
    }

    [UpdateInGroup(typeof(GameObjectDeclareReferencedObjectsGroup))]
    internal class GameDataDeclareReferencedObjectsSystem : GameObjectConversionSystem
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((GameDataSOManagerAuthoring managerAuthoring) => 
            {
                //Debug.LogFormat("GameDataDeclareReferencedObjectsSystem.OnUpdate: {0}", managerAuthoring.name);

                managerAuthoring.DeclareReferencedObjects(this);
            });
        }
    }

}
