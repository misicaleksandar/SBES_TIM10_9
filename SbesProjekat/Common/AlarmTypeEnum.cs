using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common { 

    [DataContract]
    public enum AlarmType {
        [EnumMember]
        NO_ALARM,
        [EnumMember]
        FALSE_ALARM,
        [EnumMember]
        INFO,
        [EnumMember]
        WARNING,
        [EnumMember]
        ERROR
    }
}
