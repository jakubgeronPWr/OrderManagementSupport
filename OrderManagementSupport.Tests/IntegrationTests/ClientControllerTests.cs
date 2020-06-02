using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OrderManagementSupport.Contracts;
using OrderManagementSupport.Data.Entities;
using OrderManagementSupport.EntityModel;
using Xunit;

namespace OrderManagementSupport.Tests.IntegrationTests
{
    public class ClientControllerTests: IntegrationTest
    {
        [Fact]
        public async Task GetAll_WithoutAnyPosts_ReturnEmptyResponse()
        {
            //Arrange

            //Act
            var response = await TestClient.GetAsync(ApiRoutes.Clients.GetAll);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            List<Client> clients;
            using (var sr = new StringReader(await response.Content.ReadAsStringAsync()))
            {
                var clientsOnServer = JsonConvert.DeserializeObject<List<Client>>(sr.ReadToEnd());
                clients = clientsOnServer;
            }
            clients.Should().BeEmpty( $"not empty {await response.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task GetById_WithoutAnyPosts_ReturnNotFound()
        {
            //Arrange

            //Act
            var response = await TestClient.GetAsync($"{ApiRoutes.Clients.GetAll}4");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            (await response.Content.ReadAsStringAsync()).Should().BeEmpty($"not empty {await response.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task GetAll_WithOnePosts_ReturnNonEmptyResponse()
        {
            //Arrange
            var request = new ClientEntityModel()
            {
                Address = "Street",
                City = "City",
                FirstName = "Test",
                Id = 0,
                LastName = "Test",
                ZipCode = "53-300"
            };
            var addedClient = await CreateClientAsync(request);
            
            //Act
            var response = await TestClient.GetAsync(ApiRoutes.Clients.GetAll);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            List<Client> clients;

            //List<Client> clientsOnServer = Serializer.Deserialize<Client>(await response.Content.ReadAsStreamAsync())
            using (var sr = new StringReader(await response.Content.ReadAsStringAsync()))
            {
                var clientsOnServer = JsonConvert.DeserializeObject<List<Client>>(sr.ReadToEnd());
                clients = clientsOnServer;
            }

            Assert.True(clients.Exists(c => c.LastName == "Test"));

            var deleteAfterTest = await TestClient.DeleteAsync(ApiRoutes.Clients.Delete + addedClient.Id);
            deleteAfterTest.StatusCode.Should().Be(HttpStatusCode.Accepted, "CLEAN ADDED CLIENT AFTER TEST ERROR");
        }

        private async Task<Client> CreateClientAsync(ClientEntityModel request)
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var myContent = JsonConvert.SerializeObject(request, serializerSettings);
            var stringContent = new StringContent(myContent, Encoding.UTF8, "application/json");
            var response =  await TestClient.PostAsync(ApiRoutes.Clients.Post, stringContent);

            return JsonConvert.DeserializeObject<Client>(await response.Content.ReadAsStringAsync());
        }

    }
}
