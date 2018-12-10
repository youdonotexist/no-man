//-----------------------------------------------------------------------------
// Platformer Starter Kit
// Copyright (C) Phillip O'Shea
//  
// Controller - This behavior translate the input from the user into the
//              actions of its owner. All a controller does is provide a brain
//              for the body, telling it which direction to face, when to jump
//              or when to attack.
//-----------------------------------------------------------------------------

if ( !isObject( ControllerBehavior ) )
{
    %template = new BehaviorTemplate( ControllerBehavior );
    
    %template.friendlyName = "Player Controller";
    %template.behaviorType = "Actor";
    %template.description  = "Player Controller";
    
    %template.addBehaviorField( keyLeft,   "Move Left",  KEYBIND, "keyboard left" );
    %template.addBehaviorField( keyRight,  "Move Right", KEYBIND, "keyboard right" );
    %template.addBehaviorField( keyUp,     "Move Up",    KEYBIND, "keyboard up" );
    %template.addBehaviorField( keyDown,   "Move Down",  KEYBIND, "keyboard down" );
    %template.addBehaviorField( keyJump,   "Jump",       KEYBIND, "keyboard space" );
    %template.addBehaviorField( keyAttack, "Attack",     KEYBIND, "keyboard X" );
    %template.addBehaviorField( keyReload, "Reload",     KEYBIND, "keyboard R" );
}

/// Set up the player's controller
function ControllerBehavior::onAddToScene( %this, %scenegraph )
{
    // Reset
    %this.Owner.Direction = "0 0";
    
    // Set the collision details
    %this.Owner.setObjectType( "PlayerObject" );
    %this.Owner.setCollidesWith( "PlatformObject EnemyObject PlayerTrigger" );
    
    // Make sure we have a move map
    if ( !isObject( moveMap ) )
    {
        return;
    }
    
    // Bind the appropriate keys
    moveMap.bindCmd( getWord( %this.keyLeft, 0 ),   getWord( %this.keyLeft, 1 ),   %this @ ".keyDown( Left );",   %this @ ".keyUp( Left );" );
    moveMap.bindCmd( getWord( %this.keyRight, 0 ),  getWord( %this.keyRight, 1 ),  %this @ ".keyDown( Right );",  %this @ ".keyUp( Right );" );
    moveMap.bindCmd( getWord( %this.keyUp, 0 ),     getWord( %this.keyUp, 1 ),     %this @ ".keyDown( Up );",     %this @ ".keyUp( Up );" );
    moveMap.bindCmd( getWord( %this.keyDown, 0 ),   getWord( %this.keyDown, 1 ),   %this @ ".keyDown( Down );",   %this @ ".keyUp( Down );" );
    moveMap.bindCmd( getWord( %this.keyJump, 0 ),   getWord( %this.keyJump, 1 ),   %this @ ".keyDown( Jump );",   %this @ ".keyUp( Jump );" );
    //moveMap.bindCmd( getWord( %this.keyAttack, 0 ), getWord( %this.keyAttack, 1 ), %this @ ".keyDown( Attack );", %this @ ".keyUp( Attack );" );
    //moveMap.bindCmd( getWord( %this.keyReload, 0 ), getWord( %this.keyReload, 1 ), %this @ ".keyDown( Reload );", %this @ ".keyUp( Reload );" );
    
    // Make sure the controller works
    moveMap.push();
}

/// Store the Player reference.
function ControllerBehavior::onBehaviorAdd( %this )
{
    // Store the Owner.
    $Game::Player = %this.Owner;
}

/// A key is pressed
function ControllerBehavior::keyDown( %this, %keyDown )
{
    // Left key
    if ( %keyDown $= "Left" )
    {
        %this.Owner.Direction.X = -1;
    }
    
    // Right key
    if ( %keyDown $= "Right" )
    {
        %this.Owner.Direction.X =  1;
    }
    
    // Up key
    if ( %keyDown $= "Up" )
    {
        %this.Owner.Direction.Y = -1;
    }
    
    // Down key
    if ( %keyDown $= "Down" )
    {
        %this.Owner.Direction.Y =  1;
    }
    
    // Jump key
    if ( %keyDown $= "Jump" )
    {
        %this.Owner.Jump = true;
    }
    
    // Attack key
    if ( %keyDown $= "Attack" )
    {
        // Attacking.
        %this.Owner.Attack = true;
        
        // Attack.
        %this.Owner.onAttack( 0, true );
    }
    
    // Reload key
    if ( %keyDown $= "Reload" )
    {
        // Reload.
        %this.Owner.onReload( 0, true );
    }
}

/// A key is released
function ControllerBehavior::keyUp( %this, %keyUp )
{
    // Left key
    if ( %keyUp $= "Left" && %this.Owner.Direction.X == -1 )
    {
        %this.Owner.Direction.X = 0;
    }
    
    // Right key
    if ( %keyUp $= "Right" && %this.Owner.Direction.X == 1 )
    {
        %this.Owner.Direction.X = 0;
    }
        
    // Up key
    if ( %keyUp $= "Up" )
    {
        %this.Owner.Direction.Y = 0;
    }
    
    // Down key
    if ( %keyUp $= "Down" )
    {
        %this.Owner.Direction.Y = 0;
    }
    
    // Jump key
    if ( %keyUp $= "Jump" )
    {
        %this.Owner.Jump = false;
    }
    
    // Attack key
    if ( %keyUp $= "Attack" )
    {
        %this.Owner.onAttack( 0, false );
    }
    
    // Attack key
    if ( %keyUp $= "Attack" )
    {
        %this.Owner.onReload( 0, false );
    }
}

//-----------------------------------------------------------------------------
// Mouse Methods.
//-----------------------------------------------------------------------------

$PI                    = 3.141592654;
$Game::CrosshairOffset = 100;
$Game::MinLookAngle    = -$PI * 0.09256;
$Game::MaxLookAngle    =  $PI * 0.14942;

//-----------------------------------------------------------------------------

function updateLookPosition()
{
    // Valid Player?
    if ( !isObject( $Game::Player ) || $Game::Player.getClassName() !$= "pskActor3D" )
    {
        // Quit.
        return;
    }

    // Fetch Cursor Details.
    %canvasPoint  = Canvas.getCursorPos();
    %canvasCenter = t2dVectorScale( Canvas.getExtent(), 0.5 );
    %offset       = t2dVectorSub( %canvasPoint, %canvasCenter );

    // Determine the Direction and Look Angle.
    %flip      = false;
    %lookAngle = mAtan( %offset.y, %offset.x );

    if ( %lookAngle < ( -$PI / 2.0 ) )
    {
        %flip      = true;
        %lookAngle = -( %lookAngle + $PI );
    }
    else if ( %lookAngle > ( $PI / 2.0 ) )
    {
        %flip      = true;
        %lookAngle = -( %lookAngle - $PI );
    }

    // Determine the Look Position;
    %lookPosition = ( %lookAngle - $Game::MinLookAngle ) / ( $Game::MaxLookAngle - $Game::MinLookAngle );

    // Update the Player Information.
    $Game::Player.setLookPosition( %lookPosition );
    $Game::Player.setFlipX( %flip );
    
    // Update the Crosshair Position.
    updateCrosshairPosition();
}

function updateCrosshairPosition()
{
    // Fetch the Crosshair.
    %crosshairObject = CrossHairGui;
    // Valid?
    if ( !isObject( %crosshairObject ) || !%crosshairObject.isVisible() )
    {
        // Quit.
        return;
    }
    
    // Get the Muzzle Point & Vector.
    %muzzlePoint  = sceneWindow2D.getWindowPoint( $Game::Player.getMuzzlePoint() );
    %muzzleVector = $Game::Player.getMuzzleVector();

    // Offset the Muzzle Point.
    %crosshairPoint = t2dVectorAdd( %muzzlePoint, t2dVectorScale( %muzzleVector, $Game::CrosshairOffset ) );
    // Fetch the Half Size.
    %crosshairHalfSize = t2dVectorScale( %crosshairObject.getExtent(), 0.5 );
    // Reposition the Control.
    %crosshairObject.setPosition( mCeil( %crosshairPoint.X - %crosshairHalfSize.X ),
                                  mCeil( %crosshairPoint.Y - %crosshairHalfSize.Y ) );
}

function MouseInputCtrl::onAdd(%this)
{
	%this.controlledWord = "";
}

function MouseInputCtrl::onMouseMove( %this, %modifier, %worldPosition, %clicks )
{

}

function MouseInputCtrl::onMouseDragged( %this, %modifier, %worldPosition, %clicks )
{
	%worldPos = sceneWindow2D.getWorldPoint(%worldPosition);
	
	if (%this.controlledWord)
		%this.controlledWord.setPosition(%worldPos);
}

function MouseInputCtrl::onRightMouseDragged( %this, %modifier, %worldPosition, %clicks )
{

}

//-----------------------------------------------------------------------------

function MouseInputCtrl::onMouseDown( %this, %modifier, %worldPosition, %clicks )
{
	%sg = $Game::Player.getSceneGraph();
	
	%worldPos = sceneWindow2D.getWorldPoint(%worldPosition);
	%words = %sg.pickPoint(%worldPos);
	%wordCount = getWordcount(%words);
	%word = "";
	if (%wordCount > 0)
	{
		for (%i = 0; %i < %wordCount; %i++)
		{
			%tObj = getWord(%words, %i);
			if ( (%tObj.class $= "SpecialWord" || %tObj.class $= "SpecialObject") 
					&& %tObj.puzzleComplete == false)
			{
				%canInteract = %tObj.getCollisionActive();
				if (getWord(%canInteract, 1) == true)
				{
					echo("Picked object" SPC %tObj SPC %tObj.class);
					%word = %tObj;
					break;
				}
			}
		}
		
		if (isObject(%word))
		{
			%this.controlledWord = %word;
			%this.previousWordPosition = %word.getPosition();
			
			if (%word.class $= "SpecialObject")
			{
				%this.previousConstantForce = %word.getConstantForce();
				%word.setConstantForce("0 0");
			}
			else
			{
			}
		}
	}
}

function MouseInputCtrl::onMouseUp( %this, %modifier, %worldPosition, %clicks )
{
	if ( isObject(%this.controlledWord) )
	{
		%trigger = %this.controlledWord.insideTrigger;
		echo("Look at trigger:" SPC %trigger SPC "and word" SPC %this.controlledWord);
		if (%trigger)
		{
			%trigger.doAction();
		}
		else
		{
			if (%this.controlledWord.class $= "SpecialWord")
			{
				%this.controlledWord.setPosition(%this.previousWordPosition);
				%this.controlledWord.setCollisionActive( 0, 1 );
			}
			else if (%this.controlledWord.class $= "SpecialObject" && %this.controlledWord.getName() $= "MeltTorch")
			{
				%this.controlledWord.setPosition(%this.previousWordPosition);
				%this.controlledWord.setFrame(0);
			}
			else
				%this.controlledWord.setConstantForce(%this.previousConstantForce);
			
		}
		%this.controlledWord = "";
		%this.previousWordPosition = "";
	}
}

//-----------------------------------------------------------------------------

function MouseInputCtrl::onRightMouseDown( %this, %modifier, %worldPosition, %clicks )
{
    
}

function MouseInputCtrl::onRightMouseUp( %this, %modifier, %worldPosition, %clicks )
{
   
}