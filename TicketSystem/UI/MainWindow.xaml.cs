using System.Collections.Generic;
using System.Windows;

namespace _TicketSystem
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      FillListbox();
    }
    private void FillListbox()
    {
      lbTickets.Items.Clear();
      List<Ticket> lst = Ticket.AlleLesen();
      foreach (Ticket t in lst)
      {
        lbTickets.Items.Add(t);
      }
    }
    private void btLoeschen_Click(object sender, RoutedEventArgs e)
    {
      MessageBoxResult loes = MessageBox.Show("Wollen Sie das ausgwählte Ticket wirklich löschen?", "TicketSystem", MessageBoxButton.YesNo, MessageBoxImage.Question);

      if (loes == MessageBoxResult.No)
        return;
      else
      {
        Ticket t = (Ticket)lbTickets.SelectedItem;
        t.Loeschen(lbTickets.SelectedIndex);

        //Entweder so:
        //lbTickets.Items.Remove(t);

        //Oder so:
        FillListbox();
      }
    }
    private void lbTickets_SelectionChanged(object sender, RoutedEventArgs e)
    {

      //if(lbTickets.SelectedItem != null)
      //{
      //  this.btAend.IsEnabled = false;
      //  this.btLoesch.IsEnabled = false;
      //}
      //else
      //{
      //  this.btAend.IsEnabled = true;
      //  this.btLoesch.IsEnabled = true;
      //}
      //Kürzere Schreibweise:
      this.btAend.IsEnabled = lbTickets.SelectedItem != null;
      this.btLoesch.IsEnabled = lbTickets.SelectedItem != null;
    }

    private void btEnd_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

    private void btNeu_Click(object sender, RoutedEventArgs e)
    {
      Ticket t = new Ticket();
      TicketDetailDlg dlg = new TicketDetailDlg(t, DialogMode.Neu);
      //dlg.ShowDialog() öffnet den Dialog modal (=Hauptfenster ist nicht bedienbar)

      //Der Rückgabewert von ShowDialog ist identisch mit dem 
      // im TicketDetailDlg gesetztem Wert für "DialogResult"
      bool? res = dlg.ShowDialog();

      if (res == true)
        FillListbox();

      //dlg.Show() würde den Dialog nicht-modal öffnen
    }

    private void btAend_Click(object sender, RoutedEventArgs e)
    {
      Ticket t = (Ticket)lbTickets.SelectedItem;
      TicketDetailDlg dlg = new TicketDetailDlg(t, DialogMode.Bearbeiten);
      dlg.ShowDialog();
      FillListbox();
    }
  }
}
