﻿using System;

namespace Vincente.WebJob
{
    public class LastRunTimes
    {
        public LastRunTimes() { }

        public DateTime InvoiceByMonth { get; set; }
        public DateTime TogglToTask { get; set; }
        public DateTime TogglToTimeEntry { get; set; }
        public DateTime TrelloBackup { get; set; }
        public DateTime TrelloToCard { get; set; }
    }
}
