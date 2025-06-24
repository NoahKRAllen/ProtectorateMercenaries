using Unity.Entities;
using Unity.NetCode;

namespace Common
{
    public struct MaxHitPoints : IComponentData
    {
        public int Value;
    }

    public struct CurrentHitPoints : IComponentData
    {
        [GhostField] public int Value;
    }

    [GhostComponent(PrefabType = GhostPrefabType.AllPredicted)]
    public struct DamageBufferElement : IBufferElementData
    {
        public int Value;
    }

    [GhostComponent(PrefabType = GhostPrefabType.AllPredicted, OwnerSendType = SendToOwnerType.SendToNonOwner)]
    public struct DamageThisTick : ICommandData
    {
        public NetworkTick Tick { get; set; }
        public int Value;
    }

    public struct AbilityPrefabs : IComponentData
    {
        //TODO: Will probably have to make different ones here for each ability and give proper names to them, and then just
        //link up the three abilities for the player as to what they choose to use
        public Entity QAbility;
    }

}