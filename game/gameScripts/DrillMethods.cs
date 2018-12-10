//-----------------------------------------------------------------------------
// Platformer Starter Kit
// Copyright (C) Phillip O'Shea
//  
// Drill Methods - All of the methods that makes a Drill a Drill.
//-----------------------------------------------------------------------------

function DrillClass::onAddToScene( %this )
{
    Parent::onAddToScene( %this );

    if ( isObject( DrillHeadTemplate ) )
    {
        %this.DrillHead = new t2dSceneObject()
        {
            Config     = DrillHeadTemplate;
            Scenegraph = %this.Scenegraph;

            FlipX = %this.FlipX;
        };

        %this.DrillHead.mount( %this );
    }
}

function DrillClass::onRemove( %this )
{
    if ( isObject( %this.DrillHead ) )
    {
        %this.DrillHead.safeDelete();
    }
}

function DrillClass::onRespawn( %this, %dAmount, %srcObject )
{
    %this.DisableGravity = false;
}

function DrillClass::onDeath( %this, %dAmount, %srcObject )
{
    %this.DisableGravity = true;

    if ( isObject( %this.DrillHead ) )
    {
        %this.DrillHead.safeDelete();
    }
}

function DrillClass::onWallCollision( %this, %wall, %normal )
{
    %this.Direction.X = %normal.X;
}