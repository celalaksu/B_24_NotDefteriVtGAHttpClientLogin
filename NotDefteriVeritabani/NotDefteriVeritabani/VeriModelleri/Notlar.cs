using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotDefteriVeritabani.VeriModelleri
{
    //[Table("notlar")]
    public class Notlar
    {
        //[PrimaryKey, AutoIncrement, Column("id")]
        public string ID { get; set; }
        //[MaxLength(250), Unique]
        public string NotMetin { get; set; }
        public DateTime NotTarih { get; set; }

        public string Eposta { get; set; }

    }
}
