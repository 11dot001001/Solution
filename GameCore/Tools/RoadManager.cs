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

            _bacteriums.Remove(_globalStart);
            _bacteriums.Remove(_globalTarget);

            GetWay();

            WayPosition endPosition = new WayPosition(end.Transform.Position);
            foreach (Road item in _roads)
                item.Points.Add(endPosition);
        }
        public RoadManager(IEnumerable<Road> roads) => _roads = new List<Road>(roads);

        private void GetWay()
        {
            Road road = new Road();
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
                    GetRoads(new Vertex(IsClockRotation(_globalStart.Transform.Position, target.Transform.Circle, tangencyDirection1), target, tangencyDirection1), newRoad);
                }
                if (!result2)
                {
                    Road newRoad = new Road(road);
                    _roads.Add(newRoad);
                    GetRoads(new Vertex(IsClockRotation(_globalStart.Transform.Position, target.Transform.Circle, tangencyDirection2), target, tangencyDirection2), newRoad);
                }
                _bacteriums.Remove(_globalStart);
            }
        }
        private bool IsClockRotation(Vector2 startPosition, Circle endCircle, Vector2 endDirection)
        {
            Vector2 mainDirection = endCircle.Position - startPosition;
            Vector2 compareDirection = endCircle.Position + endDirection - startPosition;
            return Vector2.SignedAngle(mainDirection, compareDirection) > 0 ? true : false;
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
            Vector2 internalPointStart, internalPointEnd, externalPointStart, externalPointEnd;

            Geometry2D.ExternalTangencyBetweenTwoCircles(vertex.Binding.Transform.Circle, target.Transform.Circle, out Vector2 externalPoint1, out Vector2 externalPoint2, out Vector2 externalPoint3, out Vector2 externalPoint4);
            Geometry2D.InternalTangencyBetweenTwoCircles(vertex.Binding.Transform.Circle, target.Transform.Circle, out Vector2 internalPoint1, out Vector2 internalPoint2, out Vector2 internalPoint3, out Vector2 internalPoint4);

            externalPoint1 = externalPoint1 - vertex.Binding.Transform.Circle.Position;
            externalPoint2 = externalPoint2 - vertex.Binding.Transform.Circle.Position;
            internalPoint1 = internalPoint1 - vertex.Binding.Transform.Circle.Position;
            internalPoint2 = internalPoint2 - vertex.Binding.Transform.Circle.Position;
            externalPoint3 = externalPoint3 - target.Transform.Circle.Position;
            externalPoint4 = externalPoint4 - target.Transform.Circle.Position;
            internalPoint3 = internalPoint3 - target.Transform.Circle.Position;
            internalPoint4 = internalPoint4 - target.Transform.Circle.Position;

            _bacteriums.Remove(target);
            _bacteriums.Remove(vertex.Binding);
            if (vertex.IsClockRotate)
            {
                isIntersect = IsIntersect(vertex.Binding.Transform.Circle.Position + internalPoint1, target.Transform.Circle.Position + internalPoint3, _bacteriums, out nearest);
                isIntersect2 = IsIntersect(vertex.Binding.Transform.Circle.Position + externalPoint1, target.Transform.Circle.Position + externalPoint3, _bacteriums, out nearest2);
                internalPointStart = internalPoint1;
                internalPointEnd = internalPoint3;
                externalPointStart = externalPoint1;
                externalPointEnd = externalPoint3;
            }
            else
            {
                isIntersect = IsIntersect(vertex.Binding.Transform.Circle.Position + internalPoint2, target.Transform.Circle.Position + internalPoint4, _bacteriums, out nearest);
                isIntersect2 = IsIntersect(vertex.Binding.Transform.Circle.Position + externalPoint2, target.Transform.Circle.Position + externalPoint4, _bacteriums, out nearest2);
                internalPointStart = internalPoint2;
                internalPointEnd = internalPoint4;
                externalPointStart = externalPoint2;
                externalPointEnd = externalPoint4;
            }
            _bacteriums.Add(vertex.Binding);
            _bacteriums.Add(target);

            if (!isIntersect)
            {
                Road newRoad = new Road(road);
                _roads.Add(newRoad);
                Geometry2D.Rotate(vertex.Binding.Transform.Circle, vertex.Direction, internalPointStart, _rotateAngle, vertex.IsClockRotate, out List<Vector2> way);
                newRoad.Points.AddRange(way.Select(x => new WayPosition(x)));
                GetRoads(new Vertex(IsClockRotation(vertex.Binding.Transform.Circle.Position + internalPointStart, target.Transform.Circle, internalPointEnd), target, internalPointEnd), newRoad);
            }
            else
                GetRoads(vertex, nearest, road);
            if (!isIntersect2)
            {
                Road newRoad = new Road(road);
                _roads.Add(newRoad);
                Geometry2D.Rotate(vertex.Binding.Transform.Circle, vertex.Direction, externalPointStart, _rotateAngle, vertex.IsClockRotate, out List<Vector2> way);
                newRoad.Points.AddRange(way.Select(x => new WayPosition(x)));
                GetRoads(new Vertex(IsClockRotation(vertex.Binding.Transform.Circle.Position + externalPointStart, target.Transform.Circle, externalPointEnd), target, externalPointEnd), newRoad);
            }
            else
                GetRoads(vertex, nearest2, road);
            _roads.Remove(road);
        }

        private void GetRoads(BacteriumBase start, Vector2 startDirection, BacteriumBase end, Vector2 endDirection, Road road)
        {
            Vector2 nearestTangencyDirection;
            bool isIntersect;
            if (end == _globalTarget)
            {
                nearestTangencyDirection = NearTangencyToEnd(end.Transform.Position, start, start.Transform.Position + startDirection);
                _bacteriums.Remove(start);
                isIntersect = IsIntersect(start.Transform.Position + nearestTangencyDirection, end.Transform.Position, _bacteriums, out BacteriumBase nearest);
                _bacteriums.Add(start);

                if (!isIntersect)
                {
                    Geometry2D.Rotate(start.Transform.Circle, startDirection, nearestTangencyDirection, _rotateAngle, out List<Vector2> way);
                    road.Points.AddRange(way.Select(x => new WayPosition(x)));
                }
                else
                {
                    Geometry2D.Point2CircleTangencyDirectionOriented(start.Transform.Position + startDirection, nearest.Transform.Circle, out Vector2 leftTangencyDirection, out Vector2 rightTangencyDirection);
                    Road newRoad = new Road(road);
                    _roads.Add(newRoad);

                    //nearestTangencyDirection = NearTangencyToEnd(start.Position + startDirection, nearest, end.Position + endDirection);
                    //road.EaseFactor *= 1 - Vector2.Angle(nearest.Position + leftTangencyDirection - start.Position + startDirection, nearest.Position + nearestTangencyDirection - start.Position + startDirection) / 180;
                    //newRoad.EaseFactor *= 1 - Vector2.Angle(nearest.Position + rightTangencyDirection - start.Position + startDirection, nearest.Position + nearestTangencyDirection - start.Position + startDirection) / 180;

                    GetRoads(start, startDirection, nearest, leftTangencyDirection, road);
                    GetRoads(nearest, leftTangencyDirection, _globalTarget, Vector2.zero, road);
                    GetRoads(start, startDirection, nearest, rightTangencyDirection, newRoad);
                    GetRoads(nearest, rightTangencyDirection, _globalTarget, Vector2.zero, newRoad);
                }
            }
            else if (start == _globalStart)
            {
                Geometry2D.Point2CircleTangencyDirection(start.Transform.Position, end.Transform.Circle, out Vector2 tangencyDirection1, out Vector2 tangencyDirection2);

                _bacteriums.Remove(start);
                _bacteriums.Remove(end);
                bool result = IsIntersect(start.Transform.Position, end.Transform.Position + tangencyDirection1, _bacteriums, out BacteriumBase nearest);
                _bacteriums.Add(start);
                _bacteriums.Add(end);

                Road newRoad = new Road(road);
                if (!result)
                {
                    _roads.Add(newRoad);
                    GetRoads(end, tangencyDirection1, _globalTarget, Vector2.zero, newRoad);
                }
                else
                    GetRoads(start, Vector2.zero, nearest, Vector2.zero, newRoad);

                _bacteriums.Remove(start);
                _bacteriums.Remove(end);
                bool result2 = IsIntersect(start.Transform.Position, end.Transform.Position + tangencyDirection2, _bacteriums, out nearest);
                _bacteriums.Add(start);
                _bacteriums.Add(end);

                newRoad = new Road(road);
                if (!result2)
                {
                    _roads.Add(newRoad);
                    GetRoads(end, tangencyDirection2, _globalTarget, Vector2.zero, newRoad);
                }
                else
                    GetRoads(start, Vector2.zero, nearest, Vector2.zero, newRoad);
            }
            else
            {
                bool isClockRotation = Vector2.SignedAngle(startDirection, road.Last().Position - start.Transform.Position) <= 0 ? false : true;
                Geometry2D.CircleDirection2CircleDirection(start.Transform.Circle, startDirection, end.Transform.Circle, endDirection, isClockRotation, _rotateAngle, out Vector2 startEndDirection, out Vector2 endEndDirection);

                _bacteriums.Remove(start);
                _bacteriums.Remove(end);
                isIntersect = IsIntersect(start.Transform.Position + startEndDirection, end.Transform.Position + endEndDirection, _bacteriums, out BacteriumBase nearest);
                _bacteriums.Add(start);
                _bacteriums.Add(end);

                if (!isIntersect)
                {
                    Geometry2D.Rotate(start.Transform.Circle, startDirection, startEndDirection, _rotateAngle, isClockRotation, out List<Vector2> way);
                    road.Points.AddRange(way.Select(x => new WayPosition(x)));
                    Geometry2D.Rotate(end.Transform.Circle, endEndDirection, endDirection, _rotateAngle, isClockRotation, out way);
                    road.Points.AddRange(way.Select(x => new WayPosition(x)));
                }
                else
                {
                    Vector2 nearestTangency = NearTangencyToEnd(start.Transform.Position + startDirection, nearest, end.Transform.Position + endDirection);
                    GetRoads(start, startDirection, nearest, nearestTangency, road);
                    GetRoads(nearest, nearestTangency, end, endDirection, road);
                }
            }
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
                if (IsIntersect(current, target, residualBacteriums[i].Transform.Position, residualBacteriums[i].BacteriumRadius))
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
        private Vector2 NearTangencyToEnd(Vector2 start, BacteriumBase circle, Vector2 end)
        {
            Geometry2D.Point2CircleTangencyDirectionOriented(start, circle.Transform.Circle, out Vector2 leftTangencyDirection, out Vector2 rightTangencyDirection);
            Vector2 direction = end - start;
            return Vector2.Angle(circle.Transform.Position + leftTangencyDirection - start, direction) < Vector2.Angle(circle.Transform.Position + rightTangencyDirection - start, direction) ? leftTangencyDirection : rightTangencyDirection;
        }

        public IEnumerator<Road> GetEnumerator() => _roads.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    public struct Vertex
    {
        public bool IsClockRotate;
        public BacteriumBase Binding;
        public Vector2 Direction;

        public Vertex(bool isClockRotate, BacteriumBase binding, Vector2 direction)
        {
            IsClockRotate = isClockRotate;
            Binding = binding ?? throw new ArgumentNullException(nameof(binding));
            Direction = direction;
        }
    }
}