using System;
using System.Collections.Generic;
using System.Text;

namespace ccphl.Entity
{
    /// <summary>
    /// 频道栏目关联实体
    /// </summary>
    [Serializable]
    public class ChannelColumn
    {
        public ChannelColumn()
        { }

        public int Id { get; set; }
        public int ColumnId { get; set; }
        public int PId { get; set; }
        public string Name { get; set; }
        public int Layer { get; set; }
        public string Url { get; set; }
        public int Authority { get; set; }
    }
}
