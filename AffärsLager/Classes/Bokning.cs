﻿using Business.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Business.Classes
{
    public class Bokning : IBokning
    {
        [Key]
        public int BokningId { get; set; }

        [ForeignKey("ExpiditNamn")]
        public int? SkapadAv { get; set; }
        public Expidit ExpiditNamn { get; set; } = null!;
        [ForeignKey("MedlemsNamn")]
        public int? Tillhör { get; set; }
        public Medlem MedlemsNamn { get; set; } = null!;

        public virtual IList<Bok> BokadeBöcker { get; set; } = new List<Bok>();
        public DateTime StartLån { get; set; }  // Det datum som skrivs in när man först planerar att boka boken ifrån i menyval 1
        public DateTime FaktisktStartLån { get; set; } //När boken faktiskt blev upphämtad av medlem, tex om upphämtning sker någon dag efter önskat startdatum på bokningen som angavs under menyval 1.
        public DateTime ÅterTid { get; set; } //När boken ska vara tillbaka lämnad, 14 dagar efter startdatum för bokningen
        public bool UppHämtad { get; set; }
        public bool Återlämnad { get; set; }

        public void Upphämtad()
        {
            UppHämtad = true;
        }
    }
}
