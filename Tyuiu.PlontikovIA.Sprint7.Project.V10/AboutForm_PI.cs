using System.Drawing;
using System.Windows.Forms;

namespace Tyuiu.PlontikovPI.Sprint7.Project.V10
{
    public class AboutForm_PI : Form
    {
        public AboutForm_PI()
        {
            Text = "О программе — Tyuiu.PlontikovPI.Sprint7";
            Size = new Size(560, 380);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            var labelAbout_PI = new Label
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(16),
                Text =
                    "Приложение «Заказы»\r\n\r\n" +
                    "Решение: Tyuiu.PlontikovPI.Sprint7\r\n" +
                    "Проект: Tyuiu.PlontikovPI.Sprint7.Project.V10\r\n" +
                    "Вариант: 10\r\n" +
                    "Инициалы: PI\r\n\r\n" +
                    "Назначение: учёт заказов, CSV-хранение, поиск, фильтрация, " +
                    "статистика и гистограмма.\r\n\r\n" +
                    "Технологии: C#, Windows Forms, библиотека классов, unit-тесты."
            };

            Controls.Add(labelAbout_PI);
        }
    }
}