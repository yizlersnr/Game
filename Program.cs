using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Game game = new Game(1200, 600, "LearnOpenTK"))
            {
                //Run takes a double, which is how many frames per second it should strive to reach.
                //You can leave that out and it'll just update as fast as the hardware will allow it.
                //game.Run();
                game.MakeCurrent();
                game.Run();
            }

            
        }

    }
}

