//-----------------------------------------------------------------------------
// Platformer Starter Kit
// Copyright (C) Phillip O'Shea
//  
// Ladders - Ladders allow actors to climb upwards or downwards. They will be
//           able to pass through one-way platforms, but will collide with
//           solid ones.
//-----------------------------------------------------------------------------

if ( !isObject( LadderBehavior ) )
{
    %template = new BehaviorTemplate( LadderBehavior );

    %template.friendlyName = "Ladder Trigger";
    %template.behaviorType = "Miscellaneous";
    %template.description  = "Ladder object";

    %template.addBehaviorField( AllowClimbX, "Ladder may be climbed sideways",    BOOL, false );
    %template.addBehaviorField( AllowClimbY, "Ladder may be climbed up and down", BOOL, true );
}

/// Set up the trigger
function LadderBehavior::onAddToScene( %this, %scenegraph )
{
    // Make sure we're a trigger
    if ( !%this.Owner.isMemberOfClass( "t2dTrigger" ) )
    {
        error( "LadderBehavior must be used with a t2dTrigger object" SPC %this.Owner );
        return;
    }

    // Apply Behavior properties to the owner
    copyBehaviorFields( %this, %this.Owner );

    // Make sure we check events properly
    %this.Owner.setEnterCallback( 1 );
    %this.Owner.setStayCallback ( 0 );
    %this.Owner.setLeaveCallback( 1 );

    // Make sure only the player can use these ones
    %this.Owner.setObjectType( "PlayerTrigger" );
    %this.Owner.setCollidesWith( "None" );
}

function LadderBehavior::onEnter( %this, %theirObject )
{
    // Record that we have an active ladder
    %theirObject.LadderObject = %this.Owner;
    %theirObject.LadderActive = false;
}

function LadderBehavior::onLeave( %this, %theirObject )
{
    // Clear the ladder
    %theirObject.LadderObject = 0;
    %theirObject.LadderActive = false;
}