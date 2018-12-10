//-----------------------------------------------------------------------------
// Platformer Starter Kit
// Copyright (C) - Phillip O'Shea
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Description:
//
// Weapons are just references for different actions to be performed. The main
// type of weapon to be used will reference PROJECTILE actions.
//
// Fields:
// 
// Ammunition - The type of ammunition this weapon uses. If this is not
//              specified, then INFINITE ammo will be assumed.
//
// CoolDown   - The time that must be taken between firing.
//
// Continuous - If set to true, the weapon will continue firing as long as the
//              trigger remains held down.
//
// Burst      - If set to true, the weapon will fire in bursts.
//
// BurstCount - The number of projectiles fired each burst round.
//
// BurstDelay - The delay between each projectile being created.
//
//-----------------------------------------------------------------------------

new ScriptObject( WeaponBase )
{
    Superclass = "pskWeapon";
};

function pskWeapon::inUse( %this, %referenceObject )
{
    %actionCount = %this.ActionCount;
    for ( %i = 0; %i < %actionCount; %i++ )
    {
        %actionInstance = %this.Action[%i];
        if ( isObject( %actionInstance ) )
        {
            if ( %actionInstance.isActive( %referenceObject ) )
            {
                return true;
            }
        }
    }

    return false;
}

//-----------------------------------------------------------------------------

new ScriptObject( ProjectileBaseAction : BaseAction )
{
    Class = "pskProjectileAction";

    ProjectileType   = "t2dStaticSprite";
    ProjectileConfig = ProjectileBaseData;
    ProjectileSpeed  = 50;

    CoolDown   = 100;
    Continuous = true;
};

new ScriptObject( ProjectileBaseAmmo )
{
    SuperClass = "pskProjectileAmmo";
};

datablock t2dSceneObjectDatablock( ProjectileBaseData )
{
    SuperClass = "pskProjectile";
    Lifetime   = 5.0;
    Size       = "2 2";

    CollisionActiveSend     = "1";
    CollisionPhysicsSend    = "1";
    CollisionActiveReceive  = "1";
    CollisionPhysicsReceive = "0";

    CollisionDetectionMode  = "PLATFORM";
    CollisionResponseMode   = "KILL";

    //NetworkInitialSendMask  = "-473972295";
    NetworkAutoSendMask     = "-536362984";
};