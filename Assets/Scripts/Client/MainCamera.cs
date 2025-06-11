using Unity.Entities;
using UnityEngine;

namespace Client
{
    public class MainCamera : IComponentData
    {
        public Camera Value;
    }
    
    public struct MainCameraTag : IComponentData {}
}