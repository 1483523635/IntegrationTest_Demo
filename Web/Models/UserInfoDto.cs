using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models
{
    public class UserInfoDto
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
