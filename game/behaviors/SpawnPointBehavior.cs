//-----------------------------------------------------------------------------
// Platformer Starter Kit
// Copyright (C) Phillip O'Shea
//  
// Spawn Point - Spawn points spawn a target object. Objects are specified in
//               script only and do not clone current scene objects. They will
//               spawn automatically, but can be forced to spawn the target
//               object. They may also despawn any spawned objects if the
//               camera pans too far away from the spawn point and the object.
//-----------------------------------------------------------------------------

if ( !isObject( SpawnPointBehavior ) )
{
    %template = new BehaviorTemplate( SpawnPointBehavior );

    %template.friendlyName = "Spawn Point";
    %template.behaviorType = "Miscellaneous";
    %template.description  = "Spawns target object when the camera gets within range";

    %template.addBehaviorField( TargetType,       "Spawn this type of object",               ENUM,   "t2dAnimatedSprite", "pskActor2D" TAB "pskActor3D" TAB "t2dAnimatedSprite" TAB "t2dStaticSprite" TAB "t2dSceneObject" );
    %template.addBehaviorField( TargetObject,     "Spawn this object (uses datablocks)",     OBJECT, "", t2dSceneObjectDatablock );
    %template.addBehaviorField( MinSpawnDistance, "Minimum spawn distance",                  FLOAT,  60 );
    %template.addBehaviorField( MaxSpawnDistance, "Maximum spawn distance",                  FLOAT,  100 );

    %template.addBehaviorField( AutoDespawn,      "Despawn object when they are far away",   BOOL,   true );
    %template.addBehaviorField( NumberToSpawn,    "Number of objects",                       INT,    1 );
    %template.addBehaviorField( SpawnInterval,    "Time between spawns (sec)",               FLOAT,  5 );
    %template.addBehaviorField( SpawnOnKill,      "Spawn another object when one is killed", BOOL,   false );
}

//-----------------------------------------------------------------------------

function SpawnPointBehavior::onBehaviorAdd( %this )
{
    // Only do this if we're not in the level editor
    if ( isToolBuild() )
    {
        return;
    }

    // Create the Actor Component
    %component = new pskSpawnPoint();

    // Copy all of the behavior information to the component
    copyBehaviorFields( %this, %component );

    // Add the component
    if ( !%this.Owner.addComponents( %component ) )
    {
        error ( "SpawnPointBehavior::onBehaviorAdd - Failed to register Spawn Point Component" );
        %component.safeDelete();
        return;
    }

    // We can now remove the behavior (it gets deleted automatically)
    %this.Owner.removeBehavior( %this );
}

function pskSpawnPoint::onAddToScene( %this, %scenegraph )
{
    // Set up the details if we have a target
    if ( isObject( %this.TargetObject ) && %this.NumberToSpawn > 0 )
    {
        %this.Owner.setObjectType( "SpawnPointObject" );
        %this.Owner.setCollidesWith( "None" );
    }

    // Hide this object
    %this.Owner.Visible = false;
}