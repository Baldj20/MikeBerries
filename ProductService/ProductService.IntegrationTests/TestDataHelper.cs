using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ProductService.BLL.DTO;
using ProductService.DAL.Entities;
using System.Net.Http.Headers;
using System.Text;

namespace ProductService.IntegrationTests;

public static class TestDataHelper
{
    public static MultipartFormDataContent CreateProductDtoFormData(
        string title = "TestProduct",
        string description = "TestDescription",
        decimal price = 100,
        List<IFormFile>? images = null,
        string providerEmail = "TestProviderEmail@mail.ru",
        string providerName = "TestProviderName"
        )
    {
        var formData = UpdateProductDtoFormData(
            title: title,
            description: description,
            price: price,
            images: images
            );

        formData.Add(new StringContent(providerEmail), "Provider.Email");
        formData.Add(new StringContent(providerName), "Provider.Name");

        return formData;
    }

    public static MultipartFormDataContent UpdateProductDtoFormData(
        string title = "UpdatedProduct",
        string description = "UpdatedDescription",
        decimal price = 200,
        List<IFormFile>? images = null
        )
    {
        var formData = new MultipartFormDataContent();

        formData.Add(new StringContent(title), "Title");
        formData.Add(new StringContent(description!), "Description");
        formData.Add(new StringContent(price.ToString()), "Price");

        if (images is null)
        {
            var image = CreateTestImageFile(
                "image1",
                "Fake JPEG Content 1",
                "image/jpeg"
                );

            var fileContent = new StreamContent(image.OpenReadStream());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);
            formData.Add(fileContent, "Images[0].Image", image.FileName);

        }
        else
        {
            for (int i = 0; i < images.Count; i++)
            {
                var fileContent = new StreamContent(images[i].OpenReadStream());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(images[i].ContentType);
                formData.Add(fileContent, $"Images[{i}].Image", images[i].FileName);
            }
        }

        return formData;
    }

    public static ProviderDto ProviderDto(
        string providerEmail = "TestProviderEmail@mail.ru",
        string providerName = "TestProviderName"
        )
    {
        return new ProviderDto
        {
            Email = providerEmail,
            Name = providerName
        };
    }

    public static UpdateProductDto UpdateProductDto()
    {
        return new UpdateProductDto
        {
            Title = "UpdatedTestProduct",
            Description = "UpdatedProductDescription",
            Price = 500,
            Images = new()
        };
    }

    public static IFormFile CreateTestImageFile(
        string fileName,
        string content,
        string contentType)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(content);
        writer.Flush();
        stream.Position = 0;

        var formFile = new FormFile(stream, 0, stream.Length, "Image", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };

        return formFile;
    }

    public static Product CreateProductEntity(
        string title = "TestProduct",
        string description = "TestDescription",
        decimal price = 100,
        List<IFormFile>? images = null,
        string providerEmail = "TestProviderEmail@mail.ru",
        string providerName = "TestProviderName"
        )
    {
        var providerId = Guid.NewGuid();
        return new Product
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description,
            Price = price,
            ProviderId = providerId,
            Provider = new Provider
            {
                Id = providerId,
                Email = providerEmail,
                Name = providerName
            },
            Images = new()
        };
    }

    public static Provider CreateProviderEntity(
        string providerEmail = "TestProviderEmail@mail.ru",
        string providerName = "TestProviderName"
        )
    {
        return new Provider
        {
            Id = Guid.NewGuid(),
            Email = providerEmail,
            Name = providerName
        };
    }

    public static StringContent CreateProviderStringContent(
        string providerName = "TestProviderName",
        string providerEmail = "TestProviderEmail@mail.ru"
        )
    {
        var provider = ProviderDto(
            providerEmail: providerEmail,
            providerName: providerName);

        var json = JsonConvert.SerializeObject(provider);

        return new StringContent(json, Encoding.UTF8, "application/json");
    }
}
