namespace OnBoardLMS.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    public partial class OrderDetail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Order_Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(256)]
        public string Order_Number { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "smalldatetime")]
        public DateTime Order_Date { get; set; }

        [Key]
        [Column(Order = 3)]
        public double Order_Total { get; set; }

        [Key]
        [Column(Order = 4, TypeName = "smalldatetime")]
        public DateTime Date_Expired { get; set; }

        [Key]
        [Column(Order = 5)]
        public byte Payment_Status { get; set; }

        [Key]
        [Column(Order = 6)]
        public bool Is_Released { get; set; }

        [Key]
        [Column(Order = 7)]
        public byte Order_Type { get; set; }

        [Key]
        [Column(Order = 8)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Quantity { get; set; }

        [Key]
        [Column(Order = 9)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Product_Id { get; set; }

        public Guid? Course_Id { get; set; }

        [Key]
        [Column(Order = 10)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Company_Id { get; set; }

        [Key]
        [Column(Order = 11)]
        [StringLength(255)]
        public string Company_Name { get; set; }

        [Key]
        [Column(Order = 12)]
        [StringLength(100)]
        public string Purchaser_User_Name { get; set; }

        [Key]
        [Column(Order = 13)]
        [StringLength(303)]
        public string Purchaser_Full_Name { get; set; }

        [Key]
        [Column(Order = 14)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Purchaser_User_Id { get; set; }

        public int? Consumed { get; set; }

        public int? Series_Id { get; set; }
    }
}
