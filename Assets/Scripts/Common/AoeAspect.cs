using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Common
{
    public readonly partial struct AoeAspect : IAspect
    {
        private readonly RefRO<AbilityInput> _abilityInput;
        private readonly RefRO<AbilityPrefabs> _abilityPrefabs;
        private readonly RefRO<PlayerTeam> _mobaTeam;
        private readonly RefRO<LocalTransform> _localTransform;

        public bool ShouldAttack => _abilityInput.ValueRO.QAbility.IsSet;
        public Entity AbilityPrefab => _abilityPrefabs.ValueRO.QAbility;
        public PlayerTeam MobaTeam => _mobaTeam.ValueRO;
        public float3 Position => _localTransform.ValueRO.Position;
    }
}