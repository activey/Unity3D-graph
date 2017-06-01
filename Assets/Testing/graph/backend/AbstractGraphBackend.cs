using System;

namespace AssemblyCSharp
{
	public abstract class AbstractGraphBackend
	{
		private GraphBackendListeners listeners = new GraphBackendListeners();
	
		public abstract AbstractGraphNode NewNode();

		public abstract AbstractGraphEdge NewEdge(AbstractGraphNode from, AbstractGraphNode to);

		public abstract AbstractGraphNode GetNodeById(long nodeId);

		public abstract long CountAllEdges();

		public abstract long CountAllNodes();

		public void AddListener(GraphBackendListener listener)
		{
			listeners.AddGraphBackendListener (listener);
		}

		public void NotifyBackendNodeCreated(AbstractGraphNode graphNode)
		{
			listeners.NotifyGraphBackendCreated (graphNode);
		}

		public void NotifyBackendEdgeCreated(AbstractGraphEdge graphEdge)
		{
			listeners.NotifyGraphBackendCreated (graphEdge);
		}
	}
}

