using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;

namespace WebQQ.Im.Bean
{
    
    public class Group
    {
        public long Gid { get; set; }
        public long Code { get; set; }
        public string Name { get; set; }
        public long Flag { get; set; }
        // public int Mask { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string MarkName { get; set; }

        public override string ToString()
        {
            return "QQGroup [gid=" + Gid + ", code=" + Code + ", name=" + Name + "]";
        }
    }
}
