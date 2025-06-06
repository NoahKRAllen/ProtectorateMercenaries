using Unity.NetCode;

namespace Common
{
    public struct TeamRequest : IRpcCommand
    {
        public TeamType Value;
    }
}

