//-----------------------------------------------------------------------------
// Platformer Starter Kit
// Copyright (C) Phillip O'Shea
//-----------------------------------------------------------------------------

// New layers can be specified in the dynamic fields of your level's scenegraph
// FieldName:  "ParallaxLayerX", where X is an integer >= 0
// FieldValue: <TYPE> <LEVEL_NAME> <ZOOM_LEVEL>
// Note: Must be called AFTER a level is loaded into the main scenewindow!
function initialiseParallaxLayers()
{
    // Current scenegraph
    %sceneGraph = sceneWindow2D.getSceneGraph();

    // Clear background layers
    for ( %i = 0; isObject( "ParallaxBackground" @ %i ); %i++ )
    {
        ( "ParallaxBackground" @ %i ).delete();
    }
 
    // Clear foreground layers
    for ( %i = 0; isObject( "ParallaxForeground" @ %i ); %i++ )
    {
        ( "ParallaxForeground" @ %i ).delete();
    }

    // Loop through the specified fields
    for ( %i = 0; %sceneGraph.getFieldValue( "ParallaxLayer" @ %i ) !$= ""; %i++ )
    {
        // Get the properties and add the new layer
        %layerProperties = %sceneGraph.getFieldValue( "ParallaxLayer" @ %i );
        newParallaxLayer( getWord( %layerProperties, 0 ),
                          getWord( %layerProperties, 1 ),
                          getWord( %layerProperties, 2 ) );
    }

    // Refresh the parallax mount
    if ( isObject( $ParallaxScrollTarget ) )
    {
        $ParallaxScrollTarget.mountCamera();
    }
}

// Create a new parallax layer
// Type:      "BG" for a background layer
//            "FG" for a foreground layer
// LevelName: Level file to load into the window
// ZoomLevel: Camera zoom, < 1.0 for background, > 1.0 for foreground
//
// Note: Layers are loaded in this format:
//          ** MiscBackgroundGUIControls
//          ParallaxBackgroundX
//             ...             ...
//          ParallaxBackground0
//          scenewindow2D
//          ParallaxForeground0
//             ...             ...
//          ParallaxForegroundX
//          ** MiscForegroundGUIControls
function newParallaxLayer( %type, %levelName, %zoomLevel )
{
    // Level file
    %levelFile = "game/data/levels/" @ %levelName @ ".t2d";
    if ( !isFile( %levelFile ) && !isFile( %levelFile @ ".dso" ) )
    {
        warn ( "newParallaxLayer() - Unknown level file \"" @ %levelFile @ "\"" );
        return;
    }

    // Find the canvas content
    %mainScreen = Canvas.getContent();

    // Set up the way we create and position the type of parallax layer
    switch$ ( %type )
    {
        // Background
        case "BG" : %layerType  = "ParallaxBackground";
                    %startIndex = 0;
                    %endIndex   = %mainScreen.getCount();

        // Foreground
        case "FG" : %layerType  = "ParallaxForeground";
                    %startIndex = %mainScreen.getCount();
                    %endIndex   = 0;
        
        // Unknown Type.
        default   : warn ( "newParallaxLayer() - Unknown layer type \"" @ %type @ "\"" );
                    return;
    }

    // Find the layer number for that type
    %layerIndex = 0;
    while ( isObject( %layerType @ %layerIndex ) )
    {
        %layerIndex += 1;
    }

    // New window
    %sceneWindow = new t2dSceneWindow( %layerType @ %layerIndex )
    {
        // Position and size
        Position = "0 0";
        Extent   = %mainScreen.getExtent();

        // Ensure it always covers the entire window
        HorizSizing = "WIDTH";
        VertSizing  = "HEIGHT";
    };

    // Add it to the gui
    %mainScreen.add( %sceneWindow );

    // Ensure it is positioned nicely in the list
    for ( %i = %startIndex; %i != %endIndex; %i += ( 2 * ( %endIndex > %startIndex ) - 1 ) )
    {
        %guiControl = %mainScreen.getObject( %i );

        // Skip it if it is the one we created
        if ( %guiControl == %sceneWindow )
        {
            continue;
        }

        // Find the closest window
        if ( %guiControl.getClassName() $= "t2dSceneWindow" )
        {
            // Order it nicely
            %mainScreen.reorderChild( %sceneWindow, %guiControl );

            // Reorder foreground windows to be on top
            if ( %startIndex > %endIndex )
            {
                %mainScreen.reorderChild( %guiControl, %sceneWindow );
            }

            // No need to continue looping
            break;
        }
    }

    // Load the prefs
    %sceneWindow.setCurrentCameraZoom( %zoomLevel );
    %sceneWindow.loadlevel( %levelFile );

    // Create a mount for the camera
    %cameraMount = new t2dSceneObject( %layerType @ "Mount" @ %layerIndex )
    {
        // Remember to add it to the scenegraph
        SceneGraph    = sceneWindow2D.getSceneGraph();

        // Store the scene window
        SceneWindow   = %sceneWindow;
    };

    // Mount the new camera to the blank scene object
    // Note: The way you mount these here should be the same way you mount
    //       the normal camera to the player. Otherwise things look squiffy!
    %sceneWindow.mountCamera( %cameraMount );

    // Create a simset to store all the mounts
    if ( !isObject( ParallaxLayerMounts ) )
    {
        new SimSet( ParallaxLayerMounts );
    }

    // Add it to the list
    ParallaxLayerMounts.add( %cameraMount );
}