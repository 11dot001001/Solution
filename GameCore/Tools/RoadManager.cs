using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameCore.Model;
using ILibrary.Maths.Geometry2D;
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

            WayPosition endPosition = new WayPosition(end.Position);
            foreach (Road item in _roads)
                item.Points.Add(endPosition);
        }
        public RoadManager(IEnumerable<Road> roads) => _roads = new List<Road>(roads);

        private void GetWay()
        {
            Road road = new Road();
            road.Points.Add(new WayPosition(_globalStart.Position));

            Vector2 startDirection = _globalTarget.Position - _globalStart.Position;
            List<BacteriumBase> tempTargets = _bacteriums.Where(x => (x.Position - _globalStart.Position).magnitude < startDirection.magnitude).ToList();
            tempTargets.RemoveAll(x => Vector2.Angle(_globalStart.Position - x.Position, _globalTarget.Position - x.Position) < 135);

            if (!IsIntersect(_globalStart.Position, _globalTarget.Position, _bacteriums, out BacteriumBase none))
                _roads.Add(road);

            foreach (BacteriumBase target in tempTargets)
            {
                Geometry2D.Point2CircleTangencyDirection(_globalStart.Position, target.Transform.Circle, out Vector2 tangencyDirection1, out Vector2 tangencyDirection2);
                _bacteriums.Remove(target);
                bool result = IsIntersect(_globalStart.Position, target.Position + tangencyDirection1, _bacteriums, out BacteriumBase nearest);
                bool result2 = IsIntersect(_globalStart.Position, target.Position + tangencyDirection2, _bacteriums, out nearest);
                _bacteriums.Add(target);
                if (!result)
                {
                    Road newRoad = new Road(road);
                    _roads.Add(newRoad);
                    GetRoads(target, tangencyDirection1, _globalTarget, Vector2.zero, newRoad);
                }
                if (!result2)
                {
                    Road newRoad = new Road(road);
                    _roads.Add(newRoad);
                    GetRoads(target, tangencyDirection2, _globalTarget, Vector2.zero, newRoad);
                }
            }
        }
        private void GetRoads(BacteriumBase start, Vector2 startDirection, BacteriumBase end, Vector2 endDirection, Road road)
        {
            Vector2 nearestTangencyDirection;
            bool isIntersect;
            if (end == _globalTarget)
            {
                nearestTangencyDirection = NearTangencyToEnd(end.Position, start, start.Position + startDirection);
                _bacteriums.Remove(start);
                isIntersect = IsIntersect(start.Position + nearestTangencyDirection, end.Position, _bacteriums, out BacteriumBase nearest);
                _bacteriums.Add(start);

                if (!isIntersect)
                {
                    Geometry2D.Rotate(start.Transform.Circle, startDirection, nearestTangencyDirection, _rotateAngle, out List<Vector2> way);
                    road.Points.AddRange(way.Select(x => new WayPosition(x)));
                }
                else
                {
                    Geometry2D.Point2CircleTangencyDirectionOriented(start.Position + startDirection, nearest.Transform.Circle, out Vector2 leftTangencyDirection, out Vector2 rightTangencyDirection);
                    Road newRoad = new Road(road);
                    _roads.Add(newRoad);

                    nearestTangencyDirection = NearTangencyToEnd(start.Position + startDirection, nearest, end.Position + endDirection);
                    road.EaseFactor *= 1 - Vector2.Angle(nearest.Position + leftTangencyDirection - start.Position + startDirection, nearest.Position + nearestTangencyDirection - start.Position + startDirection) / 180;
                    newRoad.EaseFactor *= 1 - Vector2.Angle(nearest.Position + rightTangencyDirection - start.Position + startDirection, nearest.Position + nearestTangencyDirection - start.Position + startDirection) / 180;

                    GetRoads(start, startDirection, nearest, leftTangencyDirection, road);
                    GetRoads(nearest, leftTangencyDirection, _globalTarget, Vector2.zero, road);
                    GetRoads(start, startDirection, nearest, rightTangencyDirection, newRoad);
                    GetRoads(nearest, rightTangencyDirection, _globalTarget, Vector2.zero, newRoad);
                }
            }
            else
            {
                Geometry2D.CircleDirection2CircleDirection(start.Transform.Circle, startDirection, end.Transform.Circle, endDirection, out Vector2 startEndDirection, out Vector2 endEndDirection);

                _bacteriums.Remove(start);
                _bacteriums.Remove(end);
                isIntersect = IsIntersect(start.Position + startEndDirection, end.Position + endEndDirection, _bacteriums, out BacteriumBase nearest);
                _bacteriums.Add(start);
                _bacteriums.Add(end);

                if (!isIntersect)
                {
                    Geometry2D.Rotate(start.Transform.Circle, startDirection, startEndDirection, _rotateAngle, out List<Vector2> way);
                    road.Points.AddRange(way.Select(x => new WayPosition(x)));
                    Geometry2D.Rotate(end.Transform.Circle, endEndDirection, endDirection, _rotateAngle, out way);
                    road.Points.AddRange(way.Select(x => new WayPosition(x)));
                }
                else
                {
                    Vector2 nearestTangency = NearTangencyToEnd(start.Position + startDirection, nearest, end.Position + endDirection);
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
                if (IsIntersect(current, target, residualBacteriums[i].Position, residualBacteriums[i].BacteriumRadius))
                {
                    float currentDistance = Vector2.Distance(residualBacteriums[i].Position, current);
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
            return Vector2.Angle(circle.Position + leftTangencyDirection - start, direction) < Vector2.Angle(circle.Position + rightTangencyDirection - start, direction) ? leftTangencyDirection : rightTangencyDirection;
        }

        public IEnumerator<Road> GetEnumerator() => _roads.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}