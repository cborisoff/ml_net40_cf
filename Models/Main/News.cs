namespace Models.Main
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("[ml_user].[News]")]
    public partial class News
    {
        public long id { get; set; }

        public int news_type { get; set; }

        [StringLength(100)]
        public string keyword { get; set; }

        public DateTime? news_date { get; set; }

        public string tag_list { get; set; }

        [Required]
        [StringLength(2000)]
        public string title { get; set; }

        public string sub_title { get; set; }

        public string content { get; set; }

        public DateTime? datecreate { get; set; }

        public int? usercreate { get; set; }

        public DateTime? dateupdate { get; set; }

        public int? userupdate { get; set; }

        public long? order_by { get; set; }

        public bool? active { get; set; }
    }
}
