using Unity.Entities;
using Unity.VisualScripting;
using UnityEngine;

namespace Common
{
    public class MobaTeamAuthoring : MonoBehaviour
    {
        public TeamType mobaTeam;
        
        public class MobaTeamBaker : Baker<MobaTeamAuthoring>
        {
            public override void Bake(MobaTeamAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new TeamRequest {Value = authoring.mobaTeam});
            }
        }
    }
}