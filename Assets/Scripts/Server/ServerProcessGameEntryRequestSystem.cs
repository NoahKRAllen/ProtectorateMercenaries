using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;
using Common;
using Unity.Mathematics;
using Unity.Transforms;

namespace Server
{
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial struct ServerProcessGameEntryRequestSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<GamePrefabs>();
            var builder = new EntityQueryBuilder(Allocator.Temp).WithAll<TeamRequest, ReceiveRpcCommandRequest>();
            state.RequireForUpdate(state.GetEntityQuery(builder));
        }

        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            var mercenaryPrefab = SystemAPI.GetSingleton<GamePrefabs>().Mercenary;
            
            foreach (var (teamRequest, requestSource, requestEntity) 
                     in SystemAPI.Query<RefRO<TeamRequest>, RefRO<ReceiveRpcCommandRequest>>().WithEntityAccess())
            {
                ecb.DestroyEntity(requestEntity);
                ecb.AddComponent<NetworkStreamInGame>(requestSource.ValueRO.SourceConnection);
            
                var requestedTeamType = teamRequest.ValueRO.Value;

                if (requestedTeamType == TeamType.AutoAssign)
                {
                    requestedTeamType = TeamType.Blue;
                }

                var clientId = SystemAPI.GetComponent<NetworkId>(requestSource.ValueRO.SourceConnection).Value;
            
                Debug.Log($"Server is assigning Client ID: {clientId} to the {requestedTeamType.ToString()} team.");
                
                var spawnPosition = new float3(0, 1, 0);

                switch (requestedTeamType)
                {
                    case TeamType.Blue:
                        spawnPosition = new float3(-120, 1.5f, -120);
                        break;
                    case TeamType.Red:
                        spawnPosition = new float3(120, 1.5f, 120);
                        break;
                    
                    default:
                        continue;
                }
                
                var newMerc = ecb.Instantiate(mercenaryPrefab);
                ecb.SetName(newMerc, $"Mercenary {clientId}");
                
                var newTransform = LocalTransform.FromPosition(spawnPosition);
                ecb.SetComponent(newMerc, newTransform);
                
                ecb.SetComponent(newMerc, new GhostOwner{NetworkId = clientId});
                ecb.SetComponent(newMerc, new PlayerTeam{Value = requestedTeamType});
                
                ecb.AppendToBuffer(requestSource.ValueRO.SourceConnection, new LinkedEntityGroup{Value = newMerc});
            }
        
            ecb.Playback(state.EntityManager);
        }
    }
}

