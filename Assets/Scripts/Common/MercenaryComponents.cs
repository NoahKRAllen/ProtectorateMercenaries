using Unity.Entities;
using Unity.NetCode;

namespace Common
{
    public struct MercenaryTag : IComponentData { }

    public struct NewMercenaryTag : IComponentData { }
    
    public struct CameraFollowTag : IComponentData { }
    public struct PlayerTeam : IComponentData
    {
        [GhostField] public TeamType Value;
    }
}