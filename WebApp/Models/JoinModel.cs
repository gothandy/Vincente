﻿using System;

namespace WebApp.Models
{
    public class JoinModel
    {
        public string CardId { get; set; }
        public string DomId { get; set; }
        public int? ListIndex { get; set; }
        public string ListName { get; set; }
        public string Name { get; set; }
        public string Epic { get; set; }
        public DateTime? Invoice { get; set; }
        public DateTime? Month { get; set; }
        public string UserName { get; set; }
        public decimal? Billable { get; set; }
    }
}