using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehavourTree
{
	public class Sequense : Node
	{
		public Sequense() :base() {} 

		public Sequense(List<Node> children) : base(children){}

		public override NodeState Evaluate()
		{
			bool anyChildIsRunning = false;
			foreach (Node node in children)
			{
				switch (node.Evaluate())
				{
					case NodeState.FAIL:
						state = NodeState.FAIL;
						return state;
					case NodeState.SUCCESS:
						continue;
					case NodeState.RUNNING:
						anyChildIsRunning = true;
						continue;
					default:
						state = NodeState.SUCCESS;
						return state;
				}
			}

			state = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
			return state;
		}
	}
}
