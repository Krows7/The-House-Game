using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using BehavourTree;
using Units.Settings;

public class BT_Simple : Tree
{
	protected override Node SetupTree()
	{

		Unit _unit = transform.GetComponent<Unit>();


		Node findFlagInCurrentRoom = new FindFlagInCurrentRoom(_unit);
		Node moveToFlag = new MoveToFlag(_unit);
		Node flagSequnce = new Sequence(new List<Node> { findFlagInCurrentRoom, moveToFlag });

		Node moveToRandomRoom = new MoveToRandomRoom((_unit));

		Node selector = new Selector(new List<Node> { flagSequnce, moveToRandomRoom });


		Node root = selector;
		return root;
	}

}
