﻿using Gothandy.Tables.AzureTables.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Vincente.Azure.Entities;
using Vincente.Data.Entities;

namespace Vincente.Azure.Converters
{
    internal class CardConverter : IConverter<Card, CardEntity>
    {
        public Card Read(CardEntity azureEntity)
        {
            return new Card()
            {
                Id = azureEntity.RowKey,
                DomId = azureEntity.DomId,
                Epic = azureEntity.Epic,
                Blocked = azureEntity.Blocked,
                BlockedReason = azureEntity.BlockedReason,
                ReuseDA = azureEntity.ReuseDA,
                ReuseFCP = azureEntity.ReuseFCP,
                Invoice = azureEntity.Invoice,
                ListIndex = azureEntity.ListIndex,
                ListName = azureEntity.ListName,
                Name = azureEntity.Name,
                Timestamp = azureEntity.Timestamp.LocalDateTime
            };
        }

        public CardEntity Write(Card dataEntity)
        {
            return new CardEntity()
            {
                PartitionKey = "SingleKey",
                RowKey = dataEntity.Id,
                DomId = dataEntity.DomId,
                Epic = dataEntity.Epic,
                Blocked = dataEntity.Blocked,
                BlockedReason = dataEntity.BlockedReason,
                ReuseDA = dataEntity.ReuseDA,
                ReuseFCP = dataEntity.ReuseFCP,
                Invoice = dataEntity.Invoice,
                ListIndex = dataEntity.ListIndex,
                ListName = dataEntity.ListName,
                Name = dataEntity.Name
            };
        }
    }
}
