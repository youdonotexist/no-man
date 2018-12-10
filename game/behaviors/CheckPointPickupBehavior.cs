//-----------------------------------------------------------------------------
// Platformer Starter Kit
// Copyright (C) Phillip O'Shea
//  
// Check Point - Check points record spawn point and inventory information
//               for each spawn point, and actor in the scene. Check points
//               are a collectable item, meaning they must be added to an
//               object which contains a PickUp behavior.
//-----------------------------------------------------------------------------

if ( !isObject( CheckPointPickupBehavior ) )
{
    %template = new BehaviorTemplate( CheckPointPickupBehavior );
    
    %template.friendlyName = "Check Point Pick Up";
    %template.behaviorType = "Collectable";
    %template.description  = "Set the actor's restore point to this position";
}

function CheckPointPickupBehavior::confirmPickup( %this, %targetObject, %inventoryItem )
{
    // Change the respawn position
    %targetObject.RespawnPosition = %this.Owner.Position;
    
    // Save the check point data
    saveCheckPoint();
    
    // Play the sound
    if ( isObject( checkPointPickupSound ) )
    {
        playSound( CheckPointPickupSound );
    }
    
    return true;
}

function saveCheckPoint()
{
    // The object list we'll use
    %SpawnPointList = getObjectTypeList( "SpawnPointObject" );
    if ( !isObject( %SpawnPointList ) )
    {
        return false;
    }
        
    // Record the number of objects spawned from each spawn point
    for ( %i = 0; %i < %SpawnPointList.getCount(); %i++ )
    {
        // Get the id
        %SpawnPoint = %SpawnPointList.getObject( %i ).getSpawnPointInstance();
        
        if ( !%SpawnPoint )
        {
            continue;
        }
        
        // Record the number of objects spawned at this time
        %SpawnPoint.CheckPointRecord = %SpawnPoint.SpawnCount;
        
        // Deduct the number of objects alive
        %SpawnPoint.CheckPointRecord -= %SpawnPoint.getSpawnedObjectCount();
    }
    
    // Save the contents of each actor's inventory
    %ActorList = getObjectTypeList( "ActorObject" );
    
    // Record all of the objects in the actor's inventory
    for ( %i = 0; %i < %ActorList.getCount(); %i++ )
    {
        %itemList = %ActorList.getObject( %i ).InventoryList;
        if ( isObject( %itemList ) )
        {
            %itemList.CheckPointRecord = %itemList.storeSet();
        }
    }
    
    return true;
}

function loadCheckPoint()
{        
    // The object list we'll use
    %SpawnPointList = getObjectTypeList( "SpawnPointObject" );
    if ( !isObject( %SpawnPointList ) )
    {
        return false;
    }
    
    for ( %i = 0; %i < %SpawnPointList.getCount(); %i++ )
    {
        // Get the id
        %SpawnPoint = %SpawnPointList.getObject( %i ).getSpawnPointInstance();
        
        if ( !%SpawnPoint || !%SpawnPoint.AutoDespawn )
        {
            continue;
        }
        
        // Destroy any spawned objects that haven't been removed
        %SpawnPoint.despawnAll();
        
        // Revert the number spawned to the number at the last check less removed object
        %SpawnPoint.SpawnCount = %SpawnPoint.CheckPointRecord;
        
        // Disable used spawn points
        if ( %SpawnPoint.SpawnCount == %SpawnPoint.MaxSpawnCount )
        {
            %SpawnPoint.Owner.enableUpdateCallback();
        }
    }
    
    // Restore the contents of each actor's inventory
    %ActorList = getObjectTypeList( "ActorObject" );
    
    // Restore the objects in the actor's inventory
    for ( %i = 0; %i < %ActorList.getCount(); %i++ )
    {
        %itemList = %ActorList.getObject( %i ).InventoryList;
        if ( isObject( %itemList ) )
        {
            %itemList.restoreSet( %itemList.CheckPointRecord, true );
        }
    }
    
    return true;
}