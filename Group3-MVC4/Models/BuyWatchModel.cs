using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Group3_MVC4.Models;

namespace Group3_MVC4.Models
{
    public class BuyWatchModel
    {

        public string Name { get; set; }
        public string GlassType { get; set; }
        public string CaseMeterial { get; set; }
        public string MainColor { get; set; }
        public string Images { get; set; }
        public double InTransactionPrice { get; set; }
        public int TransactionType { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public string MemberId { get; set; }
        public int ModelId { get; set; }
        public int AvailableAt { get; set; }

        
        public SelectListItem[] BrandListItems
        {
            get
            {
                return new []
                {
                    new SelectListItem {Text = "Rolex",Value = "Rolex"}, 
                    new SelectListItem {Text = "Omega",Value = "Omega"}, 
                    new SelectListItem {Text = "Casino",Value = "Casino"}, 
                    new SelectListItem {Text = "Khác",Value = "Khác"}, 
                };
            }
        }

        public SelectListItem[] ModelListItems
        {
            get
            {
                return new[]
                {
                    new SelectListItem {Text = "Model001",Value = "1"}, 
                    new SelectListItem {Text = "Model002",Value = "2"}, 
                    new SelectListItem {Text = "Model003",Value = "3"}, 
                    new SelectListItem {Text = "Khác",Value = "4"}, 
                };
            }
        }

        public SelectListItem[] GrassMaterialListItems
        {
            get
            {
                return new[]
                {
                    new SelectListItem {Text = "Sapphere",Value = "Sapphere"}, 
                    new SelectListItem {Text = "Thủy Tinh",Value = "Thủy Tinh"}, 
                    new SelectListItem {Text = "Nhựa",Value = "Nhựa"}, 
                    new SelectListItem {Text = "Khác",Value = "Khác"}, 
                };
            }
        }

        public SelectListItem[] GrassShapeListItems
        {
            get
            {
                return new[]
                {
                    new SelectListItem {Text = "Vuông",Value = "Vuông"}, 
                    new SelectListItem {Text = "Tròn",Value = "Tròn"}, 
                    new SelectListItem {Text = "Khác",Value = "Khác"}, 
                };
            }
        }
        public SelectListItem[] CaseMaterrialListItems
        {
            get
            {
                return new[]
                {
                    new SelectListItem {Text = "Kim Loai",Value = "Kim Loai"}, 
                    new SelectListItem {Text = "Hợp Kim",Value = "Hợp Kim"}, 
                    new SelectListItem {Text = "Mạ Vàng",Value = "Mạ Vàng"}, 
                };
            }
        }



    }
}