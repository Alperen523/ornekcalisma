﻿using System;
using UzmanCrm.CrmService.Domain.Abstraction;

namespace UzmanCrm.CrmService.Domain
{
    public class BaseSchema<T> : IEntity<T>
    {
        public T Id { get; set; }

        public Guid CreatedBy { get; set; }

        public Guid ModifiedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public bool IsActive { get; set; }

        public bool IsDelete { get; set; }
    }
}
