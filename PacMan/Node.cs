using System;
using Microsoft.Xna.Framework;

namespace PacMan
{
    internal class Node
    {
        internal float DistanceToEndNode;
        internal float DistanceStartToThis;
        internal float TotalDistance;
        internal bool Closed;
        internal Vector2 Pos;
        internal Node ParentNode;

        private float a;
        private float b;


        public Node(Vector2 startNode, Vector2 endNode)
        {
            Pos = startNode;
            Closed = false;
            ParentNode = null;
            SetValues(startNode, endNode);
        }

        public void SetValues(Vector2 startNode, Vector2 endNode)
        {
            Pos = startNode;
            SetDistanceToEndNode(endNode);
            SetDistanceStartToThisNode(startNode);

            TotalDistance = DistanceToEndNode + DistanceStartToThis;
        }

        public void SetDistanceToEndNode(Vector2 endNode) 
        {
            if (Pos.Y > endNode.Y)
            {
                a = Pos.Y - endNode.Y;
            }
            else
            {
                a = endNode.Y - Pos.Y;
            }

            if (Pos.X > endNode.X)
            {
                b = Pos.X - endNode.X;
            }
            else
            {
                b = endNode.X - Pos.X;
            }

            DistanceToEndNode = (float)Math.Sqrt(a * a + b * b);
        }
        public void SetDistanceStartToThisNode(Vector2 startNode)
        {
            if (Pos.Y > startNode.Y)
            {
                a = Pos.Y - startNode.Y;
            }
            else
            {
                a = startNode.Y - Pos.Y;
            }

            if (Pos.X > startNode.X)
            {
                b = Pos.X - startNode.X;
            }
            else
            {
                b = startNode.X - Pos.X;
            }
            DistanceStartToThis = (float)Math.Sqrt(a * a + b * b);
        }
    }
}
