using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan
{
    internal class InputHandler
    {
        private KeyboardState currentPressedKey, wasPressedKey;
        private Keys lastKeyPressed;

        public InputHandler()
        {
        }

        //Berättar vilken tangent som trycks ner
        public KeyboardState GetState()
        {
            wasPressedKey = currentPressedKey;
            currentPressedKey = Keyboard.GetState();
            return currentPressedKey;
        }
        //Försäkrar programmet att man måste släppa tangenten och trycka igen för att något annat ska hända
        public bool HasBeenPressed(Keys key)
        {
            return currentPressedKey.IsKeyDown(key) && !wasPressedKey.IsKeyDown(key);
        }

        //Väljer riktning baserat på sista tryckningen
        public int LastTurn(int currentDirection)
        {
            
            int result;
            switch(lastKeyPressed)
            {
                case Keys.Up:
                    result = 0;
                    break;
                case Keys.Right:
                    result = 1;
                    break;
                case Keys.Down:
                    result = 2;
                    break;
                case Keys.Left:
                    result = 3;
                    break;
                default:
                    result = currentDirection;
                    break;
            }

            return result;
        }

        //Sätter ett värde på sist tryckta knappen
        public void SetLastKey(Keys key)
        {
            lastKeyPressed = key;
            Debug.WriteLine(lastKeyPressed.ToString());
        }
    }
}
