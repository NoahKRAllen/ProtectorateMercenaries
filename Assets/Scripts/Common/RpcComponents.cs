using Unity.NetCode;

public struct TeamRequest : IRpcCommand
{
    public TeamType Value;
}
