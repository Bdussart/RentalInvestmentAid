using Docker.DotNet;
using Docker.DotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace RentalInvestmentAid.Web.Services
{
    public class DockerService
    {
        private readonly DockerClient _client;

        public DockerService()
        {
            _client = new DockerClientConfiguration().CreateClient();
        }

        public async Task<IList<ContainerListResponse>> ListContainersByNetworkAsync(string networkName)
        {
            var containers = await _client.Containers.ListContainersAsync(new ContainersListParameters() { All = true });
            return containers.Where(c => c.NetworkSettings.Networks.ContainsKey(networkName)).ToList();
        }

        public async Task StartContainerAsync(string containerId)
        {
            await _client.Containers.StartContainerAsync(containerId, new ContainerStartParameters());
        }

        public async Task StopContainerAsync(string containerId)
        {
            await _client.Containers.StopContainerAsync(containerId, new ContainerStopParameters());
        }

        // Add more methods as needed (e.g., RestartContainerAsync, RemoveContainerAsync, etc.)
    }

}