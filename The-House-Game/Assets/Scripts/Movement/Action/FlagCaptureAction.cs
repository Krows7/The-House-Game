using System.Collections;
using System.Collections.Generic;
using Units.Settings;
using UnityEngine;

public class FlagCaptureAction : BaseMoveAction
{



	public FlagCaptureAction(Cell from, Cell to, Unit unit) : base(from, to, unit)
	{
	}

	public override void Execute()
	{
		to.currentFlag.GetComponent<Flag>().StartCapture();
		base.Execute();
	}
}
