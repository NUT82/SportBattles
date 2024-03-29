﻿namespace SportBattles.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using SportBattles.Data.Common.Models;

    public class Image : BaseDeletableModel<string>
    {
        public Image()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        [MaxLength(5)]
        public string Extension { get; set; }
    }
}
