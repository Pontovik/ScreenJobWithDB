using Microsoft.EntityFrameworkCore;
using ScreenJob.Models;

namespace ScreenJob.XmlActions
{
    public class DbActions
    {
        public static void UpdateFromXml(string path)
        {
            NewOrders newOrders = WorkWithFile.GetData(path);
            using (AppDbContext db = new AppDbContext())
            {
                for (int i = 0; i < newOrders.Orders.Length; i++)
                {
                    var newOrder = newOrders.Orders[i];
                    int user_id = FindUser(db, newOrder);
                    if (user_id == -1)
                    {
                        Console.WriteLine($"Пользователя с именем {newOrder.User.UserName} не существует");
                        continue;
                    }
                    bool AreAnyProducts = false;
                    int[] product_ids = new int[newOrder.BuyProducts.Count];
                    for (int j = 0; j < newOrder.BuyProducts.Count; j++)
                    {
                        var newBuy = newOrder.BuyProducts[j];
                        int product_id = FindProductByName(db, newBuy);
                        product_ids[j] = product_id;
                        if (product_id == -1)
                        {
                            newOrder.OrderSum -= newBuy.BuyproductPrice * newBuy.BuyproductAmount;
                            Console.WriteLine($"Заказанного товара {newBuy.BuyproductName} не существует");
                            continue;
                        }
                        if (IsProductEnough(db, newBuy, product_id))
                        {
                            AreAnyProducts = true;
                        }
                        else
                        {
                            newOrder.OrderSum -= newBuy.BuyproductPrice * newBuy.BuyproductAmount;
                        }
                    }
                    if (!AreAnyProducts)
                    {
                        continue;
                    }
                    int order_id = FindOrderByNumber(db, newOrder);
                    if(order_id != -1)
                    {
                        UpdateOrder(db, newOrder, order_id, user_id);
                    }
                    else
                    {
                        InsertIntoOrder(db, newOrder,user_id);
                    }
                    for(int j = 0; j < newOrder.BuyProducts.Count; j++)
                    {
                        var newBuy = newOrder.BuyProducts[j];
                        if (product_ids[j] != -1)
                        UpdateBuyProduct(db, newBuy, product_ids[j], order_id);
                    }
                }
            }
        }

        private static void UpdateOrder(AppDbContext db, Order newOrder, int order_id, int user_id)
        {
            db.Database.ExecuteSql($"UPDATE [Order] SET order_reg_date = {newOrder.OrderRegDate}, order_sum = {newOrder.OrderSum}, user_id = {user_id} WHERE order_id = {order_id}");
        }

        private static void InsertIntoOrder(AppDbContext db, Order newOrder, int user_id)
        {
            db.Database.ExecuteSql($"INSERT INTO [Order](order_id,order_reg_date,order_number,order_sum,user_id) VALUES ({newOrder.OrderRegDate}, {newOrder.OrderNumber}, {newOrder.OrderSum}, {user_id})");
        }
        private static int FindOrderByNumber(AppDbContext db, Order newOrder)
        {
            var orders = (Order[])db.Orders.FromSqlRaw($"SELECT * FROM [Order] o WHERE o.order_number = {newOrder.OrderNumber}").ToArray();
            if (orders != null && orders.Length == 1)
            {
                return orders[0].OrderId;
            }
            return -1;
        }

        private static int FindUser(AppDbContext db, Order newOrder)
        {
            string userName = newOrder.User.UserName;
            string email = newOrder.User.UserEmail;
            var users = (User[])db.Users.FromSqlRaw($"SELECT * FROM [User] u WHERE u.user_name = '{userName}' AND u.user_email = '{email}'").ToArray();
            if (users != null && users.Length == 1)
            {
                return users[0].UserId;
            }

            return -1;
        }

        private static int FindProductByName(AppDbContext db, BuyProduct newBuy)
        {
            var products = (Product[])db.Products.FromSqlRaw($"SELECT * FROM Product p WHERE p.product_name = '{newBuy.BuyproductName}'").ToArray();
            if (products != null && products.Length == 1)
            {
                return products[0].ProductId;
            }
            return -1;
        }

        private static bool IsProductEnough(AppDbContext db, BuyProduct newBuy, int product_id)
        {
            var products = (Product[])db.Products.FromSqlRaw($"SELECT * FROM Product p WHERE p.product_id = {product_id}").ToArray();
            if (products != null && products.Length != 0)
            {
                int amount = products[0].ProductAmount == null ? 0 : products[0].ProductAmount.Value;
                if (newBuy.BuyproductAmount > amount)
                {
                    Console.WriteLine($"Недостаточно товара на складе, было заказано {newBuy.BuyproductAmount}, а на складе есть{amount}");
                    return false;
                }
                else
                {
                    int newAmount = (int)(amount - newBuy.BuyproductAmount);
                    db.Database.ExecuteSql($"UPDATE Product SET product_amount = {newAmount} WHERE product_id = {product_id}");
                }
            }
            return true;
        }

        private static void UpdateBuyProduct(AppDbContext db, BuyProduct newBuy, int product_id, int order_id)
        {
            db.Database.ExecuteSql($"INSERT INTO BuyProduct(product_id, order_id,buyproduct_amount, buyproduct_price) VALUES ({product_id}, {order_id}, {newBuy.BuyproductAmount}, {newBuy.BuyproductPrice})");
        }
    }
}
