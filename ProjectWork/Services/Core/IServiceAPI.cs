﻿using System.Net;
using Microsoft.AspNetCore.Components.Forms;

namespace ProjectWork.Services.Core
{
    public interface IServiceAPI
    {
        public UriBuilder Uri { get; set; }   
        Task<TR> PostItemAsJsonAsync<TS,TR>(TS item);
        Task<TR> AddItemAsMultipartAsync<TS,TR>(TS item, IBrowserFile file);
        Task<TR> UpdateAsMultipartAsync<TS,TR>(int id, TS item, IBrowserFile file);
        Task<HttpStatusCode> DeleteItemAsync(int page);
        Task<K> GetDataWithPageAsync<K>(int currentPage);
        Task<K> GetDataWithParamAsync<K>(Dictionary<string, string> parameters);
        Task<K> GetDetailObject<K>(int id);
        Task<TR> UpdateItemAsJsonAsync<TS,TR>(int id, TS item);
    }
}