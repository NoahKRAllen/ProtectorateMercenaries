using Unity.Entities;
using UnityEngine;

namespace Client
{
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
    public partial class InitializeMainCameraSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<MainCamera>();
        }

        protected override void OnUpdate()
        {
            Enabled = false;
            var mainCameraEntity = SystemAPI.GetSingletonEntity<MainCameraTag>();
            EntityManager.SetComponentData(mainCameraEntity, new MainCamera{ Value = Camera.main });
        }
    }
}