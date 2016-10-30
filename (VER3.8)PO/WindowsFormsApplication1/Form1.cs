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

    public partial class Form1 : Form
    {

        //******************************
        //배씨 추가
        public Panel[] p_music; //악보 패널
        public PictureBox[] pb_page; //악보 페이지 버튼

        //##################################
        //###### 배씨 추가 16.01.19 #######
        Thread thread;
        //################################

        File file = new File();

        public Image[,] img_page = new Image[,]
        {
            {
                Properties.Resources.btn_pageone_dis, Properties.Resources.btn_pagetwo_dis,
                Properties.Resources.btn_pagethree_dis, Properties.Resources.btn_pagefour_dis,
                Properties.Resources.btn_pagefive_dis
            },
            {
                Properties.Resources.btn_pageone_orig, Properties.Resources.btn_pagetwo_orig,
                Properties.Resources.btn_pagethree_orig, Properties.Resources.btn_pagefour_orig,
                Properties.Resources.btn_pagefive_orig
            },
            {
                Properties.Resources.btn_pageone_push, Properties.Resources.btn_pagetwo_push,
                Properties.Resources.btn_pagethree_push, Properties.Resources.btn_pagefour_push,
                Properties.Resources.btn_pagefive_push
            }
        };

        public Panel[] p_madi; //마디 패널

        //-->>버튼 이미지는 [3][5]로 만듬
        //1행 : 비활성화 / 2행 : 보통 / 3행 : 눌렸을 때
        public TextBox[] tb_name;
        //*********************************

        //keyDown 이벤트 두번 발생 방지 변수
        int keyDownflag = 0;
        //기미녕이 한부분
        control POctl = new control();
        menuCtr POmenu = new menuCtr();

        //STUDY 교육용 자료 폼
        Form2 study_form;

        trainningForm tf = new trainningForm();
        /*  ★달린건 아두이노 소스코드  */

        //★
        public static SerialPort arduSerialPort = new SerialPort("COM6", 9600, Parity.None, 8, StopBits.One);
        //시리얼 포트 생성

        public delegate void EventHandlerLocation(ntValue y, Panel panel);

       // public delegate void EventHandlerTrainning(ntValue y, int octave);
        public delegate void EventHandlerSound(int octave, int idx);
        public delegate void EventHandlercontroloctave();   // 옥타브 조절 이벤트 핸들러
        public delegate void EventHandlercontrolbasic();    // 박자 조절 이벤트 핸들러

        public EventHandlercontroloctave delegatenote_octaveup = null;
        public EventHandlercontroloctave delegatenote_octavedown = null;
        public EventHandlercontrolbasic delegatenote_basicup = null;
        public EventHandlercontrolbasic delegatenote_basicdown = null;

        public EventHandlerLocation delegatenote_location = null;
        public EventHandlerLocation delegatenote_locationMadi = null;
        public EventHandlerSound delegatenote_sound = null;
        //public EventHandlerTrainning delegate_trainning = null;
      
        public Form1()
        {
            try
            {
                InitializeComponent();
                this.ControlBox = false;
                this.Text = string.Empty;

                //************배씨 추가**************************

                //악보 5장 
                p_music = new Panel[5] {p_music1, p_music2, p_music3, p_music4, p_music5};
                tb_name = new TextBox[5] {tb_name1, tb_name2, tb_name3, tb_name4, tb_name5};
                p_madi = new Panel[5] {p_madi1, p_madi2_2, p_madi3, p_madi4_2, p_madi5_2};

                //######## 배씨 1.20 #################
                init();
                //####################################

                //cb_clef.SelectedIndex = 1;
                this.KeyPress += new KeyPressEventHandler(Form1_KeyPress_1);
                this.KeyDown += new System.Windows.Forms.KeyEventHandler(Form1_KeyDown);
                /* 김민영 이벤트 기능 직접정의*/
                for (int i = 0; i < tb_name.Length; i++)
                    this.tb_name[i].KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_name_KeyDown);


                arduSerialPort.Open(); // ★ 포트 오픈
                /* ★ 시리얼 포트 값 읽어들임 */
                arduSerialPort.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
                delegatenote_octaveup += new EventHandlercontroloctave(octave_up);
                delegatenote_octavedown += new EventHandlercontroloctave(octave_down);
                delegatenote_basicup += new EventHandlercontrolbasic(basicIndex_up);
                delegatenote_basicdown += new EventHandlercontrolbasic(basicIndex_down);
                delegatenote_location = new EventHandlerLocation(POctl.note_location);
                delegatenote_sound = new EventHandlerSound(POctl.note_sound);
                delegatenote_locationMadi = new EventHandlerLocation(POctl.note_locationInMadi);
               // delegate_trainning = new EventHandlerTrainning(tf.trainningmode);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        //############### 배씨 1.20 ##################
        // 생성자에서 일부 중복 사용되어서 빼냄 
        void init()
        {
            //나머지 숨기기
            for (int i = 1; i < 5; i++)
            {
                p_music[i].Hide();
                tb_name[i].Hide();
                p_madi[i].Hide();
                //->이미지 추가시 
                //pb_page[i].Image("disable이미지 추가");
            }

            //첫번째 장 보여주기
            p_music[0].Show();
            tb_name[0].Show();
            p_madi[0].Show();

            //**********************************************
            
            //현재 박자 보여주기
            current_basicIndex(POctl.basicIndex);
            //
            //현재 옥타브 보여주기
            current_ocIndex(POctl.ocIndex);
            //

            //############# 배씨추가 1.13 ##################
            pb_page = new PictureBox[5] { pb_page1, pb_page2, pb_page3, pb_page4, pb_page5 };
            for (int i = 0; i < 5; i++)
                pb_page[i].Image = img_page[0, i]; //모든 페이지 번호 비활성화로
            pb_page[POctl.now_music.smind].Image = img_page[2, POctl.now_music.smind]; //현재 페이지 클릭된 이미지로
            //##############################################


        }
        //##################################################

        /* ★ 시리얼 포트 값 읽어서 비교한다*/

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort sp = sender as SerialPort;

                if (sp.BytesToRead >= 0 && POctl.checkingMaxNode())
                {
                    byte[] buffer = new byte[2];
                    int count = sp.Read(buffer, 0, 2);

                    string num = Encoding.ASCII.GetString(buffer);

                    try
                    {
                        //  2016.01.24 정유진 -> 다중 음 낼때 이용 numCheck(int num); 

                        /*
                         * 2016.01.17 김민영 아두이노의 입력에 따라 캐스팅되도록 수정
                         * 참고 인덱스형식으로 매칭하여 받으므로 샵처리를 A B C가 아닌 9,10,11 이런식으로 변환해주세용
                         
                         */

                        ntValue notevalue = (ntValue)Enum.ToObject(typeof(ntValue), int.Parse(num) - 1);//계이름 판정

                        /* 조이스틱으로 조절하는 부분
                        switch(){
                        case :
                            // Invoke(this.delegatenote_octavedown, new object[] {});   // 옥타브올리기
                            // Invoke(this.delegatenote_octaveup, new object[] {});     // 옥타브내리기
                            // Invoke(this.delegatenote_basicdown, new object[] {});    // 박자 올리기
                            // Invoke(this.delegatenote_basicup, new object[] {});      // 박자 내리기
                        }
                        */
                        Invoke(this.delegatenote_location, new object[] { notevalue, p_music[POctl.now_music.smind] });//음표그리기
                        Invoke(this.delegatenote_locationMadi, new object[] { notevalue, p_madi[POctl.now_music.smind] });//마디음표그리기
                        Invoke(this.delegatenote_sound, new object[] { POctl.ocIndex, (int)notevalue });//소리 출력하기
                        // textBox1.Text = num;
                        
                        //Invoke(this.delegate_trainning,new object[] { notevalue, POctl.ocIndex});
                    }
                    catch
                    {

                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /* 일단 임시
        void numCheck(int num)
        {
           
            int temp = num;
            
            for (int i = 12; i > -1; i--)
            {
                if(temp == 0)
                   return;

                else if (temp >= Math.Pow(2, i))
                {
                        temp = temp - (int)Math.Pow(2, i);
                        ntValue notevalue = (ntValue)Enum.ToObject(typeof(ntValue), i);
                        Invoke(this.delegatenote_location, new object[] { notevalue, p_music[POctl.now_music.smind] });
                        Invoke(this.delegatenote_sound, new object[] { POctl.ocIndex, (int)notevalue });
                }
            }
        }
        */
        //    //기미녕이 한부분
        //    private void bt_Name_Click(object sender, EventArgs e)
        //    {
        ////        POctl.NameSetting(this.tb_name[POctl.now_music.smind], this.bt_Name, this.tb_name);
        //    }


        //    private void tb_name_KeyDown(object sender, KeyEventArgs e)
        //    {
        //        POctl.tb_name_KeyDown(e.KeyCode,this.tb_name[POctl.now_music.smind], this.btn_titleInput);
        //    }


        private void Form1_Load(object sender, EventArgs e)
        {
            //콤보박스에 음자리표 내용 집어넣기
        }

        private void load_clef(System.Drawing.Image clef)
        {
            //음자리표 그려넣기
            pb_clef01.Image = clef; pb_clef02.Image = clef;
            pb_clef03.Image = clef; pb_clef04.Image = clef;
            pb_clef05.Image = clef; pb_clef06.Image = clef;
            pb_clef07.Image = clef; pb_clef08.Image = clef;
            pb_clef09.Image = clef; pb_clef10.Image = clef;
            pb_clef11.Image = clef; pb_clef12.Image = clef;
        }



        // 제목 부분을 입력하다가 엔터를 눌렀을 때 이벤트
        private void tb_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                POctl.NameSetting(this.tb_name[POctl.now_music.smind], this.btn_titleInput, this.tb_name);
            }
        }
        // 방향기 눌렸을 때 발생하는 이벤트
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (keyDownflag == 0)
            {
                switch (e.KeyData)
                {
                    case Keys.Down:
                        octave_down();
                        break;
                    case Keys.Up:
                        octave_up();
                        break;
                    case Keys.Right:
                        basicIndex_up();
                        break;
                    case Keys.Left:
                        basicIndex_down();
                        break;
                }
            }
            keyDownflag = (++keyDownflag) % 2;
        }
        //기미녕이 한부분
        private void Form1_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!tb_name[0].Enabled && POctl.checkingMaxNode() && e.KeyChar.ToString() != "\0") // "제목입력" 상태의 버튼일 경우
            {
                try
                {
                    if (e.KeyChar == (char)13) return;

                    string input = e.KeyChar.ToString().ToUpper();
                    keyntValue key = (keyntValue)Enum.Parse(typeof(keyntValue), input);
                    ntValue notevalue = key.keyTontValue();

                    POctl.note_location(notevalue, p_music[POctl.now_music.smind]);
                    POctl.note_locationInMadi(notevalue, p_madi[POctl.now_music.smind]);
                    POctl.note_sound(POctl.ocIndex, (int)notevalue);
                }
                catch (Exception err)
                {
                    MessageBox.Show("해당키만 누르시오!" + err.Message);
                }
                e.KeyChar = '\0';
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.ExitThread();
            Environment.Exit(0);
        }

        //기미녕이 한부분
        private void octaveUp_Click(object sender, EventArgs e)
        {
            if (POctl.ocIndex > 0) POctl.ocIndex -= 1;
        }

        private void octaveDown_Click(object sender, EventArgs e)
        {
            if (POctl.ocIndex < 2) POctl.ocIndex += 1;
        }

        //*********************************************************
        //배씨가 한 부분


        //페이지 이동
        private void pagemove(int bt_num)
        {

            int before_page = POctl.now_music.smind + 1; //현재 페이지
            //MessageBox.Show("before : " + before_page + "   now : " + bt_num);

            if (before_page == bt_num)
            {
                MessageBox.Show("현재 페이지입니다.");
                return;
            }

            bool temp = POctl.gopage(bt_num, p_music); //bt_num으로의 페이지 이동 함수 호출
            if (temp) //만약 이동이 가능하다면
            {
                p_music[before_page - 1].Hide(); //이전 페이지 숨기기
                tb_name[before_page - 1].Hide();
                p_music[POctl.now_music.smind].Show(); //현재 페이지 출력
                tb_name[POctl.now_music.smind].Show();

                //############# 배씨추가 1.13 ##################
                p_madi[before_page - 1].Hide();
                p_madi[POctl.now_music.smind].Show();
                pb_page[before_page - 1].Image = img_page[1, before_page - 1];
                pb_page[POctl.now_music.smind].Image = img_page[2, POctl.now_music.smind];

                //##############################################
            }
        }

        //->나중에 악보가 다 찼을 때 다음으로 넘어가는 경우 호출할 함수임
        private void pluspage()
        {
            int before_page = POctl.now_music.smind + 1; //현재 페이지

            //############# 배씨수정 1.13 ##################
            bool temp = POctl.pluspage(p_madi1); //페이지 추가 함수 호출
            //##############################################

            if (temp) //생성 된다면
            {
                p_music[before_page - 1].Hide(); //이전 페이지 숨기기
                tb_name[before_page - 1].Hide();
                p_music[POctl.now_music.smind].Show(); //생성된 페이지 보여주기
                tb_name[POctl.now_music.smind].Show();

                //############# 배씨추가 1.13 ##################
                p_madi[before_page - 1].Hide();
                p_madi[POctl.now_music.smind].Show();
                pb_page[before_page - 1].Image = img_page[1, before_page - 1];
                pb_page[POctl.now_music.smind].Image = img_page[2, POctl.now_music.smind];

                //##############################################

                int num = POctl.now_music.max_smind + 1;
                MessageBox.Show(num + "페이지를 생성하였습니다.");
            }
            else
            {
                MessageBox.Show("페이지를 생성할 수 없습니다.");
            }
        }


        //페이지 버튼 클릭 함수
        private void pb_page1_Click(object sender, EventArgs e)
        {
            pagemove(1);
        } //페이지 1 클릭 시

        private void pb_page2_Click(object sender, EventArgs e)
        {
            pagemove(2);
        } //페이지 2 클릭 시

        private void pb_page3_Click(object sender, EventArgs e)
        {
            pagemove(3);
        } //페이지 3 클릭 시

        private void pb_page4_Click(object sender, EventArgs e)
        {
            pagemove(4);
        } //페이지 4 클릭 시

        private void pb_page5_Click(object sender, EventArgs e)
        {
            pagemove(5);
        } //페이지 5 클릭 시


        //페이지 추가 버튼 클릭시
        private void bt_pluspage_Click_1(object sender, EventArgs e)
        {
            //MessageBox.Show("페이지 추가 버튼 클릭");
            pluspage(); //페이지 추가    
        }



        /*************************************************************************************************************************
        2016.01.14 정유진
        이 아래부터 전부 버튼 -> 픽처박스로 변경하고 작업한 것
        픽처박스 이름은 모두 리소스 파일을 따라서 제작함
        헷갈리지 말라고 길게 주석 달아 놓으니 불필요해졌으면 제거 바람 
        ******************************************************************************************************************************/


        /*----------메뉴 기능 구현----------*/
        /*##################################################
          * info : 폼을 닫는다.
          * 작성자 : 김민영 2016-01-05
         ####################################################*/

        private void btn_closeform_Click(object sender, EventArgs e)
        {
            btn_closeform.Image = Properties.Resources.btn_closeform_push;
            btn_closeform.Image = Properties.Resources.btn_closeform_orig;

            POmenu.close();
            
        }

        /*##################################################
         * info : 연습모드로 들어가거나 원래모드로 돌아간다.
         * 작성자 : 김민영 2016-01-05
        ####################################################*/

        private void btn_practice_Click(object sender, EventArgs e)
        {
            POmenu.trainning(this);
        }

        private void btn_study_Click(object sender, EventArgs e)
        {
            if (study_form != null)
                study_form.Hide();
            study_form = new Form2();
            study_form.StartPosition = FormStartPosition.Manual;
            study_form.Location = new Point(this.Location.X + this.Size.Width, this.Location.Y);
            study_form.Show();
        }

        //메뉴 이벤트 ~ mouse click, mouse hover, mouse leave
        private void btn_menu_Click(object sender, EventArgs e)
        {

            btn_menu.Image = Properties.Resources.btn_menu_push;
            btn_menu.Image = Properties.Resources.btn_menu_orig;
            if (panel_menu.Visible == true)
                panel_menu.Visible = false;
            else
                panel_menu.Visible = true;
        }
        private void btn_menu_MouseHover(object sender, EventArgs e)
        {
            btn_menu.Image = Properties.Resources.btn_menu_push;
        }
        private void btn_menu_MouseLeave(object sender, EventArgs e)
        {
            btn_menu.Image = Properties.Resources.btn_menu_orig;
        }


        //메뉴닫기 이벤트 ~ mouse click, mouse hover, mouse leave
        private void btn_closemenu_Click(object sender, EventArgs e)
        {
            panel_menu.Visible = false;
            btn_closemenu.Image = Properties.Resources.btn_closemenu_push;
            btn_closemenu.Image = Properties.Resources.btn_closemenu_orig;
        }
        private void btn_closemenu_MouseHover(object sender, EventArgs e)
        {
            btn_closemenu.Image = Properties.Resources.btn_closemenu_push;
        }
        private void btn_closemenu_MouseLeave(object sender, EventArgs e)
        {
            btn_closemenu.Image = Properties.Resources.btn_closemenu_orig;
        }

        //폼닫기 이벤트 ~ mouse hover, mouse leave
        private void btn_closeform_MouseHover(object sender, EventArgs e)
        {
            btn_closeform.Image = Properties.Resources.btn_closeform_push;
        }
        private void btn_closeform_MouseLeave(object sender, EventArgs e)
        {
            btn_closeform.Image = Properties.Resources.btn_closeform_orig;
        }

        //새파일 이벤트 ~ mouse click, mouse hover, mouse leave
        private void btn_newfile_Click(object sender, EventArgs e)
        {
            btn_newfile.Image = Properties.Resources.btn_newfile_push;
            btn_newfile.Image = Properties.Resources.btn_newfile_orig;

            //############## 배씨 추가 1.20 #############
            newfile();
        }

        private void newfile()
        {
            int i = 0;
            Point p = new Point(40, 320);
            Point[] loc = { new Point(46, 70), new Point(46, 125), new Point(46, 180), new Point(46, 235), new Point(46, 290), new Point(47, 345), new Point(47, 399), new Point(47, 454), new Point(47, 509), new Point(46, 564), new Point(46, 619), new Point(46, 674) };
            for (i = 0; i < 5; i++)
            {
                p_music[i].Controls.Clear();//악보 초기화
                p_madi[i].Controls.Clear();//마디 초기화
                // 음표 추가
                for (int j = 0; j < 12; j++)
                {
                    PictureBox mb = new PictureBox();
                    mb.Size = new System.Drawing.Size(19, 49);
                    mb.BackgroundImageLayout = ImageLayout.Stretch;
                    mb.SizeMode = PictureBoxSizeMode.StretchImage;
                    mb.BackColor = Color.Transparent;
                    mb.Location = loc[j];
                    mb.Image = WindowsFormsApplication1.Properties.Resources.Treble_clef1;
                    p_music[i].Controls.Add(mb);
                }

                p_music[i].Controls.Add(tb_name[i]); //제목 입력 칸 붙이기
            }
            POctl = new control();//새 컨트롤 생성
            init();

        }




        private void btn_newfile_MouseHover(object sender, EventArgs e)
        {
            btn_newfile.Image = Properties.Resources.btn_newfile_push;
        }
        private void btn_newfile_MouseLeave(object sender, EventArgs e)
        {
            btn_newfile.Image = Properties.Resources.btn_newfile_orig;
        }


        //###########################################
        //###### 배씨 1.20 불러오기 함수 빼놓음 #####??왜빼놓음?
        void load()
        {
            Console.WriteLine("update");
            /*
             * 2016-01-19 김민영 
             * info  파일을 읽어온다. 추후 수정 필요....최대한 건들지 마시오~
             */
            if (MessageBox.Show("지금까지 작성한 악보가 사라집니다.\n 악보를 저장하시겠습니까?", "", MessageBoxButtons.YesNo)==DialogResult.Yes)
            {
                file.save(this.tb_name1.Text, POctl.now_music, p_music);
            }
            newfile();
            file.OpenFile();
            for (int i = 0; i < 5; i++)
            {
                tb_name[i].Text = file.title;
            }

            //토큰 분리 
            for (int idx = 0; idx < file.page; idx++)
            {
                char[] delimiterChars = { ',' };
                string[] words = file.music[idx].Split(delimiterChars);

                for (int i = 0; i < words.Length - 1; i += 3)
                {
                    ntValue nt = (ntValue)Enum.Parse(typeof(ntValue), words[i]);
                    int octave = int.Parse(words[i + 1]);//옥타브
                    int length = int.Parse(words[i + 2]);//길이
                    POctl.ocIndex = octave;
                    POctl.basicIndex = 4 - (int)Math.Log(length, 2.0);
                    Console.WriteLine("basicIndex = " + POctl.basicIndex);
                    POctl.note_location(nt, p_music[POctl.now_music.smind]);

                }
                if(idx!=file.page-1)
                    pluspage(); //페이지 추가    
            }
        }
        //###################################################


        //저장하기 이벤트 ~ mouse click, mouse hover, mouse leave
        private void btn_save_Click(object sender, EventArgs e)
        {
            btn_save.Image = Properties.Resources.btn_save_push;
            btn_save.Image = Properties.Resources.btn_save_orig;

            File file = new File();
            file.save(this.tb_name1.Text, POctl.now_music, p_music);

        }
        private void btn_save_MouseHover(object sender, EventArgs e)
        {
            btn_save.Image = Properties.Resources.btn_save_push;
        }
        private void btn_save_MouseLeave(object sender, EventArgs e)
        {
            btn_save.Image = Properties.Resources.btn_save_orig;
        }


        //#######################################################
        //############## 배현수  16.1.19 ########################
        private void start()
        {
            try
            {
                int now_page = POctl.now_music.smind;//현재 보고 있는 페이지만 음악으로 들려줌
                for (int i = 0; i < POctl.now_music.sm[now_page].noteind; i++)//현재 생성된 음표까지 음악출력
                {
                    POctl.play(i);//음표 해당 음 출력
                }
            }
            catch { }
        }
        //#######################################################

        //재생하기 이벤트 ~ mouse click, mouse hover, mouse leave
        private void btn_start_Click(object sender, EventArgs e)
        {
            btn_start.Image = Properties.Resources.btn_start_push;
            btn_start.Image = Properties.Resources.btn_start_orig;

            //#######################################################
            //############## 배현수  16.1.19 ########################
            thread = new Thread(start);
            thread.Start();
            //#######################################################
        }
        private void btn_start_MouseHover(object sender, EventArgs e)
        {
            btn_start.Image = Properties.Resources.btn_start_push;
        }
        private void btn_start_MouseLeave(object sender, EventArgs e)
        {
            btn_start.Image = Properties.Resources.btn_start_orig;
        }

        //정지하기 이벤트 ~ mouse click, mouse hover, mouse leave
        private void btn_stop_Click(object sender, EventArgs e)
        {
            btn_stop.Image = Properties.Resources.btn_stop_push;
            btn_stop.Image = Properties.Resources.btn_stop_orig;
            //#######################################################
            //############## 배현수  16.1.19 ########################
            thread.Abort();
            //#######################################################
        }
        private void btn_stop_MouseHover(object sender, EventArgs e)
        {
            btn_stop.Image = Properties.Resources.btn_stop_push;
        }
        private void btn_stop_MouseLeave(object sender, EventArgs e)
        {
            btn_stop.Image = Properties.Resources.btn_stop_orig;
        }

        //연습하기 이벤트 ~ mouse hover, mouse leave
        private void btn_practice_MouseHover(object sender, EventArgs e)
        {
            btn_practice.Image = Properties.Resources.btn_practice_push;
        }
        private void btn_practice_MouseLeave(object sender, EventArgs e)
        {
            btn_practice.Image = Properties.Resources.btn_practice_orig;
        }

        //학습하기 이벤트 ~ mouse hover, mouse leave
        private void btn_study_MouseHover(object sender, EventArgs e)
        {
            btn_study.Image = Properties.Resources.btn_study_push;
        }
        private void btn_study_MouseLeave(object sender, EventArgs e)
        {
            btn_study.Image = Properties.Resources.btn_study_orig;
        }




        /*---------- 상태바 움직임 ---------*/
        /*##################################################
         * info : 상태바를 누를시에 폼을 움직이게 한다.
         * 작성자 : 김민영 2016-01-05
         2016.01.14 정유진 수정
        ####################################################*/

        public Point downPoint = Point.Empty;

        private void panel_main_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            downPoint = new Point(e.X, e.Y);
        }

        private void panel_main_MouseMove(object sender, MouseEventArgs e)
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

        private void panel_main_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            downPoint = Point.Empty;
        }

        /*---------- 상태바 움직임 끝---------*/

        public int POctlbase_x { get; set; }

        //불러오기 이벤트 ~ mouse click, mouse hover, mouse leave
        private void btn_load_Click(object sender, EventArgs e)
        {
            btn_load.Image = Properties.Resources.btn_load_push;
            btn_load.Image = Properties.Resources.btn_load_orig;
            load();
        }
        private void btn_load_MouseHover(object sender, EventArgs e)
        {
            btn_load.Image = Properties.Resources.btn_load_push;
        }
        private void btn_load_MouseLeave(object sender, EventArgs e)
        {
            btn_load.Image = Properties.Resources.btn_load_orig;
        }
        //현재 옥타브를 picturebox에 출력하는 함수
        private void current_ocIndex(int ocIndex)
        {
            switch (ocIndex)
            {
                case 0:
                    pictureBox50.Image = Properties.Resources.octave_0;
                    break;
                case 1:
                    pictureBox50.Image = Properties.Resources.octave_1;
                    break;
                case 2:
                    pictureBox50.Image = Properties.Resources.octave_2;
                    break;
            }
           
            pictureBox50.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox50.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        private void octave_up()        // 옥타브 올리기
        {
            if (POctl.ocIndex > 0) POctl.ocIndex -= 1;
            current_ocIndex(POctl.ocIndex);
        }
        private void octave_down()      // 옥타브 내리기
        {
            if (POctl.ocIndex < 2) POctl.ocIndex += 1;
            current_ocIndex(POctl.ocIndex);
        }
        //옥타브 올림 이벤트 ~ mouse click, mouse hover, mouse leave
        private void btn_octave_up_Click(object sender, EventArgs e)
        {
            btn_octave_up.Image = Properties.Resources.btn_octave_up_push;
            btn_octave_up.Image = Properties.Resources.btn_octave_up_orig;
            octave_up();
        }
        private void btn_octave_up_MouseHover(object sender, EventArgs e)
        {
            btn_octave_up.Image = Properties.Resources.btn_octave_up_push;
        }
        private void btn_octave_up_MouseLeave(object sender, EventArgs e)
        {
            btn_octave_up.Image = Properties.Resources.btn_octave_up_orig;
        }

        //옥타브내림 이벤트 ~ mouse click, mouse hover, mouse leave
        private void btn_octave_down_Click(object sender, EventArgs e)
        {
            btn_octave_down.Image = Properties.Resources.btn_octave_down_push;
            btn_octave_down.Image = Properties.Resources.btn_octave_down_orig;
            octave_down();
        }
        private void btn_octave_down_MouseHover(object sender, EventArgs e)
        {
            btn_octave_down.Image = Properties.Resources.btn_octave_down_push;
        }
        private void btn_octave_down_MouseLeave(object sender, EventArgs e)
        {
            btn_octave_down.Image = Properties.Resources.btn_octave_down_orig;
        }

        //현재 박자를 picturebox에 출력하는 함수
        private void current_basicIndex(int basicIndex)
        {
            pictureBox51.Image = POctl.basic[POctl.basicIndex, 1];
            pictureBox51.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox51.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        private void basicIndex_up()
        {
            if (POctl.basicIndex == 0)
                MessageBox.Show("더 이상 박자를 올릴 수 없습니다.");
            else
                --POctl.basicIndex;
            current_basicIndex(POctl.basicIndex);
        }
        private void basicIndex_down()
        {
            if (POctl.basicIndex == 3)
                MessageBox.Show("더 이상 박자를 내릴 수 없습니다.");
            else
                ++POctl.basicIndex;
            current_basicIndex(POctl.basicIndex);
        }
        //박자 내림 이벤트 ~ mouse click, mouse hover, mouse leave 
        private void btn_time_down_Click(object sender, EventArgs e)
        {
            basicIndex_down();      // 박자 내리기 8분음표->4분음표
            btn_time_down.Image = Properties.Resources.btn_time_down_push;
            btn_time_down.Image = Properties.Resources.btn_time_down_orig;
        }
        private void btn_time_down_MouseHover(object sender, EventArgs e)
        {
            btn_time_down.Image = Properties.Resources.btn_time_down_push;
        }
        private void btn_time_down_MouseLeave(object sender, EventArgs e)
        {
            btn_time_down.Image = Properties.Resources.btn_time_down_orig;
        }

        //박자 올림 이벤트 ~ mouse click, mouse hover, mouse leave 
         private void btn_time_up_Click(object sender, EventArgs e)
        {
            basicIndex_up();        // 박자 올리기 4분음표->8분음표
            btn_time_up.Image = Properties.Resources.btn_time_up_push;
            btn_time_up.Image = Properties.Resources.btn_time_up_orig;
        }
        private void btn_time_up_MouseHover(object sender, EventArgs e)
        {
            btn_time_up.Image = Properties.Resources.btn_time_up_push;
        }
       
        private void btn_time_up_MouseLeave(object sender, EventArgs e)
        {
            btn_time_up.Image = Properties.Resources.btn_time_up_orig;
        }

        //제목입력 이벤트 ~ mouse click, mouse hover, mouse leave 
        
        private void btn_titleInput_Click(object sender, EventArgs e)
        {
            POctl.NameSetting(this.tb_name[POctl.now_music.smind], this.btn_titleInput, this.tb_name);

            if (tb_name[0].Enabled)
            {
                btn_titleInput.Image = Properties.Resources.btn_title_push;
                btn_titleInput.Image = Properties.Resources.btn_title_orig;
            }
            else
            {
                btn_titleInput.Image = Properties.Resources.btn_notitle_push;
                btn_titleInput.Image = Properties.Resources.btn_notitle_orig;
            }
        }
        private void btn_titleInput_MouseHover(object sender, EventArgs e)
        {
            if (tb_name[0].Enabled)
            {
                btn_titleInput.Image = Properties.Resources.btn_title_push;
            }
            else
            {
                btn_titleInput.Image = Properties.Resources.btn_notitle_push;
            }
        }
        private void btn_titleInput_MouseLeave(object sender, EventArgs e)
        {
            if (tb_name[0].Enabled)
            {
                btn_titleInput.Image = Properties.Resources.btn_title_orig;
            }
            else
            {
                btn_titleInput.Image = Properties.Resources.btn_notitle_orig;
            }
        }

        //페이지추가 이벤트 ~ mouse click, mouse hover, mouse leave 
        private void btn_ins_page_Click(object sender, EventArgs e)
        {
            pluspage(); //페이지 추가
            btn_ins_page.Image = Properties.Resources.btn_ins_page_push;
            btn_ins_page.Image = Properties.Resources.btn_ins_page_orig;
        }
        private void btn_ins_page_MouseHover(object sender, EventArgs e)
        {
            btn_ins_page.Image = Properties.Resources.btn_ins_page_push;
        }
        private void btn_ins_page_MouseLeave(object sender, EventArgs e)
        {
            btn_ins_page.Image = Properties.Resources.btn_ins_page_orig;
        }


    }
}
