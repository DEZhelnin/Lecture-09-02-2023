namespace _09._02._20223_lect
{ 
    //��� �����, ����� ��� ������ ���������� �� ����� �����������
    // � ��� ������� ����������� ��� ����� ���������� �� �����

    // �� ��������� ����� �������� ��������� �����, � ��� ��������� ������ ����� ���������� ������������
    public partial class Form1 : Form
    {
        private Painter p;//��� ��� Painter
        public Form1()
        {
            InitializeComponent();
            p = new Painter(mainPanel.CreateGraphics());//�������� � Painter ������� �� ��������
            p.Start();//��������� Painter
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            p.AddNew();//��� ����� �� ������ ��������� ����� �������� � Painter
        }

        private void mainPanel_Resize(object sender, EventArgs e)//���� �������� ������ ������
        {
            if (p != null)
            {
                p.MainGraphics = mainPanel.CreateGraphics();//������ �������
                                 // ��� ���� containerSize ���������� �������������
            }
        }
    }
}