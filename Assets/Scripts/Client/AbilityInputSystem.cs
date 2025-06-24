using Common;
using Unity.Entities;

namespace Client
{
    public partial class AbilityInputSystem : SystemBase
    {
        private InputSystem_Actions _inputActions;
        protected override void OnCreate()
        {
            _inputActions = new InputSystem_Actions();
        }

        protected override void OnStartRunning()
        {
            _inputActions.Enable();
        }
        
        protected override void OnStopRunning()
        {
            _inputActions.Disable();
        }

        protected override void OnUpdate()
        {
            var newAbilityInput = new AbilityInput();

            if (_inputActions.Player.AbilityOne.WasPressedThisFrame())
            {
                newAbilityInput.QAbility.Set();
            }

            foreach (var abilityInput in SystemAPI.Query<RefRW<AbilityInput>>())
            {
                abilityInput.ValueRW = newAbilityInput;
            }
        }
    }
}