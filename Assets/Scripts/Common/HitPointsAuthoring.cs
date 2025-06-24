using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Common
{
    public class HitPointsAuthoring : MonoBehaviour
    {
        public int maxHitPoints;

        public class HitPointsBaker : Baker<HitPointsAuthoring>
        {
            public override void Bake(HitPointsAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new CurrentHitPoints {Value = authoring.maxHitPoints});
                AddComponent(entity, new MaxHitPoints {Value = authoring.maxHitPoints});
                AddBuffer<DamageBufferElement>(entity);
                AddBuffer<DamageThisTick>(entity);

            }
        }
    }
}