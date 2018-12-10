//-----------------------------------------------------------------------------
// Platformer Starter Kit
// Copyright (C) - Phillip O'Shea
//-----------------------------------------------------------------------------

datablock t2dSceneObjectDatablock( BoneProjectileData : ProjectileBaseData )
{
    Class             = "BoneProjectile";
    ImageMap          = "BoneImageMap";

    CollisionCallback      = true;
    CollisionDetectionMode = "PLATFORM_CIRCLE";
    CollisionCircleScale   = 0.2;

    Lifetime          = 1.5;
    Size              = "12.000 10.000";
    
    AngularVelocity   = 1080;
    
    ProjectileDamage  = 100;
};

new ScriptObject( BoneLauncherAction : ProjectileBaseAction )
{
    ProjectileType   = "t2dStaticSprite";
    ProjectileConfig = BoneProjectileData;
    ProjectileSpeed  = 70;
    ProjectileOffset = 1;
    ProjectileArc    = 0;

    CoolDown         = 500;
    Continuous       = false;

    Burst            = false;
    BurstCount       = 3;
    BurstDelay       = 30;
};

//-----------------------------------------------------------------------------

new ScriptObject( BoneLauncherWeapon : WeaponBase )
{
    // Action
    ActionCount = 1;
    Action[0]   = BoneLauncherAction;
};

//-----------------------------------------------------------------------------

function BoneProjectile::onRemove( %this )
{
    // Create Impact Animation.
    %impactAnimation = new t2dAnimatedSprite()
    {
        SceneGraph    = %this.getSceneGraph();
        Class         = "BoneImpactEffect";
        AnimationName = "BoneFragmentAnimation";
        
        Position      = %this.Position;
        Rotation      = %this.Rotation;
        Size          = "29 27.4";
    };
    
    // Create Impact Effect.
    %impactEffect = new t2dParticleEffect()
    {
        SceneGraph    = %this.getSceneGraph();
        EffectFile    = "resources/PlatformerArt/data/particles/BoneImpactEffect.eff";
        
        Position      = %this.Position;
        Rotation      = %this.Rotation;
        Size          = "5 1";
    };
    
    // Play Effect.
    %impactEffect.playEffect();
    
    // Fetch the Volume for the Effect.
    %volume = $Game::Player.getSoundVolume( %this.Position );
    
    // Play Sound Effect.
    playSound( "BoneImpactSound", %volume );
}

function BoneImpactEffect::onAnimationEnd( %this )
{
    %this.safeDelete();
}