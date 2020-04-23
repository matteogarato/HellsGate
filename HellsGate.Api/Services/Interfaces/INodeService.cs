using HellsGate.Models.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HellsGate.Services.Interfaces
{
    public interface INodeService
    {
        public Task<List<NodeReadModel>> GetAllAsync();

        public Task<NodeReadModel> GetByIdAsync(Guid nodeId);

        public Task<bool> UpdateAsync(NodeUpdateModel node);

        public Task<bool> DeleteAsync(Guid nodeId);

        public Guid Create(NodeCreateModel node);
    }
}