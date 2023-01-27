using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace _TicketSystem
{
  public static class DBTicket
  {
    public static List<Ticket> AlleLesen()
    {
      List<Ticket> lst = new List<Ticket>();

      using (MySqlConnection con = DBZugriff.OpenDB())
      {
        String sql = "SELECT * FROM ticket";
        using (MySqlDataReader rdr = DBZugriff.ExecuteReader(sql, con))
        {
          while (rdr.Read())
          {
            Ticket t = GetDataFromReader(rdr);

            lst.Add(t);
          }
        }
      }

      return lst;
    }

    private static Ticket GetDataFromReader(MySqlDataReader rdr)
    {
      Ticket t = new Ticket();
      t.Id = rdr.GetInt32("id");
      t.Status = (TicketStatus)rdr.GetInt32("status");

      //Die Fremdschlüssel Beziehung der Datenbank wid hier in ein vollständig gefülltes
      //Kunden-Objekt überführt
      //t.Kunde.Id = rdr.GetInt32("kunde_id");
      t.Kunde = Kunde.GetKundeById(rdr.GetInt32("kunde_id"));

      t.Beschreibung = rdr.GetString("beschreibung");
      t.Zeitpunkt = rdr.GetDateTime("zeitpunkt"); //Zeitpunkt der Ticket-Erstellung
      t.Zeitstempel = rdr.GetDateTime("zeitstempel"); //Zeitpunkt der letzten Änderung
      return t;
    }

    public static void Speichern(Ticket t)
    {
      String sql = "INSERT INTO ticket (status, kunde_id, beschreibung, zeitpunkt)" +
        $" VALUES ({(int)t.Status}, {(int)t.Kunde.Id}, '{t.Beschreibung}', '{t.Zeitpunkt.ToString("yyyy-MM-dd HH:mm:ss")}')";
      DBZugriff.ExcecuteNonQuery(sql);
    }
    public static void Aendern(Ticket t)
    {
      String sql = $"UPDATE ticket SET status={(int)t.Status},beschreibung='{t.Beschreibung}', zeitpunkt='{t.Zeitpunkt.ToString("yyyy-MM-dd HH:mm:ss")}' " +
        $"WHERE id={t.Id} AND zeitstempel='{t.Zeitstempel.ToString("yyyy-MM-dd HH:mm:ss")}'";
      int anz = DBZugriff.ExcecuteNonQuery(sql);

      if (anz == 0)
      {
        // trickreich: folgende Methode löst eine Exception aus, falls das Ticket gelöscht wurde
        // die nachfolgende Zeile wird nicht mehr erreicht
        GetTicketById(t.Id);

        throw new MultiUserAccessException($"Ticket mit der ID {t.Id} wurde zwischenzeitlich geändert");
      }

    }
    public static void Loeschen(int id, Ticket t)
    {
      String sql = $"DELETE FROM ticket WHERE id={t.Id}";

      int anz = DBZugriff.ExcecuteNonQuery(sql);

      if (anz == 0)
      {
        throw new Exception($"Ticket mit der ID {t.Id} nicht gefunden");
      }
    }
    public static Ticket GetTicketById(int suchId)
    {
      using (MySqlConnection con = DBZugriff.OpenDB())
      {
        String sql = $"SELECT * FROM ticket WHERE id={suchId}";
        using (MySqlDataReader rdr = DBZugriff.ExecuteReader(sql, con))
        {
          if (rdr.Read())
          {
            Ticket t = GetDataFromReader(rdr);
            return t;
          }
          else
          {
            throw new Exception($"Ticket mit Id {suchId} existiert nicht");
          }
        }
      }
    }
  }
}
