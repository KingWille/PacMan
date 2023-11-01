using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
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
            Node[] openNodes = new Node[TileArray.GetLength(0) * TileArray.GetLength(1)];
            List<Node> closedNodes = new List<Node>();
            Node[] adjNodes = new Node[4];
            Node nextNode = null;
            bool Reached = false;

            //Hämtar alla nodes

            for(int i = 0; i < TileArray.GetLength(0); i++)
            {
                for(int j = 0; j < TileArray.GetLength(1); j++)
                {
                    if (i == 0)
                    {
                        openNodes[j] = new Node(TileArray[i, j].Pos, playerPos);
                    }
                    else
                    {
                        openNodes[j  + i * 10] = new Node(TileArray[i, j].Pos, playerPos);
                    }
                }
            }

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

                adjNodes = WalkableAdjacentTiles(openNodes, nextNode);

                //Kollar vilken node som har kortast avstånd till spelaren av grannnodesen
                foreach (Node n in adjNodes)
                {
                    if (n != null && n.TotalDistance < nextNode.TotalDistance)
                    {
                        n.ParentNode = nextNode;
                        nextNode = n;
                    }
                }

                //Kollar om pathen har nått spelaren och isåfall sätter while condition som true
                if (nextNode.Pos == playerPos)
                {
                    closedNodes.Add(nextNode);
                    nextNode.Closed = true;
                    Reached = true;
                }
            }
             return closedNodes;
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
    }
}
