﻿using System.Collections.Generic;
using System.Linq;
using Vincente.Data.Entities;
using Vincente.Formula;
using Vincente.Trello.DataObjects;

namespace Vincente.TrelloConsoleApp.Operations
{

    public class TrelloToData
    {
        public static List<Card> Execute(List<TrelloCard> trelloCards, List<Label> labels, List<List> lists)
        {
            return
                (from trelloCard in trelloCards
                 select GetDataFromTrello(trelloCard, labels, lists)).ToList();
        }

        private static Card GetDataFromTrello(TrelloCard card, List<Label> labels, List<List> lists)
        {
            var list = List.GetList(card.IdList, lists);
            var listIndex = lists.IndexOf(list);
            var listName = list.Name;
            var nameLabels = Label.GetNameLabels(card.IdLabels, labels);
            var cardName = card.Name;
            var cardId = card.Id;

            return
                new Vincente.Data.Entities.Card()
                {
                    DomId = FromName.GetDomID(cardName),
                    Id = card.Id,
                    ListIndex = listIndex,
                    ListName = listName,
                    Name = FromName.GetShortName(cardName),
                    Epic = FromLabels.GetEpic(nameLabels),
                    Invoice = FromLabels.GetInvoice(nameLabels, listName)
                };
        }
    }


}