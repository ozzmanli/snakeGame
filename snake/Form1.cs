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

namespace snake
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();
        int maxWidth;
        int maxHeight;
        Random rand = new Random();

        public Form1()
        {
            InitializeComponent();
            new setting();
        }
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left && setting.direction != "RIGHT")
            {
                setting.direction = "LEFT";
            }
            if (e.KeyCode == Keys.Right && setting.direction != "LEFT")
            {
                setting.direction = "RIGHT";
            }
            if (e.KeyCode == Keys.Up && setting.direction != "DOWN")
            {
                setting.direction = "UP";
            }
            if (e.KeyCode == Keys.Down && setting.direction != "UP")
            {
                setting.direction = "DOWN";
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
            switch (setting.direction)
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
                canvas.FillEllipse((i == Constants.headPos) ? Brushes.Black : Brushes.DarkGreen, new Rectangle(Snake[i].X * setting.Width, Snake[i].Y * setting.Height, setting.Width, setting.Height));
            }
            canvas.FillEllipse(Brushes.DarkRed, new Rectangle(food.X * setting.Width, food.Y * setting.Height, setting.Width, setting.Height));
        }

        private void RestartGame()
        {
            maxWidth = picCanvas.Width / setting.Width - 1;
            maxHeight = picCanvas.Height / setting.Height - 1;

            Snake.Clear();

            startButton.Enabled = false;
           
            Circle head = new Circle { X = rand.Next(2, maxWidth), Y = rand.Next(2, maxHeight)};
            Snake.Add(head); // adding the head part of the snake to the list

            for (int i = 0; i < 3; i++)
            {
                Circle body = new Circle();
                Snake.Add(body);
            }

            food = new Circle { X = rand.Next(2, maxWidth), Y = rand.Next(2, maxHeight) };

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
                Circle newBody = new Circle
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

                food = new Circle { X = rand.Next(2, maxWidth), Y = rand.Next(2, maxHeight) };
            }
        }
    }
}