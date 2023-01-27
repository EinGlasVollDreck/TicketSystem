using System;
using System.Windows;
using System.Windows.Controls;

namespace _TicketSystem
{
  public partial class KundeDetailDlg : Window
  {
    private Kunde _k;
    public KundeDetailDlg(Kunde k)
    {
      InitializeComponent();
      _k = k;
      this.Title = "Kunden anlegen";
    }

    private void dateAuswahl_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
    {
      tbGeburtstag.Text = dateAuswahl.Text;
    }
    private bool pruefen()
    {
      if (this.tbVorname.Text.Trim() == String.Empty)
      {
        MessageBox.Show("Der Vorname darf nicht leer sein", "TicketSystem");
        return false;
      }
      if (this.tbName.Text.Trim() == String.Empty)
      {
        MessageBox.Show("Der Name darf nicht leer sein", "TicketSystem");
        return false;
      }
      if (this.comboGeschl.SelectedItem == null)
      {
        MessageBox.Show("Das Geschlecht darf nicht leer sein", "TicketSystem");
        return false;
      }


      // Für Geb. Datum in Datepicker
      // if(dpGebdat.SelectedDate == null)
      //{
      //MessageBox.Show("Das Datum darf nicht leer sein", "TicketSystem");
      //}
      //else if (dpGebdat.SelectedDate.Value.AddYears(16) > DateTime.Today)
      //{
      //MessageBox.Show("Das Alter muss mindestens 16 betragen", "TicketSystem");
      //}
      try
      {
        DateTime dt = Convert.ToDateTime(tbGeburtstag.Text);

        if (dt.AddYears(16) > DateTime.Today)
        {
          MessageBox.Show("Das mindestalter beträgt 16 Jahre", "TicketSystem");
          return false;
        }
      }
      catch (Exception)
      {
        MessageBox.Show("Bitte korrektes Datum eingeben", "TicketSystem");
        return false;
      }
      return true;
    }
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      FillComboBoxGeschlecht();
      this.comboGeschl.SelectedItem = Gender.männlich;
    }

    private void FillComboBoxGeschlecht()
    {
      this.comboGeschl.Items.Clear();
      this.comboGeschl.Items.Add(Gender.weiblich);
      this.comboGeschl.Items.Add(Gender.männlich);
    }

    private void btnOK_Click(object sender, RoutedEventArgs e)
    {
      if (pruefen())
      {
        _k.GebDatum = Convert.ToDateTime(tbGeburtstag.Text);
        _k.Nachname = tbName.Text;
        _k.Vorname = tbVorname.Text;
        _k.Geschlecht = (Gender)comboGeschl.SelectedItem;
        _k.Anlegen();
        this.DialogResult = true;
        this.Close();
      }
    }

    private void btnAbbrechen_Click(object sender, RoutedEventArgs e)
    {
      this.DialogResult = false;
      this.Close();
    }
  }
}
