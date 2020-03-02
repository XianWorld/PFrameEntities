using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace PFrame.Entities
{
    [UpdateBefore(typeof(TransformSystemGroup))]
    public class ServiceUpdateSystemGroup : ComponentSystemGroup
    {
    }
}