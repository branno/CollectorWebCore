using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollectorWebCore.Models
{
    public class Start
    {
        public Start()
        {
            DateTimeAdded = DateTime.Now;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("datetimeadded")]
        public DateTime DateTimeAdded { get; set; }
    }
}