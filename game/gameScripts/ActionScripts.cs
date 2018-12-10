//-----------------------------------------------------------------------------
// Platformer Starterkit Pro
// Copyright (C) - Phillip O'Shea
//-----------------------------------------------------------------------------

new ScriptObject( BaseAction )
{
    SuperClass = "pskAction";
};

//-----------------------------------------------------------------------------

function pskAction::isActive( %this, %referenceObject )
{
    return isObject( %this.ReferenceObjects ) ? %this.ReferenceObjects.isMember( %referenceObject ) : false;
}

//-----------------------------------------------------------------------------

function pskAction::addReference( %this, %referenceObject )
{
    if ( !isObject( %this.ReferenceObjects ) )
    {
        %this.ReferenceObjects = new SimSet();
    }

    %this.ReferenceObjects.add( %referenceObject );
}

function pskAction::removeReference( %this, %referenceObject )
{
    if ( isObject( %this.ReferenceObjects ) && %this.ReferenceObjects.isMember( %referenceObject ) )
    {
        %this.ReferenceObjects.remove( %referenceObject );
    }
}

//-----------------------------------------------------------------------------

function pskAction::pushEvent( %this, %referenceObject, %newEvent )
{
    // Clean the event list first
    %this.cleanEvents( %referenceObject );

    // Grab the current list of events
    %eventName = "Event" @ %referenceObject.getId();
    %eventList = %this.getFieldValue( %eventName );

    // Store the value
    %this.setFieldValue( %eventName, ltrim( %eventList SPC %newEvent ) );
}

function pskAction::popEvents( %this, %referenceObject )
{
    %eventName = "Event" @ %referenceObject.getId();
    %eventList = %this.getFieldValue( %eventName );

    %eventCount = getWordCount( %eventList );
    for ( %i = 0; %i < %eventCount; %i++ )
    {
        %eventId = getWord( %eventList, %i );
        if ( isEventPending( %eventId ) )
        {
            cancel( %eventId );
        }
    }

    // Clear the field
    %this.setFieldValue( %eventName, "" );
}

function pskAction::cleanEvents( %this, %referenceObject )
{
    // Grab the current list of events
    %eventName = "Event" @ %referenceObject.getId();
    %eventList = %this.getFieldValue( %eventName );

    // Do a quick cull of old events
    %ignoreIndex = 0;
    while ( %ignoreIndex != getWordCount( %fieldValue ) )
    {
        %eventId = getWord( %eventList, %ignoreIndex );
        if ( !isEventPending( %eventId ) )
        {
            removeWord( %eventList, %ignoreIndex );
        }
        else
        {
            %ignoreIndex += 1;
        }
    }

    // Store the value
    %this.setFieldValue( %eventName, %eventList );
}

//-----------------------------------------------------------------------------

function pskAction::triggerAction( %this, %referenceObject, %trigger )
{
    if ( !isObject( %referenceObject ) )
    {
        return;
    }

    if ( %trigger && %this.isActive( %referenceObject ) )
    {
        return;
    }

    if ( %trigger )
    {
        %triggerName = "TriggerTime" @ %referenceObject.getId();
        %triggerTime = %this.getFieldValue( %triggerName );
        if ( %triggerTime $= "" )
        {
            %triggerTime = 0;
        }

        // Add the reference to the object
        %this.addReference( %referenceObject );

        %scheduleTime = %triggerTime + %this.CoolDown - getSimTime();
        if ( %scheduleTime <= 0 )
        {
            // Start the loop
            %this.triggerLoop( %referenceObject );

            if ( isObject( %this.TriggerSound ) )
            {
                // Play Trigger sound
                %referenceObject.playSound( %this.TriggerSound );
            }
        }
        else if ( %this.Continuous )
        {
            // Schedule to start the loop after the cooldown expires
            %eventId = %this.schedule( %scheduleTime, "triggerLoop", %referenceObject );

            if ( isObject( %this.TriggerSound ) )
            {
                // Schedule Trigger sound
                %referenceObject.schedule( %scheduleTime, "playSound", %this.TriggerSound );
            }

            // Push this event
            %this.pushEvent( %referenceObject, %eventId );
        }
    }
    else
    {
        // Clear the event
        %this.popEvents( %referenceObject );

        // Clear the reference
        %this.removeReference( %referenceObject );

        if ( isObject( %this.TriggerSound ) && %this.TriggerSound.Description.isLooping )
        {
            // Stop playing any sounds
            %referenceObject.stopSound();
        }
    }
}

function pskAction::triggerLoop( %this, %referenceObject )
{
    // Cancel previous events
    %this.popEvents( %referenceObject );

    // Store the current trigger time
    %this.setFieldValue( "TriggerTime" @ %referenceObject.getId(), getSimTime() );

    if ( %this.Continuous )
    {
        // Schedule the next event
        %nextEvent = %this.schedule( %this.CoolDown, "triggerLoop", %referenceObject );

        // Store the value
        %this.pushEvent( %referenceObject, %nextEvent );
    }

    if ( isObject( %this.TriggerLoopSound ) )
    {
        %referenceObject.playSound( %this.TriggerLoopSound );
    }
}