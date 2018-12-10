//-----------------------------------------------------------------------------
// Platformer Starter Kit
// Copyright (C) Phillip O'Shea
//  
// Trampoline - A type of platform that allows actors to bounce off the
//              surface. It will project the actor in the direction that the
//              platform is rotated. The jump modifier is applied when the
//              actor jumps shortly after contact with the platform.
//-----------------------------------------------------------------------------

if ( !isObject( TrampolineBehavior ) )
{
    %template = new BehaviorTemplate( TrampolineBehavior );

    %template.friendlyName = "Trampoline Platform";
    %template.behaviorType = "Platform";
    %template.description  = "Trampoline Platform object";

    %template.addBehaviorField( BounceForce,  "Bounce height", FLOAT, 100 );
    %template.addBehaviorField( JumpModifier, "Jump modifier", FLOAT, 1.5 );
}

function TrampolineBehavior::onAddtoScene( %this, %scenegraph )
{
    // Skip if we're not a platform
    %platformInstance = %this.Owner.getPlatformInstance();
    if ( !isObject( %platformInstance ) )
    {
        error( "TrampolineBehavior must be added to a platform!" );
        return;
    }

    %this.Owner.setCollisionCallback( !%platformInstance.OneWay );
}

function TrampolineBehavior::actorLanded( %this, %theirObject )
{
    // Bounce the actor
    %this.bounce( %theirObject );
}

function TrampolineBehavior::onCollision( %ourObject, %theirObject, %ourRef, %theirRef, %time, %normal, %contacts, %points )
{
    %ourObject.bounce( %theirObject );
}

function TrampolineBehavior::bounce( %this, %theirObject )
{
    // Store our jump mod
    %theirObject.BounceCoefficient = %this.JumpModifier;
    // Bounce the actor
    %theirObject.bounce( %this.BounceForce, 180 + %this.Owner.Rotation );

    // Play the sound and animate
    if ( %theirObject.getAnimationState() !$= "jump" )
    {
        if ( isObject( MushroomBounceSound ) )
        {
            playSound( MushroomBounceSound );
        }
        
        %theirObject.setAnimationState( "jump" );
    }

    // Animate the platform
    if ( %this.Owner.isMemberOfClass( t2dAnimatedSprite ) )
    {
        %this.Owner.playAnimation( %this.Owner.AnimationName );
    }
}