using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;

namespace Common
{
    [UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
    public partial struct MercMoveSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            
            
            //TODO: Swap this function from taking a moveTarget to accepting move direction from WASD input
            foreach (var (transform, movePosition, moveSpeed) 
                     in SystemAPI.Query<RefRW<LocalTransform>, RefRO<MercMoveTargetPosition>, 
                         RefRO<CharacterMoveSpeed>>().WithAll<Simulate>())
            {
                
                //Should just be able to delete this two once we have WASD input, as we will already have moveDirection given
                var moveTarget = movePosition.ValueRO.Value;
                moveTarget.y = transform.ValueRO.Position.y;

                if (math.distancesq(transform.ValueRO.Position, moveTarget) < 0.001f) continue;
                var moveDirection = math.normalize(moveTarget - transform.ValueRO.Position);
                var moveVector = moveDirection * moveSpeed.ValueRO.Value * deltaTime;
                transform.ValueRW.Position += moveVector;
                transform.ValueRW.Rotation = quaternion.LookRotationSafe(moveDirection, math.up());
            }
        }
    }
}