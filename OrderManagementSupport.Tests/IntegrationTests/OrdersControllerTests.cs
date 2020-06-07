using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OrderManagementSupport.Contracts;
using OrderManagementSupport.Data.Entities;
using OrderManagementSupport.EntityModel;
using Xunit;

namespace OrderManagementSupport.Tests.IntegrationTests
{
    public class OrdersControllerTests: IntegrationTest
    {
        private String TEST_SERVICE_MESSAGE = "TestService";

        [Fact]
        public async Task GetAll_WithoutAnyPosts_ReturnEmptyResponse()
        {
            //Arrange

            //Act
            var response = await TestClient.GetAsync(ApiRoutes.Orders.GetAll);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            List<OrderEntityModel> orders;
            using (var sr = new StringReader(await response.Content.ReadAsStringAsync()))
            {
                var ordersOnServer = JsonConvert.DeserializeObject<List<OrderEntityModel>>(sr.ReadToEnd());
                orders = ordersOnServer;
            }
            orders.Should().BeEmpty($"is empty {await response.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task GetAll_WithOnePosts_ReturnOkWithResponse()
        {
            //Arrange
            var newClient = await CreateClientAsync(CreateTestClientEntityModel());
            var request = CreateOrderForTests(newClient.Id);
            var newOrder = await CreateOrderAsync(request);

            //Act
            var response = await TestClient.GetAsync(ApiRoutes.Orders.GetAll);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            List<Order> orders;

            using (var sr = new StringReader(await response.Content.ReadAsStringAsync()))
            {
                var ordersOnServer = JsonConvert.DeserializeObject<List<Order>>(sr.ReadToEnd());
                orders = ordersOnServer;
            }

            Assert.True(orders.Any(), $"{orders.Count}");

            var catOrder = await TestClient.DeleteAsync(ApiRoutes.Orders.Delete + newOrder.OrderId);
            var catClient = await TestClient.DeleteAsync(ApiRoutes.Clients.Delete + newClient.Id);
            catClient.StatusCode.Should().Be(HttpStatusCode.Accepted, "CLEAN CLIENT AFTER TEST");
            catOrder.StatusCode.Should().Be(HttpStatusCode.Accepted, "CLEAN ORDER AFTER TEST");
        }

        [Fact]
        public async Task Post_WithGoodRequest_ReturnCreated()
        {
            //Arrange
            var newClient = await CreateClientAsync(CreateTestClientEntityModel());
            var request = CreateOrderForTests(newClient.Id);
            //Act
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var myContent = JsonConvert.SerializeObject(request, serializerSettings);
            var stringContent = new StringContent(myContent, Encoding.UTF8, "application/json");
            var response = await TestClient.PostAsync(ApiRoutes.Orders.Post, stringContent);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var createdOrder = JsonConvert.DeserializeObject<OrderEntityModel>(await response.Content.ReadAsStringAsync());
            OrderEntityModel orderByGet;
            using (var sr = new StringReader(await response.Content.ReadAsStringAsync()))
            {
                orderByGet = JsonConvert.DeserializeObject<OrderEntityModel>(sr.ReadToEnd());
            }

            orderByGet.Service.Should().Be(createdOrder.Service);

            //After
            var responseOrderByGet = await TestClient.GetAsync(ApiRoutes.Orders.GetById + createdOrder.OrderId);
            responseOrderByGet.StatusCode.Should().Be(HttpStatusCode.OK);
            var cat = await TestClient.DeleteAsync(ApiRoutes.Orders.Delete + createdOrder.OrderId);
            var catClient = await TestClient.DeleteAsync(ApiRoutes.Clients.Delete + createdOrder.ClientId);
            cat.StatusCode.Should().Be(HttpStatusCode.Accepted);
        }



        private async Task<OrderEntityModel> CreateOrderAsync(OrderEntityModel request)
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var myContent = JsonConvert.SerializeObject(request, serializerSettings);
            var stringContent = new StringContent(myContent, Encoding.UTF8, "application/json");
            var response = await TestClient.PostAsync(ApiRoutes.Orders.Post, stringContent);
            //response.Should().Be(HttpStatusCode.Created);

            return JsonConvert.DeserializeObject<OrderEntityModel>(await response.Content.ReadAsStringAsync());
        }

        private OrderEntityModel CreateOrderForTests(int clientId)
        {
            return new OrderEntityModel()
            {
                OrderId = 0,
                ClientId = clientId,
                Price = 19.99,
                OrderNumber = "",
                OrderDate = DateTime.Now,
                OrderRealizationDate = DateTime.Now.AddDays(3),
                Service = TEST_SERVICE_MESSAGE
            };
        }

    }
}
