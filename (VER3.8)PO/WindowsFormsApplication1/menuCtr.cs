using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.DirectX.AudioVideoPlayback;
using Microsoft.DirectX.DirectSound;
using Microsoft.DirectX;

namespace WindowsFormsApplication1
{
    class menuCtr
    {
        /*##################################
         * title : menu제어 클래스
         ##################################*/
        public string menuMode = "main";
        public menuCtr() {
           
        }
         /*##################################################
         * Name : close
         * info : 폼 닫기
         * 작성자 : 김민영 2016-01-05
        ####################################################*/
        public void close()
        {
            DialogResult result = MessageBox.Show("정말로 나가시겠습니까?", "나가기", MessageBoxButtons.YesNo);
            if (true == Form1.arduSerialPort.IsOpen) //포트가 열려있다면
            {
                Form1.arduSerialPort.Close();        //포트를 닫는다
            }
            if (result == DialogResult.Yes)
            {
                Application.ExitThread();
                Environment.Exit(0);
            }
        }

              
        //연습모드 
        public void trainning(Form mainForm )
        {
            trainningForm trFrom = new trainningForm();
            trFrom.Owner = mainForm;
            mainForm.Hide(); 
            trFrom.ShowDialog();
      
        }




    }
}
