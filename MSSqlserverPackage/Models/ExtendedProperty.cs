using MSSqlserverPackage.Common;
using RainSong.Common;

namespace MSSqlserverPackage.Models
{
    public class ExtendedProperty
    {
        [Field("major_id")]
        public int MajorID { get; set; }
        [Field("minor_id")]
        public int MinorID { get; set; }
        [Field("name")]
        public string Name { get; set; }
        [Field("value")]
        public string Value { get; set; }
    }
}
