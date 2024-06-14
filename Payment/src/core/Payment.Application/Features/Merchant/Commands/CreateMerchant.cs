using Azure.Messaging;
using Mapster;
using MediatR;
using Microsoft.Data.SqlClient;
using Payment.Application.Base.Models;
using Payment.Application.Constants;
using Payment.Application.Features.Dtos;
using Payment.Application.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Features.Commands
{
    public class CreateMerchant : IRequest<BaseResultWithData<MerchantDtos>>
    {
        public string? MerchtName { get; set; } = string.Empty;
        public string? WebLink { get; set; } = string.Empty;
        public string? MerchtIpnUrl { get; set; } = string.Empty;
        public string? MerchtReturnUrl { get; set; } = string.Empty;

    }



    public class CreateMerchantHandler : IRequestHandler<CreateMerchant, BaseResultWithData<MerchantDtos>>
    {
        private readonly ICurrentUserService currentUserService;
        private readonly ISqlService sqlService;
        private readonly IConnectionService connectionService;

        public CreateMerchantHandler(ICurrentUserService currentUserService, ISqlService sqlService, IConnectionService connectionService)
        {
            this.currentUserService = currentUserService;
            this.sqlService = sqlService;
            this.connectionService = connectionService;
        }

        public Task<BaseResultWithData<MerchantDtos>> Handle(
            CreateMerchant request, CancellationToken cancellationToken)
        {
            var result = new BaseResultWithData<MerchantDtos>();

            try
            {
                string connectionString = connectionService.Datebase ?? string.Empty;

                // Kiểm tra kết nối
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        conn.Close(); Console.WriteLine("success");  // In ra console nếu kết nối thành công

                    }
                    catch (Exception ex)
                    {
                        result.Set(false, MessageContants.Error);
                        result.Errors.Add(new BaseError()
                        {
                            Code = "DbConnection",
                            Message = $"Failed to connect to database: {ex.Message}"
                        });

                        return Task.FromResult(result);
                    }
                }




                var outputIdParam = sqlService.CreateOutputParameter("@InsertedId", SqlDbType.NVarChar, 50);
                var paramters = new SqlParameter[]
                {
                    new SqlParameter("@MerchtName", request.MerchtName ?? string.Empty),
                    new SqlParameter("@WebLink", request.WebLink ?? string.Empty),
                    new SqlParameter("@MerchtIpnUrl", request.MerchtIpnUrl ?? string.Empty),
                    new SqlParameter("@MerchtReturnUrl", request.MerchtReturnUrl ?? string.Empty),
                    new SqlParameter("@InsertUser", currentUserService.UserId ?? string.Empty),
                    outputIdParam
                };
                Console.WriteLine(MerchantContants.InsertSprocName+"\taaaaaaaa\t" +request.MerchtIpnUrl.ToString());
                (int affectedRows, string sqlError) = sqlService.ExecuteNonQuery(connectionString,
                    MerchantContants.InsertSprocName, paramters);

                if (affectedRows == 1)
                {
                    var resultData = request.Adapt<MerchantDtos>();
                    resultData.MerchtId = outputIdParam.Value?.ToString() ?? string.Empty;
                    result.Set(true, MessageContants.OK, resultData);
                }
                else
                {
                    result.Set(false, MessageContants.Error);
                    result.Errors.Add(new BaseError()
                    {
                        Code = "Sql",
                        Message = sqlError
                    });
                }
            }
            catch (Exception ex)
            {
                result.Set(false, MessageContants.Error);
                result.Errors.Add(new BaseError()
                {
                    Code = MessageContants.Exception,
                    Message = ex.Message,
                });
            }

            return Task.FromResult(result);
        }
    }
}