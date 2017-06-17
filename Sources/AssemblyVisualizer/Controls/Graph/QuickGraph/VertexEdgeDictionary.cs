﻿// Adopted, originally created as part of QuickGraph library
// This code is distributed under Microsoft Public License 
// (for details please see \docs\Ms-PL)

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AssemblyVisualizer.Controls.Graph.QuickGraph
{
#if !SILVERLIGHT
	[Serializable]
#endif
	public sealed class VertexEdgeDictionary<TVertex, TEdge>
		: Dictionary<TVertex, IEdgeList<TVertex, TEdge>>
			, IVertexEdgeDictionary<TVertex, TEdge>
#if !SILVERLIGHT
			, ICloneable
			, ISerializable
#endif
		where TEdge : IEdge<TVertex>
	{
		public VertexEdgeDictionary()
		{
		}

		public VertexEdgeDictionary(int capacity)
			: base(capacity)
		{
		}

#if !SILVERLIGHT
		public VertexEdgeDictionary(
			SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
#endif

		IVertexEdgeDictionary<TVertex, TEdge> IVertexEdgeDictionary<TVertex, TEdge>.Clone()
		{
			return Clone();
		}

#if !SILVERLIGHT
		object ICloneable.Clone()
		{
			return Clone();
		}
#endif

		public VertexEdgeDictionary<TVertex, TEdge> Clone()
		{
			var clone = new VertexEdgeDictionary<TVertex, TEdge>(Count);
			foreach (var kv in this)
				clone.Add(kv.Key, kv.Value.Clone());
			return clone;
		}
	}
}