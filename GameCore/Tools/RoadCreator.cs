using GameCore.Model;
using ILibrary.Maths.Geometry2D;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameCore.Tools
{
    public class RoadCreator : IEnumerable<Road>
    {
        private struct Vertex
        {
            public BacteriumModel Binding;
            public Vector2 Direction;
            public bool IsClockRotate;

            public Vertex(bool isClockRotate, BacteriumModel binding, Vector2 direction)
            {
                IsClockRotate = isClockRotate;
                Binding = binding ?? throw new ArgumentNullException(nameof(binding));
                Direction = direction;
            }
        }
        private const float _rotateAngle = 10f;
        private List<BacteriumModel> _bacteriums;
        private BacteriumModel _globalStart;
        private BacteriumModel _globalTarget;

        public RoadCreator(BacteriumModel start, BacteriumModel end, IEnumerable<BacteriumModel> bacteriums)
        {
            Roads = new List<Road>();
            _bacteriums = new List<BacteriumModel>(bacteriums);
            _globalStart = start;
            _globalTarget = end;

            GetWay();

            Vector2 endPosition = end.Transform.Position;
            int id = 0;
            foreach (Road road in Roads)
            {
                road.Points.Add(endPosition);
                road.SetLength();
                road.Id = id++;
            }
        }
        public RoadCreator(IEnumerable<Road> roads) => Roads = new List<Road>(roads);

        public List<Road> Roads { get; private set; }

        private void GetWay()
        {
            _bacteriums.Remove(_globalStart);
            _bacteriums.Remove(_globalTarget);
            Road road = new Road(_globalStart, _globalTarget);
            road.Points.Add(_globalStart.Transform.Position);

            List<BacteriumModel> tempTargets = _bacteriums.ToList();

            if (!IsIntersect(_globalStart.Transform.Position, _globalTarget.Transform.Position, _bacteriums, out BacteriumModel none))
                Roads.Add(road);

            foreach (BacteriumModel target in tempTargets)
            {
                Geometry2D.Point2CircleTangencyDirection(_globalStart.Transform.Position, target.Transform.Circle, out Vector2 tangencyDirection1, out Vector2 tangencyDirection2);
                _bacteriums.Remove(target);
                _bacteriums.Add(_globalTarget);
                bool result = IsIntersect(_globalStart.Transform.Position, target.Transform.Position + tangencyDirection1, _bacteriums, out BacteriumModel nearest);
                bool result2 = IsIntersect(_globalStart.Transform.Position, target.Transform.Position + tangencyDirection2, _bacteriums, out nearest);
                _bacteriums.Add(target);
                _bacteriums.Remove(_globalTarget);

                _bacteriums.Add(_globalStart);
                if (!result)
                {
                    Road newRoad = new Road(road);
                    Roads.Add(newRoad);
                    if (Vector2.Angle(target.Transform.Position + tangencyDirection1 - _globalStart.Transform.Position, _globalTarget.Transform.Position - _globalStart.Transform.Position) > 90)
                        newRoad.DirectionFactor = 0;
                    GetRoads(new Vertex(IsClockRotation(_globalStart.Transform.Position, target.Transform.Circle, tangencyDirection1), target, tangencyDirection1), newRoad);
                }
                if (!result2)
                {
                    Road newRoad = new Road(road);
                    Roads.Add(newRoad);
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
            BacteriumModel nearest;
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
                road.Points.AddRange(way);
            }
            else
                GetRoads(vertex, nearest, road);
        }
        private void GetRoads(Vertex vertex, BacteriumModel target, Road road)
        {
            bool isIntersect, isIntersect2;
            BacteriumModel nearest, nearest2;
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
                Roads.Add(newRoad);
                Geometry2D.Rotate(vertex.Binding.Transform.Circle, vertex.Direction, internalDirectionStart, _rotateAngle, vertex.IsClockRotate, out List<Vector2> way);
                newRoad.Points.AddRange(way);
                GetRoads(new Vertex(IsClockRotation(vertex.Binding.Transform.Circle.Position + internalDirectionStart, target.Transform.Circle, internalDirectionEnd), target, internalDirectionEnd), newRoad);
            }
            else
                GetRoads(vertex, nearest, road);
            if (!isIntersect2)
            {
                Road newRoad = new Road(road);
                Roads.Add(newRoad);
                Geometry2D.Rotate(vertex.Binding.Transform.Circle, vertex.Direction, externalDirectionStart, _rotateAngle, vertex.IsClockRotate, out List<Vector2> way);
                newRoad.Points.AddRange(way);
                GetRoads(new Vertex(IsClockRotation(vertex.Binding.Transform.Circle.Position + externalDirectionStart, target.Transform.Circle, externalDirectionEnd), target, externalDirectionEnd), newRoad);
            }
            else if(nearest != nearest2)
                GetRoads(vertex, nearest2, road);
            Roads.Remove(road);
        }      
        private bool IsClockRotation(Vector2 startPosition, Circle endCircle, Vector2 endDirection)
        {
            Vector2 mainDirection = endCircle.Position - startPosition;
            Vector2 compareDirection = endCircle.Position + endDirection - startPosition;
            return Vector2.SignedAngle(mainDirection, compareDirection) > 0 ? true : false;
        }
        private bool IsIntersect(Vector2 current, Vector2 target, IEnumerable<BacteriumModel> residualBacterium, out BacteriumModel nearestBacterium)
        {
            List<BacteriumModel> residualBacteriums = new List<BacteriumModel>(residualBacterium);
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

        public IEnumerator<Road> GetEnumerator() => Roads.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}