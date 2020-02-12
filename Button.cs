using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Input;


namespace ConsoleApp2
{
    class Button
    {
        private Text text;
        private bool inScope;
        private Vector2 position;
        private Vector2 size;

        public Button(Vector2 position, Vector2 size, bool inScope)
        {
           this.position = position;
           this.size = size;
           this.inScope = inScope;
        }
        public void MouseLocation()
        {
            if ((Mouse.GetCursorState().X > position.X) && (Mouse.GetCursorState().X < position.X + size.X)
                 && ((Mouse.GetCursorState().Y < position.Y) && (Mouse.GetCursorState().Y >  position.Y - size.Y)))
            {
                this.inScope = true;
            }
            else
            {
                this.inScope = false;
            }
            Console.WriteLine("Mouse :" + Mouse.GetCursorState());
            Console.WriteLine("Mouse :" + Mouse.GetState());
            Console.WriteLine("Position :" + position);
        }
        public void BtnClick() { }
        public void BtnHover() { }
        public void BtnDown() { }
        public void BtnUp() { }
    }
}
