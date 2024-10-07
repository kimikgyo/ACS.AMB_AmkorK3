using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA_ACS_Server
{
    public class ProductModel
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public string Barcode { get; set; }
        public string RobotName { get; set; }
        public string ProductName { get; set; }
        public int Qty { get; set; }
        public string Info1 { get; set; }
        public string Info2 { get; set; }
        public string Info3 { get; set; }
        public string Info4 { get; set; }

        public override string ToString()
        {

            return $"id={Id,-5}, " +
                   $"Barcode={Barcode}, " +
                   $"RobotName={RobotName}, " +
                   $"ProductName={ProductName}, " +
                   $"Info1={Info1}, " +
                   $"Info2={Info2}, " +
                   $"Info3={Info3}, " +
                   $"Info4 {Info4}, ";
        }
    }
}
