using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    /*
     * 
     * 
    CREATE TABLE [dbo].[WMSModels](
	    [Id] [int] IDENTITY(1,1) NOT NULL,
	    [LINE_CD] [varchar](50) NULL,
	    [POST_CD] [int] NULL,
	    [COMM_ANG] [int] NULL,
	    [RETU_TYPE] [varchar](50) NULL,
	    [OUT_Q] [int] NULL,
	    [PART_CD] [varchar](50) NULL,
	    [PART_NM] [varchar](50) NULL,
	    [OUT_WH] [varchar](50) NULL,
	    [OUT_POINT] [int] NULL,
	    [OUT_PART_CD] [varchar](50) NULL,
	    [IN_POINT] [varchar](50) NULL,
	    [WMS_IF_FLAG] [varchar](50) NULL,
	    [CREATE_DT] [datetime] NULL,
	    [MODIFY_DT] [datetime] NULL,
     CONSTRAINT [PK_WMSModelsTable] PRIMARY KEY CLUSTERED 
    (
	    [Id] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
      * 
      * 
      */


    public class WmsModel
    {
        //일단 변수명은 DB에서 주어진 이름을 사용함
        //POP정보(Call)을 이용하여 창고로 정보를 전달하고 전달한 데이터를 이용하여 창고에서 써주면 미션생성
        public int Id { get; set; }
        //=====ACS 데이터생성(촐고/입고)할때 써준다 =====
        public string LINE_CD { get; set; }         //[생산라인] *POP서버에서 주어진 데이터를 그대로 쓰기
        public int POST_CD { get; set; }            //[Post 코드] *POP 서버에서 주어진 데이터를 그대로 쓰기
        public int COMM_ANG { get; set; }           //[각도] *POP 서버에서 주어진 데이터를 그대로 쓰기
        public string RETU_TYPE { get; set; }       //[공용기 여부] *POP 서버에서 주어진 데이터를 그대로 쓰기
        public int OUT_Q { get; set; }              //[출고 수량] *PART LIST 에서 생산라인 / POST코드 / 각도 / 공용기 여부 를 비교해서 해당 수량을 SELECT 하여 쓰기
        public string PART_CD { get; set; }         //[품번] *PART LIST 에서 생산라인 / POST코드 / 각도 / 공용기 여부 를 비교해서 해당 품번을 SELECT 하여 쓰기
        public string PART_NM { get; set; }         //[품명] *PART LIST 에서 생산라인 / POST코드 / 각도 / 공용기 여부 를 비교해서 해당 품번을 SELECT 하여 쓰기
        public string OUT_WH { get; set; }          //[창고 출고 여부] *MiR 타입 선택 (생산라인 으로 구별해야함 아래 설명 참조) "Y"리클(리프트타입) "N"레일(후크타입) 쓰기
        public DateTime CREATE_DT { get; set; }     //[생성일시]ACS 데이터 전송시 생성일자

        //=====창고에서 처리후 써준다=====(출고시)
        public int OUT_POINT { get; set; }          //[출고위치] 완제품 출고위치로 창고쪽에서 데이터를 써주기로함 ACS에서 읽어야함
        public string WMS_IF_FLAG { get; set; }     //[인터페이스플래그] ACS에서 데이터생성 'N' 으로 설정하고, 창고에서 처리후 'Y'로 변경
        public DateTime MODIFY_DT { get; set; }     //[수행일시] 창고에서 데이터 전송시 생성일자

        //=====창고에서 처리후 써준다=====(입고시)
        //public int IN_READY { get; set; }           //[입고가능] 공Box 입고가능여부 창고에서 주어야함

        //창고 출고 여부 구별 방법 MC310/MC311/MC350/MC351(4곳 생산라인은 리프트모델) MC110/MC111/MC112(3곳 생산라인은 후크 모델)

        /*WMS 데이터 생성 순서
         * 1.[ACS] 생산라인 / POST코드 / 각도 / 공용기여부 / 출고 수량 / 품번 / 품명 / 창고 출고 여부/ 창고에서 자료 변경 / 생성 일시 에 대한 데이터를 쓰기합니다.
         * 2.[창고] 창고에서 출고위치 데이터를 쓰고 창고에서 자료변경데이터를 "Y" 로 변경합니다.
         * 3.[ACS] 자료 변경 데이터가 "Y"인 데이터를 읽어서 미션을 생성합니다.
         * 현재)단! 공용기 여부가 "Y"일경우 창고에서 쓰는 데이터가 없이 미션을 생성합니다.(창고에서 받아오는 자재가 아닙니다)
         * 
         * 협의중) 공BOX입고 에대한 센서 신호를 창고쪽에서 볼경우 공용기 여부가 "Y"일때도 창고 써주는 데이터를 봐야합니다.
         */


        public override string ToString()
        {
            string postName = ""; // post name
            if (RETU_TYPE == "N") postName = "투입";
            else if (RETU_TYPE == "Y") postName = "회수";

            return $"Id={Id,-5} {LINE_CD} {POST_CD,-2} {postName} {COMM_ANG,-3} {RETU_TYPE} {WMS_IF_FLAG} {OUT_Q} {PART_CD} {PART_NM} {OUT_WH} {CREATE_DT:s} {MODIFY_DT:s}";
        }
    }
}
