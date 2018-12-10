function BoatPuzzleTrigger::onEnter(%this, %obj)
{
	if (%obj == $Game::Player)
	{
		LifeBoat.setConstantForce("40 0");
		$Game::Player.mount(LifeBoat, "0 0.6");
		quote.visible = true;
	}
}

function BoatStopTrigger::onEnter(%this, %obj)
{
	if (%obj == $Game::Player)
	{
		LifeBoat.setLinearVelocity("0 0");
		LifeBoat.setConstantForce("0 0");
		
		$Game::Player.dismount();
	}
}

function BoatTeleportTrigger::onEnter(%this, %obj)
{
	echo("bt trig" SPC %obj.getName());
	if (%obj.getName() $= "LifeBoat")
	{
		%retPos = BoatReturn.getPosition();
		%retSize = BoatReturn.getSize();
		LifeBoat.setPosition(getWord(%retPos, 0) + getWord(%retSize, 0) + 10, getWord(%retPos, 1));
	}
}

function BoatReturnTrigger::onEnter(%this, %obj)
{
	echo("br trig" SPC %obj.getName());
	if (%obj.getName() $= "LifeBoat")
	{
		%retPos = BoatTeleport.getPosition();
		%retSize = BoatTeleport.getSize();
	
		LifeBoat.setPosition(getWord(%retPos, 0) - getWord(%retSize, 0) - 10, getWord(%retPos, 1));
	
		LifeBoat.setPosition(BoatTeleport.getPosition());
	}
}