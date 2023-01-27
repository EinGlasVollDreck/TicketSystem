using System;
using System.Collections.Generic;

namespace _TicketSystem
{
  public enum TicketStatus { Neu, InBearbeitung, Geloest };
  public class Ticket
  {
    private int _id;

    public int Id
    {
      get { return _id; }
      set { _id = value; }
    }

    private TicketStatus _status;

    public TicketStatus Status
    {
      get { return _status; }
      set { _status = value; }
    }

    private Kunde _kunde;

    public Kunde Kunde
    {
      get { return _kunde; }
      set { _kunde = value; }
    }

    private string _beschreibung;

    public string Beschreibung
    {
      get { return _beschreibung; }
      set { _beschreibung = value; }
    }

    private DateTime _zeitpunkt;

    public DateTime Zeitpunkt
    {
      get { return _zeitpunkt; }
      set { _zeitpunkt = value; }
    }

    //Zeitpunkt der letzten Änderung
    public DateTime Zeitstempel { get; set; }

    public Ticket(int id, TicketStatus status, Kunde kunde, string beschreibung, DateTime zeitpunkt)
    {
      Id = id;
      Status = status;
      Kunde = kunde;
      Beschreibung = beschreibung;
      Zeitpunkt = zeitpunkt;
    }

    public Ticket()
    {
      //Default für ein neues ticket
      this.Zeitpunkt = DateTime.Now;
      this.Status = TicketStatus.Neu;
    }

    public void Speichern()
    {
      DBTicket.Speichern(this);
    }

    public void Loeschen(int id)
    {
      DBTicket.Loeschen(id, this);
    }
    public static List<Ticket> AlleLesen()
    {
      return DBTicket.AlleLesen();
    }
    public void Aendern()
    {
      DBTicket.Aendern(this);
    }
    public static Ticket GetTicketById(int suchId)
    {
      return DBTicket.GetTicketById(suchId);
    }
    public override string ToString()
    {
      // Möglichkeit 1: Verwendung der ToString()-Methode von Kunde
      // return $"{this.Id}  {this.Zeitpunkt} {this.Kunde} {this.Beschreibung} {this.Status}";

      // Möglichkeit 2: Nur Nachname, Vorname des Kunden
      //return $"{this.Id.ToString().PadLeft(3)}  {this.Zeitpunkt.ToShortDateString()} {this.Status.ToString()} {this.Kunde.Nachname.PadLeft(20)} {this.Beschreibung}";
      return $"{this.Id,3}  {this.Zeitpunkt.ToShortDateString()} {this.Status,-13} {this.Kunde.Nachname,-20} {this.Beschreibung}";
    }

    //Vom Visual Studio generierte Equals Methode:
    //public override bool Equals(object obj)
    //{
    //  return obj is Ticket ticket && Id == ticket.Id;
    //}

    public override bool Equals(object obj)
    {
      //Cast Variante 1: wirft Exception, falls obj nicht auf Ticket Objekt zeigt
      //Ticket t = (Ticket)obj;

      //Cast Variante 2: setzt t auf null, falls obj nicht auf Ticket Objekt zeigt
      Ticket t = obj as Ticket;

      if (t == null)
        return false;

      //Wir legen hier fest, dass zwei Ticket Objekte dann "logisch gleich" sind
      //wenn deren ID übereinstimmt
      if (this.Id == t.Id)
        return true;
      else
        return false;

      //Kurzschreibweise:
      //return t != null && t.Id == this.Id;
    }
  }
}
