using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TopLearn.Core.DTOs
{
    public class ChargeWalletViewModel
    {
        [Display(Name = "مبلغ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
     
        public int Amount { get; set; }
    }

    public class WalletViewModel
    {
       // public int UserId { get; set; }
        public int TypeId { get; set; }


        public bool IsPay { get; set; }
        //  [Display(Name = "مبلغ")]
        // [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Amount { get; set; }

        //[Display(Name = "شرح")]
        //public bool IsPry { get; set; }

       // [Display(Name = "تایید شده")]
       // [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Description { get; set; }

      //  [Display(Name = "تاریخ و ساعت")]
        public DateTime CreateDate { get; set; }
    }
}
