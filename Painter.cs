namespace _09._02._20223_lect 
{ 
    public class Painter//класс, где отрисовываем сцену целиком
    {
        private object locker = new();//вспомогательный объект для синхронизации потоков
        private List<Animator> animators = new();//храним все аниматоры в списке
        private Size containerSize;//инфа о размере панели
        private Thread t;
        private Graphics mainGraphics;//основной графикс, где мы будем рисовать
        private BufferedGraphics bg;//буфферизованный графикс
        private bool isAlive;//переменная, говорящая о том, работает ли поток

        private volatile int objectsPainted = 0;
        public Thread PainterThread => t;
        public Graphics MainGraphics//публичное свойство для mainGraphics
        {
            get => mainGraphics;
            set
            {
                lock (locker)//lock должен быть везде, где мы обращаемся к общим данным
                {
                    mainGraphics = value;
                    ContainerSize = mainGraphics.VisibleClipBounds.Size.ToSize();//узнаем размер панели из mainGraphics
                                                                                //он float, поэтому мы с помощью ToSize округляем
                    bg = BufferedGraphicsManager.Current.Allocate(
                        mainGraphics, new Rectangle(new Point(0, 0), ContainerSize)//mainGraphics - область,куда будем отрисовывать из буффера
                                                                                   //второй параметр - позиция и размеры буфферизованного изображения
                    );
                    objectsPainted = 0;//если меняем графикс, то сбрасываем счетчик
                }
            }
        }

        public Size ContainerSize
        {
            get => containerSize;
            set
            {
                containerSize = value;
                foreach (var animator in animators)
                {
                    animator.ContainerSize = ContainerSize;//каждому аниматору присваиваем ContainerSize
                              //это надо для того, чтобы шарики реагировали на изменение размеров панели
                }
            }
        }

        public Painter(Graphics mainGraphics)
        {
            MainGraphics = mainGraphics;//присваиваем значение свойству mainGraphics
        }

        public void AddNew()//метод добавления нового аниматора (т.е. шарика)
                            //будет вызываться при нажатии кнопки Старт
        {
            var a = new Animator(ContainerSize);//новый аниматор
            animators.Add(a);//добавляем в список
            a.Start();//запускаем на выполнение
        }

        public void Start()
        {
            isAlive = true;
            t = new Thread(() =>//запускаем поток
            {
                try//ставим try чтобы программа не падала при закрытии (исчезает mainGraphics)
                {
                    while (isAlive)//добавляем цикл, чтобы все действия выполнялись регулярно
                    {
                        animators.RemoveAll(it => !it.IsAlive);//удаляем все элементы, которые уже отработали 
                                                               //в нашем случае - доехали до грницы панели
                        lock (locker)//надо синхронизировать потоки, так как mainGraphics - это общие данные
                        {
                            if (PaintOnBuffer())//если в буффере отрисованы все объекты
                            {
                                bg.Render(MainGraphics);//рендерим(отрисовываем ) изображение
                                                        //из буфферизованной графики на mainGraphics
                            }
                        }
                        if (isAlive) Thread.Sleep(30);//останавливаем поток, 
                                                   //чтобы кадр был на экране некоторое время
                                                   //Поделив 1сек(1000мс) на время сна получим кол-во кадров 
                    }
                }
                catch (ArgumentException e) { }
            });
            t.IsBackground = true;//поток фоновый
            t.Start();//запускаем поток
        }

        public void Stop()//метод остановки потока
        {
            isAlive = false;//меняем "флаг"
            t.Interrupt();//прерываем поток,чтобы можно было прервать даже во время сна
        }

        private bool PaintOnBuffer()//метод, проверяющий все ли элементы отрисовались в буффере
        {
            objectsPainted = 0;//заводим счетчик отрисованных объектов
            var objectsCount = animators.Count;//находим количество аниматоров в списке
            bg.Graphics.Clear(Color.White);//очищаем bufferedGraphics
            foreach (var animator in animators)//пробегаемся по всем аниматорам в списке
            {
                animator.PaintCircle(bg.Graphics);//отрисовываем 
                objectsPainted++;//увеличиваем счетчик
            }

            return objectsPainted == objectsCount;//сравниваем кол-во отрисованных с кол-вом в списке
        }
    }
}