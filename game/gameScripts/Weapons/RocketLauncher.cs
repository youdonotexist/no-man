//-----------------------------------------------------------------------------
// Platformer Starter Kit
// Copyright (C) - Phillip O'Shea
//-----------------------------------------------------------------------------

datablock t2dSceneObjectDatablock( RocketProjectileData : ProjectileBaseData )
{
    Class    = "RocketProjectile";
    Imagemap = "RocketLauncherRocketImageMap";
    
    CollisionCallback      = true;
    CollisionDetectionMode = "PLATFORM_CIRCLE";
    CollisionCircleScale   = 0.2;
    
    Lifetime = 2.0;
    Size     = "2 2";
    
    ProjectileDamage  = 100;
};

new ScriptObject( RocketLauncherAction : ProjectileBaseAction )
{
    ProjectileType   = "t2dStaticSprite";
    ProjectileConfig = RocketProjectileData;
    ProjectileSpeed  = 100;
    ProjectileOffset = 1;
    ProjectileArc    = 5;
    
    Ammunition       = RocketLauncherAmmo;
    
    CoolDown         = 1000;
    Continuous       = false;
    
    Burst            = false;
    BurstCount       = 3;
    BurstDelay       = 30;
    
    TriggerSound     = RocketLauncherFireSound;
};

new ScriptObject( RocketLauncherAmmo : ProjectileBaseAmmo )
{
    ClipSize     = -1;
    MaxInventory = 20;
};

//-----------------------------------------------------------------------------

new ScriptObject( RocketLauncherWeapon : WeaponBase )
{
    // Image
    ShapeFile   = "~/data/shapes/weapons/RocketLauncher/RocketLauncher.dts";
    MountPoint  = "0";
    MountOffset = "0 0.25 0";
    
    // Action
    ActionCount = 2;
    Action[0]   = RocketLauncherAction;
    Action[1]   = MachineGunAction;
};