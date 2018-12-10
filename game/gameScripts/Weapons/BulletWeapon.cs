//-----------------------------------------------------------------------------
// Platformer Starter Kit
// Copyright (C) - Phillip O'Shea
//-----------------------------------------------------------------------------

datablock t2dSceneObjectDatablock( BulletProjectileData : ProjectileBaseData )
{
    Class    = "BulletProjectile";
    Imagemap = "RocketLauncherRocketImageMap";
    
    CollisionCallback      = true;
    CollisionDetectionMode = "PLATFORM_CIRCLE";
    CollisionCircleScale   = 0.2;
    
    Lifetime = 2.0;
    Size     = "4 0.75";
    
    BlendColor = "1 1 0 1";
    ProjectileDamage  = 10;
};

new ScriptObject( MachineGunAction : ProjectileBaseAction )
{
    ProjectileType   = "t2dStaticSprite";
    ProjectileConfig = BulletProjectileData;
    ProjectileSpeed  = 400;
    ProjectileOffset = 1;
    ProjectileArc    = 5;
    
    Ammunition       = MachineGunAmmo;
    
    CoolDown         = 100;
    Continuous       = true;
    
    Burst            = false;
    BurstCount       = 3;
    BurstDelay       = 100;
    
    TriggerSound     = MachineGunFireLoopSound;
};

new ScriptObject( MachineGunAmmo : ProjectileBaseAmmo )
{
    ClipSize     = 50;
    MaxInventory = 200;
};