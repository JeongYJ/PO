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
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.DirectX.AudioVideoPlayback;
using Microsoft.DirectX.DirectSound;
using Microsoft.DirectX;

namespace WindowsFormsApplication1
{

    class control
    {

        /*
    ★
    임시로 비프음 이용해서 테스트 하기 위해 넣어놓음
    */
        
         [DllImport("KERNEL32.DLL")]
         extern public static void Beep(int freq, int dur);
         

        public int[] octave = new int[] { 55, 75, 96 };
        public int ocIndex = 1;
        public int trnIndex = 0;//트레이닝 모드시 누른 순서의 인덱스
        public int front = 0;

        public int base_x = 77;
        public int max_page = 5;


        public double time = 0.5;   // 현재 음표의 박자    (2분음표 2, 4분음표 1, 8분음표 0.5, 16분음표 0.25)
        public double time_sum = 0; // 한 마디안에 누적되어가는 박자 총합 (4/4 박자 기준, time_sum == 4일때 마디선이 그려진다.)

        // 한 마디를 크게 보여줄 때 필요한 변수들
        public int[] octave_madi = new int[] { 16, 116, 210 };



        public music now_music;

        System.Drawing.Image[] tempo = { Properties.Resources._16_s_note_orig, Properties.Resources._8_note_orig, Properties.Resources._8_note_orig };

        /*
        2016.01.25 정유진
        basic[박자,박자 거꾸로]
        basicIndex로 박자 조절하고 0,2,/1,3로 orig,rev 변경해줌
        */

        public int basicIndex = 1;
        public System.Drawing.Image[,] basic =
        {
            {Properties.Resources._16_note_orig,Properties.Resources._16_note_rev,Properties.Resources._16_s_note_orig,Properties.Resources._16_s_note_rev},
            {Properties.Resources._8_note_orig, Properties.Resources._8_note_rev,Properties.Resources._8_s_note_orig,Properties.Resources._8_s_note_rev},
            {Properties.Resources._4_note_orig, Properties.Resources._4_note_rev,Properties.Resources._4_s_note_orig,Properties.Resources._4_s_note_rev},
            {Properties.Resources._2_note_orig, Properties.Resources._2_note_rev,Properties.Resources._2_s_note_orig,Properties.Resources._2_s_note_rev}
        };

       System.Drawing.Image[] trainning = { Properties.Resources.g_8_note_rev, Properties.Resources.g_8_note_rev, Properties.Resources.g_8_note_rev };

        /*s
        [0][] = 높은옥타브
        [1][] = 일반옥타브
        [2][] = 낮은옥타브
        */
        public int[] beepLength = {1200, 600, 300, 200};
        /*
        public int[,] SampleSound = new int[,] {
              {523,587,659,698,784,880,988,1075,        // 높은 도(1075) 수정해야됨
             554,622,740,830,932},
            {261,293,329,349,392,440,494,523,
             277,311,370,415,466},
            {130,146,164,174,196,220,247,261,
             138,155,185,207,233}

        };
        */
        public SoundPlayer[,] SampleSound = {
              {new SoundPlayer("do_up.wav"),new SoundPlayer("le_up.wav"),new SoundPlayer("mi_up.wav"),new SoundPlayer("pa_up.wav"),new SoundPlayer("sol_up.wav"),new SoundPlayer("la_up.wav"),new SoundPlayer("si_up.wav"),new SoundPlayer("do_up_up.wav"),        // 높은 도(1075) 수정해야됨
              new SoundPlayer("do_up_s.wav"),new SoundPlayer("le_up_s.wav"),new SoundPlayer("pa_up_s.wav"),new SoundPlayer("sol_up_s.wav"),new SoundPlayer("la_up_s.wav")},
              {new SoundPlayer("do_orig.wav"),new SoundPlayer("le_orig.wav"),new SoundPlayer("mi_orig.wav"),new SoundPlayer("pa_orig.wav"),new SoundPlayer("sol_orig.wav"),new SoundPlayer("la_orig.wav"),new SoundPlayer("si_orig.wav"),new SoundPlayer("do_up.wav"),        // 높은 도(1075) 수정해야됨
              new SoundPlayer("do_orig_s.wav"),new SoundPlayer("le_orig_s.wav"),new SoundPlayer("pa_orig_s.wav"),new SoundPlayer("sol_orig_s.wav"),new SoundPlayer("la_orig_s.wav")},
              {new SoundPlayer("do_down.wav"),new SoundPlayer("le_down.wav"),new SoundPlayer("mi_down.wav"),new SoundPlayer("pa_down.wav"),new SoundPlayer("sol_down.wav"),new SoundPlayer("la_down.wav"),new SoundPlayer("si_down.wav"),new SoundPlayer("do_orig.wav"),        // 높은 도(1075) 수정해야됨
              new SoundPlayer("do_down_s.wav"),new SoundPlayer("le_down_s.wav"),new SoundPlayer("pa_down_s.wav"),new SoundPlayer("sol_down_s.wav"),new SoundPlayer("la_down_s.wav")}
         };
          
        /*
        public Audio[,] SampleSound = {
             {new Audio("do_up.w,av"),new Audio("le_up.wav"),new Audio("mi_up.wav"),new Audio("pa_up.wav"),new Audio("sol_up.wav"),new Audio("la_up.wav"),new Audio("si_up.wav"),new Audio("do_up_up.wav"),        // 높은 도(1075) 수정해야됨
             new Audio("do_up_s.wav"),new Audio("le_up_s.wav"),new Audio("pa_up_s.wav"),new Audio("sol_up_s.wav"),new Audio("la_up_s.wav")},
             {new Audio("do_orig.wav"),new Audio("le_orig.wav"),new Audio("mi_orig.wav"),new Audio("pa_orig.wav"),new Audio("sol_orig.wav"),new Audio("la_orig.wav"),new Audio("si_orig.wav"),new Audio("do_up.wav"),        // 높은 도(1075) 수정해야됨
             new Audio("do_orig_s.wav"),new Audio("le_orig_s.wav"),new Audio("pa_orig_s.wav"),new Audio("sol_orig_s.wav"),new Audio("la_orig_s.wav")},
             {new Audio("do_down.wav"),new Audio("le_down.wav"),new Audio("mi_down.wav"),new Audio("pa_down.wav"),new Audio("sol_down.wav"),new Audio("la_down.wav"),new Audio("si_down.wav"),new Audio("do_orig.wav"),        // 높은 도(1075) 수정해야됨
             new Audio("do_down_s.wav"),new Audio("le_down_s.wav"),new Audio("pa_down_s.wav"),new Audio("sol_down_s.wav"),new Audio("la_down_s.wav")}
        };
        */


        public control()
        {
            //악보 생성 -> 배씨 추가
            now_music = new music("", max_page);//제목""인 page장 악보생성

        }
        ~control() { }


        // 20160117 김민영
        // 계이름 영어로 변경 도레미파솔라시도 -> C , D , E , F , G , A , B , HC
        // 계이름 영어로 변경 샵 도레 파솔라 ->  SC, SD, SF,SG,SA


        public PictureBox madi_line_drawing(int x, int y)
        {
            try
            {
                PictureBox mb = new PictureBox();
                mb.Size = new System.Drawing.Size(5, 25);
                mb.BackgroundImageLayout = ImageLayout.None;
                mb.SizeMode = PictureBoxSizeMode.CenterImage;
                mb.BackColor = Color.Transparent;
                mb.Location = new Point(x, y);
                mb.Image = Properties.Resources.madi_line;

                return mb;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
        }
        //음표 picturebox 생성 및 위치 지정해서 그려넣기
        public PictureBox note_drawing(int x, int y, System.Drawing.Image note)
        {
            try
            {
                PictureBox mb = new PictureBox();
                mb.Size = new System.Drawing.Size(20, 30);
                mb.BackgroundImageLayout = ImageLayout.Stretch;
                mb.SizeMode = PictureBoxSizeMode.StretchImage;
                mb.BackColor = Color.Transparent;
                mb.Location = new Point(x, y);
                mb.Image = note;

                return mb;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
        }

        /* 2016년 1월 12일 임유진 
           마디 확대 기능*/
        public PictureBox note_drawingInMadi(int x, int y, System.Drawing.Image note)
        {
            try
            {
                PictureBox mb = new PictureBox();
                mb.Size = new System.Drawing.Size(58, 91);//사이즈 변경
                mb.BackgroundImageLayout = ImageLayout.Stretch;
                mb.SizeMode = PictureBoxSizeMode.StretchImage;
                mb.BackColor = Color.Transparent;
                mb.Location = new Point(x, y);
                mb.Image = note;
                return mb;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
        }


        
        //soundCheck->이름 변경 근데 이 함수 노트 클래스에 넣음 좋을듯 
        public void note_sound(int octave, int note)
        {
            //2016-01-17 김민영
            //switch 구문 변경
            //2016-01-24 정유진 //wav 파일로 변경
            SampleSound[octave, note].Play();
           // Beep(SampleSound[octave, note], beepLength[2]);
        }

        //############## 배씨 추가 16.01.19 ##############
        public void play(int noteind)
        {
            //2016-01-24 정유진 //mp3 파일로 변경
            int now_page = now_music.smind;//현재 페이지
            int octave_ = now_music.sm[now_page].note_arr[noteind].ocIndex;
            int note_ = (int)now_music.sm[now_page].note_arr[noteind].nt;
            int note_length = (int)now_music.sm[now_page].note_arr[noteind].length;
           // MessageBox.Show(octave_.ToString()+"note"+ now_music.sm[now_page].note_arr[noteind].length.ToString());
            SampleSound[octave_, note_].Play();
            //Beep(SampleSound[octave_, note_], beepLength[note_length/2]);
            Delay(beepLength[(int)Math.Log(note_length,2)-1]);
            SampleSound[octave_, note_].Stop();

        }
        //###############################################


        public void trainning_play(int note_, int octave_,int length_)
        {
          //  Beep(SampleSound[octave_, note_], beepLength[2]);
            SampleSound[octave_, note_].Play();
            //MessageBox.Show(Math.Log(length_,2).ToString());
            Delay(beepLength[(int)Math.Log(length_, 2) - 1]);
            SampleSound[octave_, note_].Stop();
        }

        //##################### 임유진 ###########################
        //음표 마디에 그리기
        public void note_locationInMadi(ntValue nt, Panel panel)
        {
            int y = nt.getLocationMadi();
            int _y = 0;

                if (now_music.sm[now_music.smind].count_madi >= 7)
                {
                    panel.Controls.Clear();
                    panel.BackgroundImage = Properties.Resources.emptymadi;
                    now_music.sm[now_music.smind]._x_madi = 15;
                    now_music.sm[now_music.smind].count_madi = 0;
                }

                // System.Drawing.Image[] tempo = basic[basicIndex,0];
                _y = octave_madi[ocIndex];
                
                //높은 시,도 거꾸로 된 음표 그리기
          
                //20160126 정유진 샾 추가 
                //높은 시,도 거꾸로 된 음표 그리기
                if ((ocIndex == 1 && (y == -27 || y == -41)) || ocIndex == 0) //옥타브==1 && 시or도
                {
                    if (nt == ntValue.SA || nt == ntValue.SD || nt == ntValue.SG || nt == ntValue.SF || nt == ntValue.SC)
                        panel.Controls.Add(note_drawingInMadi(now_music.sm[now_music.smind]._x_madi, _y + y + 50, basic[basicIndex, 3]));
                    else
                        panel.Controls.Add(note_drawingInMadi(now_music.sm[now_music.smind]._x_madi, _y + y + 50, basic[basicIndex, 1]));
                    //y축 위치 + 15(거꾸로 이므로 위치 맞춤)
                }
                else
                {
                    if (nt == ntValue.SA || nt == ntValue.SD || nt == ntValue.SG || nt == ntValue.SF || nt == ntValue.SC)
                        panel.Controls.Add(note_drawingInMadi(now_music.sm[now_music.smind]._x_madi, _y + y, basic[basicIndex, 2]));
                    else
                        panel.Controls.Add(note_drawingInMadi(now_music.sm[now_music.smind]._x_madi, _y + y, basic[basicIndex, 0]));
                }

                now_music.sm[now_music.smind]._x_madi += 60; //옆으로 계속 이동하면서 음표그리기
                now_music.sm[now_music.smind].count_madi++;
        }

        public void note_location(ntValue nt, Panel panel)
        {
            //기미녕이 한 부분  2016-01-17
            int y = nt.getLocation(); //위치 매칭
            int _y = 0;
            
            /*
            2016.01.26 정유진
            템포 이미지 안씀
            전부basic[,]으로 변경함.
            */
            // System.Drawing.Image[] tempo = this.tempo;
            _y = octave[ocIndex] + now_music.sm[now_music.smind].crrLine;
            int madi_y = octave[1] + now_music.sm[now_music.smind].crrLine;
            if (time_sum == 4)
            {
                panel.Controls.Add(madi_line_drawing(now_music.sm[now_music.smind]._x + 10, 6 + madi_y));       // time_sum 한 마디안에 들어갈 수 있는 박자가 모두 채워지면 마디선을 그린다.
                time_sum = 0;
                now_music.sm[now_music.smind]._x += 18;            //옆으로 계속 이동하면서 음표그리기
            }

            //20160126 정유진 샾 추가 
            //높은 시,도 거꾸로 된 음표 그리기
            if ((ocIndex == 1 && (y == -3 || y == -6)) || ocIndex == 0) //옥타브==1 && 시or도
            {
                if(nt == ntValue.SA || nt == ntValue.SD || nt == ntValue.SG || nt == ntValue.SF || nt == ntValue.SC)
                    panel.Controls.Add(note_drawing(now_music.sm[now_music.smind]._x, _y + y + 15, basic[basicIndex, 3]));
                else
                    panel.Controls.Add(note_drawing(now_music.sm[now_music.smind]._x, _y + y + 15, basic[basicIndex, 1]));
                //y축 위치 + 15(거꾸로 이므로 위치 맞춤)
            }
            else
            {
                if (nt == ntValue.SA || nt == ntValue.SD || nt == ntValue.SG || nt == ntValue.SF || nt == ntValue.SC)
                    panel.Controls.Add(note_drawing(now_music.sm[now_music.smind]._x, _y + y, basic[basicIndex, 2]));
                else
                    panel.Controls.Add(note_drawing(now_music.sm[now_music.smind]._x, _y + y, basic[basicIndex, 0]));
            }
            //tempSound 추가 -> 20160117 김민영 수정
            /* tempSound가 꼭 필요한가 흘러가는 데이터 발생될듯..
             * 어처피 사운드는 계이름만 알면 나오게 할 수있으니까 생략함
             */
            //basicIndex 0~3 => 16,8,4,2
            int tempBasicIndex = 0;



            if (basicIndex == 0)
                tempBasicIndex = 16;
            else if (basicIndex == 1)
                tempBasicIndex = 8;
            else if (basicIndex == 2)
                tempBasicIndex = 4;
            else
                tempBasicIndex = 2;

            now_music.sm[now_music.smind].add_note(nt, ocIndex, tempBasicIndex); //아직 박자가 없으므로 임시로 16분음표라 저장

            now_music.sm[now_music.smind]._x += 21; //옆으로 계속 이동하면서 음표그리기
            now_music.sm[now_music.smind].count++;

            time_sum += time;

            // 맨 첫줄은 4/4박자 기호때문에 그려지는 음표 개수가 적다.
            if (now_music.sm[now_music.smind].count >= 20 && now_music.sm[now_music.smind].crrLine == 0)
            {
                //그 줄 마지막부분에 마디선을 그려준다.
                panel.Controls.Add(madi_line_drawing(569, 6 + madi_y));
                time_sum = 0;

                //다음 줄로 이동
                now_music.sm[now_music.smind].count = 0;
                now_music.sm[now_music.smind].crrLine = now_music.sm[now_music.smind].crrLine + 55;
                now_music.sm[now_music.smind]._x = base_x;
            }
            else if (now_music.sm[now_music.smind].count >= 21)
            {
                //그 줄 마지막부분에 마디선을 그려준다.
                panel.Controls.Add(madi_line_drawing(569, 6 + madi_y));
                time_sum = 0;
                //다음 줄로 이동
                now_music.sm[now_music.smind].count = 0;
                now_music.sm[now_music.smind].crrLine = now_music.sm[now_music.smind].crrLine + 55;
                now_music.sm[now_music.smind]._x = base_x;
            }

        }


        //20160117 김민영
        //trainningmode시  추후수정하겠습니당
        public void TempoImgToTrainning()
        {
            for (int i = 0; i < trainning.Length; i++)
                tempo[i] = trainning[i];
        }
        public void TempoImgToBagic()
        {
            for (int i = 0; i < basic.Length; i++)
                tempo[i] = basic[basicIndex,i];
        }


        //######################### 배씨 수정 1.13 ######################################
        //###############################################################################



        public bool checkingMaxNode()
        {
            bool result = true;
            int line = now_music.sm[now_music.smind].crrLine / 55;
            if (line >= 12) result = false;
            return result;
        }
        //----- 키 이벤트 처리 끝 -----


        //----- 제목 이벤트 처리 시작 -----
        /*d
        정유진 
        2016-01-20 
        void -> int 형으로 변경, Button -> Image
        */
        public void NameSetting(TextBox tb_name, PictureBox bt_Image, TextBox[] tb_name_arr)
        {
            if (tb_name_arr[0].Enabled)
            {
                for (int i = 0; i < 5; i++) tb_name_arr[i].Enabled = false;
                for (int i = 0; i < max_page; i++) tb_name_arr[i].Text = tb_name.Text;

                bt_Image.Image = Properties.Resources.btn_title_orig;       // 제목 입력
                now_music.name = tb_name.Text;
                Console.WriteLine(tb_name_arr[0].Enabled);
             
            }
            else
            {
                tb_init(tb_name);
                for (int i = 0; i < 5; i++) tb_name_arr[i].Enabled = true;
                bt_Image.Image = Properties.Resources.btn_notitle_orig;     // 제목 입력 취소

            }

        }

        public void tb_name_KeyDown(Keys keyCode, TextBox tb_name, PictureBox bt_Name)
        {
            if (tb_name.Text == "제목을 입력하시오")
            {
                tb_name.Text = "";
            }

            if (keyCode == Keys.Enter)
            {
                tb_init(tb_name);
                tb_name.Enabled = false;
                bt_Name.Image = Properties.Resources.btn_notitle_orig;
            }
        }

        public void tb_init(TextBox tb_name)
        {
            if (tb_name.Text == "")
            {
                tb_name.Text = "제목을 입력하시오";
            }
        }

        //----- 제목 이벤트 처리 끝 -----

        //******************************************************
        //배씨가 한 부분

        //페이지 생성
        public bool pluspage(Panel panel)
        {
            bool temp = now_music.pluspage();
            if (temp == false)//생성할 수 없다고 반환이 되면
                MessageBox.Show("페이지를 생성할 수 없습니다.");

            return temp;
        }

        //페이지 이동
        public bool gopage(int page, Panel[] panel)
        {
            bool temp = now_music.gopage(page);
            if (temp == false)
                MessageBox.Show(page + "쪽으로 이동할 수 없습니다.");
            return temp;
        }
        //******************************************************

        private static DateTime Delay(int MS)
        {
            DateTime ThisMoment = DateTime.Now;
            TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);
            DateTime AfterWards = ThisMoment.Add(duration);

            while (AfterWards >= ThisMoment)
            {
                System.Windows.Forms.Application.DoEvents();
                ThisMoment = DateTime.Now;
            }

            return DateTime.Now;
        }
    }


}
