using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehavourTree
{

	public class Selector : Node
	{
		public Selector() : base() { }

		public Selector(List<Node> children) : base(children) { }

		public override NodeState Evaluate()
		{
			foreach (Node node in children)
			{
				switch (node.Evaluate())
				{
					case NodeState.FAIL:
						continue;
					case NodeState.SUCCESS:
						state = NodeState.SUCCESS;
						return state;
					case NodeState.RUNNING:
						state = NodeState.RUNNING;
						return state;
					default:
						continue;
				}
			}

			state = NodeState.FAIL;
			return state;
		}
	}

}
