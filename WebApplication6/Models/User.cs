using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; }    
        public string GoogleId { get; set; }
        public DateTime CreatedON { get; set;}
        public DateTime UpdatedON { get; set;}
        public int Points { get; set;}
        public string Email { get; set; }

    }
}