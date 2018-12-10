//-----------------------------------------------------------------------------
// Platformer Starter Kit
// Copyright (C) - Phillip O'Shea
//  
// Player Methods - A player is an Actor with a Player controller. The methods
//                  in this file make the game know what to do with a player.
//                  Adding specific methods to object classes gives more
//                  variety and control to the game.
//-----------------------------------------------------------------------------

/// Mount the camera and update the gui
function PlayerClass::onAddToScene( %this, %scenegraph )
{
    Parent::onAddToScene( %this, %scenegraph );
    
    %this.mountCamera();
    
    %this.updateExtraLivesGui();
    %this.updateHealthGui();
    
    // Initialise Inventory.
    %this.schedule( 0, "initInventory" );
}

function PlayerClass::initInventory( %this )
{
    // Void.
}

function PlayerClass::onReload( %this, %trigger )
{
    // Parent Call.
    Parent::onReload( %this, %trigger );
    
    // Update GUI.
    %this.updateWeaponGui();
}

/// Mount the camera if required
function PlayerClass::onLevelLoaded( %this )
{
    // If you add the player straight into the scene and not through a spawner,
    // then you need to wait until the level has been loaded to mount the
    // camera.
    %this.mountCamera();
}

/// Called when the player lands on a platform
function PlayerClass::onLanded( %this, %platformObject )
{
    if ( isObject( DragonLandSound ) )
    {
        %this.playSound( DragonLandSound );
    }
}

/// Called when the player is healed
function PlayerClass::onHeal( %this, %hAmount )
{
    %this.updateHealthGui();
}

/// Called when the player is damaged
function PlayerClass::onDamage( %this, %dAmount )
{
    %this.updateHealthGui();
}

/// Called when a player dies
function PlayerClass::onDeath( %this, %dAmount, %srcObject )
{
    // Dismount the camera
    dismountCamera();

    // Bounce the player
    %this.bounce( %this.JumpForce, 180 );

    // Update the gui
    %this.updateExtraLivesGui();
    %this.updateHealthGui();
}

/// Called when a player respawns
function PlayerClass::onRespawn( %this )
{
    // Load the checkpoint data
    loadCheckPoint();

    // Remount the camera
    %this.mountCamera();

    // Update the gui
    %this.updateHealthGui();
}

function PlayerClass::onGameOver( %this )
{
    // No more lives left!
    gameOver();
}

/// Called when a player collides with an enemy.
function PlayerClass::resolveEnemyCollision( %ourObject, %theirObject, %normal )
{
    // Resolve the collisions differently for different ai types
    switch$ ( %theirObject.ActorType )
    {
        case "Drill" : %ourObject.resolveDrillCollision( %theirObject, %normal );
    }
}

/// Called upon collision with a drill type enemy
function PlayerClass::resolveDrillCollision( %ourObject, %theirObject, %normal )
{
    // Get the contact angle
    %angle = mRadToDeg( mAtan( -%normal.X, %normal.Y ) );
    %angle = mAbs( %angle ) % 360;

    // Check if we've hit the drill properly
    if ( (%angle <= 40 || %angle >= 360 - 40 )
        && %ourObject.LinearVelocity.Y > %theirObject.LinearVelocity.Y
        && %ourObject.Position.Y < %theirObject.Position.Y )
    {
        // Do damage to the drill and bounce the player
        %theirObject.takeDamage( %theirObject.Health, %ourObject, true, true );

        // Stop them from moving
        %theirObject.Direction = "0.0 0.0";
        %theirObject.Gravity   = "0.0 0.0";

        %ourObject.bounce( %ourObject.JumpForce / 2, 180 );
    }
    else
    {
        // Do damage to the player
        %ourObject.takeDamage( 10, %theirObject, true, false );
    }
}

function PlayerClass::onAreaDamage( %this, %theirObject, %normal )
{
    // Get the contact angle
    %angle = mRadToDeg( mAtan( -%normal.Y, %normal.X ) );
    %angle = mAbs( %angle ) % 360;

    %this.bounce( %this.JumpForce, %angle - 90 );
}

/// Update the health interface
function PlayerClass::updateHealthGui( %this )
{
    // Update Peppers.
    %pepperCount = mFloor( %this.Health / 10 );
    for ( %i = 0; %i < 10; %i++ )
    {
        %ghostVisible = !( %i < %pepperCount );

        if ( isObject( "ghostPepper" @ 9 - %i ) )
        {
            eval ( "ghostPepper" @ 9 - %i @ ".Visible = %ghostVisible;" );
        }

        if ( isObject( "pepper" @ 9 - %i ) )
        {
            eval ( "pepper" @ 9 - %i @ ".Visible = !%ghostVisible;" );
        }
    }
    
    // Update Extra Lives.
    %this.updateExtraLivesGui();
}

/// Update the extra life interface
function PlayerClass::updateExtraLivesGui( %this )
{
    // Update Extra Lives.
    if ( isObject( ExtraLivesCounter ) )
    {
        ExtraLivesCounter.Text = %this.Lives;

        if ( %this.Lives < 10 )
        {
            ExtraLivesCounter.Text = 0 @ %this.Lives;
        }
    }
}

/// Update the weapon interface
function PlayerClass::updateWeaponGui( %this )
{
    %guiControl[0] = PrimaryAmmoGui;
    %guiControl[1] = SecondaryAmmoGui;
    
    %weapon      = %this.ActiveWeapon;
    %actionCount = %weapon.ActionCount;
    for ( %i = 0; %i < %actionCount; %i++ )
    {
        %ammo = %weapon.Action[%i].Ammunition;
        if ( isObject( %ammo ) && isObject( %guiControl[%i] ) )
        {
            %guiControl[%i].setText( strreplace( trim( %this.getFieldValue( %ammo.getName() ) ), " ", " | " ) );
        }
    }
}

//-----------------------------------------------------------------------------
// Dragon Player Methods
//-----------------------------------------------------------------------------

function DragonPlayer::initInventory( %this )
{
    // Parent Call.
    PlayerClass::initInventory( %this );
    
    // Shoot Fireballs.
    %this.setActiveWeapon( FireballLauncherWeapon );
}

function SadPlayer::initInventory( %this )
{
    // Parent Call.
    PlayerClass::initInventory( %this );
    
    // Shoot Fireballs.
    //%this.setActiveWeapon( FireballLauncherWeapon );
}

//-----------------------------------------------------------------------------
// Caveman Player Methods
//-----------------------------------------------------------------------------

function CavemanPlayer::initInventory( %this )
{
    // Parent Call.
    PlayerClass::initInventory( %this );
    
    // Shoot Bones.
    %this.setActiveWeapon( BoneLauncherWeapon );
}