﻿using RealEstateApp.Data.Models;


namespace RealEstateApp.Data.DataServices.Contracts
{
    public interface IFavoriteService
    {
        Task AddFavorite(string name, Guid userId, int? importance = null);
    }
}
