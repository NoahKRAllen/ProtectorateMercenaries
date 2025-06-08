using Unity.Entities;
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
                AddComponent<Team>(entity);
            }
        }
    }
}