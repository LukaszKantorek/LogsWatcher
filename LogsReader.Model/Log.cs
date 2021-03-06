﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogsWatcher.Model
{
    public class Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public DateTime InsertDate { get; set; }
        [Required]
        public LogType Type { get; set; }
        public string StackTrace { get; set; }
    }
}
