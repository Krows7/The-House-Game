using System.Collections;
using System.Collections.Generic;
using Units.Settings;
using BehavourTree;

public class BT_Group : Tree
{
	public int maxGroupSize = 3;

	protected override Node SetupTree()
	{
		Unit _unit = transform.GetComponent<Unit>();

		// group branch
		Node isGroupFinal = new IsGroupFinal(_unit, maxGroupSize);
		Node defineNextMember = new DefineNextMemberInGroup(_unit);
		Node moveToNextMember = new MoveToNextMemberInGroup(_unit);
		Node groupSequence = new Sequence(new List<Node> { isGroupFinal, defineNextMember, moveToNextMember});

		//BT_Simple branch:
		Node findFlagInCurrentRoom = new FindFlagInCurrentRoom(_unit);
		Node moveToFlag = new MoveToFlag(_unit);
		Node flagSequence = new Sequence(new List<Node> { findFlagInCurrentRoom, moveToFlag });

		Node moveToRandomRoom = new MoveToRandomRoom((_unit));

		Node btSimpleSelector = new Selector(new List<Node> { flagSequence, moveToRandomRoom });

		//final tree
		Node rootSelector = new Selector(new List<Node> { groupSequence, btSimpleSelector });
		Node root = rootSelector;
		return root;
	}
}
