using Common;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;

namespace Client
{
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.ThinClientSimulation)]
    public partial struct InitializeLocalMercSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<NetworkId>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (transform,entity) in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<GhostOwnerIsLocal>()
                         .WithNone<OwnerMercTag>().WithEntityAccess())
            {
                ecb.AddComponent(entity, new OwnerMercTag());
                ecb.SetComponent(entity, new MercMoveTargetPosition{Value = transform.ValueRO.Position});
            }
            
            ecb.Playback(state.EntityManager);
        }
    }
}