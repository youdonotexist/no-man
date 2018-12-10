//-----------------------------------------------------------------------------
// Platformer Starter Kit
// Copyright (C) Phillip O'Shea
//  
// AI Controller - This controller is a very basic method of controlling our
//                 AI. The actor will only move in one direction until told
//                 otherwise. This method is the basic implementation of any
//                 game AI and provides a foundation for you to expand upon.
//-----------------------------------------------------------------------------

if ( !isObject( AIControllerBehavior ) )
{
    %template = new BehaviorTemplate( AIControllerBehavior );

    %template.friendlyName = "AI Controller";
    %template.behaviorType = "Actor";
    %template.description  = "Movement";

    %template.addBehaviorField( ActorType, "Type of AI", ENUM, "None", "None" NL "Drill" );
}

/// Set up the controller
function AIControllerBehavior::onAddToScene( %this, %scenegraph )
{
    // Store the Actor Type
    if ( %this.ActorType !$= "" && %this.ActorType !$= "None" )
    {
        %this.Owner.ActorType = %this.ActorType;
    }

    // Initialise the direction
    %this.Owner.Direction = ( 2 * !%this.Owner.FlipX ) - 1 SPC 0;

    // Set up the collision references
    if ( %this.AIType !$= "None" )
    {
        %this.Owner.setObjectType( "EnemyObject" );
        %this.Owner.setCollidesWith( "PlatformObject PlayerObject EnemyTrigger" );
    }
}