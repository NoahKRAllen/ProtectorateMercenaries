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
        #region DOTS Controls

        

        
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
            //_inputActions.Player.Move.performed += OnWASDInput;
        }

        protected override void OnStopRunning()
        {
            _inputActions.Disable();
            _inputActions.Player.Select.performed -= OnSelectPositon;
            //_inputActions.Player.Move.performed -= OnWASDInput;
            
        }
        
        //TODO: Build WASD Input controls
        //Relearned that this has to be done in Update, as movement will happen while the buttons are held down
        /*private void OnWASDInput(InputAction.CallbackContext context)
        {
            Debug.Log("Still need to make WASD Input controls");
            
            Debug.Log($"Input coming in: {context.ReadValue<Vector2>()}");
            
            var input = context.ReadValue<Vector2>();


            var moveDir = new Vector3(input.x, 0, input.y);

            if (moveDir.magnitude > 0.1f)
            {
                var mercEntity = SystemAPI.GetSingletonEntity<OwnerMercTag>();
                EntityManager.SetComponentData(mercEntity, new MercMoveTargetPosition
                {
                    Value = moveDir
                });
            }
            
        }*/
        
        //TODO: Swap from select to move into WASD controlled twin-stick
        //OnSelectPosition will not be the function for movement once tutorial is finished
        //However, this function should work for left and right clicks instead, which can be used for abilities/actions
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
        #endregion
        #region NGO Controls
        /*private void HandleInput()
        {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
            if (input.magnitude > 0.1f)
            {
                Vector3 moveDir = new Vector3(input.x, 0, input.y).normalized;
                transform.position += moveDir * moveSpeed * Time.deltaTime;
            
                // Update network position
                UpdatePositionServerRpc(transform.position);
            }
        }*/
        #endregion
        protected override void OnUpdate()
        {
        }
    }
}