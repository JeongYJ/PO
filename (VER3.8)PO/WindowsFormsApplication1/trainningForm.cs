using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class trainningForm : Form
    {
        control POtrCtr = new control();
        Thread thread;

        public struct trNote
        {
            public ntValue note;
            public int octave;
            public int length;
        };

        public List<trNote> trScore =  new List<trNote>();
        public int allindex = 0;

        int trainningindex = 0;
        int keyDownflag = 0;

        public trainningForm()
        {
            InitializeComponent();
            current_ocIndex(POtrCtr.ocIndex);
            current_basicIndex(POtrCtr.basicIndex);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(trainningForm_KeyDown);
            MessageBox.Show("POtrCtr.ocIndex:"+ POtrCtr.ocIndex+" "+ "POtrCtr.basicIndex:"+ POtrCtr.basicIndex);
        }

        private void InitMusicSheet()
        {
            this.p_trmusic.Controls.Add(this.tb_trName);
            this.p_trmusic.Controls.Add(this.pictureBox37);
            this.p_trmusic.Controls.Add(this.pictureBox38);
            this.p_trmusic.Controls.Add(this.pictureBox39);
            this.p_trmusic.Controls.Add(this.pictureBox40);
            this.p_trmusic.Controls.Add(this.pictureBox41);
            this.p_trmusic.Controls.Add(this.pictureBox42);
            this.p_trmusic.Controls.Add(this.pictureBox43);
            this.p_trmusic.Controls.Add(this.pictureBox44);
            this.p_trmusic.Controls.Add(this.pictureBox45);
            this.p_trmusic.Controls.Add(this.pictureBox46);
            this.p_trmusic.Controls.Add(this.pictureBox47);
            this.p_trmusic.Controls.Add(this.pictureBox48);
            trainningindex = 0;
 

        }

         private void convertToSheetmusic()
         {
             POtrCtr.TempoImgToTrainning();/*
             for (int i=0; i< trScore.Length; i++)
             {
                 string input = trScore[i].ToString();
                 keyntValue key = (keyntValue)Enum.Parse(typeof(keyntValue), input);
                 ntValue notevalue = key.keyTontValue();
                 POtrCtr.note_location(notevalue, p_trmusic);
             }*/
             POtrCtr.TempoImgToBagic();
         }

         private void createSheetMusic()
         {
             convertToSheetmusic();
             saveMusicSheet(this.tb_trName.Text);
         }

         private void saveMusicSheet(string musicName)
         {
             musicName += ".jpg";
             Bitmap bmp = new Bitmap(this.p_trmusic.Width, this.p_trmusic.Height);
             this.p_trmusic.DrawToBitmap(bmp, new Rectangle(0, 0, this.p_trmusic.Width, this.p_trmusic.Height));
             bmp.Save(musicName, System.Drawing.Imaging.ImageFormat.Jpeg);

         }
         private void GetMapImage(string Path)
         {
             System.Drawing.Bitmap bitmap = new Bitmap(Path);
             p_trmusic.BackgroundImage = SetImageOpacity(bitmap, 0.50F); 


         }
         public Image SetImageOpacity(Image image, float opacity)
         {
             Bitmap bmp = new Bitmap(image.Width, image.Height);
             using (Graphics g = Graphics.FromImage(bmp))
             {
                 ColorMatrix matrix = new ColorMatrix();
                 matrix.Matrix33 = opacity;
                 ImageAttributes attributes = new ImageAttributes();
                 attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default,
                                                   ColorAdjustType.Bitmap);
                 g.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height),
                                    0, 0, image.Width, image.Height,
                                    GraphicsUnit.Pixel, attributes);
             }
             return bmp;
         }


         public bool trainningmode(ntValue nt, int ocIndex, int tempo)
         {
       
             //2017-01-17 김민영
             //순서가 맞지않는 경우란 없다 원래 카운트 부분 수정
             int crr = trainningindex;

             if (crr == allindex)
             {
                 lb_test.Text = "연습이 완료되었습니다!"; return false;
             }
             //MessageBox.Show(trScore[trainningindex].note.ToString()); 
             if (nt.ToString() != trScore[crr].note.ToString())
             {
                 lb_test.Text = "음계가 일치하지 않습니다!"; return false;

             }//음계가 일치하지 않을 시 종료

             if (ocIndex != trScore[crr].octave)
             {
                 lb_test.Text = "옥타브가 일치하지 않습니다."; return false;
             }//옥타브가 일치하지 않을 시 종료*/


             if (tempo != trScore[crr].length - 8)
             {
                  lb_test.Text ="박자가 일치하지 않습니다."; return false;
             }//박자가 일치하지 않을 시 종료*/


        
             lb_test.Text = "맞았습니다!";

             trainningindex++;
             return true;

         }
        private void readFile(string musicName){
     
            File file = new File();
            file.readfile(musicName + ".txt");
            tb_trName.Text = file.title;
            allindex = 0;
            char[] delimiterChars = { ',' };
            string[] words = file.music[0].Split(delimiterChars);

            for (int i = 0; i < words.Length - 1; i += 3)
            {
                ntValue nt = (ntValue)Enum.Parse(typeof(ntValue), words[i]);
                int octave = int.Parse(words[i + 1]);
                int length = int.Parse(words[i + 2]);
                //length = 4 - (int)Math.Log(length, 2.0);
                trNote tmp; tmp.note = nt; tmp.octave = octave; tmp.length = length;
                trScore.Add(tmp);
                allindex++;
           }

        }

        private void  Music1_Click(object sender, EventArgs e){

            p_trmusic.Controls.Clear();
            trScore.Clear();
            GetMapImage("비행기p1.jpg");
            InitMusicSheet();
            this.tb_trName.Text = "비행기";
            readFile("비행기");

        }

        private void Music2_Click(object sender, EventArgs e)
        {
            p_trmusic.Controls.Clear();
            trScore.Clear();
            this.tb_trName.Text = "섬집 아기";
            GetMapImage("섬집 아기p1.jpg");
            InitMusicSheet();
            this.tb_trName.Text = "섬집 아기";
            readFile("섬집 아기");
        }


        private void trainning_init_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("지금까지 기록을 없애고\n처음부터 다시하시겠습니까?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Image tmp = p_trmusic.BackgroundImage;
                p_trmusic.Controls.Clear();
                InitMusicSheet();
                p_trmusic.BackgroundImage = tmp;
            }
         
        }

        private void Load_Music_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Load_Music_Click");
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    Console.WriteLine(Path.GetFileName(file.FileName));
                    string fileName = Path.GetFileName(file.FileName);
                    fileName = fileName.Replace(".txt", "");
                    Console.WriteLine(fileName);
                    trScore.Clear();
                    p_trmusic.Controls.Clear();
                    this.tb_trName.Text = fileName;
                    GetMapImage(fileName + "p1.jpg");
                    InitMusicSheet();
                    this.tb_trName.Text = fileName;
                    readFile(fileName);

                }catch(Exception err){
                    MessageBox.Show("txt파일 형식인 악보를 선택하여 주세요!\n"+err.Message);
                }
            }


        }


        public bool isNoSelectedMusic()
        {
            return this.tb_trName.Text!="연습모드";
        }

        private void tranningForm_FormClosing(Object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            if (this.DialogResult == DialogResult.Cancel)
            {
                switch (MessageBox.Show(this, "정말 끝내시겠습니까?", "Do you still want ... ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    case DialogResult.No:
                        e.Cancel = true;
                        break;
                    default:
                        this.Owner.Show();
                        break;
                }
            }
        }
        private void trainningForm_KeyDown(object sender, KeyEventArgs e)
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
        private void trainningForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ( isNoSelectedMusic() && e.KeyChar.ToString() != "\0")
            {
                try
                { 
                     string input = e.KeyChar.ToString().ToUpper();
                     keyntValue key = (keyntValue)Enum.Parse(typeof(keyntValue), input);
                     ntValue notevalue = key.keyTontValue();
                     ntkrValue notename = key.keyTontkrValue();

                     lb_note.Text = notename.ToString();

                     if (trainningmode(notevalue, POtrCtr.ocIndex, POtrCtr.basicIndex) == true)
                     {
                        POtrCtr.note_location(notevalue,p_trmusic);
                        POtrCtr.note_sound(POtrCtr.ocIndex, (int)notevalue);
                     }
                }
                catch (Exception err)
                {
                    MessageBox.Show("해당키만 누르시오!" + err.Message);

                }
                e.KeyChar = '\0';
            }
        }

        private void btn_closetrainning_Click(object sender, EventArgs e)
        {
            btn_closetrainning.Image = Properties.Resources.btn_closeform_push;
            btn_closetrainning.Image = Properties.Resources.btn_closeform_orig;
            this.Close();
        }

        private void btn_closetrainning_MouseHover(object sender, EventArgs e)
        {
            btn_closetrainning.Image = Properties.Resources.btn_closeform_push;
        }

        private void btn_closetrainning_MouseLeave(object sender, EventArgs e)
        {
            btn_closetrainning.Image = Properties.Resources.btn_closeform_orig;
        }

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

        private void btn_octave_up_MouseHover(object sender, EventArgs e)
        {
            btn_octave_up.Image = Properties.Resources.btn_octave_up_push;
        }
        private void btn_octave_up_MouseLeave(object sender, EventArgs e)
        {
            btn_octave_up.Image = Properties.Resources.btn_octave_up_orig;
        }

        private void octave_up()        // 옥타브 올리기
        {
            if (POtrCtr.ocIndex > 0) POtrCtr.ocIndex -= 1;
            current_ocIndex(POtrCtr.ocIndex);
        }
        private void octave_down()      // 옥타브 내리기
        {
            if (POtrCtr.ocIndex < 2) POtrCtr.ocIndex += 1;
            current_ocIndex(POtrCtr.ocIndex);
        }
        //현재 옥타브를 picturebox에 출력하는 함수
        private void current_ocIndex(int ocIndex)
        {
            switch (ocIndex)
            {
                case 0:
                    pictureBox3.Image = Properties.Resources.octave_0;
                    break;
                case 1:
                    pictureBox3.Image = Properties.Resources.octave_1;
                    break;
                case 2:
                    pictureBox3.Image = Properties.Resources.octave_2;
                    break;
            }

            pictureBox3.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        //옥타브 올림 이벤트 ~ mouse click, mouse hover, mouse leave
        private void btn_octave_up_Click(object sender, EventArgs e)
        {
            Console.WriteLine("change before="+POtrCtr.ocIndex.ToString());
            btn_octave_up.Image = Properties.Resources.btn_octave_up_push;
            btn_octave_up.Image = Properties.Resources.btn_octave_up_orig;
            octave_up();
            Console.WriteLine("change curr=" + POtrCtr.ocIndex.ToString());
        }
        //옥타브내림 이벤트 ~ mouse click, mouse hover, mouse leave
        private void btn_octave_down_Click(object sender, EventArgs e)
        {
            Console.WriteLine("change before=" + POtrCtr.ocIndex.ToString());
            btn_octave_down.Image = Properties.Resources.btn_octave_down_push;
            btn_octave_down.Image = Properties.Resources.btn_octave_down_orig;
            octave_down();
            Console.WriteLine("change curr=" + POtrCtr.ocIndex.ToString());

        }
        private void btn_octave_down_MouseHover(object sender, EventArgs e)
        {
            btn_octave_down.Image = Properties.Resources.btn_octave_down_push;
        }
        private void btn_octave_down_MouseLeave(object sender, EventArgs e)
        {
            btn_octave_down.Image = Properties.Resources.btn_octave_down_orig;
        }
        private void basicIndex_up()
        {
            if (POtrCtr.basicIndex == 0)
                MessageBox.Show("더 이상 박자를 올릴 수 없습니다.");
            else
                --POtrCtr.basicIndex;
            current_basicIndex(POtrCtr.basicIndex);
        }
        private void basicIndex_down()
        {
            if (POtrCtr.basicIndex == 3)
                MessageBox.Show("더 이상 박자를 내릴 수 없습니다.");
            else
                ++POtrCtr.basicIndex;
            current_basicIndex(POtrCtr.basicIndex);
        }
        //현재 박자를 picturebox에 출력하는 함수
        private void current_basicIndex(int basicIndex)
        {
            pictureBox4.Image = POtrCtr.basic[POtrCtr.basicIndex, 1];
            pictureBox4.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        //박자 내림 이벤트 ~ mouse click, mouse hover, mouse leave 
        private void btn_time_down_Click(object sender, EventArgs e)
        {
            basicIndex_down();
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
            basicIndex_up();
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

       
        private void start()
        {
            try
            {
                for (int i = 0; i < allindex; i++)//현재 생성된 음표까지 음악출력
                {
         
                    Console.WriteLine("allindex = " + allindex);
                    Console.WriteLine((int)trScore[i].note);
                    POtrCtr.trainning_play((int)trScore[i].note, trScore[i].octave, trScore[i].length);//음표 해당 음 출력
                }
            }
            catch(Exception err) {
                MessageBox.Show(err.Message);
            }
        }


        //재생하기 이벤트 ~ mouse click, mouse hover, mouse leave
        private void btn_start_Click(object sender, EventArgs e)
        {
            if (allindex != 0)
            {
                thread = new Thread(start);
                thread.Start();
            }
           
         
        }
      
        //정지하기 이벤트 ~ mouse click, mouse hover, mouse leave
        private void btn_stop_Click(object sender, EventArgs e)
        {
            if (allindex != 0)
            {
                 thread.Abort();
            }
        }


    }
}
