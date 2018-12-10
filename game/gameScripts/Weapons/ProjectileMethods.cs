//-----------------------------------------------------------------------------
// Platformer Starter Kit
// Copyright (C) - Phillip O'Shea
//-----------------------------------------------------------------------------

function pskProjectileAction::triggerLoop( %this, %referenceObject )
{
    // Grab the type of ammo we're using
    // Note: If there is no ammo specified then there is assumed INFINITE ammo!
    if ( !$Game::Cheat::InfiniteAmmo )
    {
        %ammoType = %this.Ammunition;
        %ammoClip = 0;
        if ( isObject( %ammoType ) )
        {
            %ammoType = %ammoType.getName();

            if ( %ammoType.Clip != -1 )
            {
                %ammoClip = getWord( %referenceObject.getFieldValue( %ammoType ), 0 );
            }
            else
            {
                %ammoClip = %referenceObject.getFieldValue( %ammoType );
            }
        }

        if ( isObject( %ammoType ) && %ammoClip == 0 )
        {
            // No ammo remaining
            return;
        }
    }

    // Call the parent
    Parent::triggerLoop( %this, %referenceObject );

    // Create the projectile
    %this.createProjectile( %referenceObject );

    // Reduce the amount of ammo remaining
    %ammoClip -= 1;

    if ( %this.Burst )
    {
        %burstQuantity = %this.BurstCount - 1;
        if ( isObject( %ammoType ) && %burstQuantity > %ammoClip )
        {
            // Make sure we don't fire more than we have remaining
            %burstQuantity = %ammoClip;
        }

        for ( %i = 0; %i < %burstQuantity; %i++ )
        {
            %this.schedule( %this.BurstDelay * %i, "createProjectile", %referenceObject );
        }

        // Reduce the amount of ammo remaining
        %ammoClip -= %burstQuantity;
    }

    if ( isObject( %ammoType ) )
    {
        if ( %ammoType.Clip != -1 )
        {
            // Fetch the amount of ammo spare
            %ammoQuantity = getWord( %referenceObject.getFieldValue( %ammoType ), 1 );

            // Apply the new quantity
            %referenceObject.setFieldValue( %ammoType, %ammoClip SPC %ammoQuantity );
        }
        else
        {
            // Apply the new quantity
            %referenceObject.setFieldValue( %ammoType, %ammoClip );
        }
    }

    // Update GUI
    %referenceObject.updateWeaponGui();
}

function pskProjectileAction::createProjectile( %this, %referenceObject )
{
    // Fetch Projectile Information.
    %projectileType   = %this.ProjectileType;
    %projectileConfig = %this.ProjectileConfig;
    %projectileSpeed  = %this.ProjectileSpeed;
    %projectileOffset = %this.ProjectileOffset;
    %projectileArc    = %this.ProjectileArc;
    
    // Valid?
    if ( %projectileType $= "" || !isObject( %projectileConfig ) )
    {
        // Invalid.
        return false;
    }

    // Fetch the Muzzle Point & Vector.
    %muzzlePoint  = %referenceObject.getMuzzlePoint();
    %muzzleVector = %referenceObject.getMuzzleVector();

    // Random Offset?
    if ( %projectileArc > 0 )
    {
        // Offset the projected angle by some random number
        %projectionOffset       = ( 2.0 * getRandom() - 1.0 ) * ( %projectileArc / 2 );
        %projectionOffsetMatrix = mRotationMatrix( %projectionOffset );
        %muzzleVector           = mMatrixMultiply( %projectionOffsetMatrix, %muzzleVector );
    }

    // Spatial Calculations.
    %projectileFlip     = false;
    %projectilePosition = t2dVectorAdd( %muzzlePoint, t2dVectorScale( %muzzleVector, %projectileOffset ) );
    %projectileVelocity = t2dVectorScale( %muzzleVector, %projectileSpeed );
    %projectileRotation = mRadToDeg( mAtan( %muzzleVector.Y, %muzzleVector.X ) );
    %projectileAngularVelocity = %projectileConfig.AngularVelocity;
    
    // Normalize Angle.
    while ( %projectileRotation < 0 )   %projectileRotation += 360;
    while ( %projectileRotation > 360 ) %projectileRotation -= 360;
    
    // Flip the Projectile?
    if ( %projectileRotation > 90 && %projectileRotation < 270 )
    {
        %projectileFlip = true;
        %projectileAngularVelocity *= -1;
    }
    
    // Create the Projectile.
    %newProjectile = new ( %projectileType )()
    {
        Config          = %projectileConfig;
        SceneGraph      = %referenceObject.getSceneGraph();

        Layer           = $Game::Effects::Layer;
        
        FlipY           = %projectileFlip;

        Position        = %projectilePosition;
        LinearVelocity  = %projectileVelocity;
        
        Rotation        = %projectileRotation;
        AngularVelocity = %projectileAngularVelocity;
    };
    
    // Ensure Projectiles Collide with the Correct Groups.
    switch$ ( %referenceObject.ObjectType )
    {
        // Player Shooting.
        case "PlayerObject" : %newProjectile.setObjectType( "PlayerProjectile" );
                              %newProjectile.setCollidesWith( "PlatformObject EnemyObject" );
                              
        // Enemy Shooting.
        case "EnemyObject" :  %newProjectile.setObjectType( "EnemyProjectile" );
                              %newProjectile.setCollidesWith( "PlatformObject PlayerObject" );
    }
    
    // Return the Projectile.
    return %newProjectile;
}

//-----------------------------------------------------------------------------

function pskProjectile::triggerImpact( %this, %theirObject )
{
    // Already Collided?
    if ( %this.Collided )
    {
        // Ensure only one Impact is Triggered.
        return false;
    }

    %projectileCollider = %theirObject.getBehavior( ProjectileColliderBehavior );
    if ( %projectileCollider && !%projectileCollider.TriggerImpact )
    {
        // Make sure we don't handle collisions with colliders that don't want
        // the impact triggered!
        return false;
    }

    // Flag that we have Collided.
    %this.Collided = true;

    return true;
}

function pskProjectile::onCollision( %ourObject, %theirObject, %ourRef, %theirRef, %time, %normal, %contacts, %points )
{
    // Trigger Impact?
    if ( !%ourObject.triggerImpact( %theirObject ) )
    {
        // Quit.
        return;
    }
    
    // Actor?
    if ( %theirObject.isMemberOfClass( "pskActor" ) )
    {
        // Damage the Target.
        %theirObject.takeDamage( %ourObject.ProjectileDamage, %ourObject, true, true );
    }
}