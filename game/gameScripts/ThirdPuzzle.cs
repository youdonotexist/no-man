$LearnPuzzle = false;
$GiantPuzzle = false;
$SuccessPuzzle = false;

$FirstUnblock = false;
$SecondUnblock = false;
$ThirdUnblock = false;

function DieWordPlaceTrigger::onEnter (%this, %obj)
{
	if (%obj.class $= "SpecialWord" && %obj.text $= "learn")
	{
		%obj.insideTrigger = %this;
		%this.triggerObject = %obj;
	}
}

function DieWordPlaceTrigger::onLeave(%this, %obj)
{
	if (%obj.class $= "SpecialWord" && %obj.text $= "learn")
	{
		%obj.insideTrigger = "";
		%this.triggerObject = "";
	}
}

function DieWordPlaceTrigger::doAction(%this)
{
	%this.triggerObject.setPosition(%this.getPosition());
	%this.triggerObject.puzzleComplete = true;
	DieWord.SafeDelete();
	
	$LearnPuzzle = true;
	checkPuzzleThreeFinished();
}

function IceWordPlaceTrigger::onEnter (%this, %obj)
{
	if (%obj.class $= "SpecialWord" && %obj.text $= "giants")
	{
		%obj.insideTrigger = %this;
		%this.triggerObject = %obj;
	}
}

function IceWordPlaceTrigger::onLeave(%this, %obj)
{
	if (%obj.class $= "SpecialWord" && %obj.text $= "giants")
	{
		%obj.insideTrigger = "";
		%this.triggerObject = "";
	}
}

function IceWordPlaceTrigger::doAction(%this)
{
	%newSize = "20 13";
	%this.triggerObject.setSize(%newSize);
	%newX = getWord(%this.getPosition(), 0) + (getWord(%this.triggerObject.getSize(), 0) * 0.5);
	%newY = getWord(%this.getPosition(), 1) - (getWord(%this.triggerObject.getSize(), 1) * 0.5);
	%this.triggerObject.setPosition(%newX SPC %newY);
	%this.triggerObject.puzzleComplete = true;
	IceWord.SafeDelete();
	GiantWord.setCollisionActive( 0, 1 );
	
	$GiantPuzzle = true;
	checkPuzzleThreeFinished();
}

function FailureWordPlaceTrigger::onEnter (%this, %obj)
{
	if (%obj.class $= "SpecialWord" && %obj.text $= "success")
	{
		%obj.insideTrigger = %this;
		%this.triggerObject = %obj;
	}
}

function FailureWordPlaceTrigger::onLeave(%this, %obj)
{
	if (%obj.class $= "SpecialWord" && %obj.text $= "success")
	{
		%obj.insideTrigger = "";
		%this.triggerObject = "";
	}
}

function FailureWordPlaceTrigger::doAction(%this)
{
	%this.triggerObject.setPosition(%this.getPosition());
	%this.triggerObject.puzzleComplete = true;
	FailureWord.SafeDelete();
	
	$SuccessPuzzle = true;
	checkPuzzleThreeFinished();
}

function DropRock::onAddToScene(%this)
{
	%this.schedule(1000, "startDrop");
}

function DropRock::startDrop(%this)
{
	%this.setConstantForce("0 50");
}

function DropRock::raiseRock(%this)
{
	%this.setConstantForce("0 -50");
}

function DropRockTrigger::onEnter(%this, %obj)
{
	if (%obj.class $= "DropRock")
	{
		%obj.setConstantForce("0 0");
		%obj.setLinearVelocity("0 0");
		%obj.schedule(1000, "startDrop");
	}
}

function RaiseRockTrigger::onEnter(%this, %obj)
{
	echo (%obj.class SPC "enetered");
	if (%obj.class $= "DropRock")
	{
		%obj.setConstantForce("0 0");
		%obj.setLinearVelocity("0 0");
		%obj.schedule(1000, "raiseRock");
	}
}

function LearnReleaseTrigger::onEnter(%this, %obj)
{
		
}

function ThirdPuzzleTrigger::onEnter(%this, %obj)
{
	echo ("turning off learnword collisions");
	if (%obj == $Game::Player)
	{
		LearnWord.setCollisionActive(0, 0);
	}
}

function LearnUnblockTrigger::onEnter(%this, %obj)
{
	if (%obj == $Game::Player)
	{
		if (%this.getName() $= "FirstUnblock")
		{
			$FirstUnblock = true;
		}
		
		if (%this.getName() $= "SecondUnblock")
		{
			$SecondUnblock = true;
		}
		
		if (%this.getName() $= "ThirdUnblock")
		{
			$ThirdUnblock = true;
		}
		
		if  ($FirstUnblock == true && $SecondUnblock == true && $ThirdUnblock == true)
		{
			LearnBlock.visible = false;
			LearnBlock.setCollisionActive(0, 0);
			
			Globe1.SafeDelete();
			Globe2.SafeDelete();
			
			LearnWord.setCollisionActive(0, 1);
		}
	}
}

function LearnBlockTrigger::onEnter(%this, %obj)
{
	if (%obj == $Game::Player)
	{
		if  (!($FirstUnblock == true && $SecondUnblock == true && $ThirdUnblock == true))
		{
			$FirstUnblock = false;
			$SecondUnblock = false;
			$ThirdUnblock = false;
		
			LearnWord.setCollisionActive(0, 0);
		}
	}
}

$LearnPuzzle = false;
$GiantPuzzle = false;
$SuccessPuzzle = false;

function checkPuzzleThreeFinished()
{
	if (!$LearnPuzzle) return;
	if (!$GiantPuzzle) return;
	
	SuccessWord.visible = true;
	SuccessWord.setCollisionActive(0, 1);
	
	if (!$SuccessPuzzle) return;
	
	for (%i = 0; %i < ThirdPuzzleBridge.getCount(); %i++)
	{
		%bridgeSection = ThirdPuzzleBridge.getObject(%i);
		echo ("Bridge contents" SPC %bridgeSection);
		%bridgeSection.visible = false;
		%bridgeSection.setCollisionActive( 0, 0 );
	}
	
}

