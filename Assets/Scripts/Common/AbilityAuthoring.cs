using Unity.Entities;
using UnityEngine;


namespace Common
{
    public class AbilityAuthoring : MonoBehaviour
    {
        public GameObject qAbility;
        private class AbilityBaker : Baker<AbilityAuthoring>
        {
            public override void Bake(AbilityAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new AbilityPrefabs
                {
                    QAbility = GetEntity(authoring.qAbility, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}