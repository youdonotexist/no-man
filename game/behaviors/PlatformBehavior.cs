//-----------------------------------------------------------------------------
// Platformer Starter Kit
// Copyright (C) Phillip O'Shea
//  
// Platform - Platforms are either solid, or one-way. The engine only
//            recognises collisions with solid platforms, and the kit will
//            attempt to register one-way collisions through script. When a
//            platform is created, it will attempt to find a "surfaceImage".
//            This tells the kit what the surface of the platform looks like,
//            and how an actor should react upon contact.
//-----------------------------------------------------------------------------

if ( !isObject( PlatformBehavior ) )
{
    %template = new BehaviorTemplate( PlatformBehavior );

    %template.friendlyName = "Platform Object";
    %template.behaviorType = "Platform";
    %template.description  = "Platform object";

    %template.addBehaviorField( OneWay, "One-Way Platform", BOOL, false );

    %template.addBehaviorField( SurfaceFriction, "Surface Friction", FLOAT, 1.0 );
    %template.addBehaviorField( SurfaceForce,    "Surface Force",    FLOAT, 0.0 );
}

function PlatformBehavior::onAddToScene( %this, %scenegraph )
{
    // Apply Behavior properties to the owner
    copyBehaviorFields( %this, %this.Owner );

    // Ensure the platform doesn't activly collide with anything
	if (%this.Owner.visible == true)
	{
		%this.Owner.setCollisionActive( 0, 1 );
	}
	else
	{
		%this.Owner.setCollisionActive( 0, 0 );
	}
    %this.Owner.setCollisionPhysics( 0, 0 );
    %this.Owner.WorldLimitCallback = true;

    // This SimSet holds the id's of the actors currently on this platform
    %this.ActorsHolding = new SimSet();

    // Set the object type
    if ( %this.OneWay )
    {
        %this.Owner.setObjectType( "OneWayPlatform" );
    }
    else
    {
        %this.Owner.setObjectType( "SolidPlatform" );
    }
}

function PlatformBehavior::onRemove( %this )
{
    if ( isObject( %this.ActorsHolding ) )
    {
        %this.ActorsHolding.delete();
    }
}

//-----------------------------------------------------------------------------

function PlatformBehavior::onActorEnter( %this, %actor )
{
    // Record that we're holding an actor
    %this.ActorsHolding.add( %actor );

    // Notify other behaviors the actor has landed
    %behaviorCount = %this.Owner.getBehaviorCount();
    for ( %i = 0; %i < %behaviorCount; %i++ )
    {
        // Get the ith behavior
        %behavior = %this.Owner.getBehaviorByIndex( %i );

        // Skip this one
        if ( %behavior == %this )
        {
            continue;
        }

        // Notify that the actor has landed
        if ( %behavior.isMethod( "actorLanded" ) )
        {
            %behavior.actorLanded( %actor );
        }
    }
}

function PlatformBehavior::onActorLeft( %this, %actor )
{
    // Record that we're no longer holding an actor
    %this.ActorsHolding.remove( %actor );

    // Notify other behaviors the actor has left this platform
    %behaviorCount = %this.Owner.getBehaviorCount();
    for ( %i = 0; %i < %behaviorCount; %i++ )
    {
        // Get the ith behavior
        %behavior = %this.Owner.getBehaviorByIndex( %i );

        // Skip this one
        if ( %behavior == %this )
        {
            continue;
        }

        // Notify that the actor has landed
        if ( %behavior.isMethod( "actorLeft" ) )
        {
            %behavior.actorLeft( %actor );
        }
    }
}

//-----------------------------------------------------------------------------

function PlatformBehavior::onWorldLimit( %this, %limitmode, %limit )
{
    // Update inherited velocities for each of the actors on this platform
    %actorCount = %this.ActorsHolding.getCount();
    for ( %i = 0; %i < %actorCount; %i++ )
    {
        %actor = %this.ActorsHolding.getObject( %i );

        // Update the inherited velocity
        %actor.InheritedVelocity = mVectorMultiply( %this.Owner.LinearVelocity, -1 );
    }
}