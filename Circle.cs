using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _09._02._20223_lect
{
    //класс, отвечающий за отдельно взятый шарик
    public class Circle
    {
        private Random r = new();//нужен для задания рандомного цвета
        private int diam;
        private int x, y;

        public int X => x;//открываем на получение координаты и диаметр
        public int Y => y;
        public int Diam => diam;
        public Color Color { get; set; }

        public Circle(int diam, int x, int y, Color color)//конструктор с созданием заданного цвета
        {
            this.diam = diam;
            this.x = x;
            this.y = y;
            this.Color = color;
        }

        public Circle(int diam, int x, int y)//конструктор с созданием рандомного цвета
        {
            this.diam = diam;
            this.x = x;
            this.y = y;
            this.Color = Color.FromArgb(r.Next(255), r.Next(255), r.Next(255));
        }

        public void Move(int dx, int dy)//метод, сдвигающий шарик на dx и dy
        {
            x += dx;
            y += dy;
        }

        public void Paint(Graphics g)//метод отрисовки шарика
        {
            var brush = new SolidBrush(Color);//кисточка цвета Color
            g.FillEllipse(brush, X, Y, Diam, Diam);//рисуем шар
        }
    }
}