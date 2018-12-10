$DeathTreePuzzle = false;
$ColdFirePuzzle = false;
$WarmHandsPuzzle = false;
$LifeHerePuzzle = false;

function TreeDeathTrigger::onEnter(%this, %obj)
{
	echo("word entered trigger" SPC %obj SPC %obj.class SPC %obj.text);
	if (%obj.class $= "SpecialWord" && %obj.text $= "death")
	{
		echo("found death word" SPC %this);
		%obj.insideTrigger = %this;
		%this.triggerObject = %obj;
	}
}

function TreeDeathTrigger::onLeave(%this, %obj)
{
	echo("word left trigger");
	if (%obj.class $= "SpecialWord" && %obj.text $= "death")
	{
		%obj.insideTrigger = "";
		%this.triggerObject = "";
	}
}

function TreeDeathTrigger::doAction(%this)
{
	LifeTree.setFrame(1);
	%this.triggerObject.SafeDelete();
	
	LifeWord.visible = true;
	LifeWord.setCollisionActive( 0, 1 );
	%obj.insideTrigger = "";
	%this.triggerObject = "";
	
	$DeathTreePuzzle = true;
	checkPuzzleOneFinished();
	
}

function DeathWordPlaceTrigger::onEnter (%this, %obj)
{
	if (%obj.class $= "SpecialWord" && %obj.text $= "life")
	{
		%obj.insideTrigger = %this;
		%this.triggerObject = %obj;
	}
}

function DeathWordPlaceTrigger::onLeave(%this, %obj)
{
	echo("word left death trigger");
	if (%obj.class $= "SpecialWord" && %obj.text $= "life")
	{
		%obj.insideTrigger = "";
		%this.triggerObject = "";
	}
}

function DeathWordPlaceTrigger::doAction(%this)
{
	%this.triggerObject.setPosition(%this.getPosition());
	%this.triggerObject.puzzleComplete = true;
	
	$LifeHerePuzzle = true;
	checkPuzzleOneFinished();
}

function FireColdTrigger::onEnter(%this, %obj)
{
	if (%obj.class $= "SpecialWord" && %obj.text $= "cold")
	{
		%obj.insideTrigger = %this;
		%this.triggerObject = %obj;
	}
}

function FireColdTrigger::onLeave(%this, %obj)
{
	if (%obj.class $= "SpecialWord" && %obj.text $= "cold")
	{
		%obj.insideTrigger = "";
		%this.triggerObject = "";
	}
}

function FireColdTrigger::doAction(%this)
{
	WarmFire.visible = false;
	FireGlow.visible = false;
	%this.triggerObject.SafeDelete();
	
	WarmWord.visible = true;
	WarmWord.setCollisionActive( 0, 1 );
	
	%obj.insideTrigger = "";
	%this.triggerObject = "";
	
	$ColdFirePuzzle = true;
	checkPuzzleOneFinished();

}

function ColdWordPlaceTrigger::onEnter (%this, %obj)
{
	if (%obj.class $= "SpecialWord" && %obj.text $= "warm")
	{
		%obj.insideTrigger = %this;
		%this.triggerObject = %obj;
	}
}

function ColdWordPlaceTrigger::onLeave(%this, %obj)
{
	if (%obj.class $= "SpecialWord" && %obj.text $= "warm")
	{
		%obj.insideTrigger = "";
		%this.triggerObject = "";
	}
}

function ColdWordPlaceTrigger::doAction(%this)
{
	%this.triggerObject.setPosition(%this.getPosition());
	%this.triggerObject.puzzleComplete = true;
	
	$WarmHandsPuzzle = true;
	checkPuzzleOneFinished();

}

function checkPuzzleOneFinished()
{
	if (!$DeathTreePuzzle) return;
	if (!$ColdFirePuzzle) return;
	if (!$WarmHandsPuzzle) return;
	if (!$LifeHerePuzzle) return;
	
	for (%i = 0; %i < FirstPuzzleBridge.getCount(); %i++)
	{
		%bridgeSection = FirstPuzzleBridge.getObject(%i);
		echo ("Bridge contents" SPC %bridgeSection);
		%bridgeSection.visible = true;
		%bridgeSection.setCollisionActive( 0, 1 );
	}
}

