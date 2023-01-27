using System;
using System.Collections.Generic;
using System.Windows;

namespace _TicketSystem
{
  /// <summary>
  /// Interaktionslogik für TicketDetailDlg.xaml
  /// </summary>
  //
  public enum DialogMode { Neu, Bearbeiten }
  public partial class TicketDetailDlg : Window
  {
    private Ticket _ticket;
    private DialogMode mode;
    public TicketDetailDlg(Ticket t, DialogMode mode)
    {
      this._ticket = t;
      this.mode = mode;
      InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      addKunde.IsEnabled = mode == DialogMode.Neu;
      this.Title = DialogMode.Bearbeiten == mode ? "Ticket bearbeiten" : "Neues Ticket anlegen";
      if (DialogMode.Bearbeiten == mode)
      {
        this.tbBeschreibung.Text = _ticket.Beschreibung;
        this.comboKunde.SelectedItem = _ticket.Kunde;
        this.comboKunde.IsEnabled = false;
      }
      else
      {
        this.comboStatus.IsEnabled = false;
      }
      this.lbDate.Content = _ticket.Zeitpunkt;
      this.comboStatus.SelectedItem = _ticket.Status;
      FillComboBoxKunde();
      FillComboBoxStatus();
    }
    private void FillComboBoxKunde()
    {
      comboKunde.Items.Clear();
      List<Kunde> lst = Kunde.Allelesen();
      foreach (Kunde k in lst)
      {
        comboKunde.Items.Add(k);
      }
    }
    private void FillComboBoxStatus()
    {
      comboStatus.Items.Clear();
      comboStatus.Items.Add(TicketStatus.Neu);
      comboStatus.Items.Add(TicketStatus.InBearbeitung);
      comboStatus.Items.Add(TicketStatus.Geloest);
    }

    private void btOk_Click(object sender, RoutedEventArgs e)
    {
      //Hier noch Fehlerprüfung (Kunde ausgewählt? Beschreibung eingegeben?)
      //-> MessageBox wenn Fehler

      //if(tbBeschreibung.Text.Trim() == "")
      if (String.IsNullOrWhiteSpace(tbBeschreibung.Text))
      {
        MessageBox.Show("Bitte fügen Sie eine Beschreibung hinzu!", "TicketSystem", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      if (this.comboKunde.SelectedItem == null)
      {
        MessageBox.Show("Bitte wählen Sie einen Kunden aus!", "TicketSystem", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
      }

      _ticket.Status = (TicketStatus)comboStatus.SelectedItem;
      _ticket.Kunde = (Kunde)comboKunde.SelectedItem;
      _ticket.Beschreibung = tbBeschreibung.Text;
      if (DialogMode.Neu == mode)
      {

        _ticket.Speichern();
      }
      else
      {
        try
        {
          this._ticket.Aendern();
        }
        catch (MultiUserAccessException ex)
        {
          MessageBoxResult res = MessageBox.Show(ex.Message + "\n\nNeu laden?", "TicketSystem", MessageBoxButton.YesNo, MessageBoxImage.Question);
          if (res == MessageBoxResult.Yes)
          {
            this._ticket = Ticket.GetTicketById(this._ticket.Id);
            SetDataInUI();
            return;
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show($"Fehler beim Speichern\n\n{ex.Message}", "TicketSystem", MessageBoxButton.OK, MessageBoxImage.Error);
        }
      }
      this.DialogResult = true;
      this.Close();
    }
    private void SetDataInUI()
    {
      comboStatus.SelectedItem = this._ticket.Status;
      tbBeschreibung.Text =this._ticket.Beschreibung;
      lbDate.Content = this._ticket.Zeitpunkt;
    }

    private void addKunde_Click(object sender, RoutedEventArgs e)
    {
      Kunde k = new Kunde();
      KundeDetailDlg dlg = new KundeDetailDlg(k);
      bool? res = dlg.ShowDialog();
      if (res == true)
      {
        FillComboBoxKunde();
        this.comboKunde.SelectedItem = k;

      }
    }

    private void btAbbrechen_Click(object sender, RoutedEventArgs e)
    {
      this.DialogResult = false;
      this.Close();
    }
  }
}
