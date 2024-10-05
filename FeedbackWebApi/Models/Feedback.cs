﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FeedbackWebApi.Models
{
    [Table("Feedback", Schema = "dbo")]
    public class Feedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FeedbackId { get; set; }

        public long UserId { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

        [MaxLength(100)]
        public string CreatedBy { get; set; }

        public DateTime DateTimeCreated { get; set; }

        [MaxLength(100)]
        public string UpdatedBy { get; set; }

        public DateTime DateTimeUpdated { get; set; }

    }
}
