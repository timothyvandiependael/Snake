using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MijnSnake
{

    public enum Richting
    {
        Op,
        Neer,
        Links,
        Rechts
    };

    public class Player
    {
        public static int Punten { get; set; }
        public static int Score { get; set; }
        public static int Snelheid { get; set; }
        public static bool GameOver { get; set; }
        public static Richting SnakeRichting { get; set; }
        public static int Breedte { get; set; }
        public static int Hoogte { get; set; }

        public Player()
        {
            Punten = 100;
            Score = 0;
            Snelheid = 7;
            GameOver = false;
            SnakeRichting = Richting.Neer;
            Breedte = 15;
            Hoogte = 15;
        }
    }

}
