using System;

namespace AssemblyCSharp
{
	public class SimpleGraphEdge : AbstractGraphEdge
	{

		private long id;
		private AbstractGraphNode startNode;
		private AbstractGraphNode endNode;

		public SimpleGraphEdge (long id, AbstractGraphNode startNode, AbstractGraphNode endNode)
		{
			this.id = id;
			this.startNode = startNode;
			this.endNode = endNode;
		}

		public override AbstractGraphNode GetStartGraphNode ()
		{
			return startNode;
		}

		public override AbstractGraphNode GetEndGraphNode ()
		{
			return endNode;
		}

		public override long GetId ()
		{
			return id;
		}
	}
}

