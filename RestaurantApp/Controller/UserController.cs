using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantApp
{
    public class UserController
    {
        ConnectUser connectUser = new ConnectUser();
        public async Task<bool> saveInfo (UserData user, string address, string phone)
        {
            return await connectUser.SaveInfo(user, address, phone);
        }

        public async Task<bool> checkUserAddress(UserData client) 
        { 
            return await connectUser.CheckUserAdress(client);
        }

        public async Task<List<OrderData>> getOrders( UserData client )
        {
            return await connectUser.GetOrders(client);
        }

        public async Task<List<OrderDetailsData>> getOrderDetails( OrderData order )
        {
            return await connectUser.GetOrderDetails(order);
        } 
    }
}
