using _09._02._20223_lect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _09._02._20223_lect
{
    public class Animator//класс отвечает за анимацию одного шарика
    {
        private Circle c;//собственно, сам шарик
        private Thread? t = null;
        public bool IsAlive => t == null || t.IsAlive;//открываем доступ к аналогичному свойству из потока
        public Size ContainerSize { get; set; }//передаем инфу о размере панельки
                                              

        public Animator(Size containerSize)
        {
            Random rnd = new Random();
            ContainerSize = containerSize;
            c = new Circle(50, rnd.Next(0, 10), ContainerSize.Height / 2 - 50 + rnd.Next(75));
        }

        public void Start()
        {
            t = new Thread(() =>//создаем поток
                                //лямбда содержит те действия, которые нам нужно выполнить
            {
                while (c.X + c.Diam < ContainerSize.Width)//пока шарик не дойдет до правого края
                {
                    Thread.Sleep(30);//ожидание, чтобы шарик находился здесь какое-то время (30 мс)
                    c.Move(1, 0);//сдвигаем вправо на 1 пиксель
                }
            });
            t.IsBackground = true;//поток является фоновым (прекращает работу с закрытием формы)
            t.Start();//запускаем поток на выполнение
        }

        public void PaintCircle(Graphics g)//метод отрисовки шарика на графиксе
                                           //добавили этот метод,т.к. метод отрисовки у нас был в классе Circle
                                           //а отрисовкой будет заниматься Painter
        {
            c.Paint(g);
        }
    }
}