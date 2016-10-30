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

namespace WindowsFormsApplication1
{
    class File
    {
        public File() { }
        ~File() { }

        public void save(string title, music musicSheet, Panel[] p_music)
        {
            string text = "";
            text += "[" + title + "]";
            for (int i = 0; i <= musicSheet.max_smind; i++)
            {
                text += "{";
                for (int j = 0; j < musicSheet.sm[i].note_arr.Length; j++)
                {
                    try
                    {
                        ntValue note = musicSheet.sm[i].note_arr[j].nt;
                        int octave = musicSheet.sm[i].note_arr[j].ocIndex;
                        int length = musicSheet.sm[i].note_arr[j].length;
                        text += "<" + note.ToString() + "," + octave.ToString() + "," + length.ToString() + ">";
                    }
                    catch { continue; }
                }
                text += "}";
            }
            text += "e";
            if (text != "e")
            {
                SaveFileDialog(text, title, p_music, musicSheet.max_smind);
            }
            else
            {
                MessageBox.Show("악보를 제작한 후 저장해보세요!");
            }
        }

        private void SaveFileDialog(string text, string title, Panel[] p_music, int page)
        {
           
            Console.WriteLine(text);

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = title;
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt";

            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK
                && saveFileDialog1.FileName.Length > 0)
            {
                string savePath = System.IO.Path.GetDirectoryName(saveFileDialog1.FileName);
                System.IO.StreamWriter file = new System.IO.StreamWriter(saveFileDialog1.FileName, false, System.Text.Encoding.Default);
                file.WriteLine(text);
                file.Close();

                for (int i = 0; i <= page; i++)
                {
                    string Path = savePath + "\\" + title + "p" + (i + 1) + ".jpg";
                    Bitmap bmp = new Bitmap(p_music[i].Width, p_music[i].Height);
                    p_music[i].DrawToBitmap(bmp, new Rectangle(0, 0, p_music[i].Width, p_music[i].Height));
                    bmp.Save(Path, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
        }


        public string[] music = { "", "", "", "", "" };
        public string title = "";
        public int page = 0;

        public void readfile(string url)
        {
            try
            {
                using (StreamReader sr = new StreamReader(url, Encoding.Default))
                {
                    String line;
                    int idx = 0; this.page = 0;

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line[0] == '[')
                        {
                            idx++;
                            while (line[idx] != ']')
                            {
                                title += line[idx].ToString();
                                idx++;
                            }
                        }
                        idx++;

                        while (line[idx] != 'e')
                        {
                            if (line[idx] == '{')
                            {
                                string tmp = ""; idx++;
                                while (line[idx] != '}')
                                {
                                    if (line[idx] == '<')
                                    {
                                        idx++;
                                        while (line[idx] != '>')
                                        {
                                            tmp += line[idx];
                                            idx++;
                                        }
                                        tmp += ",";
                                    }
                                    idx++;
                                }
                                music[page] = tmp;
                            }
                            page++;
                            idx++;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        public void OpenFile()
        {
            this.title = "";

            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                readfile(file.FileName);
            }
        }

    }
}
