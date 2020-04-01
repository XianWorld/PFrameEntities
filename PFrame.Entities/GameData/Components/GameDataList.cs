using System;
using Unity.Entities;
using Unity.Mathematics;

namespace PFrame.Entities
{
    public enum ECommonGameDataType : byte
    {
        ServiceStage,
        Font3D,
        Font,
        Max
    }

    public struct GameDataList : IComponentData
    {
        public byte TypeId;
    }

    [Serializable]
    public struct LevelData
    {
        public ushort Id;
        public byte Level;
    }

    public struct LevelDataArrayAsset
    {
        public ushort Count;
        public BlobArray<LevelData> Array;
    }

    //public struct ArrayAsset<T> where T : struct
    //{
    //    public int Count;
    //    public BlobArray<T> Array;
    //}

    public struct UshortArrayAsset
    {
        public ushort Count;
        public BlobArray<ushort> Array;
    }

    public struct ShortArrayAsset
    {
        public ushort Count;
        public BlobArray<short> Array;
    }

    public struct IntArrayAsset
    {
        public ushort Count;
        public BlobArray<int> Array;
    }

    public struct Float3ArrayAsset
    {
        public ushort Count;
        public BlobArray<float3> Array;
    }
}