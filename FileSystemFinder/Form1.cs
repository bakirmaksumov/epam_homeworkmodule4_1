using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileSystemFinder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnSubmit.Click += (s, b) =>
            {
                if (!string.IsNullOrEmpty(txtSearchName.Text))
                {
                    StartInitalProcess(txtSearchName.Text);
                }
            };
        }

        private void StartInitalProcess(string filename)
        {
            var inputFile = new FileSystemVisitor(filename, new FileSystemMainProcess(), (info) => true);
            inputFile.Start += (s, b) =>
              {
                  txtMain.AppendText("Инициализировать\r\n");
              };
            
            inputFile.Finish += (s, b) =>
            {
                txtMain.AppendText("Заканчивать\r\n");
            };


            inputFile.FileFinded += (s, b) =>
            {
                txtMain.AppendText("Найденный файл: " + b.FindedItem.Name+"\r\n");
            };

            inputFile.DirectoryFinded += (s, b) =>
            {
                txtMain.AppendText("Найден каталог: " + b.FindedItem.Name + "\r\n");
                if (b.FindedItem.Name.Length == 4)
                {
                    b.ActionType = StatusEnum.StopSearch;
                }
            };

            inputFile.FilteredFileFinded += (s, b) =>
            {
                txtMain.AppendText("Создан отфильтрованный файл: " + b.FindedItem.Name + "\r\n");
            };

            inputFile.FilteredDirectoryFinded += (s, b) =>
            {
                txtMain.AppendText("Создан отфильтрованный каталог: " + b.FindedItem.Name + "\r\n");
                if (b.FindedItem.Name.Length == 4)
                    b.ActionType = StatusEnum.StopSearch;
            };

            foreach (var fileSysInfo in inputFile.GetFileSystemInfo())
            {
                txtMain.AppendText(fileSysInfo.ToString()+ "\r\n");

            }


        }
       
       
    }
}
