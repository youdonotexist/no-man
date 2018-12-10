$BeatPuzzle = false;
$WaitPuzzle = false;
$BreakPuzzle = false;
$FlowPuzzle = false;

function RockBreakTrigger::onEnter(%this, %obj)
{
	echo("rock had something enter" SPC %obj.class);
	if (%obj.class $= "SpecialObject")
	{
		%downwardVelocity = %obj.getLinearVelocity();
		%down = getWord(%downwardVelocity, 0);
		echo ("speed of" SPC %down SPC %obj.getName());
		
		%down = mAbs(%down);
		
		if (%down > 3)
		{
			//if it's a rock, break
			if (strstr(%obj.getName(), "Rock") >= 0)
			{
				CrackedRock.SafeDelete();
				BreakWord.visible = true;
				BreakWord.setCollisionActive( 0, 1 );
				
				%this.setEnterCallback( false );
			}
			else if (%obj.getName() $= "FlowWater")
			{
				CrackedRock.SafeDelete();
				FlowWord.visible = true;
				FlowWord.setCollisionActive( 0, 1 );
				
				%this.setEnterCallback( false );
				
			}
			//if it's water, do something else
		}
	}

}

function BeatWordPlaceTrigger::onEnter (%this, %obj)
{
	if (%obj.class $= "SpecialWord" && %obj.text $= "break")
	{
		%obj.insideTrigger = %this;
		%this.triggerObject = %obj;
	}
}

function BeatWordPlaceTrigger::onLeave(%this, %obj)
{
	if (%obj.class $= "SpecialWord" && %obj.text $= "break")
	{
		%obj.insideTrigger = "";
		%this.triggerObject = "";
	}
}

function BeatWordPlaceTrigger::doAction(%this)
{
	%this.triggerObject.setPosition(%this.getPosition());
	%this.triggerObject.puzzleComplete = true;
	BeatWord.SafeDelete();
	
	$BreakPuzzle = true;
	checkPuzzleTwoFinished();
}

function WaitWordPlaceTrigger::onEnter (%this, %obj)
{
	if (%obj.class $= "SpecialWord" && %obj.text $= "flow")
	{
		%obj.insideTrigger = %this;
		%this.triggerObject = %obj;
	}
}

function WaitWordPlaceTrigger::onLeave(%this, %obj)
{
	if (%obj.class $= "SpecialWord" && %obj.text $= "flow")
	{
		%obj.insideTrigger = "";
		%this.triggerObject = "";
	}
}

function WaitWordPlaceTrigger::doAction(%this)
{
	%this.triggerObject.setPosition(%this.getPosition());
	%this.triggerObject.puzzleComplete = true;
	WaitWord.SafeDelete();
	
	$FlowPuzzle = true;
	checkPuzzleTwoFinished();
}

function LightTorchTrigger::onEnter(%this, %obj)
{
	echo("something inside");
	if (%obj.class $= "SpecialObject" && %obj.getName() $= "MeltTorch")
	{
		%obj.setFrame(1);
	}
}

function MeltIceTrigger::onEnter(%this, %obj)
{
	if (%obj.class $= "SpecialObject" && %obj.getName() $= "MeltTorch")
	{
		if (%obj.getFrame() == 1)
		{
			CrackedRock.SafeDelete();
			IceWater.SafeDelete();
			FlowWord.visible = true;
			FlowWord.setCollisionActive( 0, 1 );
		}
	}
}



function checkPuzzleTwoFinished()
{
	//if the non-unique not solved, return
	
	if ($BreakPuzzle || $FlowPuzzle)
	{
		if($BreakPuzzle)
		{
			for (%i = 0; %i < BreakBridge.getCount(); %i++)
			{
				%bridgeSection = BreakBridge.getObject(%i);
				echo ("Bridge contents" SPC %bridgeSection);
				%bridgeSection.visible = true;
				%bridgeSection.setCollisionActive( 0, 1 );
			}
		}
		else
		{
			for (%i = 0; %i < FlowBridge.getCount(); %i++)
			{
				%bridgeSection = FlowBridge.getObject(%i);
				echo ("Bridge contents" SPC %bridgeSection);
				%bridgeSection.visible = true;
				%bridgeSection.setCollisionActive( 0, 1 );
			}

		} 
	}
	else
	{
		return;
	}
}

