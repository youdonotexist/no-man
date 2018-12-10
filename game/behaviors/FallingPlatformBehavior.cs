//-----------------------------------------------------------------------------
// Platformer Starter Kit
// Copyright (C) Phillip O'Shea
//  
// Falling Platform - When an actor lands upon this platform, they will have
//                    a few moments (specified by the FallTimeOut field) before
//                    the platform falls from the sky.
//-----------------------------------------------------------------------------

if ( !isObject( FallingPlatformBehavior ) )
{
    %template = new BehaviorTemplate( FallingPlatformBehavior );

    %template.friendlyName = "Falling Platform";
    %template.behaviorType = "Platform";
    %template.description  = "Falling Platform object";

    %template.addBehaviorField( AutoRecover,      "Respawn, otherwise destroy",        BOOL,   true );

    %template.addBehaviorField( Gravity,          "Gravity applied when falling",      FLOAT,  150.0 );
    %template.addBehaviorField( FallTimeOut,      "Time before falling",               FLOAT,  0.5 );
    %template.addBehaviorField( RecoverTimeOut,   "Time before recovery",              FLOAT,  2.0 );

    %template.addBehaviorField( FallAnimation,    "Animation played while falling",    OBJECT, "", t2dAnimationDatablock );
    %template.addBehaviorField( RecoverAnimation, "Animation played while recovering", OBJECT, "", t2dAnimationDatablock );
}

function FallingPlatformBehavior::onAddtoScene( %this, %scenegraph )
{
    // Skip if we're not a platform
    %platformInstance = %this.Owner.getPlatformInstance();
    if ( !isObject( %platformInstance ) )
    {
        error( "FallingPlatformBehavior must be added to a platform!" );
        return;
    }

    // Recovery position
    %this.RecoverPosition = %this.Owner.Position;
}

function FallingPlatformBehavior::actorLanded( %this, %actor )
{
    // Schedule this platform to fall
    %this.schedule( %this.FallTimeOut * 1000, "startFall" );
}

function FallingPlatformBehavior::startFall( %this )
{
    // Apply the fall force
    %this.Owner.setConstantForce( 0 SPC %this.Gravity, true );

    // Stop us colliding with things
    %this.Owner.setCollisionActiveReceive( 0 );

    // Schedule this platform to recover
    %this.schedule( %this.RecoverTimeOut * 1000, "checkRecovery" );

    // Set the falling animation
    if ( isObject( %this.FallAnimation ) && %this.Owner.isMemberOfClass( "t2dAnimatedSprite" ) )
    {
        %this.Owner.setAnimation( %this.FallAnimation );
    }
}

function FallingPlatformBehavior::checkRecovery( %this )
{
    // Hide the platform
    %this.Owner.Visible = false;

    if ( %this.AutoRecover )
    {
        // Schedule the recovery
        %this.startRecovery();
    }
    else
    {
        // Destroy the object instead
        %this.Owner.safeDelete();
    }
}

function FallingPlatformBehavior::startRecovery( %this )
{
    // Make sure we're visible
    %this.Owner.Visible = true;

    // Make sure we can collide with things
    %this.Owner.setCollisionActiveReceive( 1 );

    // Kill velocity
    %this.Owner.setLinearVelocity( "0.0 0.0" );

    // Kill the fall force
    %this.Owner.setConstantForce( "0.0 0.0", true );

    // Reposition the platform
    %this.Owner.setPosition( %this.RecoverPosition );

    // Set the recovery animation
    if ( isObject( %this.RecoverAnimation ) && %this.Owner.isMemberOfClass( "t2dAnimatedSprite" ) )
    {
        %this.Owner.setAnimation( %this.RecoverAnimation );
    }
}