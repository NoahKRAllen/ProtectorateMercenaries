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
                     in SystemAPI.Query<TeamRequest, ReceiveRpcCommandRequest>().WithEntityAccess())
            {
                ecb.DestroyEntity(requestEntity);
                ecb.AddComponent<NetworkStreamInGame>(requestSource.SourceConnection);
            
                var requestedTeamType = teamRequest.Value;

                if (requestedTeamType == TeamType.AutoAssign)
                {
                    requestedTeamType = TeamType.Blue;
                }

                var clientId = SystemAPI.GetComponent<NetworkId>(requestSource.SourceConnection).Value;
            
                Debug.Log($"Server is assigning Client ID: {clientId} to the {requestedTeamType.ToString()} team.");
                
                var newMerc = ecb.Instantiate(mercenaryPrefab);
                ecb.SetName(newMerc, $"Mercenary {clientId}");
                
                var spawnPosition = new float3(0, 1, 0);
                var newTransform = LocalTransform.FromPosition(spawnPosition);
                ecb.SetComponent(newMerc, newTransform);
                ecb.SetComponent(newMerc, new GhostOwner{NetworkId = clientId});
                ecb.SetComponent(newMerc, new Team{Value = requestedTeamType});
                
                ecb.AppendToBuffer(requestSource.SourceConnection, new LinkedEntityGroup{Value = newMerc});
            }
        
            ecb.Playback(state.EntityManager);
        }
    }
}

