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
    internal class EnemyController : Controllers
    {

        private Texture2D PathTile;
        private Vector2 LastPlayerLinedUpTile;
        private List<Node> Path;
        public EnemyController(Animation animation, Texture2D tex, Texture2D path, int tileSize, float speed, Tiles[,] tileArray)
        {
            Animation = animation;
            Tex = tex;
            DirectionIndex = 1;
            TileSize = tileSize;
            Speed = speed;
            TileArray = tileArray;
            PathTile = path;

            Path = new List<Node>();
        }

        //Upprepar fienders rörelse
        public override Vector2 KeepMoving(Vector2 position, Vector2 playerPos, GameTime gameTime, bool released)
        {
            Vector2 newPos = position;

            if (released)
            {

                //Kollar att man inte försöker gå igenom en vägg
                if (CurrentTile(LastLinedUpTile)[DirectionIndex])
                {
                    newPos += Direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                //Sätter positionen till lika med nästa tile
                if (Vector2.Distance(position, Destination) < 5)
                {
                    newPos = Destination;
                }


                //Kollar att fienden och spelaren är upplinad med en ruta
                ChecklinedUp(position, playerPos);

                //Tar bort nodes från path efterhand som de används
                if (Path.Count != 0)
                {
                    if (Path[0].Pos == LastLinedUpTile)
                    {
                        Path.RemoveAt(0);
                    }
                }

                //Sätter movement till vilket håll pathen är
                if (Path.Count != 0 && position == LastLinedUpTile)
                {

                    if (Path[0].Pos.X > LastLinedUpTile.X)
                    {
                        Movement(new Vector2(1, 0), position);
                        DirectionIndex = 1;
                    }
                    else if (Path[0].Pos.X < LastLinedUpTile.X)
                    {
                        Movement(new Vector2(-1, 0), position);
                        DirectionIndex = 3;
                    }
                    else if (Path[0].Pos.Y > LastLinedUpTile.Y)
                    {
                        Movement(new Vector2(0, 1), position);
                        DirectionIndex = 2;
                    }
                    else if (Path[0].Pos.Y < LastLinedUpTile.Y)
                    {
                        Movement(new Vector2(0, -1), position);
                        DirectionIndex = 0;
                    }
                }
            }
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

        public List<Node> CheckAvailablePaths(Vector2 pos, Vector2 playerPos)
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
                //Kollar om där är flera nodes med samma avstånd
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

            //Lägger till pathen i slut listan
            while(nextNode.ParentNode != null)
            {
                finishedPath.Add(nextNode);
                nextNode = nextNode.ParentNode;
            }
            finishedPath.Reverse();
             return finishedPath;
        }
        //Ritar fiendens animation
        public override void DrawMovement(Vector2 Pos, GameTime gameTime, SpriteBatch sb)
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
                        walkableNodes[0] = nodes[i];
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
            //sätter fiendens lineup
            for (int i = 0; i < TileArray.GetLength(0); i++)
            {
                for (int j = 0; j < TileArray.GetLength(1); j++)
                {
                    if (Vector2.Distance(pos, TileArray[i, j].Pos) < 3 && LastLinedUpTile != pos)
                    {
                        LastLinedUpTile = TileArray[i, j].Pos;
                        break;
                    }
                }
            }

            //Sätter spelarens lineup
            for (int i = 0; i < TileArray.GetLength(0); i++)
            {
                for (int j = 0; j < TileArray.GetLength(1); j++)
                {
                    if (Vector2.Distance(playerPos, TileArray[i, j].Pos) < 2 && LastPlayerLinedUpTile != playerPos)
                    {
                        LastPlayerLinedUpTile = TileArray[i, j].Pos;
                        Path = CheckAvailablePaths(LastLinedUpTile, LastPlayerLinedUpTile);
                        break;
                    }
                }
            }
        }

        public Node DetermineRouteIfSameCost(Node[] adjNodes, Vector2 playerPos)
        {

            Node result = adjNodes[0];

            //Avgör bästa noden att välja när två nodes har samma avstånd till spelaren
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
