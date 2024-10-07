using System.Collections.Generic;

namespace INA_ACS_Server
{
    public class PlcConfig
    {
        public int Id { get; set; }


        public string PlcModuleUse { get; set; }                 // PLC Module 사용 미사용                  (DB 저장)
        public string PlcIpAddress { get; set; }                 // PLC Ip번호                              (DB 저장)
        public int PortNumber { get; set; }                      // PLC 포트 번호                           (DB 저장)
        public string PlcModuleName { get; set; }                // PLC Module 이름                         (DB 저장)
        public string PlcMapType { get; set; }                   // 1Word인지 타입확인                      (DB 저장)
        public string ReadFirstMapAddress { get; set; }          // 읽는 영역에 첫번째 영역 주소 예(DT11000)(DB 저장)
        public string ReadSecondMapAddress { get; set; }         // 읽는 영역에 마지막 영역 주소 예(DT11010)(DB 저장)
        public string WriteFirstMapAddress { get; set; }         // 쓰는 영역에 첫번째 영역 주소 예(DT12000)(DB 저장)
        public string WriteSecondMapAddress { get; set; }        // 쓰는 영역에 마지막 영역 주소 예(DT12010)(DB 저장)
        public string PlcMapValue { get; set; }                  // 데이터 Value값                          (DB 저장 안함)
        public bool Connect { get; set; }                        // PLC 연결 확인                           (DB 저장 안함)
        public string ControlMode { get; set; }                  // ControlMode(자동/수동)                  (DB 저장)
        public int CallNotOverlapCount { get; set; }             //Call 중복방지 Count                      (DB 저장)
        public int DisplayFlag { get; set; }                     //WiseModule 그리드에 표시하기 위한 신호
        public int ConnectRetry { get; set; }

        public ServiceReadData serviceReadData = new ServiceReadData();
        public ServiceWriteData serviceWriteData = new ServiceWriteData();

        //아이세로미림 사용
        public bool OperatorPLCModuleResetFlag { get; set; }    //작업자가 PLCModule Reset을 누른경우
        public override string ToString()
        {

            return $"id={Id,-5}, " +
                   $"PlcModuleUse={PlcModuleUse,-5}, " +
                   $"PlcIpAddress={PlcIpAddress,-5}, " +
                   $"PortNumber={PortNumber,-5}, " +
                   $"PlcModuleName={PlcModuleName,-5}, " +
                   $"PlcMapType={PlcMapType,-5}, " +
                   $"ReadFirstMapAddress={ReadFirstMapAddress,-5}, " +
                   $"ReadSecondMapAddress={ReadSecondMapAddress,-5}, " +
                   $"WriteFirstMapAddress={WriteFirstMapAddress,-5}, " +
                   $"WriteSecondMapAddress={WriteSecondMapAddress,-5}, " +
                   $"PlcMapValue={PlcMapValue,-5}, " +
                   $"ControlMode={ControlMode,-5}, " +
                   $"Connect={Connect,-5}, " +
                   $"serviceReadData.DT11000= {serviceReadData.DT11000,-5}" +
                   $"serviceReadData.DT11001= {serviceReadData.DT11001,-5}" +
                   $"serviceReadData.DT11002= {serviceReadData.DT11002,-5}" +
                   $"serviceReadData.DT11003= {serviceReadData.DT11003,-5}" +
                   $"serviceReadData.DT11004= {serviceReadData.DT11004,-5}" +
                   $"serviceReadData.DT11005= {serviceReadData.DT11005,-5}" +
                   $"serviceReadData.DT11006= {serviceReadData.DT11006,-5}" +
                   $"serviceReadData.DT11007= {serviceReadData.DT11007,-5}" +
                   $"serviceReadData.DT11008= {serviceReadData.DT11008,-5}" +
                   $"serviceReadData.DT11009= {serviceReadData.DT11009,-5}" +
                   $"serviceReadData.DT11010= {serviceReadData.DT11010,-5}" +
                   $"serviceWriteData.DT12000= {serviceWriteData.DT12000,-5}" +
                   $"serviceWriteData.DT12001= {serviceWriteData.DT12001,-5}" +
                   $"serviceWriteData.DT12002= {serviceWriteData.DT12002,-5}" +
                   $"serviceWriteData.DT12003= {serviceWriteData.DT12003,-5}" +
                   $"serviceWriteData.DT12004= {serviceWriteData.DT12004,-5}" +
                   $"serviceWriteData.DT12005= {serviceWriteData.DT12005,-5}" +
                   $"serviceWriteData.DT12006= {serviceWriteData.DT12006,-5}" +
                   $"serviceWriteData.DT12007= {serviceWriteData.DT12007,-5}" +
                   $"serviceWriteData.DT12008= {serviceWriteData.DT12008,-5}" +
                   $"serviceWriteData.DT12009= {serviceWriteData.DT12009,-5}" +
                   $"serviceWriteData.DT12010= {serviceWriteData.DT12010,-5}";
        }

    }
    public class ServiceReadData
    {
        //========================================================== Read 영역 Value
        public int DT11000 { get; set; }                    //PLC신호 0과 1반복
        public int DT11001 { get; set; }                    //CallFlag (Call신호)
        public int DT11002 { get; set; }                    //CallNotOverlapCount(중복방지Count)
        public int DT11003 { get; set; }                    //반송량 수량
        public int DT11004 { get; set; }                    //Spare
        public int DT11005 { get; set; }                    //Spare
        public int DT11006 { get; set; }                    //Spare
        public int DT11007 { get; set; }                    //Spare
        public int DT11008 { get; set; }                    //Spare
        public int DT11009 { get; set; }                    //Spare
        public int DT11010 { get; set; }                    //Spare
    }
    public class ServiceWriteData
    {
        //========================================================== Write 영역 Value
        public int DT12000 { get; set; }                    //ACS 신호 0과1반복 
        public int DT12001 { get; set; }                    //스케줄러 신호
        public int DT12002 { get; set; }                    //CallNotOverlapCount(중복방지Count) 
        public int DT12003 { get; set; }                    //반송수량 확인 
        public int DT12004 { get; set; }                    //Spare
        public int DT12005 { get; set; }                    //Spare
        public int DT12006 { get; set; }                    //Spare
        public int DT12007 { get; set; }                    //Spare
        public int DT12008 { get; set; }                    //Spare
        public int DT12009 { get; set; }                    //Spare
        public int DT12010 { get; set; }                    //Spare
    }


}
