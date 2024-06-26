﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorAdmin.Helpers;
using BlazorAdmin.Services;
using HtmlAgilityPack;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;

namespace BlazorAdmin.Pages.OrderPage;

public partial class OrderList : BlazorComponent
{

    List<Order> Orders;
    OrderItem _orderItem;
    CatalogItemOrdered _catalogItem;
    public string status;
    bool showOrder = false;


    [Microsoft.AspNetCore.Components.Inject]
    private ToastService _toastService { get; set; }

    [Microsoft.AspNetCore.Components.Inject]
    private HttpClient _httpClient { get; set; }

    [Microsoft.AspNetCore.Components.Inject]
    private HttpService _service { get; set; }

    string _apiUrl = "https://localhost:5001/";
    string _debug = "https://localhost:44315/";
    string uri = "admin/orderlist";
    public string name;
    public string price;
    public string count;
    public int Id;
    public async Task<List<Order>> ParseOrdersFromContent(string content)
    {
        List<Order> orders = new List<Order>();

        try
        {
            var document = new HtmlDocument();
            document.LoadHtml(content);

            var rows = document.DocumentNode.SelectNodes("//table[@class='table']//tbody//tr");
            if (rows != null)
            {
                foreach (var row in rows)
                {
                    var cells = row.SelectNodes("td");
                    if (cells != null && cells.Count >= 1)
                    {
                        var orderDate = cells[0].InnerText.Replace(" &#x2B;03:00", "").Trim();
                        var buyerId = cells[1].InnerText.Trim();
                        var total = decimal.Parse(cells[2].InnerText, CultureInfo.InvariantCulture);
                        var status = cells[3].InnerText.Trim();
                        var id = cells[4].InnerText.Trim();

                        int plusIndex = orderDate.LastIndexOf('+');
                        if (plusIndex >= 0)
                        {
                            orderDate = orderDate.Substring(0, plusIndex).Trim();
                        }
                        var orderDateClean = orderDate.Replace(" AM", "").Replace(" PM", "").Replace(" +03:00", "");
                        // Doğru tarih formatını belirlemek
                        string[] format = { "dd/MM/yyyy HH:mm:ss" };

                        DateTime dateTime;
                        bool isValidDate = DateTime.TryParseExact(orderDateClean, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);

                        if (isValidDate)
                        {
                            var order = new Order
                            {
                                OrderDate = dateTime,
                                BuyerId = buyerId,
                                TotalPrice = total,
                                Status = status,
                                Id = int.Parse(id)
                            };

                            orders.Add(order);
                        }
                        else
                        {
                            // Hatalı tarih formatı için hata mesajı
                            Console.WriteLine($"Error parsing date: {orderDate}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ParseOrdersFromContent hatası: {ex.Message}");
        }

        return orders;
    }


    private async void GetOrderItems(int id)
    {
        var orderItemsResponse = await _httpClient.GetAsync(_apiUrl + "order/detail/" + id);
        if (orderItemsResponse.IsSuccessStatusCode)
        {
            var orderItemsContent = await orderItemsResponse.Content.ReadAsStringAsync();


            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(orderItemsContent);


            // Sipariş detaylarını almak için XPath ifadeleri
            string orderItemName = htmlDoc.DocumentNode.SelectSingleNode("//section[@class='esh-orders-detail-item esh-orders-detail-item--middle col-xs-3']")?.InnerText.Trim();
            string orderItemQuantity = htmlDoc.DocumentNode.SelectSingleNode("//section[@class='esh-orders-detail-item col-xs-1']")?.InnerText.Trim();
            string orderItemTotalPrice = htmlDoc.DocumentNode.SelectSingleNode("//section[@class='esh-orders-detail-item esh-orders-detail-item--middle col-xs-2']")?.InnerText.Trim().Replace("$", ""); ;
            string orderStatus = htmlDoc.DocumentNode.SelectSingleNode("//section[@class='esh-orders-detail-item col-xs-3' and text()='Pending']")?.InnerText.Trim();

            Console.WriteLine("Order Item Name: " + orderItemName);
            Console.WriteLine("Order Item Quantity: " + orderItemQuantity);
            Console.WriteLine("Order Item Total Price: " + orderItemTotalPrice);

            count = orderItemQuantity;
            price = orderItemTotalPrice;
            name = orderItemName;


            status = orderStatus;
        }
    }



    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                Console.WriteLine("HTTP isteği gönderiliyor...");
                var response = await _httpClient.GetAsync(_apiUrl + uri);
                Console.WriteLine("HTTP isteği tamamlandı.");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Orders = await ParseOrdersFromContent(content);
                }
                else
                {
                    // Hata durumunda kullanıcıya bilgi ver
                    _toastService.ShowToast($"Sipariş kalemleri alınırken bir hata oluştu: {response.ReasonPhrase}", ToastLevel.Error);
                    Console.WriteLine($"HTTP isteği başarısız: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanıcıya bilgi ver
                _toastService.ShowToast($"Siparişler alınırken bir hata oluştu: {ex.Message}", ToastLevel.Error);
                Console.WriteLine($"HTTP isteği sırasında hata oluştu: {ex.Message}");
            }
            CallRequestRefresh();
            StateHasChanged();
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    private void ShowOrderDetailModal(int id)
    {
        GetOrderItems(id);
        if (name != null)
        {
            showOrder = true;
        }
        Id = id;


    }

    private void CloseModal()
    {
        showOrder = false;
    }

    public async void ApproveOrder()
    {
        try
        {
            if (name != null)
            {

                if (true)
                {
                    Orders[Id].SetStatus();
                    status = "Approved";
                    _toastService.ShowToast("Sipariş onaylandı.", ToastLevel.Success);
                    CloseModal();
                }
                else
                {
                    _toastService.ShowToast("Sipariş onaylanırken bir hata oluştu.", ToastLevel.Error);
                }
            }
        }
        catch (Exception ex)
        {
            _toastService.ShowToast("Sipariş onaylanırken bir hata oluştu.", ToastLevel.Error);
        }


    }
}
