namespace _09._02._20223_lect
{ 
    //нам нужно, чтобы все шарики рисовались на одном изображении
    // а эот готовое изображение уже будет выводиться на экран

    // за отрисовку будет отвечать отдельный поток, а все остальные потоки будут заниматься перемещением
    public partial class Form1 : Form
    {
        private Painter p;//это наш Painter
        public Form1()
        {
            InitializeComponent();
            p = new Painter(mainPanel.CreateGraphics());//передаем в Painter графикс из панельки
            p.Start();//запускаем Painter
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            p.AddNew();//при клике на кнопку добавляем новый аниматор в Painter
        }

        private void mainPanel_Resize(object sender, EventArgs e)//если меняется размер панели
        {
            if (p != null)
            {
                p.MainGraphics = mainPanel.CreateGraphics();//меняем графикс
                                 // при этом containerSize поменяется автоматически
            }
        }
    }
}