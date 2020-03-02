using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace PFrame.Entities
{
    public class GameDataUtil
    {
        public static string GetDataIdsString<T>(IEnumerable<T> datas, char seperateChar = ' ')
            where T : IGameData
        {
            string ids = "";
            bool isFirst = true;
            foreach (var carData in datas)
            {
                if (isFirst)
                    isFirst = false;
                else
                    ids += seperateChar;
                ids += carData.DataId;
            }

            return ids;
        }

        public static FixedList32<int> GetDataIds<T>(T[] datas)
            where T : IGameData
        {
            var len = datas.Length;
            FixedList32<int> ids = new FixedList32<int>();
            for (int i = 0; i < len; i++)
            {
                //ids[i] = datas[i].DataId;
                ids.Add(datas[i].DataId);
            }
            return ids;
        }

        //public static NativeArray<int> GetDataIds<T>(T[] datas)
        //    where T : IGameData
        //{
        //    var len = datas.Length;
        //    var ids = new NativeArray<int>(len, Allocator.Persistent);
        //    for(int i = 0; i < len; i++)
        //    {
        //        ids[i] = datas[i].DataId;
        //    }
        //    return ids;
        //}

        //public static NativeArray<int> GetDataIdArray(string idsString, char seperateChar = ' ')
        //{
        //    var idStrs = idsString.Split(seperateChar);
        //    if (idStrs == null || idStrs.Length == 0)
        //        return default;

        //    var len = idStrs.Length;
        //    var idsArray = new NativeArray<int>(len, Allocator.Temp);
        //    for (int i = 0; i < len; i++)
        //    {
        //        idsArray[i] = int.Parse(idStrs[i]);
        //    }

        //    return idsArray;
        //}

        //public static int GetRandomDataId(string idsString, char seperateChar = ' ')
        //{
        //    var idStrs = idsString.Split(seperateChar);
        //    if (idStrs == null || idStrs.Length == 0)
        //        return -1;

        //    var len = idStrs.Length;
        //    var index = MathUtil.Random.NextInt(len);
        //    return int.Parse(idStrs[index]);
        //}
    }
}