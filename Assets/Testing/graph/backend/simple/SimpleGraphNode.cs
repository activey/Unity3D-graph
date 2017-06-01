using System;

namespace AssemblyCSharp
{
	public class SimpleGraphNode : AbstractGraphNode
	{

		private SimpleGraphBackend graphBackend;
		private long id;

		public SimpleGraphNode (SimpleGraphBackend simpleGraphBackend, long id)
		{
			this.graphBackend = simpleGraphBackend;
			this.id = id;
		}

		public override void Accept (GraphEdgeVisitor graphEdgeVisitor)
		{
			graphBackend.FindEdges (id).ForEach (edge => {
				graphEdgeVisitor(edge);
			});
		}

		public override long GetDegree ()
		{
			return graphBackend.FindEdges (id).Count;
		}

		public override bool IsAdjacent (AbstractGraphNode graphNode)
		{
			return graphBackend.FindEdges (id, graphNode.GetId ()).Count > 0;
		}

		public override long GetId ()
		{
			return id;
		}
	}
}

