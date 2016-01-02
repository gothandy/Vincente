﻿using Gothandy.Tables.Azure;
using System.Collections.Generic;
using System.Linq;
using Vincente.Data.Entities;
using Vincente.Data.Interfaces;

namespace TrelloConsoleApp
{
    public class DataToAzure
    {
        public static Results Execute(ICardWrite table, List<Card> previousCards, List<Card> currentCards)
        {
            Results results = new Results();

            InsertReplaceOrIgnore(table, currentCards, previousCards, results);
            Delete(table, currentCards, previousCards, results);

            table.BatchComplete();

            return results;
        }
        private static void InsertReplaceOrIgnore(ICardWrite table, List<Card> currentCards, List<Card> previousCards, Results results)
        {
            foreach (Card currentCard in currentCards)
            {

                var previousCard = (from c in previousCards
                                    where c.Id == currentCard.Id
                                    select c).FirstOrDefault();

                if (previousCard == null)
                {
                    results.Inserted++;
                    table.BatchInsert(currentCard);
                }
                else if (previousCard.Equals(currentCard))
                {
                    results.Ignored++;
                }
                else
                {
                    results.Replaced++;
                    table.BatchReplace(currentCard);
                }
            }
        }

        private static void Delete(ICardWrite table, List<Card> currentCards, List<Card> previousCards, Results results)
        {
            foreach (Card previousCard in previousCards)
            {
                var currentCard =
                    (from c in currentCards
                     where c.Id == previousCard.Id
                     select c).FirstOrDefault();

                if (currentCard == null)
                {
                    results.Deleted++;
                    table.BatchDelete(previousCard);
                }
            }
        }
    }
}