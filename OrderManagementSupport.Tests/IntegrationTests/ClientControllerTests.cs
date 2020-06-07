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
    [Collection("Sequential")]
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
            clients.Should().BeEmpty( $"is empty {await response.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task GetById_WithoutAnyPosts_ReturnNotFound()
        {
            //Arrange

            //Act
            var response = await TestClient.GetAsync($"{ApiRoutes.Clients.GetAll}4");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            (await response.Content.ReadAsStringAsync()).Should().BeEmpty($"is empty {await response.Content.ReadAsStringAsync()}");
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
            deleteAfterTest.StatusCode.Should().Be(HttpStatusCode.Accepted, "There is Client on DB");
        }

        [Fact]
        public async Task Post_WithGoodRequest_ReturnCreated()
        {
            //Arrange
            var request = CreateTestClientEntityModel();
            //Act
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var myContent = JsonConvert.SerializeObject(request, serializerSettings);
            var stringContent = new StringContent(myContent, Encoding.UTF8, "application/json");

            var response = await TestClient.PostAsync(ApiRoutes.Clients.Post, stringContent);

            var newClient = JsonConvert.DeserializeObject<Client>(await response.Content.ReadAsStringAsync());

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created, "post was successful");
            newClient.LastName.Should().Be(request.LastName, "post should not change data");
            newClient.FirstName.Should().Be(request.FirstName, "post should not change data");

            //After
            await TestClient.DeleteAsync(ApiRoutes.Clients.Delete + newClient.Id);

        }

        [Fact]
        public async Task Post_WithEmptyRequest_ReturnBadRequest()
        {
            //Arrange
            //Act
            var stringContent = new StringContent("", Encoding.UTF8, "application/json");

            var response = await TestClient.PostAsync(ApiRoutes.Clients.Post, stringContent);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest, "no post");

            //After
        }

        [Fact]
        public async Task Put_WithGoodRequest_ReturnOK()
        {
            var changedData = "Changed Last Name";
            //Arrange
            var request = CreateTestClientEntityModel();
            var newClient = await CreateClientAsync(request);
            //Act
            var newRequest = request;
            newRequest.Id = newClient.Id;
            newRequest.LastName = changedData;
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var myContent = JsonConvert.SerializeObject(newRequest, serializerSettings);
            var stringContent = new StringContent(myContent, Encoding.UTF8, "application/json");

            var response = await TestClient.PutAsync(ApiRoutes.Clients.Put + newClient.Id, stringContent );

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Accepted, "post was successful");
            Client clientOnServer;
            using (var sr = new StringReader(await response.Content.ReadAsStringAsync()))
            {
                clientOnServer = JsonConvert.DeserializeObject<Client>(sr.ReadToEnd());
            }
            clientOnServer.LastName.Should().Be(changedData, "post should changed data");

            //After
            var cat = await TestClient.DeleteAsync(ApiRoutes.Clients.Delete + newClient.Id);
            cat.StatusCode.Should().Be(HttpStatusCode.Accepted);
        }

        [Fact]
        public async Task Delete_WithExistingId_ReturnAccepted()
        {
            //Arrange
            var request = CreateTestClientEntityModel();
            var newClient = CreateClientAsync(request);

            //Act
            var response = await TestClient.DeleteAsync(ApiRoutes.Clients.Delete + newClient.Id);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Accepted, "delete done");
            var cat = await TestClient.GetAsync($"{ApiRoutes.Clients.GetAll}{newClient.Id}");
            cat.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_WithNoClients_ReturnNotFound()
        {
            //Arrange
            var id = 1;

            //Act
            var response = await TestClient.DeleteAsync(ApiRoutes.Clients.Delete + id);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound, "not found client to delete");

            //After
        }





    }
}
