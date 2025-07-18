﻿using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;

namespace Common
{
    public struct MercenaryTag : IComponentData { }

    public struct NewMercenaryTag : IComponentData { }
    
    public struct CameraFollowTag : IComponentData { }
    public struct OwnerMercTag : IComponentData { }
    public struct PlayerTeam : IComponentData
    {
        [GhostField] public TeamType Value;
    }
    
    //Not merc specific because it can be used on the regular mobs as well
    public struct CharacterMoveSpeed : IComponentData
    {
        //Ghosting this to allow modification mid-match from power-ups and/or debuffs
        [GhostField]public float Value;
    }
    
    
    //TODO: This will be updated/removed once we swap to WASD controls for movement
    [GhostComponent(PrefabType = GhostPrefabType.AllPredicted)]
    public struct MercMoveTargetPosition : IInputComponentData
    {
        [GhostField(Quantization = 0)] public float3 Value;
    }
    
    [GhostComponent(PrefabType = GhostPrefabType.AllPredicted)]
    public struct AbilityInput : IInputComponentData
    {
        //Currently used in-place of the label AoeAbility used in the course
        [GhostField]public InputEvent QAbility;
    }
}