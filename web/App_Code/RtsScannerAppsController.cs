﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebApi;
using RabbitMQ.Client;
using Newtonsoft.Json.Linq;
using Bespoke.PosEntt.EnttAcceptances.Domain;
using System.Data;
using System.Data.SqlClient;
using System.Text;

[RoutePrefix("api/rtsa")]
public class RtsScannerAppsController : BaseApiController
{
    [Route("download-acceptance/{branch}")]
    [HttpGet]
    public async Task<IHttpActionResult> DownloadAcceptanceAsync(string branch)
    {
        var connectionString = @"Server=(localdb)\ProjectsV13;Database=Entt;Trusted_Connection=True;";
        var conn = new SqlConnection(connectionString);
        var acceptances = new List<EnttAcceptance>();
        using (var cmd = new SqlCommand("SELECT [Id],[ConsignmentNo],[DateTime],[LocationId],[PickupNo],[Postcode],[Pl9No],[ShipperAccountNo],[ParentWeight] FROM [Entt].[Acceptance] WHERE [LocationId] = @LocationId AND [DateTime] > @StartDate AND [DateTime] <= @EndDate", conn))
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Parameters.AddWithValue("@LocationId", branch);
            cmd.Parameters.AddWithValue("@StartDate", DateTime.Today);
            cmd.Parameters.AddWithValue("@EndDate", DateTime.Now);

            using (var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
            {
                while (reader.Read())
                {
                    var a = new EnttAcceptance
                    {
                        Id = reader.GetString(0)
                        , ConsignmentNo = reader.GetString(1)
                        , DateTime = reader.GetDateTime(2)
                        , LocationId = reader.GetString(3)
                        , Postcode = reader.GetValue(5).ToString()
                        ,Pl9No = reader.GetValue(6).ToString()
                        ,ShipperAccountNo = reader.GetValue(7).ToString()
                        //,Weight = reader.GetDecimal(8)
                    };
                    acceptances.Add(a);
                    var weight = reader[8];
                    if (DBNull.Value != weight)
                    {
                        try
                        {
                            a.Weight = decimal.Parse(weight.ToString());
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(weight);
                        }

                    }
                }
            }
        }

        var sb = new StringBuilder();
        foreach (var item in acceptances)
        {
          sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\r\n",item.ConsignmentNo, item.ShipperAccountNo, item.DateTime,item.LocationId, item.Pl9No, item.Postcode, item.Weight);
        }
        var fileContent = Encoding.ASCII.GetBytes(sb.ToString());
        return File(fileContent,"text/plain","sample.txt");
    }

    [Route("download-acceptance-connote/{branch}")]
    [HttpGet]
    public async Task<IHttpActionResult> DownloadAcceptanceConnoteAsync(string branch)
    {
        var connectionString = ConfigurationManager.GetEnvironmentVariable("EnttConnectionString");
        var conn = new SqlConnection(connectionString);
        var acceptances = new List<EnttAcceptance>();
        using (var cmd = new SqlCommand("SELECT DISTINCT [ConsignmentNo] FROM [Entt].[Acceptance] WHERE ([LocationId] = @LocationId AND [DateTime] >= @StartDate AND [DateTime] <= @EndDate) OR ([IsPupStatCode] = 1 AND [PupStatCodeLocation] = @LocationId AND [PupStatCodeDateTime] >= @StartDate AND [PupStatCodeDateTime] <= @EndDate) OR ([IsMissort] = 1 AND [MissortLocation] = @LocationId AND[MissortDateTime] >= @StartDate AND[MissortDateTime] <= @EndDate)", conn))
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Parameters.AddWithValue("@LocationId", branch);
            cmd.Parameters.AddWithValue("@StartDate", DateTime.Today);
            cmd.Parameters.AddWithValue("@EndDate", DateTime.Now);

            using (var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
            {
                while (reader.Read())
                {
                    var a = new EnttAcceptance
                    {
                        ConsignmentNo = reader.GetString(0)                        
                    };
                    acceptances.Add(a);                    
                }
            }
        }

        var sb = new StringBuilder();
        foreach (var item in acceptances)
        {
            sb.AppendFormat("{0}\r\n", item.ConsignmentNo);
        }
        var fileContent = Encoding.ASCII.GetBytes(sb.ToString());
        return File(fileContent, "text/plain", "sample.txt");
    }
}