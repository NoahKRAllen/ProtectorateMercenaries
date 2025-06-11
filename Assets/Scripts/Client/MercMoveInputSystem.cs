using Unity.Entities;
using Unity.NetCode;
using Unity.Physics;
using UnityEngine.InputSystem;

namespace Client
{
    [UpdateInGroup(typeof(GhostInputSystemGroup))]
    public partial class MercMoveInputSystem : SystemBase
    {
        private InputSystem_Actions _inputActions;
        
        //Won't need this as its for the click to move for later
        private CollisionFilter _selectionFilter;
        protected override void OnCreate()
        {
            _inputActions = new InputSystem_Actions();
            _selectionFilter = new CollisionFilter
            {
                BelongsTo = 1 << 5, //Raycasts
                CollidesWith = 1 << 0 //Ground Plane
            };
        }

        protected override void OnStartRunning()
        {
            _inputActions.Enable();
            _inputActions.Player.Select.performed += OnSelectPositon;
        }

        protected override void OnStopRunning()
        {
            _inputActions.Disable();
            _inputActions.Player.Move.performed -= OnSelectPositon;
        }

        private void OnSelectPositon(InputAction.CallbackContext context)
        {
            
        }

        protected override void OnUpdate()
        {
            throw new System.NotImplementedException();
        }
    }
}