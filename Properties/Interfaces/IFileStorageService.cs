using CoffeeNChill.Functions.Properties.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeNChill.Functions.Properties.Interfaces
{
    public interface IFileStorageService
    {
        Task<StaffDocument> UploadDocumentAsync(IFormFile file, string directoryName);

        Task<Stream> DownloadDocumentAsync(string fileName);

        Task<bool> DeleteDocumentAsync(string fileName);

        Task<List<StaffDocument>> GetaLLDocumentsAsync();
    }
} 
