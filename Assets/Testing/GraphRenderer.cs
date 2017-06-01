using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class GraphRenderer : MonoBehaviour {

	public GraphScenePrefabs graphScenePrefabs;
	public Camera cam;

	private Graph graph;
	private GraphScene graphScene;

	void Start()
	{
		graph = new Graph (InitializeGraphBackend ());
		graphScene = new GraphScene (graph, graphScenePrefabs);

		GenerateTestData ();

		graphScene.DrawGraph ();
	}

	private AbstractGraphBackend InitializeGraphBackend()
	{
		return new SimpleGraphBackend ();
	}

	private void GenerateTestData()
	{
		new ExponentialGraphGenerator ().GenerateGraph (graph);
		//new MultiEdgesForSingleNodeGraphGenerator().GenerateGraph(graph);
	}

	void Update ()
	{
		graphScene.Update (1);
	}
}
