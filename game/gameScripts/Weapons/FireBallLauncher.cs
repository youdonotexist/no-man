//-----------------------------------------------------------------------------
// Platformer Starter Kit
// Copyright (C) - Phillip O'Shea
//-----------------------------------------------------------------------------

datablock t2dSceneObjectDatablock( FireBallProjectileData : ProjectileBaseData )
{
    Class             = "FireBallProjectile";
    AnimationName     = "FireBallAnimation";

    CollisionCallback      = true;
    CollisionDetectionMode = "PLATFORM_CIRCLE";
    CollisionCircleScale   = 0.2;

    Lifetime          = 1.5;
    Size              = "12.000 10.000";
    
    ProjectileDamage  = 100;
};

new ScriptObject( FireBallLauncherAction : ProjectileBaseAction )
{
    ProjectileType   = "t2dAnimatedSprite";
    ProjectileConfig = FireBallProjectileData;
    ProjectileSpeed  = 70;
    ProjectileOffset = 1;
    ProjectileArc    = 0;

    CoolDown         = 500;
    Continuous       = false;

    Burst            = false;
    BurstCount       = 3;
    BurstDelay       = 30;

    //TriggerSound     = RocketLauncherFireSound;
};

//-----------------------------------------------------------------------------

new ScriptObject( FireBallLauncherWeapon : WeaponBase )
{
    // Action
    ActionCount = 1;
    Action[0]   = FireBallLauncherAction;
};

//-----------------------------------------------------------------------------

function FireBallProjectile::onRemove( %this )
{
    // Create Impact Animation.
    %impactEffect = new t2dAnimatedSprite()
    {
        SceneGraph    = %this.getSceneGraph();
        Class         = "FireBallImpactEffect";
        AnimationName = "FireBallImpactAnimation";
        
        Position      = %this.Position;
        Rotation      = getRandom( 0, 360 );
        Size          = "15 15";
    };
    
    // Fetch the Volume for the Effect.
    %volume = $Game::Player.getSoundVolume( %this.Position );
    
    // Play Sound Effect.
    playSound( "FireballImpactSound", %volume );
}

function FireBallImpactEffect::onAnimationEnd( %this )
{
    %this.safeDelete();
}