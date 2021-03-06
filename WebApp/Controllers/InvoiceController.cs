﻿using Gothandy.Mvc.Navigation.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Vincente.Data.Entities;
using Vincente.Data.Tables;
using Vincente.WebApp.Models;

namespace WebApp.Controllers
{
    public class InvoiceController : BaseController
    {
        private IEnumerable<Activity> invoiceData;

        public InvoiceController(InvoiceByMonth invoiceData)
        {
            this.invoiceData = invoiceData.Query();
        }

        // GET: Invoice
        public ActionResult List()
        {
            var invoice =
                from e in invoiceData
                where e.Invoice != null && e.Month <= e.Invoice
                group e by new
                {
                    e.Invoice
                }
                into g
                orderby g.Key.Invoice descending
                select new InvoiceModel()
                {
                    Invoice = g.Key.Invoice,
                    Total = g.Sum(e => e.Billable)
                };

            return View(invoice);
        }

        public ActionResult ByEpic(int year, int month)
        {
            var invoice = new DateTime(year, month, 1);

            ViewBag.Title = invoice.ToString("MMM yyyy");
            
            ViewBag.Invoice = invoice;
            //ViewBag.CurrentCurrent = GetCurrentCurrent(invoice);
            //ViewBag.CurrentPrevious = GetCurrentPrevious(invoice);
            //ViewBag.FutureCurrent = GetFutureCurrent(invoice);
            //ViewBag.FuturePrevious = GetFuturePrevious(invoice);

            var result =
                from e in invoiceData
                where e.Invoice == invoice && e.Month <= invoice
                group e by new
                {
                    e.Invoice,
                    e.Epic
                }
                into g
                orderby g.Key.Epic
                select new Activity
                {
                    Invoice = g.Key.Invoice,
                    Epic = g.Key.Epic,
                    Billable = g.Sum(e => e.Billable)
                };

            var workdone =
                from e in invoiceData
                where e.Invoice == invoice
                group e by new
                {
                    e.Month
                } into g
                orderby g.Key.Month descending
                select new Activity
                {
                    Month = g.Key.Month,
                    Billable = g.Sum(e => e.Billable)
                };

            ViewBag.WorkDone = workdone;

            return View(result);
        }

        private decimal GetCurrentCurrent(DateTime invoice)
        {
            return
                (from e in invoiceData
                 where e.Invoice == invoice && e.Month == invoice
                 select e.Billable).Sum().GetValueOrDefault();
        }

        private decimal GetCurrentPrevious(DateTime invoice)
        {
            return
                (from e in invoiceData
                 where e.Invoice == invoice && e.Month < invoice
                 select e.Billable).Sum().GetValueOrDefault();
        }

        private decimal GetFutureCurrent(DateTime invoice)
        {
            return
                (from e in invoiceData
                 where (e.Invoice == null || e.Invoice > invoice) && e.Month == invoice
                 select e.Billable).Sum().GetValueOrDefault();
        }

        private decimal GetFuturePrevious(DateTime invoice)
        {
            return
                (from e in invoiceData
                 where (e.Invoice == null || e.Invoice > invoice) && e.Month < invoice
                 select e.Billable).Sum().GetValueOrDefault();
        }

        public ActionResult Detail(int year, int month, string epic)
        {
            var invoice = new DateTime(year, month, 1);

            ViewBag.Title = epic;

            ancestors.Add(new NavLink()
            {
                LinkText = invoice.ToString("MMM yyyy"),
                ActionName = "ByEpic",
                ControllerName = controller,
                RouteValues = new { year = year, month = month }
            });

            var result =
                from e in invoiceData
                where e.Invoice == invoice && e.Epic == epic && e.Month <= invoice
                group e by new
                {
                    e.CardId,
                    e.Invoice,
                    e.Epic,
                    e.DomId,
                    e.Name
                }
                into g
                orderby g.Key.DomId
                select new Activity()
                {
                    CardId = g.Key.CardId,
                    Invoice = g.Key.Invoice,
                    Epic = g.Key.Epic,
                    DomId = g.Key.DomId,
                    Name = g.Key.Name,
                    Billable = g.Sum(e => e.Billable)
                };

            return View(result);
        }
    }
}