using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Bussy.Server.Databases;
using Bussy.Server.Models.Order;
using Bussy.Server.Wrappers;
using MediatR;
using Sieve.Models;
using Sieve.Services;

namespace Bussy.Server.Domain.Orders.Features
{
    public static class GetOrderList
    {
        public class OrderListQuery : IRequest<PagedList<OrderModel>>
        {
            public OrderParametersModel QueryParameters { get; set; }

            public OrderListQuery(OrderParametersModel queryParameters)
            {
                QueryParameters = queryParameters;
            }
        }

        public class Handler : IRequestHandler<OrderListQuery, PagedList<OrderModel>>
        {
            private readonly BussyDbContext _db;
            private readonly SieveProcessor _sieveProcessor;
            private readonly IMapper _mapper;

            public Handler(BussyDbContext db, IMapper mapper, SieveProcessor sieveProcessor)
            {
                _mapper = mapper;
                _db = db;
                _sieveProcessor = sieveProcessor;
            }

            public async Task<PagedList<OrderModel>> Handle(OrderListQuery request, CancellationToken cancellationToken)
            {
                var collection = _db.Orders
                    as IQueryable<Order>;

                var sieveModel = new SieveModel
                {
                    Sorts = request.QueryParameters.SortOrder ?? "Id",
                    Filters = request.QueryParameters.Filters
                };

                var appliedCollection = _sieveProcessor.Apply(sieveModel, collection);
                var dtoCollection = appliedCollection
                    .ProjectTo<OrderModel>(_mapper.ConfigurationProvider);

                return await PagedList<OrderModel>.CreateAsync(dtoCollection,
                    request.QueryParameters.PageNumber,
                    request.QueryParameters.PageSize,
                    cancellationToken);
            }
        }
    }
}