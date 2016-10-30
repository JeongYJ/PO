using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.DirectX.AudioVideoPlayback;
using Microsoft.DirectX.DirectSound;
using Microsoft.DirectX;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            this.ControlBox = false;
            this.Text = string.Empty;
            pic_study.BackgroundImageLayout = ImageLayout.Stretch;
            pic_study.BackgroundImage = Properties.Resources.note_2;
  
        }
        
        /*##################################################
         * info : 교육용 자료 상태바를 누를시에 스터디 폼을 움직이게 한다.
         * 작성자 : 임유진 2016-01-12
        ####################################################*/
        public Point downPoint = Point.Empty;

        private void studystateBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            downPoint = new Point(e.X, e.Y);
        }
        private void studystateBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (downPoint == Point.Empty)
            {
                return;
            }
            Point location = new Point(
                this.Left + e.X - downPoint.X,
                this.Top + e.Y - downPoint.Y);
            this.Location = location;
        }

        private void studystateBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            downPoint = Point.Empty;
        }


        /*---------- 상태바 움직임 끝---------*/
        /*
        정유진
        2016-01-20
        메뉴 음표, 계이름, 강약세기 교육용 자료  UI 수정
        */
        //화면종료 
        private void btn_closeStudy_Click(object sender, EventArgs e)
        {
            btn_closeStudy.Image = Properties.Resources.btn_closeform_push;
            btn_closeStudy.Image = Properties.Resources.btn_closeform_orig;
            this.Close();
        }
        private void btn_closeStudy_MouseHover(object sender, EventArgs e)
        {
            btn_closeStudy.Image = Properties.Resources.btn_closeform_push;
        }
        private void btn_closeStudy_MouseLeave(object sender, EventArgs e)
        {
            btn_closeStudy.Image = Properties.Resources.btn_closeform_orig;
        }

        //음표
        private void btn_note_Click(object sender, EventArgs e)
        {
            btn_note.Image = Properties.Resources.btn_s_note_push;
            btn_note.Image = Properties.Resources.btn_s_note_orig;
            pic_study.BackgroundImage = Properties.Resources.note_2;
        }
        private void btn_note_MouseHover(object sender, EventArgs e)
        {
            btn_note.Image = Properties.Resources.btn_s_note_push;
        }
        private void btn_note_MouseLeave(object sender, EventArgs e)
        {
            btn_note.Image = Properties.Resources.btn_s_note_orig;
        }

        //계이름
        private void btn_syllable_Click(object sender, EventArgs e)
        {
            btn_syllable.Image = Properties.Resources.btn_s_syllable_push;
            btn_syllable.Image = Properties.Resources.btn_s_syllable_orig;
            pic_study.BackgroundImage = Properties.Resources.syllablename_c;
        }
        private void btn_syllable_MouseHover(object sender, EventArgs e)
        {
            btn_syllable.Image = Properties.Resources.btn_s_syllable_push;
        }
        private void btn_syllable_MouseLeave(object sender, EventArgs e)
        {
            btn_syllable.Image = Properties.Resources.btn_s_syllable_orig;
        }

        //세기
        private void btn_strength_Click(object sender, EventArgs e)
        {
            btn_strength.Image = Properties.Resources.btn_s_strength_push;
            btn_strength.Image = Properties.Resources.btn_s_strength_orig;
            pic_study.BackgroundImage = Properties.Resources.strength;
        }
        private void btn_strength_MouseHover(object sender, EventArgs e)
        {
            btn_strength.Image = Properties.Resources.btn_s_strength_push;
        }
        private void btn_strength_MouseLeave(object sender, EventArgs e)
        {
            btn_strength.Image = Properties.Resources.btn_s_strength_orig;
        }
    }
}
