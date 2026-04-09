using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace RestaurantApp
{
    public class FullOrder : INotifyPropertyChanged
    {
        private OrderData _order;
        public OrderData order
        {
            get => _order;
            set { _order = value; OnPropertyChanged(); }
        }
        public List<OrderDetailsData> details { get; set; }
        public UserData users { get; set; }
        public bool IsNotCancelled => order.status != "Cancelado" && order.status != "Entregado";
        public bool IsInProcess => order.status == "Ordenado";
        public bool IsSent => order.status == "En preparación" && order.eatlocation == "Domicilio";
        public bool IsDelivered => order.status == "Enviado" || (order.status == "En preparación" && order.eatlocation != "Domicilio");
        public bool IsFinalized => !(order.status == "Entregado");
        public bool IsDelivery => order.eatlocation == "Domicilio";
        public int GridHeight => (details.Count * 80)+50;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public void RefreshStatus()
        {
            OnPropertyChanged(nameof(order));
            OnPropertyChanged(nameof(IsInProcess));
            OnPropertyChanged(nameof(IsSent));
            OnPropertyChanged(nameof(IsDelivered));
            OnPropertyChanged(nameof(IsNotCancelled));
            OnPropertyChanged(nameof(IsFinalized));
        }
    }
}
