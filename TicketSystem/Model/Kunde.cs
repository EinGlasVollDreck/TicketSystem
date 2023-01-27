using System.Collections.Generic;

namespace _TicketSystem
{
  public class Kunde : Person //Klasse Kunde wird von Person abgeleitet (=vererbt)
  {
    private int _id;

    public int Id
    {
      get { return _id; }
      set { _id = value; }
    }

    public void Anlegen()
    {
      DBKunde.Anlegen(this);
    }

    public static List<Kunde> Allelesen()
    {
      //Wegen der Schichtentrennung wird der Aufruf hier nur durchgereicht
      return DBKunde.Allelesen();
    }

    public override string ToString()
    {
      //return $"{Id} {Geschlecht} {Vorname} {Nachname} {GebDatum.ToShortDateString()} {Alter} Jahre";
      //return $"{Id} " + base.ToString();
      return $"{Vorname} {Nachname}";
    }
    public static Kunde GetKundeById(int suchId)
    {
      return DBKunde.GetKundeById(suchId);
    }
    public override bool Equals(object obj)
    {
      Kunde k = obj as Kunde;
      return k != null && k.Id == this.Id;
    }
  }
}
