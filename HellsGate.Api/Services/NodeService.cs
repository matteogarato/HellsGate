using HellsGate.Api.Infrastructure;
using HellsGate.Models.Context;
using HellsGate.Models.DatabaseModel;
using HellsGate.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HellsGate.Services
{
    public class NodeService : INodeService
    {
        private readonly AppSettings _appSettings;
        private readonly HellsGateContext _context;

        public NodeService(HellsGateContext context, IOptions<AppSettings> appSettings)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            _appSettings = appSettings.Value ?? throw new ArgumentNullException(nameof(appSettings)); ;
        }

        public async Task<NodeReadModel> Authenticate(string nodeName, string macAddress)
        {
            if (string.IsNullOrEmpty(nodeName))
            {
                return null;
            }
            if (string.IsNullOrWhiteSpace(macAddress))
            {
                return null;
            }
            if (!await _context.Nodes.AnyAsync(n => n.Name == nodeName && n.MacAddress == macAddress))
            {
                return null;
            }
            var node = await _context.Nodes.FirstAsync(n => n.Name == nodeName && n.MacAddress == macAddress);
            var nodeReaded = JToken.FromObject(node).ToObject<NodeReadModel>();
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, node.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            nodeReaded.Token = tokenHandler.WriteToken(token);
            return nodeReaded;
        }

        public Guid Create(NodeCreateModel node)
        {
            try
            {
                var savenode = JToken.FromObject(node).ToObject<NodeModel>();
                savenode.Id = Guid.NewGuid();
                savenode.CreatedAt = DateTime.UtcNow;
                _context.Nodes.Add(savenode);
                _context.SaveChanges();
                return savenode.Id;
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during Update", MethodBase.GetCurrentMethod(), ex);
                return Guid.Empty;
            }
        }

        public async Task<bool> DeleteAsync(Guid nodeId)
        {
            try
            {
                var any = await _context.Nodes.AnyAsync(n => n.Id == nodeId);
                if (!any)
                { return false; }
                _context.Nodes.Remove(_context.Nodes.First(n => n.Id == nodeId));
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during Delete", MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }

        public async Task<List<NodeReadModel>> GetAllAsync()
        {
            try
            {
                var list = await _context.Nodes.ToListAsync();
                var readedRes = new List<NodeReadModel>();
                list.ForEach(l => readedRes.Add(JToken.FromObject(l).ToObject<NodeReadModel>()));
                return readedRes;
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "", MethodBase.GetCurrentMethod(), ex);
                return null;
            }
        }

        public async Task<NodeReadModel> GetByIdAsync(Guid nodeId)
        {
            try
            {
                if (_context.Nodes.Any(n => n.Id.Equals(nodeId)))
                {
                    var model = await _context.Nodes.FirstAsync(n => n.Id.Equals(nodeId));
                    var res = JToken.FromObject(model).ToObject<NodeReadModel>();
                    return res;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "", MethodBase.GetCurrentMethod(), ex);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(NodeUpdateModel node)
        {
            try
            {
                var savenode = JToken.FromObject(node).ToObject<NodeModel>();
                var any = await GetByIdAsync(node.Id);
                if (any == null)
                {
                    return false;
                }
                else
                {
                    var toUpdate = _context.Nodes.First(n => n.Id == node.Id);
                    toUpdate = savenode;
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during Update", MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }
    }
}