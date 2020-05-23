using System;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml.Serialization;

namespace assignment3
{
    [XmlRoot("SeatList")]
    public class Seat
    {
        private string seatName;
        private string customerName;
        [XmlIgnore]
        private Button btnSeat;
        private Boolean isReserved;
        private string btnSeatString;

        public Seat()
        {
            this.seatName = "";
            this.customerName = "";
            this.btnSeat = new Button();
            this.btnSeat.Background = new SolidColorBrush(Colors.LightGreen);
            this.isReserved = false;
            this.btnSeatString = XamlWriter.Save(this.btnSeat);
        }
        public Seat(String seatName, String customerName, Button btnSeat) 
        {
            this.seatName = seatName;
            this.customerName = customerName;
            this.btnSeat = btnSeat;
            this.btnSeat.Content = seatName;
            this.btnSeat.Background = new SolidColorBrush(Colors.LightGreen);
            this.btnSeatString = XamlWriter.Save(btnSeat);
        }
        public String SeatName
        {
            get => seatName;
            set => seatName = value; 
        }
        public String CustomerName
        {
            get => customerName;
            set => customerName = value;
        }
        [XmlIgnore]
        public Button BtnSeat
        {
            get => btnSeat;
            set => btnSeat = value;
        }
        public Boolean IsReserved
        {
            get => isReserved;
            set => isReserved = value;
        }
        public String BtnSeatString
        {
            get => btnSeatString;
            set => btnSeatString = value;
        }
        public void ReserveSeat(string customerName)
        {
            this.CustomerName = customerName;
            this.BtnSeat.Content = customerName;
            this.IsReserved = true;
            this.BtnSeat.Background = new SolidColorBrush(Colors.LightPink);
            this.BtnSeatString = XamlWriter.Save(this.BtnSeat);
        }
        public void RemoveReservation() 
        {
            this.customerName = "";
            this.btnSeat.Content = this.seatName;
            this.isReserved = false;
            this.BtnSeat.Background = new SolidColorBrush(Colors.LightGreen);
            this.btnSeatString = XamlWriter.Save(this.BtnSeat);
        }
    }
} 
