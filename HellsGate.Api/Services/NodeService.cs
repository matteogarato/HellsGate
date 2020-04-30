using HellsGate.Models.Context;
using HellsGate.Models.DatabaseModel;
using HellsGate.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HellsGate.Services
{
    public class NodeService : INodeService
    {
        private readonly HellsGateContext _context;

        public NodeService(HellsGateContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
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