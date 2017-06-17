﻿// Adopted, originally created as part of GraphSharp library
// This code is distributed under Microsoft Public License 
// (for details please see \docs\Ms-PL)

using System.ComponentModel;
using AssemblyVisualizer.Controls.Graph.QuickGraph;

namespace AssemblyVisualizer.Controls.Graph.GraphSharp.Highlight
{
	public abstract class HighlightAlgorithmBase<TVertex, TEdge, TGraph, TParameters> :
		IHighlightAlgorithm<TVertex, TEdge, TGraph>
		where TVertex : class
		where TEdge : IEdge<TVertex>
		where TGraph : class, IBidirectionalGraph<TVertex, TEdge>
		where TParameters : class, IHighlightParameters
	{
		protected HighlightAlgorithmBase(
			IHighlightController<TVertex, TEdge, TGraph> controller,
			IHighlightParameters parameters)
		{
			Controller = controller;
			TrySetParameters(parameters);
		}

		public IHighlightController<TVertex, TEdge, TGraph> Controller { get; private set; }

		public TParameters Parameters { get; private set; }

		IHighlightParameters IHighlightAlgorithm<TVertex, TEdge, TGraph>.Parameters
		{
			get { return Parameters; }
		}

		public abstract void ResetHighlight();
		public abstract bool OnVertexHighlighting(TVertex vertex);
		public abstract bool OnVertexHighlightRemoving(TVertex vertex);
		public abstract bool OnEdgeHighlighting(TEdge edge);
		public abstract bool OnEdgeHighlightRemoving(TEdge edge);

		public bool IsParametersSettable(IHighlightParameters parameters)
		{
			return parameters != null && parameters is TParameters;
		}

		public bool TrySetParameters(IHighlightParameters parameters)
		{
			if (IsParametersSettable(parameters))
			{
				if (Parameters != null)
					Parameters.PropertyChanged -= OnParameterPropertyChanged;
				Parameters = (TParameters) parameters;
				if (Parameters != null)
					Parameters.PropertyChanged += OnParameterPropertyChanged;
				return true;
			}
			return false;
		}

		protected virtual void OnParametersChanged()
		{
			ResetHighlight();
		}

		private void OnParameterPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			OnParametersChanged();
		}
	}
}