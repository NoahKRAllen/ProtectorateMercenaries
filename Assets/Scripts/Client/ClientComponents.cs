using Unity.Entities;
using Common;
namespace Client
{
    public struct ClientTeamRequest : IComponentData
    {
        public TeamType Value;
    }    
}

