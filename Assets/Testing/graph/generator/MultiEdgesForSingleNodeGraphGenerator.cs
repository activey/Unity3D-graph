using System;

namespace AssemblyCSharp
{
	public class MultiEdgesForSingleNodeGraphGenerator : AbstractGraphGenerator
	{

		public override void GenerateGraph (Graph graph)
		{
			AbstractGraphNode firstNode =  graph.NewNode();
			AbstractGraphNode secondNode = graph.NewNode ();
			AbstractGraphNode thirdNode = graph.NewNode ();

			graph.NewEdge (firstNode, secondNode);
			graph.NewEdge (secondNode, firstNode);
			graph.NewEdge (firstNode, secondNode);
			graph.NewEdge (firstNode, secondNode);
			graph.NewEdge (firstNode, secondNode);
			graph.NewEdge (firstNode, secondNode);
			graph.NewEdge (firstNode, secondNode);

			graph.NewEdge (secondNode, thirdNode);
		}
	}
}

