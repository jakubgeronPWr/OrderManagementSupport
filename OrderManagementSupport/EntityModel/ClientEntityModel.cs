﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSupport.EntityModel
{
    public class ClientEntityModel
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(2)]
        public string LastName { get; set; }
        [Required]
        [MinLength(4)]
        public string ZipCode { get; set; }
        [Required]
        [MinLength(3)]
        public string City { get; set; }
        [Required]
        [MinLength(3)]
        public string Address { get; set; }
    }
}
