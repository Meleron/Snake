using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeAI
{
    public partial class SnakeForm : Form
    {

        public const int FIELD_WIDTH = 30;
        public const int FIELD_HEIGHT = 30;
        public const int CELL_SIZE = 20;
        private const int AMOUNT_OF_SEGMENTS = 3;
        public List<PictureBox> snake;
        public List<PictureBox> food = new List<PictureBox>();
        AI snakeAI = new AI();
        private string currentDirection = "r";
        //private string directionOnNextStep = "r";
        private Queue<string> directionOnNextStep = new Queue<string>();
        private bool foodFound = false;

        public SnakeForm()
        {
            InitializeComponent();
            GenerateField();
            snakeAI.snake = new List<PictureBox>();
            snake = snakeAI.snake;
            SeedSnake(AMOUNT_OF_SEGMENTS);
            directionOnNextStep.Enqueue("r");
            this.Size = new Size(FIELD_WIDTH * CELL_SIZE + 17, FIELD_HEIGHT * CELL_SIZE + 40);
            moveTimer.Tick += MoveTimer_Tick;
            //moveTimer.Interval = 100;
            moveTimer.Interval = 200;
            moveTimer.Start();
            foodTimer.Tick += FoodTimer_Tick;
            //foodTimer.Interval = 1000;
            foodTimer.Interval = 900;
            foodTimer.Start();
            this.KeyDown += SnakeForm_KeyDown;
        }

        private void FoodTimer_Tick(object sender, EventArgs e)
        {
            GenerateFood();
        }

        private void MoveTimer_Tick(object sender, EventArgs e)
        {
            if (directionOnNextStep.Count != 1)
                directionOnNextStep.Dequeue();
            UpdateSnakeLocation(directionOnNextStep.Peek());
            //snakeAI.MoveNext(this);
            //moveTimer.Stop();
        }

        private void SnakeForm_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Up || e.KeyCode == Keys.Right || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left) && directionOnNextStep.Count >= 2)
                directionOnNextStep.Dequeue();
            switch (e.KeyCode)
            {
                //case Keys.Up:
                case Keys.W:
                    {
                        if (directionOnNextStep.Peek() == "u")
                            break;
                        if (currentDirection != "d")
                            directionOnNextStep.Enqueue("u");
                        break;
                    }
                //case Keys.Right:
                case Keys.D:
                    {
                        if (directionOnNextStep.Peek() == "r")
                            break;
                        if (currentDirection != "l")
                            directionOnNextStep.Enqueue("r");
                        break;
                    }
                //case Keys.Down:
                case Keys.S:
                    {
                        if (directionOnNextStep.Peek() == "d")
                            break;
                        if (currentDirection != "u")
                            directionOnNextStep.Enqueue("d");
                        break;
                    }
                //case Keys.Left:
                case Keys.A:
                    {
                        if (directionOnNextStep.Peek() == "l")
                            break;
                        if (currentDirection != "r")
                            directionOnNextStep.Enqueue("l");
                        break;
                    }
            }
        }

        public void GenerateFood()
        {
            if (food.Count < 5)
            {

                Random rnd = new Random();
                PictureBox newFood = new PictureBox();
                int foodX;
                int foodY;
                while (true)
                {
                    foodX = rnd.Next(FIELD_WIDTH) * CELL_SIZE;
                    foodY = rnd.Next(FIELD_HEIGHT) * CELL_SIZE;
                    if (!snake.Where(cell => cell.Location.X == foodX && cell.Location.Y == foodY).Any())
                        break;
                }
                newFood.BackColor = Color.Yellow;
                newFood.Location = new Point(foodX, foodY);
                newFood.Size = new Size(CELL_SIZE, CELL_SIZE);
                Controls.Add(newFood);
                food.Add(newFood);
            }
        }

        private void GenerateField()
        {
            for (int i = 0; i <= FIELD_WIDTH; i++)
            {
                PictureBox xLine = new PictureBox();
                xLine.BackColor = Color.Black;
                xLine.Location = new Point(i * CELL_SIZE, 0);
                xLine.Size = new Size(1, CELL_SIZE * FIELD_HEIGHT);
                Controls.Add(xLine);
            }
            for (int i = 0; i <= FIELD_HEIGHT; i++)
            {
                PictureBox yLine = new PictureBox();
                yLine.BackColor = Color.Black;
                yLine.Location = new Point(0, i * CELL_SIZE);
                yLine.Size = new Size(CELL_SIZE * FIELD_WIDTH, 1);
                Controls.Add(yLine);
            }
        }

        public void UpdateSnakeLocation(string direction)
        {
            PictureBox snakeHead = snake.ElementAt(snake.Count - 1);
            PictureBox snakeTail = new PictureBox();
            if (!foodFound)
                snakeTail = snake.ElementAt(0);
            else
            {
                snakeTail.Location = snake.ElementAt(0).Location;
                snakeTail.BackColor = snake.ElementAt(0).BackColor;
                snakeTail.Size = snake.ElementAt(0).Size;
                Controls.Add(snakeTail);
            }
            switch (direction)
            {
                case "u":
                    {
                        currentDirection = "u";
                        snakeTail.Location = new Point(snakeHead.Location.X, snakeHead.Location.Y - CELL_SIZE);
                        if (snakeTail.Location.Y < 0) snakeTail.Location = new Point(snakeTail.Location.X, (FIELD_HEIGHT - 1) * CELL_SIZE);
                        break;
                    }
                case "r":
                    {
                        currentDirection = "r";
                        snakeTail.Location = new Point(snakeHead.Location.X + CELL_SIZE, snakeHead.Location.Y);
                        if (snakeTail.Location.X > (FIELD_WIDTH - 1) * CELL_SIZE) snakeTail.Location = new Point(0, snakeTail.Location.Y);
                        break;
                    }
                case "d":
                    {
                        currentDirection = "d";
                        snakeTail.Location = new Point(snakeHead.Location.X, snakeHead.Location.Y + CELL_SIZE);
                        if (snakeTail.Location.Y > (FIELD_HEIGHT - 1) * CELL_SIZE) snakeTail.Location = new Point(snakeTail.Location.X, 0);
                        break;
                    }
                case "l":
                    {
                        currentDirection = "l";
                        snakeTail.Location = new Point(snakeHead.Location.X - CELL_SIZE, snakeHead.Location.Y);
                        if (snakeTail.Location.X < 0) snakeTail.Location = new Point((FIELD_WIDTH - 1) * CELL_SIZE, snakeTail.Location.Y);
                        break;
                    }
            }
            // Tail becomes hew head
            snake.Add(snakeTail);
            if (!foodFound)
                snake.RemoveAt(0);
            else
            {
                foodFound = false;
            }
            CheckHead(snakeTail);
        }

        private void CheckHead(PictureBox head)
        {
            foreach (PictureBox f in food)
            {
                if (head.Location == f.Location)
                {
                    foodFound = true;
                    f.Dispose();
                    food.Remove(f);
                    return;
                }
            }
            foreach (PictureBox s in snake)
            {
                if (head == s)
                    continue;
                if (head.Location == s.Location)
                {
                    GameOver(head);
                }
            }
            if (head.Location.X < 0 ||
                    head.Location.X > (FIELD_WIDTH - 1) * CELL_SIZE ||
                    head.Location.Y < 0 ||
                    head.Location.Y > (FIELD_HEIGHT - 1) * CELL_SIZE)
            {
                GameOver(head);

            }
        }

        private void SeedSnake(int amountOfSegments)
        {
            int startX = 5;
            int startY = 5;
            if (amountOfSegments > FIELD_WIDTH - 5)
                throw new Exception("Слишком много сегметов");
            for (int i = 0; i < amountOfSegments; i++)
            {
                snake.Add(new PictureBox
                {
                    BackColor = Color.Red,
                    Location = new Point(i * CELL_SIZE + startX * CELL_SIZE, 0 + startY * CELL_SIZE),
                    Size = new Size(CELL_SIZE, CELL_SIZE)
                });
                Controls.Add(snake[i]);
            }
        }

        private void GameOver(PictureBox head)
        {
            foreach (PictureBox pb in snake.Where(cell => cell.Location == head.Location))
                pb.BackColor = Color.Gray;
            snake.ElementAt(snake.Count - 2).BackColor = Color.LightGray;
            moveTimer.Stop();
            foodTimer.Stop();
            MessageBox.Show("Your score: " + (snake.Count - AMOUNT_OF_SEGMENTS));
            Environment.Exit(0);
        }
    }
}
