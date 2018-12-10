//-----------------------------------------------------------------------------
// Platformer Starter Kit
// Copyright (C) Phillip O'Shea
//  
// Pick-Up Item - In most games, there are objects which can be collected.
//                Add this behavior to an object that you want to be
//                collectable. The type of object that is to be picked up
//                must be specified in another behavior (see PepperPickup or
//                CheckPoint behaviors for more information).
//-----------------------------------------------------------------------------

if ( !isObject( PickupBehavior ) )
{
    %template = new BehaviorTemplate( PickupBehavior );

    %template.friendlyName = "Pick-up Item";
    %template.behaviorType = "Collectable";
    %template.description  = "Pick up this item";

    %template.addBehaviorField( AddToInventory, "Add this item to inventory",            BOOL, false );
    %template.addBehaviorField( DeleteOnPickup, "Delete this item once it is picked up", BOOL, true );
}

function PickupBehavior::onAddToScene( %this, %scenegraph )
{
    // Make sure this object doesn't collide
    %this.Owner.setCollisionActive( 0, 1 );
    %this.Owner.setCollisionPhysics( 0, 0 );
    %this.Owner.setCollisionCallback( 1 );

    // Make sure the trigger only collides with the player
    %this.Owner.setObjectType( "PlayerTrigger" );
    %this.Owner.setCollidesWith( "PlayerObject" );
    
    if ( %this.Owner.isMethod( "setNeverSolvePhysics" ) )
    {
        // No Physics resolution
        %this.Owner.setNeverSolvePhysics( true );
    }
}

function PickupBehavior::onCollision( %this, %theirObject, %ourRef, %theirRef, %time, %normal, %contacts, %points )
{
    // Should it be added to the inventory?
    %inventoryItem = %this.AddToInventory;

    // Add it to the inventory
    if ( %inventoryItem && isObject( %theirObject.InventoryList ) )
    {
        // Find the inventory list
        %inventory = %theirObject.InventoryList;

        // Cancel pick-up if there are already too many items
        if ( %inventory.getCount() == %inventory.MaxItems )
        {
            // Cannot pick up this item
            error ( "PickupBehavior::onEnter() - Inventory Full." );
            return;
        }

        // Hide the item
        %this.Owner.Visible = false;

        // If it was from a spawn point, remove it from the spawn point's list
        if ( isObject( %this.Owner.SpawnPoint ) )
        {
            %this.Owner.SpawnPoint.SpawnedObjects.remove( %this.Owner );
        }

        // Add it to the inventory
        %inventory.add( %this.Owner );
    }

    // Notify other behaviors the object has been picked up
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

        // Check if we've been picked up or not
        if ( %behavior.isMethod( "confirmPickup" ) )
        {
            %canDelete = %behavior.confirmPickup( %theirObject, %inventoryItem );
        }
    }

    // Destroy the object?
    if ( %this.DeleteOnPickup && %canDelete )
    {
        if ( !%inventoryItem )
        {
            %this.Owner.safeDelete();
        }
    }
}