﻿using ProjectWork.Models.Artwork;
using ProjectWork.Models.Core;

namespace ProjectWork.ViewModels
{
    public interface IViewModel<T>
    {
        GenericData<T> GenericData { get; set; }
        Parameters Parameters { get; set; }
        Paginator Paginator { get; set; }

        Task GetGenericDataFromPageAsync();
        Task AddItemAsync(T artwork);
        Task DeleteItemAsync(int id);
        Task UpdateItemAsync(T artwork);
    }
}