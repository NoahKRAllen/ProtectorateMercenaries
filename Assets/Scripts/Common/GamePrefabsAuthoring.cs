using Unity.Entities;
using UnityEngine;

namespace Common
{
    public class GamePrefabsAuthoring : MonoBehaviour
    {
        public GameObject mercenary;

        public class GamePrefabsBaker : Baker<GamePrefabsAuthoring>
        {
            public override void Bake(GamePrefabsAuthoring authoring)
            {
                var prefabContainerEntity = GetEntity(TransformUsageFlags.None);
                AddComponent(prefabContainerEntity, new GamePrefabs
                {
                    Mercenary = GetEntity(authoring.mercenary, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}