using AutoMapper;
using Grpc.Core;
using PlatformService.Interface;

namespace PlatformService.Grpc
{
    public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
    {
        private readonly IPlatformRepo _platformRepo;
        private readonly IMapper _mapper;

        public GrpcPlatformService(IPlatformRepo platformRepo, IMapper mapper)
        {
            _platformRepo = platformRepo;
            _mapper = mapper;
        }

        public override async Task<PlatformResponse> GetAllPlatforms(GetAllRequest request,
        ServerCallContext context)
        {
            Console.WriteLine("GRPC procedure Recieved. Processing");
            var response = new PlatformResponse();
            var platforms = await _platformRepo.GetPlatforms();

            foreach(var plat in platforms)
            {
                response.Platform.Add(_mapper.Map<GrpcPlatformModel>(plat));
            }
            return response;
        }
    }
}