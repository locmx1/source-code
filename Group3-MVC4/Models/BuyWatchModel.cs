using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Group3_MVC4.Models;

namespace Group3_MVC4.Models
{
    public class ModelViewModel
    {
        public string Name
        {
            get;
            set;
        }
    }


    public class BuyWatchModel
    {
        public string Name { get; set; }
        public string GlassType { get; set; }
        public string CaseMeterial { get; set; }
        public string MainColor { get; set; }
        public string Images { get; set; }
        public string ThreeType { get; set; }
        public double InTransactionPrice { get; set; }
        public int TransactionType { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public string MemberId { get; set; }
        public string ModelName { get; set; }
        public string Brand { get; set; }
        public int AvailableAt { get; set; }
        public string phoneNumber { get; set; }
        
        // Treo tuong
        public string Shape { get; set; }
        public string SizeWall { get; set; }

        //Deo tay
        public string SizeWrist { get; set; }
        public bool Gender { get; set; }
        public bool WatchType { get; set; }
        public bool IsSmartWatch { get; set; }
        public int WaterProof { get; set; }

        public string ChainMaterial { get; set; }
        //Qua Lac
        public string SizePiece { get; set; }

        public List<Brand> AddListOfBrand()
        {
            using (var ctx = new WatchShopEntities())
            {
                return ctx.Brands.ToList();
            }

        }
        public SelectListItem[] WatchTypeListItems
        {
            get
            {
                return new[]
                {
                    new SelectListItem {Text = "Đeo Tay",Value = "Đeo Tay"}, 
                    new SelectListItem {Text = "Quả Lắc",Value = "Quả Lắc"},
                    new SelectListItem {Text = "Treo Tường",Value = "Treo Tường"}
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
                    new SelectListItem {Text = "Thủy Tinh Thường",Value = "Thủy Tinh Thường"},
                    new SelectListItem {Text = "Kính Cường Lực",Value = "Kính Cường Lực"}, 
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
                    new SelectListItem {Text = "Thép Không Rỉ",Value = "Thép Không Rỉ"},
                    new SelectListItem {Text = "Crom",Value = "Crom"},
                    new SelectListItem {Text = "Mạ Vàng",Value = "Mạ Vàng"}, 
                    new SelectListItem {Text = "Mạ Bạc",Value = "Mạ Bạc"}, 
                    new SelectListItem {Text = "Khác",Value = "Khác"}, 

                };
            }
        }

        public SelectListItem[] ColorListItems
        {
            get
            {
                return new[]
                {
                    new SelectListItem {Text = "Xanh Lục",Value = "Xanh Lục"}, 
                    new SelectListItem {Text = "Xanh Dương",Value = "Xanh Dương"}, 
                    new SelectListItem {Text = "Đỏ",Value = "Đỏ"},
                    new SelectListItem {Text = "Hồng",Value = "Hồng"},
                    new SelectListItem {Text = "Vàng Sẫm",Value = "Vàng Sẫm"}, 
                    new SelectListItem {Text = "Vàng Sáng",Value = "Vàng Sáng"}, 
                    new SelectListItem {Text = "Bạc",Value = "Bạc"},
                    new SelectListItem {Text = "Tím",Value = "Tím"},
                    new SelectListItem {Text = "Cam",Value = "Cam"},
                    new SelectListItem {Text = "Đen",Value = "Đen"},
                    new SelectListItem {Text = "Xám",Value = "Xám"},
                };
            }
        }

        public SelectListItem[] ChainMaterrialListItems
        {
            get
            {
                return new[]
                {
                    new SelectListItem {Text = "Da",Value = "Da"}, 
                    new SelectListItem {Text = "Kim Loai",Value = "Kim Loai"}, 
                    new SelectListItem {Text = "Hợp Kim",Value = "Hợp Kim"}, 
                    new SelectListItem {Text = "Thép Không Rỉ",Value = "Thép Không Rỉ"},
                    new SelectListItem {Text = "Crom",Value = "Crom"},
                    new SelectListItem {Text = "Mạ Vàng",Value = "Mạ Vàng"}, 
                    new SelectListItem {Text = "Mạ Bạc",Value = "Mạ Bạc"}, 
                    new SelectListItem {Text = "Khác",Value = "Khác"}, 

                };
            }
        }
        public SelectListItem[] ATMListItems
        {
            get
            {
                return new[]
                {
                    new SelectListItem {Text = "0 ATM",Value = "0"}, 
                    new SelectListItem {Text = "1 ATM",Value = "1"}, 
                    new SelectListItem {Text = "3 ATM",Value = "3"}, 
                    new SelectListItem {Text = "5 ATM",Value = "5"}, 
                    new SelectListItem {Text = "10 ATM",Value = "10"}, 
                    new SelectListItem {Text = "15 ATM",Value = "15"}, 
                    new SelectListItem {Text = "20 ATM",Value = "20"}, 

                };
            }
        }
    }
}