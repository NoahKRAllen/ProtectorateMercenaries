using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Common
{
    public readonly partial struct AoeAspect : IAspect
    {
        private readonly RefRO<AbilityInput> _abilityInput;
        private readonly RefRO<AbilityPrefabs> _abilityPrefabs;
        private readonly RefRO<PlayerTeam> _team;
        private readonly RefRO<LocalTransform> _localTransform;

        public bool ShouldAttack => _abilityInput.ValueRO.QAbility.IsSet;
        public Entity AbilityPrefab => _abilityPrefabs.ValueRO.QAbility;
        public PlayerTeam Team => _team.ValueRO;
        public float3 AttackPosition => _localTransform.ValueRO.Position;
    }
}