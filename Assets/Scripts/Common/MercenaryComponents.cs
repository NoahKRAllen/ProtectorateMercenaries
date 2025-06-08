using Unity.Entities;

namespace Common
{
    public struct MercenaryTag : IComponentData { }

    public struct NewMercenaryTag : IComponentData { }

    public struct Team : IComponentData
    {
        public TeamType Value;
    }
}