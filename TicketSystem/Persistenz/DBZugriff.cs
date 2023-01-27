using MySql.Data.MySqlClient;
using System;

namespace _TicketSystem
{
  public static class DBZugriff
  {
    public static MySqlConnection OpenDB()
    {
      String conStr = System.Configuration.ConfigurationManager.ConnectionStrings["connectar"].ConnectionString;
      MySqlConnection con = new MySqlConnection(conStr);
      con.Open();

      return con;
    }
    public static void CloseDB(MySqlConnection con)
    {
      con.Close();
    }
    public static int ExcecuteNonQuery(String sql)
    {
      using (MySqlConnection con = DBZugriff.OpenDB())
      {
        MySqlCommand cmd = new MySqlCommand(sql, con);
        int anz = cmd.ExecuteNonQuery();
        return anz;
      }
    }

    public static MySqlDataReader ExecuteReader(String sql, MySqlConnection con)
    {
      MySqlCommand cmd = new MySqlCommand(sql, con);
      MySqlDataReader reader = cmd.ExecuteReader();
      return reader;
    }

    public static int GetLastInsertID(MySqlConnection con)
    {
      MySqlCommand cmd = new MySqlCommand("SELECT LAST_INSERT_ID()", con);
      int id = Convert.ToInt32(cmd.ExecuteScalar());
      return id;
    }

  }
}
