﻿using Microsoft.AspNetCore.Http;

namespace ProductService.BLL.DTO;

public class UploadProductImageDto
{
    public required IFormFile Image { get; set; }
}
