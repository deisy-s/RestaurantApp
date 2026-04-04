using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace RestaurantApp
{
    public class CartItems : INotifyPropertyChanged
    {
        public int menuID {  get; set; }
        public string name { get; set; }
        public string? comments { get; set; }
        public float price { get; set; }
        public string imageUrl { get; set; }
        private int _quantity;
        public int quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
