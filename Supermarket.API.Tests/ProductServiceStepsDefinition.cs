using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using SpecFlow.Internal.Json;
using Supermarket.API.Resources;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;

namespace Supermarket.API.Tests
{
    [Binding]
    public class ProductServiceStepsDefinition
    {
        private readonly WebApplicationFactory<Startup> _factory;

        private HttpClient Client { get; set; }
        private Uri BaseUri { get; set; }
        private Task<HttpResponseMessage> Response { get; set; }
        private CategoryResource Category { get; set; }
        private ProductResource Product { get; set; }
        
        public ProductServiceStepsDefinition(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Given(@"the Endpoint https://localhost:(.*)/api/v(.*)/products is available")]
        public void GivenTheProductsEndpointIsAvailable(int port, int version)
        {
            BaseUri = new Uri($"https://localhost:{port}/api/v{version}/products");
            Client = _factory.CreateClient(new WebApplicationFactoryClientOptions {BaseAddress = BaseUri});
        }

        [Given(@"A category is already stored")]
        public async void GivenACategoryIsAlreadyStored(Table existingCategoryResource)
        {
            var categoryUri = new Uri("https://localhost:5001/api/v1/categories");
            var resource = existingCategoryResource.CreateSet<SaveCategoryResource>().First();
            var content = new StringContent(resource.ToJson(), Encoding.UTF8, MediaTypeNames.Application.Json);
            var categoryResponse = Client.PostAsync(categoryUri, content);
            var categoryResponseData = await categoryResponse.Result.Content.ReadAsStringAsync();
            var existingCategory = JsonConvert.DeserializeObject<CategoryResource>(categoryResponseData);
            Category = existingCategory;
        }

        [When(@"A Post Request is sent")]
        public void WhenAPostRequestIsSent(Table saveProductResource)
        {
            var resource = saveProductResource.CreateSet<SaveProductResource>().First();
            var content = new StringContent(resource.ToJson(),Encoding.UTF8, MediaTypeNames.Application.Json);
            Response = Client.PostAsync(BaseUri, content);
        }

        [Then(@"A Response with Status (.*) is received")]
        public void ThenAResponseWithStatusIsReceived(int expectedStatus)
        {
            var expectedStatusCode = ((HttpStatusCode) expectedStatus).ToString();
            var actualStatusCode = Response.Result.StatusCode.ToString();
            Assert.Equal(expectedStatusCode, actualStatusCode);
        }

        [Then(@"A Product Resource is included in Response")]
        public async void ThenAProductResourceIsIncludedInResponse(Table expectedProductResource)
        {
            var expectedResource = expectedProductResource.CreateSet<ProductResource>().First();
            expectedResource.Category = Category;
            var responseData = await Response.Result.Content.ReadAsStringAsync();
            var resource = JsonConvert.DeserializeObject<ProductResource>(responseData);
            expectedResource.Id = resource.Id;
            var jsonExpectedResource = expectedResource.ToJson();
            var jsonActualResource = resource.ToJson();
            Assert.Equal(jsonExpectedResource, jsonActualResource);
        }

        [Then(@"A Message of ""(.*)"" is included in Response Body")]
        public async void ThenAMessageIsIncludedInResponseBodyWithValue(string expectedMessage)
        {
            var actualMessage = await Response.Result.Content.ReadAsStringAsync();
            var jsonExpectedMessage = expectedMessage.ToJson();
            var jsonActualMessage = actualMessage.ToJson();
            Assert.Equal(jsonExpectedMessage, jsonActualMessage);
        }
        
        [Given(@"A Product is already stored")]
        public async void GivenAProductIsAlreadyStored(Table existingProductResource)
        {
            var resource = existingProductResource.CreateSet<SaveProductResource>().First();
            var content = new StringContent(resource.ToJson(),Encoding.UTF8, MediaTypeNames.Application.Json);
            var productResponse = Client.PostAsync(BaseUri, content);
            var productResponseData = await productResponse.Result.Content.ReadAsStringAsync();
            var existingProduct = JsonConvert.DeserializeObject<ProductResource>(productResponseData);
            Product = existingProduct;
        }
    }
}