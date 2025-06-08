using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

namespace Common
{
    public class MercenaryAuthoring : MonoBehaviour
    {
        public class MercenaryBaker : Baker<MercenaryAuthoring>
        {
            public override void Bake(MercenaryAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<MercenaryTag>(entity);
                AddComponent<NewMercenaryTag>(entity);
                AddComponent<CameraFollowTag>(entity);
                AddComponent<PlayerTeam>(entity);
                AddComponent<URPMaterialPropertyBaseColor>(entity);
            }
        }
    }
}