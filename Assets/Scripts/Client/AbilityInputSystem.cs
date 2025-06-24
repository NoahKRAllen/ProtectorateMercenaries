using Unity.Entities;
using UnityEngine.InputSystem;

namespace Client
{
    public partial class AbilityInputSystem : SystemBase
    {
        private InputSystem_Actions _inputSystem;
        protected override void OnUpdate()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnStartRunning()
        {
            base.OnStartRunning();
        }
        
        protected override void OnStopRunning()
        {
            base.OnStopRunning();
        }
    }
}