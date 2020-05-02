using HellsGate.Models;
using HellsGate.Models.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HellsGate.Services.Interfaces
{
    public interface INodeService
    {
        public Task<NodeReadModel> Authenticate(string nodeName, string MacAddress, WellknownAuthorizationLevel AuthValue);

        public Guid Create(NodeCreateModel node);

        public Task<bool> DeleteAsync(Guid nodeId);

        public Task<List<NodeReadModel>> GetAllAsync();

        public Task<NodeReadModel> GetByIdAsync(Guid nodeId);

        public Task<bool> UpdateAsync(NodeUpdateModel node);
    }
}