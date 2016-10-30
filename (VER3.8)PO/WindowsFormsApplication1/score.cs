using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DirectX.AudioVideoPlayback;
using Microsoft.DirectX.DirectSound;
using Microsoft.DirectX;

namespace WindowsFormsApplication1
{

    // 음계 입력 매칭 
     public enum ntValue
     {
        C , D , E , F , G , A , B , HC, SC, SD, SF, SG, SA
     }
     public enum ntkrValue
     {
         도, 레, 미, 파, 솔, 라, 시, 높은도, 도샵, 레샵, 파샵, 솔샵, 라샵
     }
     // 키보드 입력 매칭 
     public enum keyntValue
     {
         A, S, D, F, G, H, J, K, W, E, T, Y, U
     }
     public static class noteInterface
     {

        static int[] noteToLocation = { 14, 11, 8, 5, 2, -1, -3, -6, 14, 11, 5, 2, -1 };
        static int[] noteTolocationMadi = { 55, 42, 29, 16, 3, -13, -27, -41, 55, 42, 16, 3, -13 };//샵부분 수정필요

        public static int getLocation(this ntValue note)
         {
             return noteToLocation[(int)note];
         }
         public static int getLocationMadi(this ntValue note)
         {
             return noteTolocationMadi[(int)note];
         }

         public static ntValue keyTontValue(this keyntValue note)
         {
             ntValue notevalue = (ntValue)Enum.ToObject(typeof(ntValue), (int)note);//계이름 판정
             return notevalue;
         }

         public static ntkrValue keyTontkrValue(this keyntValue note)
         {
             ntkrValue notevalue = (ntkrValue)Enum.ToObject(typeof(ntkrValue), (int)note);//계이름 판정
             return notevalue;
         }
    }

    //음표
    class note
    {
        public int length = 0;      //온음표, 2분음표
        public int[] octave = new int[] { 69, 75, 96 };    //옥타브
        public int ocIndex =0;
        public ntValue nt;//도레미

        //public int _sound = 0 ;

        //음표 생성자수정 20170118 김민영
        //위치가 아닌 계이름과 옥타브를 저장하도록함 -> 추후에 박자저장 필요

        public note(ntValue nt, int ocIndex, int length)
        {
            this.nt = nt; //계이름
            this.ocIndex = ocIndex; //옥타브
            this.length = length; //박자
        }

    }



    //악보
    class sheetmusic
    {
        public note[] note_arr = new note[1000]; //음표 배열


        public int page = 0;                    //몇 페이지
        public int meter_signature = 0;         //박자표
        public int noteind = 0;                 //음표 인덱스


        //##### 배씨 추가 #####
        //###### 1.13 #########
        public int _x = 97;
        public int count=0;
        public int crrLine = 0;
        public int _x_madi = 0;
        public int count_madi = 0;
        //######################

        //생성자
        public sheetmusic(int _page)
        {
            page = _page;                //몇 페이지
            meter_signature = 0;         //박자표
            noteind = 0;                 //음표 인덱스
        }

        //음표 추가
        /*
            2016.01.17 김민영 
         *  note클래스 수정 필요정보만 담도록 변경
         *  더 필요한 정보가 있다면 수정 부탁
        */
        public void add_note(ntValue nt, int ocIndex, int length)
        {
            note_arr[noteind++] = new note(nt, ocIndex, length); 
        }

        public void initSheetmusic()
        {
            page = 0;
            meter_signature = 0;
            noteind = 0;
            note_arr.Initialize();
        }


    }


    //*****************************************************************
    //배씨 추가

    //음악(악보)
    //ind는 전부 인덱스로 표시 
    //ex) 1쪽 -> ind=0
    class music
    {
        public string name;     //음악 이름
        public int max_smind = 0; //생성한 악보의 최대 페이지
        public int smind;         //악보 보여주는 페이지
        public int max_page;    //만들 수 있는 악보 최대 페이지->배열크기
        public sheetmusic[] sm; //악보 모음

        //받은 파라미터 이름으로 악보 생성
        public music(string _name, int _max_page)
        {
            name = _name;
            max_page = _max_page;

            //################## 수배현 1.13 ##########################
            sm = new sheetmusic[max_page];//max_page 크기 만큼 악보생성
            for (int i = 0; i < max_page; i++)
                sm[i] = new sheetmusic(i);
            //##########################################################

            smind = 0;
            max_smind = 0;
        }

        //악보 페이지 생성
        public bool pluspage()
        {
            //현재 max_page쪽 인 경우 -> 다음 페이지로 넘어갈 수 없음
            if (smind + 1 >= max_page)
            {
                return false;
            }
            
            max_smind++;
            smind = max_smind;
            return true;//페이지생성가능하다.
        }

        //페이지 이동
        public bool gopage(int page)
        {
            //최대 페이지를 넘어서는 인덱스로 넘어가고 싶을 때
            //-> 아무것도 없는 페이지로 넘어가고 싶을 때
            if (page - 1 > max_smind)
            {
                return false;//이동할 수 없다.
            }

            smind = page - 1;//해당 페이지로 smind변경
            return true;//이동가능하다.
        }
    }
    //******************************************************************
}
