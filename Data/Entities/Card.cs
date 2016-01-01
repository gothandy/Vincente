﻿using Gothandy.Tables.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vincente.Data.Entities 
{
    public class Card : ICompare<Card>
    {
        public string Id { get; set; }
        public string DomId { get; set; }
        public int ListIndex { get; set; }
        public string ListName { get; set; }
        public string Name { get; set; }
        public string Epic { get; set; }
        public DateTime? Invoice { get; set; }
        public DateTime Timestamp { get; set; }
        public List<long> TaskIds { get; set; }

        public bool ValueEquals(Card other)
        {
            if (this.Id != other.Id) return false;
            if (this.DomId != other.DomId) return false;
            if (this.ListIndex != other.ListIndex) return false;
            if (this.ListName != other.ListName) return false;
            if (this.Name != other.Name) return false;
            if (this.Epic != other.Epic) return false;
            if (this.Invoice != other.Invoice) return false;
            if (this.TaskIds != null)
            {
                if (other.TaskIds == null) return false;
                if (!this.TaskIds.SequenceEqual(other.TaskIds)) return false;
            }
            else
            {
                if (other.TaskIds != null) return false;
            }

            return true;
        }

        public bool KeyEquals(Card other)
        {
            if (this.Id != other.Id) return false;

            return true;
        }
    }
}
