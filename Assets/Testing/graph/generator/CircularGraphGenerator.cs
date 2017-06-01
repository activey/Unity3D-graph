using System;

namespace AssemblyCSharp
{
	public class CircularGraphGenerator : AbstractGraphGenerator
	{
		private const int NODES = 100;

		public override void GenerateGraph (Graph graph)
		{
			AbstractGraphNode root = graph.NewNode ();
			AbstractGraphNode lastNode = GenerateCircleSegment (graph, root, 0);

			graph.NewEdge (lastNode, root);
		}

		private AbstractGraphNode GenerateCircleSegment(Graph graph, AbstractGraphNode startNode, int segmentIndex)
		{
			if (segmentIndex > NODES) {
				return startNode;
			}
			AbstractGraphNode neighborNode = graph.NewNode ();
			graph.NewEdge (startNode, neighborNode);
			return GenerateCircleSegment (graph, neighborNode, segmentIndex + 1);
		}
	}
}

