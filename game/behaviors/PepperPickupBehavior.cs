//-----------------------------------------------------------------------------
// Platformer Starter Kit
// Copyright (C) Phillip O'Shea
//  
// Pepper Pickup - This provides health to Players! (Must be added to a pick-
//                 up object)
//-----------------------------------------------------------------------------

if ( !isObject( PepperPickupBehavior ) )
{
    %template = new BehaviorTemplate( PepperPickupBehavior );

    %template.friendlyName = "Pepper Pick-up";
    %template.behaviorType = "Collectable";
    %template.description  = "This pepper restores health";

    %template.addBehaviorField( HealAmount, "Amount of health restored", INT, 10 );
}

function PepperPickupBehavior::onAddToScene( %this, %scenegraph )
{
    if ( %this.Owner.isMemberOfClass( t2dAnimatedSprite ) )
    {    
        %animation  = %this.Owner.AnimationName;
        %frameCount = getWordCount( %animation.AnimationFrames );

        %this.Owner.setAnimationFrame( getRandom( 0, ( %frameCount - 1 ) ) );
    }
}

function PepperPickupBehavior::confirmPickup( %this, %targetObject, %inventoryItem )
{
    if ( %targetObject.isMethod( "healDamage" ) )
    {
        %targetObject.healDamage( 10 );
    }

    if ( isObject( PepperPickupSound ) )
    {
        %targetObject.playSound( PepperPickupSound );
    }

    return true;
}