using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class EdgeComponent : AbstractSceneComponent
	{
		public bool MultiEdge = false;
		private AbstractGraphEdge graphEdge;

		public EdgeComponent (AbstractGraphEdge graphEdge, GameObject visualComponent) : base(visualComponent)
		{
			this.graphEdge = graphEdge;
			InitializeEdgeComponent ();
		}

		private void InitializeEdgeComponent()
		{
			LineRenderer line = GetVisualComponent ().GetComponent<LineRenderer> ();
			line.name = "Edge_" + graphEdge.GetId ();

			float angle = UnityEngine.Random.Range (0, 360);
		
			float xRotation = Mathf.Cos (Mathf.Deg2Rad * angle) * 100;
			float yRotation = Mathf.Sin (Mathf.Deg2Rad * angle) * 100;
			float zRotation = Mathf.Cos (Mathf.Deg2Rad * angle) * 100;
			GetVisualComponent().transform.Rotate(new Vector3 (xRotation, yRotation, zRotation));
		}

		public AbstractGraphEdge GetGraphEdge()
		{
			return graphEdge;
		}

		public void UpdateGeometry(Vector3 start, Vector3 end)
		{
			LineRenderer line = GetVisualComponent ().GetComponent<LineRenderer> ();
			line.SetPosition (0, start);
			line.SetPosition (1, end);
		}

	}
}

