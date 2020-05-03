using AutoMapper;
using SocialNetwork.BLL.DTOs;
using SocialNetwork.BLL.Interfaces;
using SocialNetwork.BLL.Validators;
using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SocialNetwork.BLL.Services
{
    public class RequestService : IRequestService
    {
            private INetworkUnitOfWork _uow;

            private IMapper _mapper;

            public RequestService(INetworkUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public IEnumerable<RequestDTO> GetAll()
            {
                var requests = _uow.RequestRepository.GetAll().ToList();

                return _mapper.Map<IEnumerable<RequestDTO>>(requests);
            }

            public IEnumerable<RequestDTO> GetAllByRequestId(int requestId)
            {
                var requests = _uow.RequestRepository.GetAll()
                    .Where(r => r.RequestId == requestId).ToList();

                return _mapper.Map<IEnumerable<RequestDTO>>(requests);
            }

            public RequestDTO Get(int id)
            {
                var request = _uow.RequestRepository.Get(id);

                return _mapper.Map<RequestDTO>(request);
            }

            public RequestDTO Create(RequestDTO item)
            {
                if (!RequestValidator.IsRequestValid(item))
                {
                    return null;
                }

                var request = _uow.RequestRepository.Get((int)item.RequestId);
                if (request == null)
                {
                    return null;
                }

                var requestDto = _mapper.Map<Request>(item);
                requestDto = _uow.RequestRepository.Create(request);

                _uow.Save();

                return _mapper.Map<RequestDTO>(requestDto);
            }

            public RequestDTO Update(int id, RequestDTO item)
            {
                if (!RequestValidator.IsRequestValid(item))
                {
                    return null;
                }

                if (_uow.RequestRepository.Get((int)item.RequestId) == null)
                {
                    return null;
                }

                item.RequestId = id;
                var request = _mapper.Map<Request>(item);
                request = _uow.RequestRepository.Update(request);
                _uow.Save();

                return _mapper.Map<RequestDTO>(request);
            }

            public RequestDTO Delete(int id)
            {
                var request = _uow.RequestRepository.Delete(id);
                _uow.Save();

                return _mapper.Map<RequestDTO>(request);
            }
    }
}
