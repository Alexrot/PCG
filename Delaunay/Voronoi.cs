﻿using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace csDelaunay {

	public class Voronoi {

		private SiteList sites;
		private List<Triangle> triangles;

		private List<Edge> edges;
		public List<Edge> Edges {get{return edges;}}

		// TODO generalize this so it doesn't have to be a rectangle;
		// then we can make the fractal voronois-within-voronois
		private Rect plotBounds;
		public Rect PlotBounds {get{return plotBounds;}}
		
		private Dictionary<Vector2,Site> sitesIndexedByLocation;
		public Dictionary<Vector2,Site> SitesIndexedByLocation {get{return sitesIndexedByLocation;}}

		private System.Random weigthDistributor;

		public void Dispose() {
			sites.Dispose();
			sites = null;

			foreach (Triangle t in triangles) {
				t.Dispose();
			}
			triangles.Clear();

			foreach (Edge e in edges) {
				e.Dispose();
			}
			edges.Clear();

			plotBounds = Rect.zero;
			sitesIndexedByLocation.Clear();
			sitesIndexedByLocation = null;
		}

		public Voronoi(List<Vector2> points, Rect plotBounds) {
			weigthDistributor = new System.Random();
			Init(points,plotBounds);
		}

		public Voronoi(List<Vector2> points, Rect plotBounds, int lloydIterations) {
			weigthDistributor = new System.Random();
			Init(points,plotBounds);
			LloydRelaxation(lloydIterations);
		}

		private void Init(List<Vector2> points, Rect plotBounds) {
			sites = new SiteList();
			sitesIndexedByLocation = new Dictionary<Vector2, Site>();
			AddSites(points);
			this.plotBounds = plotBounds;
			triangles = new List<Triangle>();
			edges = new List<Edge>();
			
			FortunesAlgorithm();
		}

		private void AddSites(List<Vector2> points) {
			for (int i = 0; i < points.Count; i++) {
				AddSite(points[i], i);
			}
		}

		private void AddSite(Vector2 p, int index) {
			float weigth = (float)weigthDistributor.NextDouble() * 100;
			Site site = Site.Create(p, index, weigth);
			sites.Add(site);
			sitesIndexedByLocation[p] = site;
		}

		public List<Vector2> Region (Vector2 p) {
			Site site;
			if (sitesIndexedByLocation.TryGetValue(p, out site)) {
				return site.Region(plotBounds);
			} else {
				return new List<Vector2>();
			}
		}

		public List<Vector2> NeighborSitesForSite(Vector2 coord) {
			List<Vector2> points = new List<Vector2>();
			Site site;
			if (sitesIndexedByLocation.TryGetValue(coord, out site)) {
				List<Site> sites = site.NeighborSites();
				foreach (Site neighbor in sites) {
					points.Add(neighbor.Coord);
				}
			}

			return points;
		}

		public List<Circle> Circles() {
			return sites.Circles();
		}

		public List<LineSegment> VoronoiBoundarayForSite(Vector2 coord) {
			return LineSegment.VisibleLineSegments(Edge.SelectEdgesForSitePoint(coord, edges));
		}
		/*
		public List<LineSegment> DelaunayLinesForSite(Vector2 coord) {
			return DelaunayLinesForEdges(Edge.SelectEdgesForSitePoint(coord, edges));
		}*/

		public List<LineSegment> VoronoiDiagram() {
			return LineSegment.VisibleLineSegments(edges);
		}
		/*
		public List<LineSegment> Hull() {
			return DelaunayLinesForEdges(HullEdges());
		}*/

		public List<Edge> HullEdges() {
			return edges.FindAll(edge=>edge.IsPartOfConvexHull());
		}

		public List<Vector2> HullPointsInOrder() {
			List<Edge> hullEdges = HullEdges();

			List<Vector2> points = new List<Vector2>();
			if (hullEdges.Count == 0) {
				return points;
			}

			EdgeReorderer reorderer = new EdgeReorderer(hullEdges, typeof(Site));
			hullEdges = reorderer.Edges;
			List<LR> orientations = reorderer.EdgeOrientations;
			reorderer.Dispose();

			LR orientation;
			for (int i = 0; i < hullEdges.Count; i++) {
				Edge edge = hullEdges[i];
				orientation = orientations[i];
				points.Add(edge.Site(orientation).Coord);
			}
			return points;
		}

		public List<List<Vector2>> Regions() {
			return sites.Regions(plotBounds);
		}

		public List<Vector2> SiteCoords() {
			return sites.SiteCoords();
		}

		private void FortunesAlgorithm() {
			Site newSite, bottomSite, topSite, tempSite;
			Vertex v, vertex;
			Vector2 newIntStar = Vector2.zero;
			LR leftRight;
			Halfedge lbnd, rbnd, llbnd, rrbnd, bisector;
			Edge edge;

			Rect dataBounds = sites.GetSitesBounds();

			int sqrtSitesNb = (int)Math.Sqrt(sites.Count() + 4);
			HalfedgePriorityQueue heap = new HalfedgePriorityQueue(dataBounds.y, dataBounds.height, sqrtSitesNb);
			EdgeList edgeList = new EdgeList(dataBounds.x, dataBounds.width, sqrtSitesNb);
			List<Halfedge> halfEdges = new List<Halfedge>();
			List<Vertex> vertices = new List<Vertex>();

			Site bottomMostSite = sites.Next();
			newSite = sites.Next();

			while (true) {
				if (!heap.Empty()) {
					newIntStar = heap.Min();
				}

				if (newSite != null &&
				    (heap.Empty() || CompareByYThenX(newSite, newIntStar) < 0)) {
					// New site is smallest
					//Debug.Log("smallest: new site " + newSite);

					// Step 8:
					lbnd = edgeList.EdgeListLeftNeighbor(newSite.Coord);	// The halfedge just to the left of newSite
					//UnityEngine.Debug.Log("lbnd: " + lbnd);
					rbnd = lbnd.edgeListRightNeighbor;		// The halfedge just to the right
					//UnityEngine.Debug.Log("rbnd: " + rbnd);
					bottomSite = RightRegion(lbnd, bottomMostSite);			// This is the same as leftRegion(rbnd)
					// This Site determines the region containing the new site
					//UnityEngine.Debug.Log("new Site is in region of existing site: " + bottomSite);

					// Step 9
					edge = Edge.CreateBisectingEdge(bottomSite, newSite);
					//UnityEngine.Debug.Log("new edge: " + edge);
					edges.Add(edge);

					bisector = Halfedge.Create(edge, LR.LEFT);
					halfEdges.Add(bisector);
					// Inserting two halfedges into edgelist constitutes Step 10:
					// Insert bisector to the right of lbnd:
					edgeList.Insert(lbnd, bisector);

					// First half of Step 11:
					if ((vertex = Vertex.Intersect(lbnd, bisector)) != null) {
						vertices.Add(vertex);
						heap.Remove(lbnd);
						lbnd.vertex = vertex;
						lbnd.ystar = vertex.y + newSite.Dist(vertex);
						heap.Insert(lbnd);
					}

					lbnd = bisector;
					bisector = Halfedge.Create(edge, LR.RIGHT);
					halfEdges.Add(bisector);
					// Second halfedge for Step 10::
					// Insert bisector to the right of lbnd:
					edgeList.Insert(lbnd, bisector);

					// Second half of Step 11:
					if ((vertex = Vertex.Intersect(bisector, rbnd)) != null) {
						vertices.Add(vertex);
						bisector.vertex = vertex;
						bisector.ystar = vertex.y + newSite.Dist(vertex);
						heap.Insert(bisector);
					}

					newSite = sites.Next();
				} else if (!heap.Empty()) {
					// Intersection is smallest
					lbnd = heap.ExtractMin();
					llbnd = lbnd.edgeListLeftNeighbor;
					rbnd = lbnd.edgeListRightNeighbor;
					rrbnd = rbnd.edgeListRightNeighbor;
					bottomSite = LeftRegion(lbnd, bottomMostSite);
					topSite = RightRegion(rbnd, bottomMostSite);
					// These three sites define a Delaunay triangle
					// (not actually using these for anything...)
					// triangles.Add(new Triangle(bottomSite, topSite, RightRegion(lbnd, bottomMostSite)));

					v = lbnd.vertex;
					v.SetIndex();
					lbnd.edge.SetVertex(lbnd.leftRight, v);
					rbnd.edge.SetVertex(rbnd.leftRight, v);
					edgeList.Remove(lbnd);
					heap.Remove(rbnd);
					edgeList.Remove(rbnd);
					leftRight = LR.LEFT;
					if (bottomSite.y > topSite.y) {
						tempSite = bottomSite;
						bottomSite = topSite;
						topSite = tempSite;
						leftRight = LR.RIGHT;
					}
					edge = Edge.CreateBisectingEdge(bottomSite, topSite);
					edges.Add(edge);
					bisector = Halfedge.Create(edge, leftRight);
					halfEdges.Add(bisector);
					edgeList.Insert(llbnd, bisector);
					edge.SetVertex(LR.Other(leftRight), v);
					if ((vertex = Vertex.Intersect(llbnd, bisector)) != null) {
						vertices.Add(vertex);
						heap.Remove(llbnd);
						llbnd.vertex = vertex;
						llbnd.ystar = vertex.y + bottomSite.Dist(vertex);
						heap.Insert(llbnd);
					}
					if ((vertex = Vertex.Intersect(bisector, rrbnd)) != null) {
						vertices.Add(vertex);
						bisector.vertex = vertex;
						bisector.ystar = vertex.y + bottomSite.Dist(vertex);
						heap.Insert(bisector);
					}
				} else {
					break;
				}
			}

			// Heap should be empty now
			heap.Dispose();
			edgeList.Dispose();

			foreach (Halfedge halfedge in halfEdges) {
				halfedge.ReallyDispose();
			}
			halfEdges.Clear();

			// we need the vertices to clip the edges
			foreach (Edge e in edges) {
				e.ClipVertices(plotBounds);
			}
			// But we don't actually ever use them again!
			foreach (Vertex ve in vertices) {
				ve.Dispose();
			}
			vertices.Clear();
		}

		public void LloydRelaxation(int nbIterations) {
			// Reapeat the whole process for the number of iterations asked
			for (int i = 0; i < nbIterations; i++) {
				List<Vector2> newPoints = new List<Vector2>();
				// Go thourgh all sites
				sites.ResetListIndex();
				Site site = sites.Next();

				while (site != null) {
					// Loop all corners of the site to calculate the centroid
					List<Vector2> region = site.Region(plotBounds);
					if (region.Count < 1) {
						site = sites.Next();
						continue;
					}
					
					Vector2 centroid = Vector2.zero;
					float signedArea = 0;
					float x0 = 0;
					float y0 = 0;
					float x1 = 0;
					float y1 = 0;
					float a = 0;
					// For all vertices except last
					for (int j = 0; j < region.Count-1; j++) {
						x0 = region[j].x;
						y0 = region[j].y;
						x1 = region[j+1].x;
						y1 = region[j+1].y;
						a = x0*y1 - x1*y0;
						signedArea += a;
						centroid.x += (x0 + x1)*a;
						centroid.y += (y0 + y1)*a;
					}
					// Do last vertex
					x0 = region[region.Count-1].x;
					y0 = region[region.Count-1].y;
					x1 = region[0].x;
					y1 = region[0].y;
					a = x0*y1 - x1*y0;
					signedArea += a;
					centroid.x += (x0 + x1)*a;
					centroid.y += (y0 + y1)*a;

					signedArea *= 0.5f;
					centroid.x /= (6*signedArea);
					centroid.y /= (6*signedArea);
					// Move site to the centroid of its Voronoi cell
					newPoints.Add(centroid);
					site = sites.Next();
				}

				// Between each replacement of the cendroid of the cell,
				// we need to recompute Voronoi diagram:
				Rect origPlotBounds = this.plotBounds;
				Dispose();
				Init(newPoints,origPlotBounds);
			}
		}

		private Site LeftRegion(Halfedge he, Site bottomMostSite) {
			Edge edge = he.edge;
			if (edge == null) {
				return bottomMostSite;
			}
			return edge.Site(he.leftRight);
		}
		
		private Site RightRegion(Halfedge he, Site bottomMostSite) {
			Edge edge = he.edge;
			if (edge == null) {
				return bottomMostSite;
			}
			return edge.Site(LR.Other(he.leftRight));
		}

		public static int CompareByYThenX(Site s1, Site s2) {
			if (s1.y < s2.y) return -1;
			if (s1.y > s2.y) return 1;
			if (s1.x < s2.x) return -1;
			if (s1.x > s2.x) return 1;
			return 0;
		}
		
		public static int CompareByYThenX(Site s1, Vector2 s2) {
			if (s1.y < s2.y) return -1;
			if (s1.y > s2.y) return 1;
			if (s1.x < s2.x) return -1;
			if (s1.x > s2.x) return 1;
			return 0;
		}
	}
}
