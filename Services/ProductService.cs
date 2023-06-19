using Cassandra;
using Cassandra.Mapping;
using core7_cassandra_angular14.Entities;

namespace core7_cassandra_angular14.Services
{

    public interface IProductService {
        Boolean AddProduct(Product product);
        IEnumerable<Product> ListAll(int page);
        IEnumerable<Product> SearchAll(string key);
        IEnumerable<Product> Dataset();

        int TotPage();

    }

    public class ProductService : IProductService
    {
        private Cluster _cluster;
        private IMapper _mapper;
        private Cassandra.ISession _session;

        public ProductService() {
            _cluster = Cluster.Builder().AddContactPoint("127.0.0.1").WithPort(9042).Build();
            _session = _cluster.Connect("core7");
            _mapper = new Mapper(_session);
        }
        
        private int NextId() {
            var cnt = _mapper.Single<Product>("SELECT count(id)+1 as Id FROM products");
            return cnt.Id;
        }

        private bool findDescription(string description) {
            try {
                Product prodDesc = _mapper.Single<Product>("SELECT descriptions FROM products WHERE descriptions = ? ALLOW FILTERING", description);
                if (prodDesc is not null) {
                    prodDesc.Descriptions = null;
                    return true;
                }
                return false;
            } catch(Exception) {} 
            return false;
        }        
        public bool AddProduct(Product product)
        {
            if (findDescription(product.Descriptions) == true) {
                throw new Exception("Descriptions is already exists....");
            }
            int ln = NextId();
            var addprod = _session.Prepare("INSERT INTO products(id,descriptions,qty,unit,cost_price,sell_price,sale_price,alert_level,critical_level,category,created_at) VALUES(?,?,?,?,?,?,?,?,?,?,toTimestamp(now()))");
            var batch = new BatchStatement()
            .Add(addprod.Bind(ln ,product.Descriptions,product.Qty,product.Unit,product.Cost_price,product.Sell_price,product.Sale_price,product.Alert_level,product.Critical_level,product.Category));
            _session.ExecuteAsync(batch);
            return true;
        }

        public IEnumerable<Product> ListAll(int page)
        {
            try {
                var perpage = 5;
                var offset = (page -1) * perpage;

                var rs = _mapper.Fetch<Product>("SELECT * FROM products")
                .OrderBy(b => b.Id)
                .Skip(offset)
                .Take(perpage)
                .ToList();
                return rs;
            } catch(Exception ex) {
                throw new Exception(ex.Message);
            } 
        }

        public IEnumerable<Product> SearchAll(string key)
        {
            var rs = _mapper.Fetch<Product>("SELECT * FROM products WHERE category = ? ALLOW FILTERING",key);    
            return rs.ToList();
        }

        public IEnumerable<Product> Dataset()
        {
            throw new NotImplementedException();
        }

        public int TotPage()
        {
            var perpage = 5;
            var totrecs = _mapper.Single<Product>("SELECT count(*) as Id FROM products");            
            int totpage = (int)Math.Ceiling((float)(totrecs.Id) / perpage);
            return totpage;
        }
    }
}


