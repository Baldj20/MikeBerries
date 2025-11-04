using Microsoft.AspNetCore.Http;
using ProductService.BLL.DTO;
using ProductService.DAL.Entities;
using System.Net.Http.Headers;

namespace ProductService.IntegrationTests;

public static class TestDataHelper
{
    public static MultipartFormDataContent CreateProductDto(
        string title = "TestProduct",
        string description = "TestDescription",
        decimal price = 100,
        List<IFormFile>? images = null,
        string providerEmail = "TestProviderEmail@mail.ru",
        string providerName = "TestProviderName"
        )
    {
        var createProductDto = new CreateProductDto
        {
            Title = title,
            Description = description,
            Price = price,
            Images = new(),
            Provider = ProviderDto(providerEmail, providerName)
        };

        var formData = new MultipartFormDataContent();

        formData.Add(new StringContent(createProductDto.Title), "Title");
        formData.Add(new StringContent(createProductDto.Description!), "Description");
        formData.Add(new StringContent(createProductDto.Price.ToString()), "Price");
        formData.Add(new StringContent(createProductDto.Provider.Name), "Provider.Name");
        formData.Add(new StringContent(createProductDto.Provider.Email), "Provider.Email");

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

    public static Product CreateProductEntity()
    {
        var providerId = Guid.NewGuid();
        return new Product
        {
            Id = Guid.NewGuid(),
            Title = "TestTitle",
            Description = "TestDescription",
            Price = 100,
            ProviderId = providerId,
            Provider = new Provider
            {
                Id = providerId,
                Email = "TestProviderEmail@mail.ru",
                Name = "TestProviderName"
            },
            Images = new()
        };
    }
}
