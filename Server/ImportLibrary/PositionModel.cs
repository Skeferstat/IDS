namespace ImportLibrary;

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("Generic")]
public class PositionModel
{
    [XmlElement("fileInfo")]
    public FileInfo? FileInfo { get; set; }

    [XmlElement("header")]
    public Header? Header { get; set; }

    [XmlElement("ArtPos")]
    public List<ArticlePosition> ArticleList { get; set; }
}

public class FileInfo
{
    [XmlElement("name")]
    public string Name { get; set; }

    [XmlElement("size")]
    public string Size { get; set; }

    [XmlElement("creation")]
    public DateTime Creation { get; set; }

    [XmlElement("lastwrite")]
    public DateTime LastWrite { get; set; }

    [XmlElement("format")]
    public string Format { get; set; }
}

public class Header
{
    [XmlElement("AN_KreditorNr")]
    public string AN_KreditorNr { get; set; }

    [XmlElement("AG_DebitorNr")]
    public string AG_DebitorNr { get; set; }

    [XmlElement("DI_DP")]
    public string DI_DP { get; set; }

    [XmlElement("AN_VergNr")]
    public string AN_VergNr { get; set; }

    [XmlElement("PI_Name")]
    public string PI_Name { get; set; }

    [XmlElement("PI_Bez")]
    public string PI_Bez { get; set; }

    [XmlElement("AG_VergNr")]
    public string AG_VergNr { get; set; }

    [XmlElement("PI_Waehrung")]
    public string PI_Waehrung { get; set; }

    [XmlElement("DI_Version")]
    public string DI_Version { get; set; }

    [XmlElement("BI_SachBearb")]
    public string BI_SachBearb { get; set; }

    [XmlElement("DI_Datum")]
    public DateTime DI_Datum { get; set; }
}

public class ArticlePosition
{
    [XmlElement("PosNrHW")]
    public string PosNrHandwerker { get; set; }     // dest.RefItems.Customer

    [XmlElement("PosNrGH")]
    public string PosNrGH { get; set; }             // dest.RefItems.Supplier

    [XmlElement("ArtNr")]
    public string ArtNr { get; set; }               // dest.ArtNo

    [XmlElement("Menge")]
    public decimal Menge { get; set; }              // dest.Qty

    [XmlElement("Kurztext1")]
    public string Kurztext1 { get; set; }          // dest.Langtext

    [XmlElement("Kurztext2")]
    public string Kurztext2 { get; set; }           // dest.Kurztext

    [XmlElement("PrBrutto")]
    public decimal PrBrutto { get; set; }           // dest.OfferPrice

    [XmlElement("PrBasis")]
    public decimal PrBasis { get; set; }            // dest.PriceBasis

    [XmlElement("PrNetto")]
    public decimal PrNetto { get; set; }            // dest.NetPrice

    [XmlElement("Rabatt1")]
    public decimal Rabatt1 { get; set; }

    [XmlElement("Rabatt2")]
    public decimal Rabatt2 { get; set; }

    [XmlElement("Art")]
    public string Art { get; set; }

    [XmlElement("Einheit")]
    public string Einheit { get; set; }

    [XmlElement("PreisKZ")]
    public string PreisKZ { get; set; }

    [XmlElement("Lagerkennzeichen")]
    public string Lagerkennzeichen { get; set; }

    [XmlElement("OZ")]
    public string OZ { get; set; }
}

