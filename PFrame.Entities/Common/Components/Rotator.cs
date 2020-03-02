using Unity.Entities;

namespace PFrame.Entities
{
    [GenerateAuthoringComponent]
    public struct Rotator : IComponentData
    {
        public float RotateSpeed;
    }
}