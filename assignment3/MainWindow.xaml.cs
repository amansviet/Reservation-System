using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using System.Xml.Serialization;

namespace assignment3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Seat> allSeats = new List<Seat>();
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                // Initializing list of all seats in the application
                Seat seatA = new Seat("SeatA", "", btnSeatA);
                allSeats.Add(seatA);
                Seat seatB = new Seat("SeatB", "", btnSeatB);
                allSeats.Add(seatB);
                Seat seatC = new Seat("SeatC", "", btnSeatC);
                allSeats.Add(seatC);
                Seat seatD = new Seat("SeatD", "", btnSeatD);
                allSeats.Add(seatD);
                Seat seatE = new Seat("SeatE", "", btnSeatE);
                allSeats.Add(seatE);
                Seat seatF = new Seat("SeatF", "", btnSeatF);
                allSeats.Add(seatF);
                Seat seatG = new Seat("SeatG", "", btnSeatG);
                allSeats.Add(seatG);
                Seat seatH = new Seat("SeatH", "", btnSeatH);
                allSeats.Add(seatH);
                Seat seatI = new Seat("SeatI", "", btnSeatI);
                allSeats.Add(seatI);
                Seat seatJ = new Seat("SeatJ", "", btnSeatJ);
                allSeats.Add(seatJ);
                Seat seatK = new Seat("SeatK", "", btnSeatK);
                allSeats.Add(seatK);
                Seat seatL = new Seat("SeatL", "", btnSeatL);
                allSeats.Add(seatL);
                Seat seatM = new Seat("SeatM", "", btnSeatM);
                allSeats.Add(seatM);
                Seat seatN = new Seat("SeatN", "", btnSeatN);
                allSeats.Add(seatN);
                Seat seatO = new Seat("SeatO", "", btnSeatO);
                allSeats.Add(seatO);
                Seat seatP = new Seat("SeatP", "", btnSeatP);
                allSeats.Add(seatP);
                Seat seatQ = new Seat("SeatQ", "", btnSeatQ);
                allSeats.Add(seatQ);
                Seat seatR = new Seat("SeatR", "", btnSeatR);
                allSeats.Add(seatR);
                Seat seatS = new Seat("SeatS", "", btnSeatS);
                allSeats.Add(seatS);
                Seat seatT = new Seat("SeatT", "", btnSeatT);
                allSeats.Add(seatT);
            }
            catch (Exception exception) {
                tbNotifications.Text = $" Exception: {exception.Message}\nPlease close and reload the application";
                SetStatus("failed");
            }
        }
        private void BtnReserveSeat_Click(object sender, RoutedEventArgs e)
        {
            try {
                // Setting notification text to empty string
                tbNotifications.Text = "";
                // Check if all seats are reserved before starting to reserve a desired seat for user
                if (AreAllSeatsReserved())
                {
                    tbNotifications.Text = "All seats are already reserved, no more seats available";
                    SetStatus("failed");
                    return;
                }

                if (string.IsNullOrWhiteSpace(tbCustomerName.Text))
                {
                    tbNotifications.Text = "Customer Name cannot be blank";
                    SetStatus("failed");
                    return;
                }

                if (cbSeatNumber.SelectedIndex == -1)
                {
                    tbNotifications.Text = "Please select an available seat from the list";
                    SetStatus("failed");
                    return;
                }

                int selectedSeatIndex = cbSeatNumber.SelectedIndex;
                String selectedSeatName = cbSeatNumber.SelectionBoxItem.ToString();
                string titleCaseCustomerName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tbCustomerName.Text.ToLower());
                if (allSeats.ElementAt(selectedSeatIndex).IsReserved)
                {
                    tbNotifications.Text = $"{allSeats.ElementAt(selectedSeatIndex).SeatName} is already reserved for {allSeats.ElementAt(selectedSeatIndex).CustomerName}";
                    SetStatus("failed");
                }
                else
                {
                    cbSeatNumber.Items[selectedSeatIndex] = $"{selectedSeatName} (reserved)";
                    allSeats.ElementAt(selectedSeatIndex).ReserveSeat(titleCaseCustomerName);
                    tbNotifications.Text = $"{allSeats.ElementAt(selectedSeatIndex).SeatName} has been reserved for {titleCaseCustomerName}";
                    SetSeatBtnContext();
                    SetStatus("success");
                    
                }
            } catch (Exception exception) 
            {
                SetStatus("failed");
                tbNotifications.Text = exception.Message;
            }
        }
        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                // Setting notification text to empty string
                tbNotifications.Text = "";
                if ((tbCustomerName.Text != "" && cbSeatNumber.SelectedIndex != -1) || (string.IsNullOrWhiteSpace(tbCustomerName.Text) && cbSeatNumber.SelectedIndex == -1))
                {
                    // notify the user if both name and seat numbers are given to remove a reservation
                    tbNotifications.Text = "To remove a reservation, please enter either Customer Name or Seat Number, but not both.";
                    SetStatus("failed");
                    return;
                }

                if (cbSeatNumber.SelectedIndex != -1)
                {
                    int selectedSeatIndex = cbSeatNumber.SelectedIndex;
                    if (allSeats.ElementAt(selectedSeatIndex).IsReserved)
                    {
                        allSeats.ElementAt(selectedSeatIndex).RemoveReservation();
                        tbNotifications.Text = $"Reservation removed for {allSeats.ElementAt(selectedSeatIndex).SeatName}";
                        SetStatus("success");

                        // Reset the combobox selected item name to its original name
                        cbSeatNumber.Items[selectedSeatIndex] = $"{allSeats.ElementAt(selectedSeatIndex).SeatName}";
                    }
                    else
                    {
                        tbNotifications.Text = $"{allSeats.ElementAt(selectedSeatIndex).SeatName} is not reserved, cannot be unreserved";
                        SetStatus("failed");
                    }
                }

                if (tbCustomerName.Text != "")
                {
                    Seat seatToUnreserve = null;
                    int indexSeatToUnreserve = 0;
                    string titleCaseCustomerName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tbCustomerName.Text.ToLower());

                    foreach (Seat seat in allSeats)
                    {
                        if (seat.CustomerName == titleCaseCustomerName)
                        {
                            seatToUnreserve = seat;
                            indexSeatToUnreserve = allSeats.IndexOf(seat);
                            break;
                        }
                    }

                    if (seatToUnreserve is null)
                    {
                        tbNotifications.Text = $"No reservation found for {titleCaseCustomerName}";
                        SetStatus("failed");
                    }
                    else
                    {
                        // Reset the combobox selected item name to its original name
                        cbSeatNumber.Items[indexSeatToUnreserve] = $"{allSeats.ElementAt(indexSeatToUnreserve).SeatName}";

                        seatToUnreserve.RemoveReservation();
                        tbNotifications.Text = $"Reservation removed for {titleCaseCustomerName} from {allSeats.ElementAt(indexSeatToUnreserve).SeatName}";
                        SetStatus("success");
                    }
                }
                SetSeatBtnContext();
            }
            catch (Exception exception)
            {
                SetStatus("failed");
                tbNotifications.Text = $"Exception: {exception.Message}\nPlease Try Again !!!";
            }
        }
        private void BtnClearAllSeats_Click(object sender, RoutedEventArgs e)
        {
            try {
                bool isAtleastOneSeatReserved = false;
               
                foreach (Seat seat in allSeats)
                {
                    if (seat.IsReserved)
                    {
                        isAtleastOneSeatReserved = true;
                        // Reset the combobox selected item name to its original name
                        int comboBoxItemIndexToReset = allSeats.IndexOf(seat);
                        cbSeatNumber.Items[comboBoxItemIndexToReset] = $"{allSeats.ElementAt(comboBoxItemIndexToReset).SeatName}";
                        seat.RemoveReservation();
                    }
                }
                if (isAtleastOneSeatReserved)
                {
                    SetStatus("success");
                    tbNotifications.Text = "All seat reservations have been cleared";
                }
                else
                {
                    SetStatus("info");
                    tbNotifications.Text = "No seat has been cleared as all seats are already available";
                }
                SetSeatBtnContext();
            } 
            catch (Exception exception)
            {
                SetStatus("failed");
                tbNotifications.Text = $"Exception: {exception.Message}\nPlease Try Again !!!";
            }
        }
        private void SetStatus(string status)
        {
            if (status == "failed")
            {
                lblResult.Content = "Error";
                lblResult.Background = new SolidColorBrush(Colors.OrangeRed);
            }
            else if (status == "success")
            {
                lblResult.Content = "Success";
                lblResult.Background = new SolidColorBrush(Colors.Green);
            }
            else
            {
                lblResult.Content = "Result";
                lblResult.Background = new SolidColorBrush(Colors.Yellow);
            }

            // clean up the user input after reservation is done
            cbSeatNumber.SelectedIndex = -1;
            tbCustomerName.Text = "";
        }
        private bool AreAllSeatsReserved()
        {
            foreach (Seat seat in allSeats)
            {
                if (!seat.IsReserved)
                {
                    return false;
                }
            }
            return true;
        }
        private void CbLinqQueries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Using switch to select index of the selected item from the combobox
            switch (cbLinqQuery.SelectedIndex.ToString())
            {
                case "1":
                    LinqQuery1();
                    break;
                case "2":
                    LinqQuery2();
                    break;
                case "3":
                    LinqQuery3();
                    break;
            }
            cbLinqQuery.SelectedIndex = 0;
        }
        private void LinqQuery1()
        {
            try {
                var query = from seat in allSeats
                            where seat.IsReserved is true
                            orderby seat.CustomerName descending
                            select seat;
                if (query.Count() == 0)
                {
                    tbNotifications.Text = "No Reservations found, Nothing returned for LINQ#1";
                    SetStatus("info");
                    return;
                }
                else 
                {
                    var tbNotificationStringBuilder = new System.Text.StringBuilder();
                    String resultString = "Results for LINQ #1:\nList of all customer names Z to A  ";
                    tbNotificationStringBuilder.AppendLine(resultString);
                    foreach (Seat seat in query)
                    {
                        tbNotificationStringBuilder.AppendLine(seat.CustomerName);
                    }
                    tbNotifications.Text = tbNotificationStringBuilder.ToString();
                    SetStatus("success");
                }
            }
            catch (Exception exception)
            {
                SetStatus("failed");
                tbNotifications.Text = $"Exception: {exception.Message}\nPlease Try Again !!!";
            }
        }
        private void LinqQuery2()
        {
            try {
                var query = from seat in allSeats
                            where seat.IsReserved is true
                            orderby seat.CustomerName.Length
                            select seat;
                if (query.Count() == 0)
                {
                    tbNotifications.Text = "No Reservations found, Nothing returned for LINQ#2";
                    SetStatus("info");
                    return;
                }
                else 
                {
                    var tbNotificationStringBuilder = new System.Text.StringBuilder();
                    String resultString = "Results for LINQ #2:\nList of all customer names, shortest to longest";
                    tbNotificationStringBuilder.AppendLine(resultString);
                    foreach (Seat seat in query)
                    {
                        tbNotificationStringBuilder.AppendLine(seat.CustomerName);
                    }
                    tbNotifications.Text = tbNotificationStringBuilder.ToString();
                    SetStatus("success");
                }
            }
            catch (Exception exception)
            {
                SetStatus("failed");
                tbNotifications.Text = $"Exception: {exception.Message}\nPlease Try Again !!!";
            }
        }
        private void LinqQuery3()
        {
            try
            {
                var query = from seat in allSeats
                            where seat.IsReserved is false
                            orderby seat.SeatName ascending
                            select seat;
                if (query.Count() == 0)
                {
                    tbNotifications.Text = "No unreserved seat found, Nothing returned for LINQ#3";
                    SetStatus("info");
                    return;
                }
                else
                {
                    var tbNotificationStringBuilder = new System.Text.StringBuilder();
                    String resultString = "Results for LINQ #3:\nList of all unreserved seats";
                    tbNotificationStringBuilder.AppendLine(resultString);
                    foreach (Seat seat in query)
                    {
                        tbNotificationStringBuilder.AppendLine(seat.SeatName);
                    }
                    tbNotifications.Text = tbNotificationStringBuilder.ToString();
                    SetStatus("success");
                }
            }
            catch (Exception exception)
            {
                SetStatus("failed");
                tbNotifications.Text = $"Exception: {exception.Message}\nPlease Try Again !!!";
            }
        }
        private void MnuLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {   
                // Check whethere a valid xml file exists in bin/debug folder of the application
                string xmlFileToRead = "seating_list.xml";
                if (!File.Exists(xmlFileToRead))
                {
                    SetStatus("failed");
                    tbNotifications.Text = $"File {xmlFileToRead} does not exist or is not a valid xml file\nIf you havent saved seat reservations yet, please try saving first";
                    return;
                }

                this.allSeats = null;
                // Desrialize the xml file into a SeatList object
                XmlSerializer serializer = new XmlSerializer(typeof(List<Seat>));
                StreamReader streamReader = new StreamReader(xmlFileToRead);
                allSeats = (List<Seat>)serializer.Deserialize(streamReader);
                streamReader.Close();

                SetSeatBtnContext();
                
                SetStatus("success");
                tbNotifications.Text = "Seating plan loaded successfully.\nPlease check the xml file at /bin/debug/seating_list.xml";
            }
            catch (Exception exception)
            {
                SetStatus("failed");
                tbNotifications.Text = $"Exception: {exception.Message}\n\nLoading of seating plan failed.\nPlease Try Again !!!";
            }
        }
        private void MnuSave_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                TextWriter writer = new StreamWriter("seating_list.xml");
                XmlSerializer serializer = new XmlSerializer(typeof(List<Seat>));
                serializer.Serialize(writer, allSeats);
                writer.Close();

                SetStatus("success");
                tbNotifications.Text = "Seating plan saved successfully.\n\nPlease check the xml file at /bin/debug/seating_list.xml";
            }
            catch (Exception exception)
            {
                SetStatus("failed");
                tbNotifications.Text = $"Exception: {exception.Message}\n\nSaving seating plan failed.\nPlease Try Again !!!";
            }
        }
        private void SetSeatBtnContext() 
        {
            foreach (Seat seat in allSeats)
            {
                Button btnSeat = (Button)this.FindName($"btn{seat.SeatName}");
                if (seat.IsReserved)
                {
                    btnSeat.Background = new SolidColorBrush(Colors.LightPink);
                    btnSeat.Content = seat.CustomerName;
                    cbSeatNumber.Items[allSeats.IndexOf(seat)] = $"{seat.SeatName} (reserved)";
                }
                else
                {
                    btnSeat.Background = new SolidColorBrush(Colors.LightGreen);
                    btnSeat.Content = seat.SeatName;
                    cbSeatNumber.Items[allSeats.IndexOf(seat)] = $"{seat.SeatName}";
                }
            }
        }
    }
}
