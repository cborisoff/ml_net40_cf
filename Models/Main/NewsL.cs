namespace Models.Main
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ml_user.NewsL")]
    public partial class NewsL
    {
        public long id { get; set; }

        public long master_id { get; set; }

        [Required]
        [StringLength(2)]
        public string lang { get; set; }

        public string tag_list { get; set; }

        [Required]
        [StringLength(2000)]
        public string title { get; set; }

        public string sub_title { get; set; }

        public string content { get; set; }
    }
}
