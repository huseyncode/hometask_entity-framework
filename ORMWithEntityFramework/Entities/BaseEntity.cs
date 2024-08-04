﻿using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMWithEntityFramework.Entities
{
    internal class BaseEntity
    {
       public int Id { get; set; }  

        public bool IsDeleted { get; set; }
    }
}
