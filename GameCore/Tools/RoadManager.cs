using GameCore.Model;
using ILibrary.Maths.Geometry2D;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameCore.Tools
{
    public class RoadManager : IEnumerable<Road>
    {
        private struct Vertex
        {
            public BacteriumBase Binding;
            public Vector2 Direction;
            public bool IsClockRotate;

            public Vertex(bool isClockRotate, BacteriumBase binding, Vector2 direction)
            {
                IsClockRotate = isClockRotate;
                Binding = binding ?? throw new ArgumentNullException(nameof(binding));
                Direction = direction;
            }
        }
        private const float _rotateAngle = 10f;
        private List<BacteriumBase> _bacteriums;
        private List<Road> _roads = new List<Road>();
        private BacteriumBase _globalStart;
        private BacteriumBase _globalTarget;

        public Road[] Roads => _roads.ToArray();

        public RoadManager(BacteriumBase start, BacteriumBase end, IEnumerable<BacteriumBase> bacteriums)
        {
            _bacteriums = new List<BacteriumBase>(bacteriums);
            _globalStart = start;
            _globalTarget = end;

            GetWay();

            WayPosition endPosition = new WayPosition(end.Transform.Position);
            foreach (Road road in _roads)
            {
                road.Points.Add(endPosition);
                road.SetLength();
            }
        }
        public RoadManager(IEnumerable<Road> roads) => _roads = new List<Road>(roads);

        private void GetWay()
        {
            _bacteriums.Remove(_globalStart);
            _bacteriums.Remove(_globalTarget);
            Road road = new Road(_globalStart, _globalTarget);
            road.Points.Add(new WayPosition(_globalStart.Transform.Position));

            List<BacteriumBase> tempTargets = _bacteriums.ToList();

            if (!IsIntersect(_globalStart.Transform.Position, _globalTarget.Transform.Position, _bacteriums, out BacteriumBase none))
                _roads.Add(road);

            foreach (BacteriumBase target in tempTargets)
            {
                Geometry2D.Point2CircleTangencyDirection(_globalStart.Transform.Position, target.Transform.Circle, out Vector2 tangencyDirection1, out Vector2 tangencyDirection2);
                _bacteriums.Remove(target);
                _bacteriums.Add(_globalTarget);
                bool result = IsIntersect(_globalStart.Transform.Position, target.Transform.Position + tangencyDirection1, _bacteriums, out BacteriumBase nearest);
                bool result2 = IsIntersect(_globalStart.Transform.Position, target.Transform.Position + tangencyDirection2, _bacteriums, out nearest);
                _bacteriums.Add(target);
                _bacteriums.Remove(_globalTarget);

                _bacteriums.Add(_globalStart);
                if (!result)
                {
                    Road newRoad = new Road(road);
                    _roads.Add(newRoad);
                    if (Vector2.Angle(target.Transform.Position + tangencyDirection1 - _globalStart.Transform.Position, _globalTarget.Transform.Position - _globalStart.Transform.Position) > 90)
                        newRoad.DirectionFactor = 0;
                    GetRoads(new Vertex(IsClockRotation(_globalStart.Transform.Position, target.Transform.Circle, tangencyDirection1), target, tangencyDirection1), newRoad);
                }
                if (!result2)
                {
                    Road newRoad = new Road(road);
                    _roads.Add(newRoad);
                    if (Vector2.Angle(target.Transform.Position + tangencyDirection2 - _globalStart.Transform.Position, _globalTarget.Transform.Position - _globalStart.Transform.Position) > 90)
                        newRoad.DirectionFactor = 0;
                    GetRoads(new Vertex(IsClockRotation(_globalStart.Transform.Position, target.Transform.Circle, tangencyDirection2), target, tangencyDirection2), newRoad);
                }
                _bacteriums.Remove(_globalStart);
            }
        }
        private void GetRoads(Vertex vertex, Road road)
        {
            bool isIntersect;
            BacteriumBase nearest;
            Vector2 tangencyDirection;

            Geometry2D.Point2CircleTangencyDirectionOriented(_globalTarget.Transform.Position, vertex.Binding.Transform.Circle, out Vector2 leftTangencyDirection, out Vector2 rightTangencyDirection);

            _bacteriums.Remove(vertex.Binding);
            if (vertex.IsClockRotate)
            {
                isIntersect = IsIntersect(_globalTarget.Transform.Position, vertex.Binding.Transform.Position + rightTangencyDirection, _bacteriums, out nearest);
                tangencyDirection = rightTangencyDirection;
            }
            else
            {
                isIntersect = IsIntersect(_globalTarget.Transform.Position, vertex.Binding.Transform.Position + leftTangencyDirection, _bacteriums, out nearest);
                tangencyDirection = leftTangencyDirection;
            }
            _bacteriums.Add(vertex.Binding);

            if (!isIntersect)
            {
                Geometry2D.Rotate(vertex.Binding.Transform.Circle, vertex.Direction, tangencyDirection, _rotateAngle, vertex.IsClockRotate, out List<Vector2> way);
                road.Points.AddRange(way.Select(x => new WayPosition(x)));
            }
            else
                GetRoads(vertex, nearest, road);
        }
        private void GetRoads(Vertex vertex, BacteriumBase target, Road road)
        {
            bool isIntersect, isIntersect2;
            BacteriumBase nearest, nearest2;
            Vector2 internalDirectionStart, internalDirectionEnd, externalDirectionStart, externalDirectionEnd;

            Geometry2D.ExternalTangencyBetweenTwoCircles(vertex.Binding.Transform.Circle, target.Transform.Circle, out Vector2 externalDirection1, out Vector2 externalDirection2, out Vector2 externalDirection3, out Vector2 externalDirection4);
            Geometry2D.InternalTangencyBetweenTwoCircles(vertex.Binding.Transform.Circle, target.Transform.Circle, out Vector2 internalDirection1, out Vector2 internalDirection2, out Vector2 internalDirection3, out Vector2 internalDirection4);

            externalDirection1 = externalDirection1 - vertex.Binding.Transform.Circle.Position;
            externalDirection2 = externalDirection2 - vertex.Binding.Transform.Circle.Position;
            internalDirection1 = internalDirection1 - vertex.Binding.Transform.Circle.Position;
            internalDirection2 = internalDirection2 - vertex.Binding.Transform.Circle.Position;
            externalDirection3 = externalDirection3 - target.Transform.Circle.Position;
            externalDirection4 = externalDirection4 - target.Transform.Circle.Position;
            internalDirection3 = internalDirection3 - target.Transform.Circle.Position;
            internalDirection4 = internalDirection4 - target.Transform.Circle.Position;

            _bacteriums.Remove(target);
            _bacteriums.Remove(vertex.Binding);
            if (vertex.IsClockRotate)
            {
                isIntersect = IsIntersect(vertex.Binding.Transform.Circle.Position + internalDirection1, target.Transform.Circle.Position + internalDirection3, _bacteriums, out nearest);
                isIntersect2 = IsIntersect(vertex.Binding.Transform.Circle.Position + externalDirection1, target.Transform.Circle.Position + externalDirection3, _bacteriums, out nearest2);
                internalDirectionStart = internalDirection1;
                internalDirectionEnd = internalDirection3;
                externalDirectionStart = externalDirection1;
                externalDirectionEnd = externalDirection3;
            }
            else
            {
                isIntersect = IsIntersect(vertex.Binding.Transform.Circle.Position + internalDirection2, target.Transform.Circle.Position + internalDirection4, _bacteriums, out nearest);
                isIntersect2 = IsIntersect(vertex.Binding.Transform.Circle.Position + externalDirection2, target.Transform.Circle.Position + externalDirection4, _bacteriums, out nearest2);
                internalDirectionStart = internalDirection2;
                internalDirectionEnd = internalDirection4;
                externalDirectionStart = externalDirection2;
                externalDirectionEnd = externalDirection4;
            }
            _bacteriums.Add(vertex.Binding);
            _bacteriums.Add(target);

            if (!isIntersect)
            {
                Road newRoad = new Road(road);
                _roads.Add(newRoad);
                Geometry2D.Rotate(vertex.Binding.Transform.Circle, vertex.Direction, internalDirectionStart, _rotateAngle, vertex.IsClockRotate, out List<Vector2> way);
                newRoad.Points.AddRange(way.Select(x => new WayPosition(x)));
                GetRoads(new Vertex(IsClockRotation(vertex.Binding.Transform.Circle.Position + internalDirectionStart, target.Transform.Circle, internalDirectionEnd), target, internalDirectionEnd), newRoad);
            }
            else
                GetRoads(vertex, nearest, road);
            if (!isIntersect2)
            {
                Road newRoad = new Road(road);
                _roads.Add(newRoad);
                Geometry2D.Rotate(vertex.Binding.Transform.Circle, vertex.Direction, externalDirectionStart, _rotateAngle, vertex.IsClockRotate, out List<Vector2> way);
                newRoad.Points.AddRange(way.Select(x => new WayPosition(x)));
                GetRoads(new Vertex(IsClockRotation(vertex.Binding.Transform.Circle.Position + externalDirectionStart, target.Transform.Circle, externalDirectionEnd), target, externalDirectionEnd), newRoad);
            }
            else if(nearest != nearest2)
                GetRoads(vertex, nearest2, road);
            _roads.Remove(road);
        }      
        private bool IsClockRotation(Vector2 startPosition, Circle endCircle, Vector2 endDirection)
        {
            Vector2 mainDirection = endCircle.Position - startPosition;
            Vector2 compareDirection = endCircle.Position + endDirection - startPosition;
            return Vector2.SignedAngle(mainDirection, compareDirection) > 0 ? true : false;
        }
        private bool IsIntersect(Vector2 current, Vector2 target, IEnumerable<BacteriumBase> residualBacterium, out BacteriumBase nearestBacterium)
        {
            List<BacteriumBase> residualBacteriums = new List<BacteriumBase>(residualBacterium);
            if (residualBacteriums.Count == 0)
            {
                nearestBacterium = null;
                return false;
            }
            float minDistance = float.MaxValue;
            int minDistanceBacterium = 0;
            for (int i = 0; i < residualBacteriums.Count; i++)
                if (IsIntersect(current, target, residualBacteriums[i].Transform.Position, residualBacteriums[i].Transform.BacteriumRadius))
                {
                    float currentDistance = Vector2.Distance(residualBacteriums[i].Transform.Position, current);
                    if (currentDistance < minDistance)
                    {
                        minDistance = currentDistance;
                        minDistanceBacterium = i;
                    }
                }
            nearestBacterium = residualBacteriums[minDistanceBacterium];
            return minDistance == float.MaxValue ? false : true;
        }
        private bool IsIntersect(Vector2 start, Vector2 end, Vector2 center, float radius)
        {
            Vector2 a = start - end;
            Vector2 b = center - end;
            if (b.magnitude > a.magnitude)
                return false;
            float cosAngle = Mathf.Cos(Vector2.Angle(a, b) * Mathf.Deg2Rad);
            if (cosAngle < 0f)
                return false;
            Vector2 point = end + a.normalized * b.magnitude * cosAngle;

            float distance = (center - point).magnitude;

            return distance > radius ? false : true;
        }

        public IEnumerator<Road> GetEnumerator() => _roads.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}