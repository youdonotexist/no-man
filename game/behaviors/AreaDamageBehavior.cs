//-----------------------------------------------------------------------------
// Platformer Starter Kit
// Copyright (C) Phillip O'Shea
//  
// Area Damage - This behavior can be owned by either a Trigger or a Scene
//               Object. If a player enters the trigger it will take damage,
//               and will continue to do so each interval. If an actor
//               collides with a scene object, then it will take damage upon
//               collision.
//-----------------------------------------------------------------------------

if ( !isObject( AreaDamageBehavior ) )
{
    %template = new BehaviorTemplate( AreaDamageBehavior );

    %template.friendlyName = "Area Damage";
    %template.behaviorType = "Miscellaneous";
    %template.description  = "Area Damage applied to objects within area, or who collide with it";

    %template.addBehaviorField( PlayerOnly, "Damage the player, not other actors",       BOOL,  false );
    %template.addBehaviorField( Amount,     "Amount of damage",                          FLOAT, 0.0 );
    %template.addBehaviorField( Interval,   "Scheduled time (sec) for recursive damage", FLOAT, 0.0 );
}

/// Set up the object
function AreaDamageBehavior::onAddToScene( %this, %scenegraph )
{
    // Set up triggers
    if ( %this.Owner.getClassName() $= "t2dTrigger" )
    {
        // Records the objects in its limits
        %this.ObjectList = new SimSet();

        // Make sure we check events properly
        %this.Owner.setEnterCallback( 1 );
        %this.Owner.setStayCallback ( 0 );
        %this.Owner.setLeaveCallback( %this.Interval > 0 );
    }

    // Set up scene objects
    if ( %this.Owner.getClassName() $= "t2dSceneObject" )
    {
        // Make sure we can receive collisions properly
        %this.Owner.setCollisionActive( 0, 1 );
        %this.Owner.setCollisionPhysics( 0, 1 );
        %this.Owner.setCollisionCallback( 1 );
    }

    // Collision system
    if ( %this.PlayerOnly )
    {
        %this.Owner.setObjectType( "PlayerTrigger" );
        %this.Owner.setCollidesWith( "PlayerObject" );
    }
    else
    {
        %this.Owner.setObjectType( "ActorTrigger" );
        %this.Owner.setCollidesWith( "ActorObject" );
    }
}

/// Adds damage to the actor on collision
function AreaDamageBehavior::onCollision( %ourObject, %theirObject, %ourRef, %theirRef, %time, %normal, %contacts, %points )
{
    // Damage the actor
    %theirObject.takeDamage( %ourObject.Amount, %ourObject, true, false );

    if ( %theirObject.isMethod( "onAreaDamage" ) )
    {
        %theirObject.onAreaDamage( %ourObject, %normal );
    }
}

/// Adds damage to the actor when entering the trigger
function AreaDamageBehavior::onEnter( %this, %theirObject )
{
    // If it is a recursive event
    if ( %this.Interval > 0 )
    {
        %this.ObjectList.Add( %theirObject );
        %this.recursiveDamage( %theirObject );

        return;
    }

    // Once only damage
    %theirObject.takeDamage( %this.Amount, %this, true, false );
}

function AreaDamageBehavior::onLeave( %this, %theirObject )
{
    // Remove the object from the list
    if ( %this.ObjectList.isMember( %theirObject ) )
    {
        %this.ObjectList.Remove( %theirObject );
    }
}

function AreaDamageBehavior::recursiveDamage( %this, %theirObject )
{
    // Make sure it is listed
    if ( %this.ObjectList.isMember( %theirObject ) )
    {
        // Take the damage
        %theirObject.takeDamage( %this.Amount, %this, true, false );

        // Schedule the next event
        %this.schedule( %this.Interval * 1000, "recursiveDamage", %theirObject );
    }
}