﻿using Dapper;
using FreeCourse.Shared.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Npgsql;
using System.Data;
using Microsoft.Extensions.Configuration;


namespace FreeCourse.Service.Discount.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnection _dbConnection;

        public DiscountService(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
        }

        public async Task<Response<NoContent>> Delete(int id)
        {
            var status = await _dbConnection.ExecuteAsync("delete from discount Where id=@Id", new { Id=id });
            return status > 0 ? Response<NoContent>.Success(204) : Response<NoContent>.Fail("Discount not found", 404);
        }

        public async Task<Response<List<Models.Discount>>> GetAll()
        {
            var discounts = await _dbConnection.QueryAsync<Models.Discount>("Select * from discount");
            return Response<List<Models.Discount>>.Success(discounts.ToList(),200);
        }

        public async Task<Response<Models.Discount>> GetByCodeAndUserId(string code, string userId)
        {
            var discounts = await _dbConnection.QueryAsync<Models.Discount>
                ("Select * from discount where userid=@UserId and code=@Code", 
                new {UserId=userId, Code=code});
            var hasDiscount = discounts.FirstOrDefault();
            return hasDiscount == null ? Response<Models.Discount>.Fail("Id or code is wrong", 404) : Response<Models.Discount>.Success(hasDiscount, 200);
        }

        public async Task<Response<Models.Discount>> GetById(int Id)
        {
            var discount = (await _dbConnection.
                QueryAsync<Models.Discount>($"Select * from discount Where id=@Id", new { Id = Id }))
                .SingleOrDefault();
            if (discount == null) return Response<Models.Discount>.Fail("Discount not found", 404);
            return Response<Models.Discount>.Success(discount, 200);
        }

        public async Task<Response<NoContent>> Save(Models.Discount discount)
        {
            var saveStatus = await _dbConnection.ExecuteAsync("INSERT INTO discount (userid,rate,code) VALUES(@UserId, @Rate, @Code)", discount);
            if (saveStatus > 0) return Response<NoContent>.Success(204);
            return Response<NoContent>.Fail("An error occured", 500);
        }

        public async Task<Response<NoContent>> Update(Models.Discount discount)
        {
            var status = await _dbConnection
                .ExecuteAsync("update discount set userid=@UserId, code=@Code, rate=@Rate where id=@Id",
                new { Id = discount.Id, UserId = discount.UserId, Code = discount.Code, Rate = discount.Rate });
            if (status > 0) return Response<NoContent>.Success(204);
            return Response<NoContent>.Fail("Discount not found", 404);
        }
    }
}
