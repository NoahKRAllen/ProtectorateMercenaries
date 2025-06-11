using Common;
using Unity.Entities;
using Unity.NetCode;
using Unity.Physics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Client
{
    [UpdateInGroup(typeof(GhostInputSystemGroup))]
    public partial class MercMoveInputSystem : SystemBase
    {
        private InputSystem_Actions _inputActions;
        private CollisionFilter _selectionFilter;
        protected override void OnCreate()
        {
            _inputActions = new InputSystem_Actions();
            _selectionFilter = new CollisionFilter
            {
                BelongsTo = 1 << 5, //Raycasts
                CollidesWith = 1 << 0 //Ground Plane
            };
            RequireForUpdate<OwnerMercTag>();
        }
        
        protected override void OnStartRunning()
        {
            _inputActions.Enable();
            _inputActions.Player.Select.performed += OnSelectPositon;
            _inputActions.Player.Move.performed += OnWASDInput;
        }

        protected override void OnStopRunning()
        {
            _inputActions.Disable();
            _inputActions.Player.Select.performed -= OnSelectPositon;
            _inputActions.Player.Move.performed -= OnWASDInput;
            
        }

        private void OnWASDInput(InputAction.CallbackContext context)
        {
            Debug.Log("Still need to make WASD Input controls");
        }
        
        //TODO: Swap from select to move into WASD controlled twin-stick
        //OnSelectPosition will not be the function for movement once tutorial is finished
        //However, this function should work for left and right clicks instead
        private void OnSelectPositon(InputAction.CallbackContext context)
        {
            var collisionWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;
            var cameraEntity = SystemAPI.GetSingletonEntity<MainCameraTag>();
            var mainCamera = EntityManager.GetComponentObject<MainCamera>(cameraEntity).Value;

            var mousePosition = Input.mousePosition;
            mousePosition.z = 100f;
            var worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

            var selectionInput = new RaycastInput
            {
                Start = mainCamera.transform.position,
                End = worldPosition,
                Filter = _selectionFilter
            };

            if (collisionWorld.CastRay(selectionInput, out var closestHit))
            {
                var mercEntity = SystemAPI.GetSingletonEntity<OwnerMercTag>();
                //This line will be changed over to the select struct once WASD is implemented
                EntityManager.SetComponentData(mercEntity, new MercMoveTargetPosition
                {
                    Value = closestHit.Position
                });
            }
        }

        protected override void OnUpdate()
        {
        }
    }
}