using System;
using System.Collections.Generic;
using System.Text;
using static Android.Graphics.ColorSpace;
using static Supabase.Postgrest.Constants;

namespace RestaurantApp
{
    public class ConnectAdmin : ConnectSupabase
    {
        public async Task<bool> SaveMenu(MenuData menu)
        {
            try
            {
                await Connect();

                await _client.From<MenuData>().Insert(menu);
            }
            catch (Exception ex)
            {
                sError = ex.Message;
                return false;
            }
            return true;
        }

        public async Task<string> UploadImage(FileResult file)
        {
            try
            {
                await Connect();

                // Convertir FileResult a bytes
                var stream = await file.OpenReadAsync();
                byte[] imageData;
                using (MemoryStream ms = new MemoryStream())
                {
                    await stream.CopyToAsync(ms);
                    imageData = ms.ToArray();
                }

                // Nombre único para evitar colisiones
                string fileName = $"{Guid.NewGuid()}_{file.FileName}";

                // Subir al bucket 'menu-images'
                await _client.Storage.From("menu_images").Upload(imageData, fileName);

                // Obtener la URL pública final
                string publicUrl = _client.Storage.From("menu_images").GetPublicUrl(fileName);

                return publicUrl;
            }
            catch (Exception ex)
            {
                sError = ex.Message;
                return null;
            }
        }

        public async Task<bool> UpdateMenu(MenuData menu, string ogName)
        {
            try
            {
                await Connect();

                var result = await _client.From<MenuData>().Where(x=>x.Name == ogName).Get();
                var newID = result.Models.FirstOrDefault();

                if(newID != null)
                {
                    menu.id = newID.id;
                }

                await _client.From<MenuData>().Where(x => x.id == menu.id).Update(menu);
            }
            catch (Exception ex)
            {
                sError = ex.Message;
                return false;
            }
            return true;
        }

        public async Task<List<OrderData>> GetOrders(DateTime dtToday)
        {
            try
            {
                await Connect();

                DateTime startOfDay = dtToday.Date.ToUniversalTime(); // Inicio del dia (00:00:00)
                DateTime endOfDay = startOfDay.AddDays(1).AddTicks(-1); // Fin del dia (23:59:59.9999999)

                var result = await _client.From<OrderData>().Where(x => x.date >= startOfDay).Where(x => x.date <= endOfDay).Order(x => x.id, Ordering.Ascending).Get();

                foreach(var order in result.Models)
                {
                    DateTime local = order.date.ToLocalTime();
                    order.date = local;
                }

                return result.Models;

            }
            catch (Exception ex)
            {
                sError = ex.Message;
                return null;
            }
        }

        public async Task<List<OrderData>> GetOrdersWeek(DateTime dtToday)
        {
            try
            {
                await Connect();

                DateTime startOfDay = dtToday.Date.ToUniversalTime(); // Inicio del dia (00:00:00)
                DateTime startOfWeek = startOfDay.AddDays(-(int)startOfDay.DayOfWeek); // Inicio de la semana (domingo)
                DateTime endOfWeek = startOfWeek.AddDays(7).AddTicks(-1); // Fin de la semana (sábado)

                var result = await _client.From<OrderData>().Where(x => x.date >= startOfWeek).Where(x => x.date <= endOfWeek).Order(x => x.id, Ordering.Descending).Get();

                foreach(var order in result.Models)
                {
                    DateTime local = order.date.ToLocalTime();
                    order.date = local;
                }

                return result.Models;

            }
            catch (Exception ex)
            {
                sError = ex.Message;
                return null;
            }
        }

        public async Task<List<OrderData>> GetOrdersDay(DateTime dtToday)
        {
            try
            {
                await Connect();

                DateTime startOfDay = dtToday.Date.ToUniversalTime(); // Inicio del dia (00:00:00)
                DateTime endOfDay = startOfDay.AddDays(1).AddTicks(-1); // Fin del dia (23:59:59.9999999)

                var result = await _client.From<OrderData>().Where(x => x.date >= startOfDay).Where(x => x.date <= endOfDay).Order(x => x.id, Ordering.Descending).Get();

                foreach(var order in result.Models)
                {
                    DateTime local = order.date.ToLocalTime();
                    order.date = local;
                }

                return result.Models;

            }
            catch (Exception ex)
            {
                sError = ex.Message;
                return null;
            }
        }

        public async Task<List<OrderData>> GetOrdersMonth(DateTime dtToday)
        {
            try
            {
                await Connect();

                DateTime startOfDay = dtToday.Date.ToUniversalTime(); // Inicio del dia de hoy (00:00:00)
                DateTime startOfMonth = new DateTime(startOfDay.Year, startOfDay.Month, 1); // Inicio del mes
                DateTime endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1); // Fin del mes

                var result = await _client.From<OrderData>().Where(x => x.date >= startOfMonth).Where(x => x.date <= endOfMonth).Order(x => x.id, Ordering.Descending).Get();

                foreach (var order in result.Models)
                {
                    DateTime local = order.date.ToLocalTime();
                    order.date = local;
                }

                return result.Models;

            }
            catch (Exception ex)
            {
                sError = ex.Message;
                return null;
            }
        }

        public async Task<List<OrderDetailsData>> GetOrderDetails(OrderData order)
        {
            try
            {
                await Connect();

                var result = await _client.From<OrderDetailsData>().Where(x => x.orderid == order.id).Get();
                return result.Models;

            }
            catch (Exception ex)
            {
                sError = ex.Message;
                return null;
            }
        }

        public async Task<UserData> GetClientDetails(OrderData order)
        {
            try
            {
                await Connect();

                var result = await _client.From<UserData>().Where(x => x.id == order.userid).Get();

                return result.Models.FirstOrDefault();

            }
            catch (Exception ex)
            {
                sError = ex.Message;
                return null;
            }
        }
    }
}
