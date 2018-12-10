//-----------------------------------------------------------------------------
// Platformer Starter Kit
// Copyright (C) Phillip O'Shea
//  
// Enemy Turn Trigger - When an enemy enters this trigger, it will be forced
//                      to turn and face the other direction. This allows you
//                      to control where an enemy can move to.
//-----------------------------------------------------------------------------

if ( !isObject( EnemyTurnBehavior ) )
{
    %template = new BehaviorTemplate( EnemyTurnBehavior );

    %template.friendlyName = "Enemy Turn Trigger";
    %template.behaviorType = "Miscellaneous";
    %template.description  = "When an enemy object enters, it will change direction";
}

/// Set up the trigger
function EnemyTurnBehavior::onAddToScene( %this, %scenegraph )
{
    // Make sure we're a trigger
    if ( %this.Owner.getClassName() !$= "t2dTrigger" )
    {
        error( "EnemyTurnBehavior::onAddToScene() - This Behavior must be used with a t2dTrigger object" );
        return;
    }

    // Make sure we check events properly
    %this.Owner.setEnterCallback( 1 );
    %this.Owner.setStayCallback ( 0 );
    %this.Owner.setLeaveCallback( 0 );

    // Ensure that the player doesn't collide with this trigger
    %this.Owner.setObjectType( "EnemyTrigger" );
    %this.Owner.setCollidesWith( "None" );
}

function EnemyTurnBehavior::onEnter( %this, %theirObject )
{
    // Redirect the actor
    %theirObject.Direction.X *= -1;
}