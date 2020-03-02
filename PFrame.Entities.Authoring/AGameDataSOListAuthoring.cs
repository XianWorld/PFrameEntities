using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace PFrame.Entities.Authoring
{
    public abstract class AGameDataSO : ScriptableObject, IGameData
	{
	    public int Id;
        public string Name;
	
	    public int DataId => Id;
        public string DataName => Name;
    }

    public abstract class AGameDataSOListAuthoring<T, TData> : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
        where T : struct, IBufferElementData, IGameData
	    where TData : AGameDataSO
	{
	    public int TypeId;
	    public TData[] Datas;
	
	    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
	    {
	        var gameDataList = new GameDataList
	        {
	            TypeId = TypeId
	        };
	        dstManager.AddComponentData<GameDataList>(entity, gameDataList);
	
	        var buffer = dstManager.AddBuffer<T>(entity);
	
	        for(int i = 0; i< Datas.Length; i++)
	        {
	            var item = Datas[i];
                if(item != null)
                {
                    var data = GetGameData(conversionSystem, item);
                    buffer.Add(data);
                }
            }
        }

        public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            for (int i = 0; i < Datas.Length; i++)
            {
                var item = Datas[i];
                DeclareReferencedPrefab(referencedPrefabs, item);
            }
        }

        public abstract T GetGameData(GameObjectConversionSystem conversionSystem, TData item);
        public abstract void DeclareReferencedPrefab(List<GameObject> referencedPrefabs, TData item);
    }
}
