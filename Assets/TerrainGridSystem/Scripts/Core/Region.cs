using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TGS.Geom;

namespace TGS {
    public class Region {

        public Polygon polygon;

        /// <summary>
        /// Points coordinates with applied grid offset and scale
        /// </summary>
        public List<Vector2> points;

        /// <summary>
        /// Scaled rect (rect with grid offset and scale applied)
        /// </summary>
        public Rect rect2D;
        public float rect2DArea;

        /// <summary>
        /// Cells in this region.
        /// </summary>
        public List<Cell> cells;

        /// <summary>
        /// Original grid segments. Segments coordinates are not scaled.
        /// </summary>
        public List<Segment> segments;

        public IAdmin entity;

        public Renderer renderer;
        public GameObject surfaceGameObject { get { return renderer != null ? renderer.gameObject : null; } }
        public Material cachedMat;

        /// <summary>
        /// Used internally to ensure smaller territory surfaces are rendered before others
        /// </summary>
        public int sortIndex;

        public Material customMaterial { get; set; }

        public Vector2 customTextureScale, customTextureOffset;
        public float customTextureRotation;
        public bool customRotateInLocalSpace;

        public delegate bool ContainsFunction(float x, float y);
        public ContainsFunction Contains;

        public bool isBox;

        /// <summary>
        /// If the gameobject contains one or more children surfaces with name splitSurface due to having +65000 vertices
        /// </summary>
		public List<Renderer> childrenSurfaces;

        public Region(IAdmin entity, bool isBox) {
            this.entity = entity;
            this.isBox = isBox;
            if (isBox) {
                segments = new List<Segment>(4);
                Contains = PointInBox;
            } else {
                segments = new List<Segment>(6);
                Contains = PointInPolygon;
            }
        }


        public void Clear() {
            polygon = null;
            if (points != null) {
                points.Clear();
            }
            segments.Clear();
            rect2D.width = rect2D.height = 0;
            rect2DArea = 0;
            if (surfaceGameObject != null) {
                Object.DestroyImmediate(surfaceGameObject);
            }
            customMaterial = null;
            childrenSurfaces = null;
            cells.Clear();
        }

        public void DestroySurface() {
            if (renderer != null) {
                Object.DestroyImmediate(renderer.gameObject);
                renderer = null;
            }
        }

        public Region Clone() {
            Region c = new Region(entity, isBox);
            c.customMaterial = this.customMaterial;
            c.customTextureScale = this.customTextureScale;
            c.customTextureOffset = this.customTextureOffset;
            c.customTextureRotation = this.customTextureRotation;
            c.points = new List<Vector2>(points);
            c.polygon = polygon.Clone();
            c.segments = new List<Segment>(segments);
            c.rect2D = rect2D;
            c.rect2DArea = rect2DArea;
            return c;
        }


        public void SetPoints(List<Vector2> points) {
            this.points = points;
            UpdateBounds();
        }

        public void Enlarge(float amount) {
            Vector2 center = rect2D.center;
            int pointCount = points.Count;
            for (int k = 0; k < pointCount; k++) {
                Vector2 p = points[k];
                float DX = center.x - p.x;
                float DY = center.y - p.y;
                p.x -= DX * amount;
                p.y -= DY * amount;
                points[k] = p;
            }
        }

        public bool Intersects(Region other) {

            if (points == null || other == null || other.points == null)
                return false;

            Rect otherRect = other.rect2D;

            if (otherRect.xMin > rect2D.xMax)
                return false;
            if (otherRect.xMax < rect2D.xMin)
                return false;
            if (otherRect.yMin > rect2D.yMax)
                return false;
            if (otherRect.yMax < rect2D.yMin)
                return false;

            int pointCount = points.Count;
            int otherPointCount = other.points.Count;

            for (int k = 0; k < otherPointCount; k++) {
                int j = pointCount - 1;
                bool inside = false;
                Vector2 p = other.points[k];
                for (int i = 0; i < pointCount; j = i++) {
                    if (((points[i].y <= p.y && p.y < points[j].y) || (points[j].y <= p.y && p.y < points[i].y)) &&
                        (p.x < (points[j].x - points[i].x) * (p.y - points[i].y) / (points[j].y - points[i].y) + points[i].x))
                        inside = !inside;
                }
                if (inside)
                    return true;
            }

            for (int k = 0; k < pointCount; k++) {
                int j = otherPointCount - 1;
                bool inside = false;
                Vector2 p = points[k];
                for (int i = 0; i < otherPointCount; j = i++) {
                    if (((other.points[i].y <= p.y && p.y < other.points[j].y) || (other.points[j].y <= p.y && p.y < other.points[i].y)) &&
                        (p.x < (other.points[j].x - other.points[i].x) * (p.y - other.points[i].y) / (other.points[j].y - other.points[i].y) + other.points[i].x))
                        inside = !inside;
                }
                if (inside)
                    return true;
            }

            return false;
        }

        bool PointInBox(float x, float y) {
            return x >= rect2D.xMin && x <= rect2D.xMax && y >= rect2D.yMin && y <= rect2D.yMax;
        }

        bool PointInPolygon(float x, float y) {
            if (points == null)
                return false;

            if (x > rect2D.xMax || x < rect2D.xMin || y > rect2D.yMax || y < rect2D.yMin)
                return false;

            int numPoints = points.Count;
            int j = numPoints - 1;
            bool inside = false;
            for (int i = 0; i < numPoints; j = i++) {
                if (((points[i].y <= y && y < points[j].y) || (points[j].y <= y && y < points[i].y)) &&
                    (x < (points[j].x - points[i].x) * (y - points[i].y) / (points[j].y - points[i].y) + points[i].x))
                    inside = !inside;
            }
            return inside;
        }

        public bool ContainsPoint(Vector2 point) {
            return PointInPolygon(point.x, point.y);
        }

        public bool ContainsRegion(Region otherRegion) {
            if (!rect2D.Overlaps(otherRegion.rect2D))
                return false;

            if (!Contains(otherRegion.rect2D.xMin, otherRegion.rect2D.yMin))
                return false;
            if (!Contains(otherRegion.rect2D.xMin, otherRegion.rect2D.yMax))
                return false;
            if (!Contains(otherRegion.rect2D.xMax, otherRegion.rect2D.yMin))
                return false;
            if (!Contains(otherRegion.rect2D.xMax, otherRegion.rect2D.yMax))
                return false;

            int opc = otherRegion.points.Count;
            for (int k = 0; k < opc; k++) {
                if (!Contains(otherRegion.points[k].x, otherRegion.points[k].y))
                    return false;
            }
            return true;
        }


        public void UpdateBounds() {
            float minx, miny, maxx, maxy;
            minx = miny = float.MaxValue;
            maxx = maxy = float.MinValue;
            int pointsCount = points.Count;
            for (int p = 0; p < pointsCount; p++) {
                Vector2 point = points[p];
                if (point.x < minx)
                    minx = point.x;
                if (point.x > maxx)
                    maxx = point.x;
                if (point.y < miny)
                    miny = point.y;
                if (point.y > maxy)
                    maxy = point.y;
            }
            float rectWidth = maxx - minx;
            float rectHeight = maxy - miny;
            rect2D = new Rect(minx, miny, rectWidth, rectHeight);
            rect2DArea = rectWidth * rectHeight;
        }
    }
}

