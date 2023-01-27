using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace _TicketSystem
{
  //Eine statische Klasse, hier dürfen klassen nur statisch sein!
  public static class DBKunde
  {

    public static void Anlegen(Kunde k)
    {
      using (MySqlConnection con = DBZugriff.OpenDB())
      {
        String sql = "INSERT INTO kunde (geschlecht, vorname, nachname, geburtstag) " +
        $"VALUES ({Convert.ToInt32(k.Geschlecht)}, '{k.Vorname}', '{k.Nachname}', '{k.GebDatum.ToString("yyyy-MM-dd")}')";

        MySqlCommand cmd = new MySqlCommand(sql, con);
        cmd.ExecuteNonQuery();

        k.Id = DBZugriff.GetLastInsertID(con);
      }
    }

    public static List<Kunde> Allelesen()
    {
      List<Kunde> lst = new List<Kunde>();

      using (MySqlConnection con = DBZugriff.OpenDB())
      {
        String sql = "SELECT * FROM kunde";
        using (MySqlDataReader rdr = DBZugriff.ExecuteReader(sql, con))
        {
          while (rdr.Read())
          {
            Kunde k = GetDataFromReader(rdr);
            lst.Add(k);
          }
        }
      }
      return lst;
    }

    private static Kunde GetDataFromReader(MySqlDataReader rdr)
    {
      Kunde k = new Kunde();
      k.Id = rdr.GetInt32("id");
      k.Geschlecht = (Gender)rdr.GetInt32("geschlecht");
      k.Vorname = rdr.GetString("vorname");
      k.Nachname = rdr.GetString("nachname");
      k.GebDatum = rdr.GetDateTime("geburtstag");
      return k;
    }

    public static Kunde GetKundeById(int suchId)
    {
      using (MySqlConnection con = DBZugriff.OpenDB())
      {
        String sql = $"SELECT * FROM kunde WHERE id={suchId}";
        using (MySqlDataReader rdr = DBZugriff.ExecuteReader(sql, con))
        {
          if (rdr.Read())
          {
            Kunde k = GetDataFromReader(rdr);
            return k;
          }
          else
          {
            throw new Exception($"Kunde mit Id {suchId} existiert nicht");
          }
        }
      }
    }
  }
}
