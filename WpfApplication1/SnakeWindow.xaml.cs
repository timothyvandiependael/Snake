using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MijnSnake
{
    /// <summary>
    /// Interaction logic for SnakeWindow.xaml
    /// </summary>
    public partial class SnakeWindow : Window
    {

        private List<Ellipse> Snake = new List<Ellipse>();
        private Ellipse food = new Ellipse();
        private int Xpos, Ypos, SnelheidsTeller;
        private System.Windows.Threading.DispatcherTimer Klok;

        public SnakeWindow()
        {
            InitializeComponent();
            for (var x = 1; x <= 22; x++)
            {
                SpeelVeld.ColumnDefinitions.Add(new ColumnDefinition());
                SpeelVeld.RowDefinitions.Add(new RowDefinition());
            }

            StartSpel();


        }

        private void StartSpel()
        {
            lblGameOver.Visibility = Visibility.Hidden;

            Snake.Clear();
            SpeelVeld.Children.Clear();

            new Player();

            SnelheidsTeller = 0;

            Klok = new System.Windows.Threading.DispatcherTimer();
            Klok.Tick += new EventHandler(Klok_Tik);
            Klok.Interval = new TimeSpan(0, 0, 0, 0, 1000 / Player.Snelheid);
            Klok.Start();



            Ellipse hoofd = new Ellipse() {Width = 15, Height = 15, Fill = new SolidColorBrush(Colors.Black)};
            Snake.Add(hoofd);

            ZetCirkel(hoofd,11,11);

            Xpos = 11;
            Ypos = 11;

            lblScore.Content = Player.Score.ToString();
            GenereerFood();

        }

        private void ZetCirkel(Ellipse pCirkel, int pXPos, int pYPos)
        {
            if (SpeelVeld.Children.Contains(pCirkel))
                SpeelVeld.Children.Remove(pCirkel);

            Grid.SetColumn(pCirkel, pXPos);
            Grid.SetRow(pCirkel, pYPos);
            SpeelVeld.Children.Add(pCirkel);
        }

        private void GenereerFood()
        {
            Random random = new Random();
            food = new Ellipse() {Width = 15, Height = 15, Fill = new SolidColorBrush(Colors.Red)};
            
            ZetCirkel(food,random.Next(1,22),random.Next(1,22));
        }



        private void Klok_Tik(object sender, EventArgs e)
        {
            if (Player.GameOver)
            {
                lblGameOver.Visibility = Visibility.Visible;
                if (Keyboard.IsKeyDown(Key.Enter))
                    StartSpel();
            }
            else
            {

                MovePlayer();
            }

        }

        private void MovePlayer()
        {
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {

                    Xpos = Grid.GetColumn(Snake[0]);
                    Ypos = Grid.GetRow(Snake[0]);

                    switch (Player.SnakeRichting)
                    {
                        case Richting.Rechts:
                            Xpos++;
                            break;
                        case Richting.Links:
                            Xpos--;
                            break;
                        case Richting.Op:
                            Ypos--;
                            break;
                        case Richting.Neer:
                            Ypos++;
                            break;
                    }

                    ZetCirkel(Snake[0], Xpos, Ypos);

                    if (Xpos < 1 || Xpos > 22 || Ypos < 1 || Ypos > 22)
                    {
                        Die();
                    }

                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Grid.GetColumn(Snake[i]) == Grid.GetColumn(Snake[j]) &&
                            Grid.GetRow(Snake[i]) == Grid.GetRow(Snake[j]))
                            Die();
                    }

                    if (Grid.GetColumn(Snake[0]) == Grid.GetColumn(food) && Grid.GetRow(Snake[0]) == Grid.GetRow(food))
                    {
                        Eat();
                    }

                }
                else
                {
                    ZetCirkel(Snake[i], Grid.GetColumn(Snake[i-1]), Grid.GetRow(Snake[i-1]));
                }
            }
        }

        private void Die()
        {
            Player.GameOver = true;
        }

        private void Eat()
        {
            Ellipse groei = new Ellipse() {Width=15, Height=15, Fill=new SolidColorBrush(Colors.Green)};
            
            ZetCirkel(groei, Grid.GetColumn(Snake[Snake.Count - 1]), Grid.GetRow(Snake[Snake.Count - 1]));

            Snake.Add(groei);

            Player.Score += Player.Punten;
            lblScore.Content = Player.Score.ToString();

            SnelheidsTeller++;

            if (SnelheidsTeller == 4)
            {
                SnelheidsTeller = 0;
                Player.Snelheid++;
                Player.Punten += 50;
                Klok.Interval = new TimeSpan(0, 0, 0, 0, 1000 / Player.Snelheid);
            }

            SpeelVeld.Children.Remove(food);
            GenereerFood();

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Right) && Player.SnakeRichting != Richting.Links)
                Player.SnakeRichting = Richting.Rechts;
            else if (Keyboard.IsKeyDown(Key.Left) && Player.SnakeRichting != Richting.Rechts)
                Player.SnakeRichting = Richting.Links;
            else if (Keyboard.IsKeyDown(Key.Up) && Player.SnakeRichting != Richting.Neer)
                Player.SnakeRichting = Richting.Op;
            else if (Keyboard.IsKeyDown(Key.Down) && Player.SnakeRichting != Richting.Op)
                Player.SnakeRichting = Richting.Neer;
        }

    }
}
