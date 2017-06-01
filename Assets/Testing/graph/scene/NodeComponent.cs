using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class NodeComponent : AbstractSceneComponent
	{
		private AbstractGraphNode graphNode;

		public NodeComponent (AbstractGraphNode graphNode, GameObject visualComponent) : base(visualComponent)
		{
			this.graphNode = graphNode;
			InitializeNodeComponent ();
		}

		private void InitializeNodeComponent()
		{
			SpriteRenderer sprite = GetVisualComponent ().GetComponent<SpriteRenderer> ();
			sprite.name = "Node_" + graphNode.GetId ();

			TextMesh text = GetVisualComponent ().GetComponentInChildren<TextMesh>();
			text.text = "" + graphNode.GetId ();
		}

		public AbstractGraphNode GetGraphNode()
		{
			return graphNode;
		}

	}
}

