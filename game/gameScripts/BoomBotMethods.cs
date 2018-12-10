//-----------------------------------------------------------------------------
// Platformer Starter Kit
// Copyright (C) - Phillip O'Shea
//-----------------------------------------------------------------------------

function BoomBotClass::onAddToScene( %this, %scenegraph )
{
    // Parent Call.
    PlayerClass::onAddToScene( %this, %scenegraph );

    // Enable Update Callback.
    %this.enableUpdateCallback();

    // Ensure the Crosshair is Visible.
    CrossHairGui.setVisible( true );
}

function BoomBotClass::onRemoveFromScene( %this )
{
    // Ensure the Crosshair is Hidden.
    CrossHairGui.setVisible( false );
}

function BoomBotClass::onUpdate( %this )
{
    // Update the Crosshair Position.
    updateCrosshairPosition();
}

function BoomBotClass::initInventory( %this )
{
    // Parent Call.
    PlayerClass::initInventory( %this );
    
    // Shoot Rockets.
    %this.setActiveWeapon( RocketLauncherWeapon );
    
    // Set Ammo Limits.
    %this.RocketLauncherAmmo = 10;
    %this.MachineGunAmmo     = "50 50";
    
    // Update GUI.
    %this.updateWeaponGui();
}