using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeAI
{
    class AI
    {
        private const int VISION = 5;
        public List<PictureBox> snake;
        bool flag = false;

        public void MoveNext(SnakeForm field)
        {
            PictureBox head = field.snake.ElementAt(snake.Count - 1);
            List<PictureBox> upright = field.food.Where(f => f.Location.X >= head.Location.X && f.Location.X <= (head.Location.X + VISION * SnakeForm.CELL_SIZE) &&
                                           f.Location.Y < head.Location.Y && f.Location.Y >= (head.Location.Y - VISION * SnakeForm.CELL_SIZE)).ToList();
            List<PictureBox> downright = field.food.Where(f => f.Location.X > head.Location.X && f.Location.X <= (head.Location.X + VISION * SnakeForm.CELL_SIZE) &&
                                           f.Location.Y >= head.Location.Y && f.Location.Y <= (head.Location.Y + VISION * SnakeForm.CELL_SIZE)).ToList();
            List<PictureBox> downleft = field.food.Where(f => f.Location.X <= head.Location.X && f.Location.X >= (head.Location.X - VISION * SnakeForm.CELL_SIZE) &&
                                           f.Location.Y > head.Location.Y && f.Location.Y <= (head.Location.Y + VISION * SnakeForm.CELL_SIZE)).ToList();
            List<PictureBox> upleft = field.food.Where(f => f.Location.X < head.Location.X && f.Location.X >= (head.Location.X - VISION * SnakeForm.CELL_SIZE) &&
                                           f.Location.Y <= head.Location.Y && f.Location.Y >= (head.Location.Y - VISION * SnakeForm.CELL_SIZE)).ToList();

            //MessageBox.Show($"Up right: {upright.Count()}\n Down right: {downright.Count()}\n Down left: {downleft.Count()}\n Up left: {upleft.Count()}");
            if (!flag)
            {
                field.UpdateSnakeLocation("d");
                flag = true;
            }
            else
            {
                field.UpdateSnakeLocation("r");
                flag = false;
            }
        }
    }
}
