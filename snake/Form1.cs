using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
static class Constants
{
    public const int headPos = 0;
    public const int gameTimerInterval = 500;
}

class snakeCircleBody
{
    public int X { get; set; }
    public int Y { get; set; }
    public snakeCircleBody()
    {
        X = 0;
        Y = 0;
    }
}

class initialSettings
{
    public static int Width { get; set; }
    public static int Height { get; set; }
    public static string direction;
    public initialSettings()
    {
        Width = 15;
        Height = 15;
        direction = "LEFT";
    }
}

namespace snake
{
    public partial class Form1 : Form
    {
        private List<snakeCircleBody> Snake = new List<snakeCircleBody>();
        private snakeCircleBody food = new snakeCircleBody();
        int maxWidth;
        int maxHeight;
        Random rand = new Random();

        public Form1()
        {
            InitializeComponent();
            new initialSettings();
        }
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left && initialSettings.direction != "RIGHT")
            {
                initialSettings.direction = "LEFT";
            }
            if (e.KeyCode == Keys.Right && initialSettings.direction != "LEFT")
            {
                initialSettings.direction = "RIGHT";
            }
            if (e.KeyCode == Keys.Up && initialSettings.direction != "DOWN")
            {
                initialSettings.direction = "UP";
            }
            if (e.KeyCode == Keys.Down && initialSettings.direction != "UP")
            {
                initialSettings.direction = "DOWN";
            }
        }
        private void StartGame(object sender, EventArgs e)
        {
            RestartGame();
        }
        private void GameTimerEvent(object sender, EventArgs e)
        {
            checkFood();
            checkSnakeCollision();
            updateSnakePos();
            picCanvas.Invalidate();
        }

        private void updateSnakePos()
        {
            int bodyIndex = Snake.Count - 1;
            while (bodyIndex >= 0)
            {
                if (bodyIndex == Constants.headPos)
                {
                    updateSnakeHead();
                }
                else
                {
                    Snake[bodyIndex].X = Snake[bodyIndex - 1].X;
                    Snake[bodyIndex].Y = Snake[bodyIndex - 1].Y;
                }
                bodyIndex--;
            }

        }
        private void updateSnakeHead()
        {
            switch (initialSettings.direction)
            {
                case "LEFT":
                    Snake[Constants.headPos].X--;

                    if (Snake[Constants.headPos].X < 0)
                    {
                        gameOver();
                    }
                    break;
                case "RIGHT":
                    Snake[Constants.headPos].X++;

                    if (Snake[Constants.headPos].X > maxWidth)
                    {
                        gameOver();
                    }
                    break;
                case "DOWN":
                    Snake[Constants.headPos].Y++;

                    if (Snake[Constants.headPos].Y > maxHeight)
                    {
                        gameOver();
                    }
                    break;
                case "UP":
                    Snake[Constants.headPos].Y--;

                    if (Snake[Constants.headPos].Y < 0)
                    {
                        gameOver();
                    }
                    break;
                default:
                    break;
            }
        }
        private void drawSnake(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            for (int i = 0; i < Snake.Count; i++)
            {
                canvas.FillEllipse((i == Constants.headPos) ? Brushes.Black : Brushes.DarkGreen, new Rectangle(Snake[i].X * initialSettings.Width, Snake[i].Y * initialSettings.Height, initialSettings.Width, initialSettings.Height));
            }
            canvas.FillEllipse(Brushes.DarkRed, new Rectangle(food.X * initialSettings.Width, food.Y * initialSettings.Height, initialSettings.Width, initialSettings.Height));
        }

        private void RestartGame()
        {
            maxWidth = picCanvas.Width / initialSettings.Width - 1;
            maxHeight = picCanvas.Height / initialSettings.Height - 1;

            Snake.Clear();

            startButton.Enabled = false;
           
            snakeCircleBody head = new snakeCircleBody { X = rand.Next(2, maxWidth), Y = rand.Next(2, maxHeight)};
            Snake.Add(head); // adding the head part of the snake to the list

            for (int i = 0; i < 3; i++)
            {
                snakeCircleBody body = new snakeCircleBody();
                Snake.Add(body);
            }

            food = new snakeCircleBody { X = rand.Next(2, maxWidth), Y = rand.Next(2, maxHeight) };

            gameTimer.Start();
        }

        private void checkSnakeCollision()
        {
            for (int j = 1; j < Snake.Count; j++)
            {
                if (Snake[Constants.headPos].X == Snake[j].X && Snake[Constants.headPos].Y == Snake[j].Y)
                {
                    gameOver();
                }
            }
        }

        private void gameOver()
        {
            gameTimer.Interval = Constants.gameTimerInterval;
            gameTimer.Stop();
            startButton.Enabled = true;
        }
        private void checkFood()
        {
            if (Snake[Constants.headPos].X == food.X && Snake[Constants.headPos].Y == food.Y)
            {
                snakeCircleBody newBody = new snakeCircleBody
                {
                    X = Snake[Snake.Count - 1].X,
                    Y = Snake[Snake.Count - 1].Y
                };

                if(gameTimer.Interval / 2 == 0)
                {
                    gameTimer.Interval = 5;
                }
                else
                {
                    gameTimer.Interval = gameTimer.Interval / 2;
                }
                Snake.Add(newBody);

                food = new snakeCircleBody { X = rand.Next(2, maxWidth), Y = rand.Next(2, maxHeight) };
            }
        }
    }
}