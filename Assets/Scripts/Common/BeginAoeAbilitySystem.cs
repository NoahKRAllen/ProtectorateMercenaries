using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;

namespace Common
{
    [UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
    public partial struct BeginAoeAbilitySystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }
    }
}