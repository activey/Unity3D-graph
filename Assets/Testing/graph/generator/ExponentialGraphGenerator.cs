using System;

namespace AssemblyCSharp
{
	public class ExponentialGraphGenerator : AbstractGraphGenerator
	{
		private const int MAX_LEVEL = 3;

		public override void GenerateGraph (Graph graph)
		{
			AbstractGraphNode root = graph.NewNode ();
			GenerateDescedants (1, graph, root);
		}

		void GenerateDescedants (int level, Graph graph, AbstractGraphNode startNode)
		{
			if (level > MAX_LEVEL) {
				return;
			}
			for (int index = 0; index < Math.Exp(level); index++) {
				AbstractGraphNode descedantNode = graph.NewNode ();
				graph.NewEdge (startNode, descedantNode);

				GenerateDescedants (level + 1, graph, descedantNode);
			}
		}
	}
}

