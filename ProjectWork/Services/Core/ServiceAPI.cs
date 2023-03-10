﻿using System.Net;
using System.Net.Http.Json;
using System.Web;
using Microsoft.AspNetCore.Components.Forms;

namespace ProjectWork.Services.Core;

public class ServiceAPI : IServiceAPI
{
    private readonly HeadersBuilder _headersBuilder;
    private readonly HeadersDirector _headersDirector = new();
    private readonly ImageOptions _imageOptions = new();
    private readonly string url;

    public ServiceAPI(string url)
    {
        var handler = new HttpClientHandler { UseCookies = false };
        _headersBuilder = new HeadersBuilder(new HttpClient(handler));
        _headersDirector.Builder = _headersBuilder;
        this.url = url;
        Uri = new UriBuilder(url);
    }

    public ServiceAPI(string url, ImageOptions imageOptions)
    {
        var handler = new HttpClientHandler { UseCookies = false };
        _headersBuilder = new HeadersBuilder(new HttpClient(handler));
        _headersDirector.Builder = _headersBuilder;
        this.url = url;
        _imageOptions = imageOptions;
        Uri = new UriBuilder(url);
    }

    public UriBuilder Uri { get; set; }

    public async Task<K> GetDataWithPageAsync<K>(int currentPage)
    {
        _headersDirector.BuildGenericGetHeader();
        return await HandleRequest.Requested(_headersBuilder.GetHttpClient()
            .GetFromJsonAsync<K>($"{url}?page={currentPage}"));
    }

    public async Task<K> GetDataWithParamAsync<K>(Dictionary<string, string> parameters)
    {
        BuildUri(parameters);
        await _headersDirector.AuthenticatedHeader();
        return await HandleRequest.Requested(_headersBuilder.GetHttpClient().GetFromJsonAsync<K>(Uri.Uri));
    }

    public async Task<K> GetDetailObject<K>(int id)
    {
        _headersDirector.BuildGenericGetHeader();
        return await HandleRequest.Requested(_headersBuilder.GetHttpClient().GetFromJsonAsync<K>($"{url}{id}/"));
    }

    public async Task<HttpStatusCode> DeleteItemAsync(int page)
    {
        await _headersDirector.AuthenticatedHeader();
        var tempMessage = await HandleRequest.Requested(_headersBuilder.GetHttpClient().DeleteAsync($"{url}{page}/"));
        return await HandleResponse.Responded(tempMessage);
    }

    public async Task<TR> PostItemAsJsonAsync<TS, TR>(TS item)
    {
        await _headersDirector.AuthenticatedHeader();
        var tempMessage = await HandleRequest.Requested(_headersBuilder.GetHttpClient().PostAsJsonAsync(Uri.Uri, item));
        return await HandleResponse.Responded<TR>(tempMessage);
    }

    public async Task<TR> AddItemAsMultipartAsync<TS, TR>(TS item, IBrowserFile file)
    {
        await _headersDirector.AuthenticatedHeader();
        var content = await HandleMultipart.Build(item, file, _imageOptions);
        var tempMessage = await HandleRequest.Requested(_headersBuilder.GetHttpClient().PostAsync(Uri.Uri, content));
        return await HandleResponse.Responded<TR>(tempMessage);
    }


    public async Task<TR> UpdateItemAsJsonAsync<TS, TR>(int id, TS item)
    {
        await _headersDirector.AuthenticatedHeader();
        var tempMessage =
            await HandleRequest.Requested(_headersBuilder.GetHttpClient().PutAsJsonAsync($"{url}{id}/", item));
        return await HandleResponse.Responded<TR>(tempMessage);
    }

    public async Task<TR> UpdateAsMultipartAsync<TS, TR>(int id, TS item, IBrowserFile file)
    {
        await _headersDirector.AuthenticatedHeader();
        var content = await HandleMultipart.Build(item, file, _imageOptions);
        var tempMessage =
            await HandleRequest.Requested(_headersBuilder.GetHttpClient().PatchAsync($"{url}{id}/", content));
        return await HandleResponse.Responded<TR>(tempMessage);
    }

    private void BuildUri(Dictionary<string, string> parameters)
    {
        Uri.Query = "";
        var query = HttpUtility.ParseQueryString(Uri.Query);
        foreach (var item in parameters) query[item.Key] = item.Value;
        Uri.Query = query.ToString() ?? string.Empty;
    }
}