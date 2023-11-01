using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Channels;

namespace PacMan
{
    internal class EnemyController
    {
        private Animation Animation;
        private Texture2D Tex;
        private Texture2D PathTile;
        private int Index;
        private int SecondIndexer;
        private int IndexMultiplierFrame1;
        private int IndexMultiplierFrame2;
        private int DirectionIndex;
        private int TileSize;
        private float Speed;
        private Vector2 Destination;
        private Vector2 Direction;
        private Vector2 LastLinedUpTile;
        private Vector2 LastPlayerLinedUpTile;
        private Tiles[,] TileArray;
        private Node[,] NodeArray;
        private List<Node> Path;
        public EnemyController(Animation animation, int indexer, Texture2D tex, Texture2D path, int tileSize, float speed, Tiles[,] tileArray)
        {
            Animation = animation;
            Tex = tex;
            DirectionIndex = 1;
            TileSize = tileSize;
            Speed = speed;
            TileArray = tileArray;
            PathTile = path;

            NodeArray = new Node[TileArray.GetLength(0), TileArray.GetLength(1)];
        }

        //Vänder uppåt
        public void DirectionUp(Vector2 position, GameTime gameTime)
        {

            Movement(new Vector2(0, -1), position);
            DirectionIndex = 0;
        }
        //Vänder neråt
        public void DirectionDown(Vector2 position, GameTime gameTime)
        {

            Movement(new Vector2(0, 1), position);
            DirectionIndex = 2;
        }
        //Vänder åt vänster
        public void DirectionLeft(Vector2 position, GameTime gameTime)
        {

            Movement(new Vector2(-1, 0), position);
            DirectionIndex = 3;
        }

        //Vänder åt höger
        public void DirectionRight(Vector2 position, GameTime gameTime)
        {
            Movement(new Vector2(1, 0), position);
            DirectionIndex = 1;
        }

        //Upprepar fienders rörelse
        public Vector2 KeepMoving(Vector2 position, Vector2 playerPos, GameTime gameTime)
        {
            Vector2 newPos = position;

            //Kollar att där inte är en vägg i vägen
            if (CurrentTile(LastLinedUpTile)[DirectionIndex])
            {
                Movement(Direction, position);
                newPos += Direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            //Kollar att fienden och spelaren är upplinad med en ruta
            ChecklinedUp(position, playerPos);

            Path = CheckAvailablePaths(LastLinedUpTile, LastPlayerLinedUpTile, gameTime);

            return newPos;


        }

        //Sätter nästa destination för spelaren
        public void Movement(Vector2 direction, Vector2 position)
        {

            Direction = direction;

            Vector2 newDestination = position + direction * TileSize * 2;

            Destination = newDestination;

        }

        //Kollar om man får lov att röra sig i riktningen man vill
        public Dictionary<int, bool> CurrentTile(Vector2 position)
        {
            if (position.X < TileSize)
            {
                position.X = TileSize;
            }
            return TileArray[(int)position.Y / (TileSize * 2), (int)position.X / (TileSize * 2)].AllowedDirections;
        }

        public List<Node> CheckAvailablePaths(Vector2 pos, Vector2 playerPos, GameTime gameTime)
        {
            Node[] openNodes = new Node[TileArray.GetLength(0) * TileArray.GetLength(1) - 22];
            List<Node> tempOpenNodes = new List<Node>();
            List<Node> closedNodes = new List<Node>();
            List<Node> adjNodes = new List<Node>();
            List<Node> tempAdjNodes = new List<Node>();
            List<Node> finishedPath = new List<Node>();
            Node nextNode = null;
            bool Reached = false;

            //Hämtar alla nodes

            for(int i = 0; i < TileArray.GetLength(0); i++)
            {
                for(int j = 0; j < TileArray.GetLength(1); j++)
                {
                    if (i == 0 && TileArray[i,j].AllowedDirections != null)
                    {
                        tempOpenNodes.Add(new Node(TileArray[i, j].Pos, playerPos));
                    }
                    else if(TileArray[i,j].AllowedDirections != null)
                    {
                        tempOpenNodes.Add(new Node(TileArray[i, j].Pos, playerPos));
                    }
                }
            }

            openNodes = tempOpenNodes.ToArray();
            tempOpenNodes.Clear();

            //Sätter första noden
            foreach(Node node in openNodes) 
            {
                if(node.Pos == pos)
                {
                    nextNode = node;
                    break;
                }
            }

            //Loop som ger kortast path
            while (!Reached)
            {
                closedNodes.Add(nextNode);
                nextNode.Closed = true;

                tempAdjNodes = WalkableAdjacentTiles(openNodes, nextNode).ToList();

                //Kollar vilken node som har kortast avstånd till spelaren av grann nodesen
                foreach(Node n in tempAdjNodes)
                {
                    if(n != null)
                    {
                        adjNodes.Add(n);
                    }
                }

                adjNodes.Sort(0, adjNodes.Count, new ArraySorter());
                
                if(adjNodes.Count == 0)
                {
                    closedNodes.Remove(nextNode);
                    nextNode = nextNode.ParentNode;
                }
                else if(adjNodes.Count >= 2)
                {
                    adjNodes[0] = DetermineRouteIfSameCost(adjNodes.ToArray(), playerPos);
                    adjNodes[0].ParentNode = nextNode;
                    nextNode = adjNodes[0];
                }
                else
                {
                    adjNodes[0].ParentNode = nextNode;
                    nextNode = adjNodes[0];
                }

                tempAdjNodes.Clear();
                adjNodes.Clear();

                //Kollar om pathen har nått spelaren och isåfall sätter while condition som true
                if (nextNode.Pos == playerPos)
                {
                    closedNodes.Add(nextNode);
                    nextNode.Closed = true;
                    Reached = true;
                }
            }

            while(nextNode.ParentNode != null)
            {
                finishedPath.Add(nextNode);
                nextNode = nextNode.ParentNode;
            }
             return finishedPath;
        }
        //Ritar fiendens animation
        public void DrawMovement(Vector2 Pos, GameTime gameTime, SpriteBatch sb)
        {
            switch(DirectionIndex)
            {
                case 0:
                    IndexMultiplierFrame1 = 4;
                    IndexMultiplierFrame2 = 5;
                    break;
                case 1:
                    IndexMultiplierFrame1 = 0;
                    IndexMultiplierFrame2 = 1;
                    break;
                case 2:
                    IndexMultiplierFrame1 = 6;
                    IndexMultiplierFrame2 = 7;
                    break;
                case 3:
                    IndexMultiplierFrame1 = 2;
                    IndexMultiplierFrame2 = 3;
                    break;
            }

            SecondIndexer = Animation.RunAnimation(gameTime);
            
            if(SecondIndexer == 0)
            {
                sb.Draw(Tex, Pos, Animation.Rects2[Index, (SecondIndexer + 1) * IndexMultiplierFrame1], Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 1f);
            }
            else
            {
                sb.Draw(Tex, Pos, Animation.Rects2[Index, SecondIndexer * IndexMultiplierFrame2], Color.White, 0f, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 1f);
            }

            foreach(Node n in Path)
            {
                sb.Draw(PathTile, n.Pos, Color.White);
            }
        }

        public Node[] WalkableAdjacentTiles(Node[] nodes, Node currentNode)
        {
            Node[] walkableNodes = new Node[4];

            //Lägger till grann tilsen till currentNode
            for(int i = 0; i < nodes.Length; i++)
            {
                if (nodes[i].Pos.Y == currentNode.Pos.Y && currentNode.Pos.X + TileSize * 2 == nodes[i].Pos.X)
                {
                    if (!nodes[i].Closed)
                    {
                        walkableNodes[1] = nodes[i];
                    }
                }
                else if (nodes[i].Pos.Y == currentNode.Pos.Y && currentNode.Pos.X - TileSize * 2 == nodes[i].Pos.X)
                {
                    if (!nodes[i].Closed)
                    {
                        walkableNodes[3] = nodes[i];
                    }
                }
                else if (nodes[i].Pos.X == currentNode.Pos.X && currentNode.Pos.Y + TileSize * 2 == nodes[i].Pos.Y)
                {
                    if (!nodes[i].Closed)
                    {
                        walkableNodes[2] = nodes[i];
                    }
                }
                else if (nodes[i].Pos.X == currentNode.Pos.X && currentNode.Pos.Y - TileSize * 2 == nodes[i].Pos.Y)
                {
                    if (!nodes[i].Closed)
                    {
                        walkableNodes[1] = nodes[i];
                    }
                }
            }

            //Kollar vilka nodes som faktiskt är walkabale
            for (int i = 0; i < 4; i++)
            {
                if(!CurrentTile(currentNode.Pos)[i])
                {
                    walkableNodes[i] = null;
                }
            }

            return walkableNodes;
        }

        public void ChecklinedUp(Vector2 pos, Vector2 playerPos)
        {
            for (int i = 0; i < TileArray.GetLength(0); i++)
            {
                for (int j = 0; j < TileArray.GetLength(1); j++)
                {
                    if (Vector2.Distance(pos, TileArray[i, j].Pos) < 2 && LastLinedUpTile != pos)
                    {
                        LastLinedUpTile = TileArray[i, j].Pos;
                        break;
                    }
                }
            }

            for (int i = 0; i < TileArray.GetLength(0); i++)
            {
                for (int j = 0; j < TileArray.GetLength(1); j++)
                {
                    if (Vector2.Distance(playerPos, TileArray[i, j].Pos) < 2 && LastPlayerLinedUpTile != playerPos)
                    {
                        LastPlayerLinedUpTile = TileArray[i, j].Pos;
                        break;
                    }
                }
            }
        }

        public Node DetermineRouteIfSameCost(Node[] adjNodes, Vector2 playerPos)
        {

            Node result = adjNodes[0];

            if (adjNodes[0].TotalDistance == adjNodes[1].TotalDistance)
            {
                foreach(Node node in adjNodes)
                {
                    if (playerPos.Y > node.Pos.Y && node != null)
                    {
                        if (CurrentTile(node.Pos)[2])
                        {
                            result = node;
                        }
                    }
                    else if(playerPos.Y < node.Pos.Y && node != null)
                    {
                        if (CurrentTile(node.Pos)[0])
                        {
                            result = node;
                        }
                    }
                    else if (playerPos.X < node.Pos.X && node != null)
                    {
                        if (CurrentTile(node.Pos)[3])
                        {
                            result = node;
                        }
                    }
                    else if (playerPos.X > node.Pos.X && node != null)
                    {
                        if (CurrentTile(node.Pos)[1])
                        {
                            result = node;
                        }
                    }
                }

            }


            return result;
        }
    }
}
