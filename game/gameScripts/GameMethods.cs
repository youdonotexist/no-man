//-----------------------------------------------------------------------------
// Platformer Starter Kit
// Copyright (C) Phillip O'Shea
//  
// Game Methods - This file contains many of the global functions that are
//                used throughout this kit.
//-----------------------------------------------------------------------------

$Game::Cheat::InfiniteAmmo = true;

$Game::Effects::Layer = 2;
$Game::Effects::SizeThreshold = 100;

//-----------------------------------------------------------------------------

/// Sets up the foundations for the kit
function initialisePlatformerKit()
{
    // Object Manager.
    exec ( "./ObjectManager.cs" );
    
    // Register Platformer Kit Types.
    registerObjectType( "SpawnPointObject" );
    registerObjectType( "PlatformObject OneWayPlatform" );
    registerObjectType( "PlatformObject SolidPlatform" );

    registerObjectType( "ActorObject PlayerObject" );
    registerObjectType( "ActorObject EnemyObject" );

    registerObjectType( "ActorTrigger PlayerTrigger" );
    registerObjectType( "ActorTrigger EnemyTrigger" );

    registerObjectType( "Projectile PlayerProjectile" );
    registerObjectType( "Projectile EnemyProjectile" );
    registerObjectType( "ProjectileCollider" );

    // Setup Object Information.
    setPlatformMask( getObjectTypeGroup( "PlatformObject" ) );
    setTriggerMask( getObjectTypeGroup( "ActorTrigger" ) );
    setSpawnPointMask( getObjectTypeGroup( "SpawnPointObject" ) );

    // Sound Manager.
    exec ( "./SoundManager.cs" );
    // Parallax Manager.
    exec ( "./ParallaxMethods.cs" );

    // Action Scripts.
    exec ( "./ActionScripts.cs" );

    // Weapon Scripts.
    exec ( "./Weapons/WeaponData.cs" );
    exec ( "./Weapons/ProjectileMethods.cs" );
    exec ( "./Weapons/FireBallLauncher.cs" );
    exec ( "./Weapons/BoneLauncher.cs" );
    exec ( "./Weapons/BulletWeapon.cs" );
    exec ( "./Weapons/RocketLauncher.cs" );

    // Base Actor Scripts.
    exec ( "./ActorMethods.cs" );
    // Derived Actor Scripts.
    exec ( "./BoomBotMethods.cs" );
    exec ( "./PlayerMethods.cs" );
    exec ( "./DrillMethods.cs" );
	exec ( "./FirstPuzzle.cs");
	exec ( "./SecondPuzzle.cs");
	exec ( "./ThirdPuzzle.cs");
	exec ( "./BoatPuzzle.cs");
}

//-----------------------------------------------------------------------------

function t2dSceneObject::getPlatformInstance( %this )
{
    %platformBehavior = %this.getBehavior( PlatformBehavior );
    if ( isObject( %platformBehavior ) )
    {
        return %platformBehavior;
    }

    return 0;
}

function t2dSceneObject::getSpawnPointInstance( %this )
{
    %spawnPointComponent = %this.getComponentByName( pskSpawnPoint );
    if ( isObject( %spawnPointComponent ) )
    {
        return %spawnPointComponent;
    }

    return 0;
}

//-----------------------------------------------------------------------------

/// Load any ingame features here
function t2dSceneGraph::onLevelLoaded( %this )
{
    if ( sceneWindow2D.getSceneGraph() != %this )
    {
        return;
    }

    // Ensure that the channel volumes are reset
    %soundChannel = getSoundChannel();
    %soundVolume  = $SoundManager::Sound::Volume;

    %musicChannel = getMusicChannel();
    %musicVolume  = $SoundManager::Music::Volume;

    alxSetChannelVolume( %soundChannel, %soundVolume );
    alxSetChannelVolume( %musicChannel, %musicVolume );

    // Play the musica
    if ( isObject( GoodMusic ) )
    {
        playMusic( GoodMusic );
    }

    // Delete the old manager
    if ( isObject( SpawnPointManager ) )
    {
        SpawnPointManager.delete();
    }

    // Create the Spawn Point Manager
    %spawnPointManager = new pskSpawnPointManager( SpawnPointManager )
    {
        SceneGraph     = %this;
        UpdateInterval = 200;
    };

    // Initialise layers in the next frame
    schedule( 32, 0, initialiseParallaxLayers );
	
	echo ("On level loaded..");
}

// setViewLimitOn("-300 -165", "300 55");
// setViewLimitOn("-150 -37.5", "150 37.5");
function setViewLimitOn( %minPoint, %maxPoint )
{
    %content      = Canvas.getContent();
    %contentCount = %content.getCount();
    for ( %i = 0; %i < %contentCount; %i++ )
    {
        %contentObj = %content.getObject( %i );
        if ( %contentObj.getClassName() !$= "t2dSceneWindow" )
        {
            continue;
        }
        
        %cameraZoomMod     = 1 / %contentObj.getCurrentCameraZoom();
        %cameraZoomFactorX = %contentObj.getCurrentCameraSize().X * ( %cameraZoomMod - 1 ) / 2.0;
        %cameraZoomFactorY = %contentObj.getCurrentCameraSize().Y * ( %cameraZoomMod - 1 ) / 2.0;
        
        %cameraMinPoint = t2dVectorSub( %minPoint, %cameraZoomFactorX SPC %cameraZoomFactorY );
        %cameraMaxPoint = t2dVectorAdd( %maxPoint, %cameraZoomFactorX SPC %cameraZoomFactorY );
        
        %contentObj.setViewLimitOn( %cameraMinPoint SPC %cameraMaxPoint );
    }
}

//-----------------------------------------------------------------------------

/// Returns the scene time
function getSceneTime()
{
    return sceneWindow2D.getSceneGraph().getSceneTime();
}

//-----------------------------------------------------------------------------

function copyBehaviorFields( %behavior, %dstObject )
{
    %template = %behavior.Template;
    if ( !isObject( %template ) )
    {
        return false;
    }

    %fieldCount = %template.getBehaviorFieldCount();
    for ( %i = 0; %i < %fieldCount; %i++ )
    {
        %fieldName  = getField( %template.getBehaviorField( %i ), 0 );
        %fieldValue = %behavior.getFieldValue( %fieldName );
        
        %dstObject.setFieldValue( %fieldName, %fieldValue );
    }

    return true;
}

function t2dSceneObject::getComponentByName( %this, %name )
{
    %count = %this.getComponentCount();
    for ( %i = 0; %i < %count; %i++ )
    {
        %comp = %this.getComponent( %i );
        if ( %comp.getClassName() $= %name )
        {
            return %comp;
        }
    }

    return -1;
}

//-----------------------------------------------------------------------------

/// Just a quick function to ensure all cameras get mounted in the same way
function t2dSceneWindow::mountCamera( %this, %mountTarget )
{
    %this.mount( %mountTarget, "0 -1", 0, true );
}

/// Mounts the scene camera to the object
function t2dSceneObject::mountCamera( %this )
{
    if ( sceneWindow2D.getIsCameraMounted() )
    {
        dismountCamera();
    }

    // Mount the camera to the objects
    sceneWindow2D.mountCamera( %this );

    // Mount parallax mounts
    if ( isObject( ParallaxLayerMounts ) )
    {
        for ( %i = 0; %i < ParallaxLayerMounts.getCount(); %i++ )
        {
            %parallaxMount  = ParallaxLayerMounts.getObject( %i );
            %parallaxWindow = %parallaxMount.SceneWindow;

            // Mount the window to the mount
            %parallaxWindow.mountCamera( %parallaxMount );

            // Mount to the target
            %parallaxMount.mount( %this );
        }
    }
}

/// Dismounts the scene camera
function dismountCamera()
{
    if ( !sceneWindow2D.getIsCameraMounted() )
    {
        return;
    }

    // Dismount the camera from the object
    sceneWindow2D.dismount();

    // Dismount parallax mounts
    if ( isObject( ParallaxLayerMounts ) )
    {
        for ( %i = 0; %i < ParallaxLayerMounts.getCount(); %i++ )
        {
            %parallaxMount  = ParallaxLayerMounts.getObject( %i );
            %parallaxWindow = %parallaxMount.SceneWindow;

            // Dismount the camera
            %parallaxWindow.dismount();
            
            // Dismount the mount
            %parallaxMount.dismount();
        }
    }
}

//-----------------------------------------------------------------------------

function loadNewLevel( %levelFile, %delay )
{
    // Opt out early if nothing was passed
    if ( %levelFile $= "" )
    {
        return;
    }

    // Prevents a crash ; )
    if ( %delay $= "" || %delay < 100 ) %delay = 100;

    // If you haven't included a directory then we'll use the default
    if ( !isFile( %levelFile ) )
    {
        %levelFile = "game/data/levels/" @ %levelFile;
    }

    // Add on the extension if needed
    %fileExtn = ".t2d";
    if ( getSubStr( %levelFile, strlen( %levelFile ) - strlen( %fileExtn ), strlen( %fileExtn ) ) !$= %fileExtn )
    {
        %levelFile = %levelFile @ %fileExtn;
    }

    // Load the file if possible
    if ( isFile( %levelFile ) )
    {
        sceneWindow2D.schedule( %delay, "loadLevel", %levelFile );
    }
    else
    {
        warn( "loadNewLevel() - Level not found: " @ %levelFile );
    }
}

//-----------------------------------------------------------------------------

/// Displays the game over sequence
function gameOver()
{
    if ( isObject( gameOverBanner ) )
    {
        // Dismount the camera
        dismountCamera();

        // Show the game over banner
        gameOverBanner.Position = sceneWindow2D.getCurrentCameraPosition();
        gameOverBanner.Visible  = true;

        // Stop the current music
        stopMusic();
    }
}

/// Displays the level completed sequence
function levelComplete()
{
    if ( isObject( congratulationsBanner ) )
    {
        // Dismount the camera
        dismountCamera();

        // Show the congratulations banner
        congratulationsBanner.Position = sceneWindow2D.getCurrentCameraPosition();
        congratulationsBanner.Visible  = true;

        if ( isObject( LevelCompleteMusic ) )
        {
            // Stop any playing sounds and mute the sound effects channel
            alxStopAll();
            alxSetChannelVolume( getSoundChannel(), 0.0 );

            // Play the level complete music
            playMusic( LevelCompleteMusic );
        }
    }
}

//-----------------------------------------------------------------------------

/// %vA = 2d vector; %vB = int/2d vector
function mVectorMultiply( %vA, %vB )
{
    %x = ( %vA.X * %vB.X );
    %y = ( getWordCount( %vB ) > 1 ) ? ( %vA.Y * %vB.Y ) : ( %vA.Y * %vB.X );
    
    return %x SPC %y;
}

/// Ensures x isn't outside of the min and max values
function mClamp( %x, %min, %max )
{
    if ( %x < %min )
        return %min;
    
    if ( %x > %max )
        return %max;
        
    return %x;
}

/// See if two vectors are equal
function mVectorsEqual( %va, %vb )
{
    return ( %va.x == %vb.x && %va.y == %vb.y );
}

/// See if a point is within a line
function mAxisOverlap( %x, %vA, %vB )
{
    if ( %x >= %vA && %x <= %vB )
        return true;
    
    return false;
}

function mMin( %a, %b )
{
    if ( getWordCount( %a ) == 2 && getWordCount( %b ) == 2 )
    {
        if ( t2dVectorLength( %a ) < t2dVectorLength( %b ) )
            return %a;
        
        return %b;
    }
    else
    {
        if ( %a < %b )
            return %a;
        
        return %b;
    }
}

function mMax( %a, %b )
{
    if ( getWordCount( %a ) == 2 && getWordCount( %b ) == 2 )
    {
        if ( t2dVectorLength( %a ) > t2dVectorLength( %b ) )
            return %a;
        
        return %b;
    }
    else
    {
        if ( %a > %b )
            return %a;
        
        return %b;
    }
}

/// Creates a rotation matrix
function mRotationMatrix( %angle )
{
    %angle = mDegToRad( %angle );
    %sin   = mSin( %angle );
    %cos   = mCos( %angle );
    
    return %cos SPC %sin SPC -%sin SPC %cos;
}

/// Multiply a 2x2 matrix with a 2x1 vector
function mMatrixMultiply( %m, %v )
{
    %m11 = getWord( %m, 0 );
    %m12 = getWord( %m, 1 );
    %m21 = getWord( %m, 2 );
    %m22 = getWord( %m, 3 );
    
    %v1  = getWord( %v, 0 );
    %v2  = getWord( %v, 1 );
    
    %x   = %m11 * %v1 + %m12 * %v2;
    %y   = %m21 * %v1 + %m22 * %v2;
    
    return %x SPC %y;
}

//-----------------------------------------------------------------------------

/// Loops through all of the objects in a SimSet and returns them as a string
function SimSet::storeSet( %this )
{
    // Loop through all the objects and store them in a string
    %objectCount = %this.getCount();
    for ( %i = 0; %i < %objectCount; %i++ )
    {
        %listString = %listString SPC %this.getObject( %i );
    }

    // Trim and return
    return trim( %listString );
}

/// Loop through objects in a SimSet and make sure they are on the specified list.
/// It will also attempt to re-add removed items not in the set.
function SimSet::restoreSet( %this, %objectList, %delete, %restore )
{
    if ( %delete  $= "" ) %delete  = true;
    if ( %restore $= "" ) %restore = true;

    // Delete new items
    for ( %i = 0; %i < %this.getCount(); %i++ )
    {
        // Grab the object
        %object = %this.getObject( %i );

        // Check if it is part of the original list
        if ( !isWordInList( %objectList, %object ) )
        {
            // Remove it from the set
            %this.remove( %object );

            // Delete it if requested
            if ( %delete )
            {
                %object.safeDelete();
            }

            // Decrement the counter
            %i--;
        }
    }
    
    // Attempt to restore old items
    if ( %restore )
    {
        for ( %i = 0; %object < getWordCount( %objectList ); %i++ )
        {
            // Grab the object
            %object = getWord( %objectList, %i );

            // Check if it is still part of the set
            if ( !%this.isMember( %object ) )
            {
                // Check if it still exists
                if ( !isObject( %object ) )
                {
                    continue;
                }

                // Re-add it to the list
                %this.add( %object );
            }
        }
    }
}

//-----------------------------------------------------------------------------

// Find the first platform at the given position
function getPlatform( %worldPosition, %step )
{
    if ( %step $= "" ) %step = 50;
    
    %sceneGraph = sceneWindow2D.getSceneGraph();
    for ( %i = 0;
         %pickList = %sceneGraph.pickLine( t2dVectorAdd( %worldPosition, 0 SPC %step * ( %i + 0 ) ), t2dVectorAdd( %worldPosition, 0 SPC %step * ( %i + 1 ) ) );
         %i++ )
    {
        %objectCount = getWordCount( %pickList );
        for ( %j = 0; %j < %objectCount; %j++ )
        {
            %object = getWord( %pickList, %j );
            %platformBehavior = %object.getBehavior( PlatformBehavior );
            if ( %platformBehavior )
            {
                %surfacePoint = %platformBehavior.getSurfacePosition( %worldPosition.X );
                if ( %surfacePoint.Y >= %worldPosition.Y )
                    return %object;
            }
        }
        
        // Quit if we're stretching too far
        if ( %i == 100 )
            break;
    }
}

//-----------------------------------------------------------------------------

function isWordInList( %list, %word )
{
   %wordCount = getWordCount( %list );
   for ( %i = 0; %i < %wordCount; %i++ )
      if ( getWord( %list, %i ) $= %word )
         return true;
   
   return false;
}

function isWordInFieldList( %list, %word )
{
   %fieldCount = getFieldCount( %list );
   for ( %i = 0; %i < %fieldCount; %i++ )
      if ( isWordInList( getField( %list, %i ), %word ) )
         return true;
   
   return false;
}

function removeWordFromList( %list, %word )
{
   %wordCount = getWordCount( %list );
   for ( %i = 0; %i < %wordCount; %i++ )
      if ( getWord( %list, %i ) $= %word )
         return removeWord( %list, %i );
   
   return %list;
}

//-----------------------------------------------------------------------------

/// Bug fix
function t2dSceneObject::onFrameChange( %this, %frame )
{
    // Without this empty function TGB will not call the behavior's "onFrameChange" function
}